using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using System.Linq;
//////using Microsoft.AspNetCore.Mvc;
//////using Microsoft.AspNetCore.Authorization;
//////using Microsoft.AspNetCore.Authentication;
//////using Microsoft.AspNetCore.Authentication.Cookies;
//////using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Web;
using System.Net.Http;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

//////using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
//////using System.IdentityModel.Tokens.Jwt;
using System.Net;
//////using System.Security.Cryptography.X509Certificates;
using System.Text;

//////using Microsoft.AspNetCore.Http;
//////using Microsoft.AspNetCore.HttpOverrides;
//////using Akka.Actor;
//////using mmria.server.Controllers;

using Epi.Web.Enter.Common.Message;
using System.Web.Security;
using Epi.Web.MVC.Models;


using System.Web.Mvc;

namespace Epi.Web.MVC.Controllers
{
    public partial class LoginController : Controller
    {
        public const string ClientId = "urn:gov:gsa:openidconnect.profiles:sp:sso:logingov:aspnet_example";
        public const string ClientUrl = "http://localhost:60201";
        public const string IdpUrl = "https://idp.int.identitysandbox.gov";
        public const string AcrValues = "http://idmanagement.gov/ns/assurance/loa/1";

		//////private IConfiguration _configuration;
		//////private IHttpContextAccessor _accessor;
		//////private ActorSystem _actorSystem;


		//////public AccountController(IHttpContextAccessor httpContextAccessor, ActorSystem actorSystem, IConfiguration configuration)
		//////{
		//////    _accessor = httpContextAccessor;
		//////    _actorSystem = actorSystem;
		//////    _configuration = configuration;
		//////}
/*
        public ActionResult Index()
        {
            if (TempData["email"] == null)
            {
                ViewBag.Message = "Log in to see your account.";
            }
            else
            {
                ViewBag.Message = $"Welcome back {TempData["email"]}!";
                ViewBag.Content = $"Your user ID is: {TempData["id"]}";
            }
            return View();
        }
*/
        [AllowAnonymous]
        public ActionResult SignIn()
        {
			var sams_endpoint_authorization = ConfigurationManager.AppSettings["SAMS_ENDPOINT_AUTHORIZATION"];
			var sams_client_id = ConfigurationManager.AppSettings["SAMS_CLIENT_ID"];
			var sams_callback_url = ConfigurationManager.AppSettings["SAMS_CALLBACK_URL"];

			var state = Guid.NewGuid().ToString("N");
            var nonce = Guid.NewGuid().ToString("N");

            var sams_url = $"{sams_endpoint_authorization}?" +
                "&client_id=" + sams_client_id +
                "&redirect_uri=" + $"{sams_callback_url}" +
                "&response_type=code" +
                "&scope=" + System.Web.HttpUtility.HtmlEncode("openid profile email") +
                "&state=" + state +
                "&nonce=" + nonce;

            System.Diagnostics.Debug.WriteLine($"url: {sams_url}");

            return Redirect(sams_url);
        }

        [AllowAnonymous]
        public async Task<ActionResult> SignInCallback()
        {
			var sams_endpoint_authorization = ConfigurationManager.AppSettings["SAMS_ENDPOINT_AUTHORIZATION"];
			var sams_endpoint_token = ConfigurationManager.AppSettings["SAMS_ENDPOINT_TOKEN"];
			var sams_endpoint_user_info = ConfigurationManager.AppSettings["SAMS_ENDPOINT_USER_INFO"];
			var sams_endpoint_token_validation = ConfigurationManager.AppSettings["SAMS_TOKEN_VALIDATION"];
			var sams_endpoint_user_info_sys = ConfigurationManager.AppSettings["SAMS_ENDPOINT_USER_INFO_SYS"];
			var sams_client_id = ConfigurationManager.AppSettings["SAMS_CLIENT_ID"];
			var sams_client_secret = ConfigurationManager.AppSettings["SAMS_CLIENT_SECRET"];
			var sams_callback_url = ConfigurationManager.AppSettings["SAMS_CALLBACK_URL"];

			var querystring = this.Request.QueryString.ToString();
			var querystring_skip = querystring.Substring(1, querystring.Length - 1);
            var querystring_array = querystring_skip.Split('&');
			var querystring_dictionary = new Dictionary<string,string>();

            foreach(string item in this.Request.QueryString)
            {
				querystring_dictionary.Add(item, this.Request.QueryString[item]);
			}

            var code = querystring_dictionary["code"];
            var state = querystring_dictionary["state"];
            System.Diagnostics.Debug.WriteLine($"code: {code}");
            System.Diagnostics.Debug.WriteLine($"state: {state}");

            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, sams_endpoint_token);

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "client_id", sams_client_id },
                { "client_secret", sams_client_secret },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "scope", "openid profile email"},
                {"redirect_uri", sams_callback_url }
            });

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            var access_token = payload.Value<string>("access_token");
            var refresh_token = payload.Value<string>("refresh_token");
            var expires_in = payload.Value<int>("expires_in");
			var unix_time = DateTimeOffset.UtcNow.AddSeconds(expires_in);
			var scope = payload.Value<string>("scope");
            var id_token = payload.Value<string>("id_token");;

            var id_array = id_token.Split('.');
            var replaced_value = id_array[1].Replace('-', '+').Replace('_', '/');
            var base64 = replaced_value.PadRight(replaced_value.Length + (4 - replaced_value.Length % 4) % 4, '=');

            var id_0 = DecodeToken(id_array[0]);
            var id_1 = DecodeToken(id_array[1]);
            var id_body = Base64Decode(base64);

            var user_info_sys_request = new HttpRequestMessage(HttpMethod.Post, sams_endpoint_user_info + "?token=" + id_token);

            user_info_sys_request.Headers.Add("Authorization","Bearer " + access_token);
            user_info_sys_request.Headers.Add("client_id", sams_client_id);
            user_info_sys_request.Headers.Add("client_secret", sams_client_secret);

            response = await client.SendAsync(user_info_sys_request);
            response.EnsureSuccessStatusCode();

            var temp_string = await response.Content.ReadAsStringAsync();
            payload = JObject.Parse(temp_string);

            var email = payload.Value<string>("email");

			UserLoginModel UserLoginModel = new UserLoginModel();
			UserLoginModel.UserName = email;
			UserLoginModel.Password = "cO3wJrlcn";
			UserLoginModel.SAMS = true;
			return GetAuthenticatedUserr(UserLoginModel, "/Home/Index"); //>> return GetValidatedUser(... dpbrown
        }

		private ActionResult GetAuthenticatedUserr(UserLoginModel Model, string ReturnUrl)
		{
			SetTermOfUse();
			string formId = "", pageNumber;

			if (ReturnUrl == null || !ReturnUrl.Contains("/"))
			{
				ReturnUrl = "/Home/Index";
			}
			else
			{
				formId = ReturnUrl.Substring(0, ReturnUrl.IndexOf('/'));
				pageNumber = ReturnUrl.Substring(ReturnUrl.LastIndexOf('/') + 1);
			}

			try
			{
				Epi.Web.Enter.Common.Message.UserAuthenticationResponse result = _isurveyFacade.ValidateUser(Model.UserName, Model.Password);
				if (result.UserIsValid)
				{
					if (result.User.ResetPassword)
					{
						UserResetPasswordModel model = new UserResetPasswordModel();
						model.UserName = Model.UserName;
						model.FirstName = result.User.FirstName;
						model.LastName = result.User.LastName;
						ReadPasswordPolicy(model);
						return ResetPassword(model);
					}
					else
					{

						FormsAuthentication.SetAuthCookie(Model.UserName, false);
						string UserId = Epi.Web.Enter.Common.Security.Cryptography.Encrypt(result.User.UserId.ToString());
						Session["UserId"] = UserId;
						//Session["UsertRole"] = result.User.Role;
						Session["UserHighestRole"] = result.User.UserHighestRole;
						Session["UserEmailAddress"] = result.User.EmailAddress;
						Session["UserFirstName"] = result.User.FirstName;
						Session["UserLastName"] = result.User.LastName;
						Session["UGuid"] = result.User.UGuid;
						return RedirectToAction(Epi.Web.MVC.Constants.Constant.INDEX, "Home", new { surveyid = formId });
						//return Redirect(ReturnUrl);
					}
				}
				//else
				{
					ModelState.AddModelError("", "The email or password you entered is incorrect.");
					Model.ViewValidationSummary = true;
					return View(Model);
				}
			}
			catch (Exception)
			{
				ModelState.AddModelError("", "The email or password you entered is incorrect.");
				Model.ViewValidationSummary = true;
				return View(Model);
				throw;
			}



		}

		public string GetRequestIP(bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            ////////if (tryUseXForwardHeader)
            ////////    ip = GetHeaderValueAs<string>("X-Forwarded-For").SplitCsv().FirstOrDefault();

            ////////// RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            ////////if (ip.IsNullOrWhitespace() && _accessor.HttpContext?.Connection?.RemoteIpAddress != null)
            ////////    ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            ////////if (ip.IsNullOrWhitespace())
            ////////    ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            ////////// _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            ////////if (ip.IsNullOrWhitespace())
            ////////    throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        public T GetHeaderValueAs<T>(string headerName)
        {
            ////////Microsoft.Extensions.Primitives.StringValues values;

            ////////if (_accessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            ////////{
            ////////    string rawValues = values.ToString();   // writes out as Csv when there are multiple.

            ////////    if (!rawValues.IsNullOrWhitespace())
            ////////        return (T)Convert.ChangeType(values.ToString(), typeof(T));
            ////////}
            return default(T);
        }

        public async Task create_user_principal(string p_user_name, List<string> p_role_list, DateTime p_session_expire_date_time)
        {
            const string Issuer = "https://contoso.com";
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, p_user_name, ClaimValueTypes.String, Issuer));


            foreach(var role in p_role_list)
            {
                if(role == "_admin")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "installation_admin", ClaimValueTypes.String, Issuer));
                }
            }

            ////////foreach(var role in mmria.server.util.authorization.get_current_user_role_jurisdiction_set_for(p_user_name).Select( jr => jr.role_name).Distinct())
            ////////{
            ////////    claims.Add(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, Issuer));
            ////////}

            //////////Response.Cookies.Append("uid", p_user_name);
            //////////Response.Cookies.Append("roles", string.Join(",",p_role_list));

            ////////var userIdentity = new ClaimsIdentity("SuperSecureLogin");
            ////////userIdentity.AddClaims(claims);
            ////////var userPrincipal = new ClaimsPrincipal(userIdentity);

            ////////var session_idle_timeout_minutes = 30;

            ////////if(_configuration["mmria_settings:session_idle_timeout_minutes"] != null)
            ////////{
            ////////    int.TryParse(_configuration["mmria_settings:session_idle_timeout_minutes"], out session_idle_timeout_minutes);
            ////////}

            ////////await HttpContext.SignInAsync(
            ////////    CookieAuthenticationDefaults.AuthenticationScheme,
            ////////    userPrincipal,
            ////////    new AuthenticationProperties
            ////////    {
            ////////        ExpiresUtc = p_session_expire_date_time,
            ////////        IsPersistent = false,
            ////////        AllowRefresh = true,
            ////////    });
        }

        private string DecodeToken(string p_value)
        {
            var replaced_value = p_value.Replace('-', '+').Replace('_', '/');
            var base64 = replaced_value.PadRight(replaced_value.Length + (4 - replaced_value.Length % 4) % 4, '=');
            return Base64Decode(base64);
        }

        private string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private bool checkID(string idBody, string issuer, string clientID)
        {
            dynamic o = JObject.Parse(idBody);

            if (o.iss != issuer) return false;
            if (o.aud != clientID) return false;
            if (o.exp < DateTime.UtcNow) return false;

            return true;
        }

        private Models.User add_new_user(string p_name, string p_password)
        {
            return new Models.User(){
                _id = $"org.couchdb.user:{p_name}",
                password =  p_password,
                password_scheme = "pbkdf2",
                iterations = 10,
                name = p_name,
                roles = new List<string>().ToArray(),
                type = "user",
                derived_key =  "a1bb5c132df5b7df7654bbfa0e93f9e304e40cfe",
                salt = "510427706d0deb511649021277b2c05d",
                is_active = true,
                is_enabled = true
                };
        }

    }
}
