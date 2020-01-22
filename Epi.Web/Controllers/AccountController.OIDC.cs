using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Web;
using System.Net.Http;


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Akka.Actor;

using mmria.server.Controllers;


/*
https://github.com/18F/identity-oidc-aspnet

*/

namespace mmria.common.Controllers
{
    public partial class AccountController : Controller
    {
        public const string ClientId = "urn:gov:gsa:openidconnect.profiles:sp:sso:logingov:aspnet_example";
        public const string ClientUrl = "http://localhost:50764";
        public const string IdpUrl = "https://idp.int.identitysandbox.gov";
        public const string AcrValues = "http://idmanagement.gov/ns/assurance/loa/1";


       // private IConfiguration _configuration;
        private IHttpContextAccessor _accessor;
        private ActorSystem _actorSystem;


        public AccountController(IHttpContextAccessor httpContextAccessor, ActorSystem actorSystem, IConfiguration configuration)
        {
            _accessor = httpContextAccessor;
            _actorSystem = actorSystem;
            _configuration = configuration;
        }
        private IConfiguration _configuration;
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

            var sams_endpoint_authorization = _configuration["sams:endpoint_authorization"];
            var sams_endpoint_token = _configuration["sams:endpoint_token"];
            var sams_endpoint_user_info = _configuration["sams:endpoint_user_info"];
            var sams_endpoint_token_validation = _configuration["sams:token_validation"];
            var sams_endpoint_user_info_sys = _configuration["sams:user_info_sys"];
            var sams_client_id = _configuration["sams:client_id"];
            var sams_callback_url = _configuration["sams:callback_url"];        

            var state = Guid.NewGuid().ToString("N");
            var nonce = Guid.NewGuid().ToString("N");

            var sams_url = $"{sams_endpoint_authorization}?" +
                "&client_id=" + sams_client_id +
                //"&prompt=select_account" +
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
            var sams_endpoint_authorization = _configuration["sams:endpoint_authorization"];
            var sams_endpoint_token = _configuration["sams:endpoint_token"];
            var sams_endpoint_user_info = _configuration["sams:endpoint_user_info"];
            var sams_endpoint_token_validation = _configuration["sams:token_validation"];
            var sams_endpoint_user_info_sys = _configuration["sams:endpoint_user_info_sys"];
            var sams_client_id = _configuration["sams:client_id"];
            var sams_client_secret = _configuration["sams:client_secret"];
            
            var sams_callback_url = _configuration["sams:callback_url"];        

            //?code=6c17b2a3-d65a-44fd-a28c-9aee982f80be&state=a4c8326ca5574999aa13ca02e9384c3d
            // Retrieve code and state from query string, pring for debugging
            var querystring = Request.QueryString.Value;
            var querystring_skip = querystring.Substring(1, querystring.Length -1);
            var querystring_array = querystring_skip.Split("&");

            var querystring_dictionary = new Dictionary<string,string>();
            foreach(string item in querystring_array)
            {
                var pair = item.Split("=");
                querystring_dictionary.Add(pair[0], pair[1]);
            }

            var code = querystring_dictionary["code"];
            var state = querystring_dictionary["state"];
            System.Diagnostics.Debug.WriteLine($"code: {code}");
            System.Diagnostics.Debug.WriteLine($"state: {state}");

            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, sams_endpoint_token);

            /*
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "client_id", sams_client_id },
                { "client_secret", sams_client_secret },
                { "grant_type", "client_credentials" },
                { "code", code },
            });
             */

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


            var scope = payload.Value<string>("scope");

            //HttpContext.Session.SetString("access_token", access_token);
            //HttpContext.Session.SetString("refresh_token", refresh_token);

            var unix_time = DateTimeOffset.UtcNow.AddSeconds(expires_in);
            //HttpContext.Session.SetString("expires_at", unix_time.ToString());



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

            /* 
            user_info_sys_request.Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "client_id", sams_client_id },
                { "client_secret", sams_client_secret },
                { "grant_type", "client_credentials" },
                { "scope", scope },
            });
            */



            response = await client.SendAsync(user_info_sys_request);
            response.EnsureSuccessStatusCode();

            var temp_string = await response.Content.ReadAsStringAsync();
            payload = JObject.Parse(temp_string);

            
            var email = payload.Value<string>("email");


            //check if user exists
            var config_couchdb_url = _configuration["mmria_settings:couchdb_url"];
            var config_timer_user_name = _configuration["mmria_settings:timer_user_name"];
            var config_timer_password = _configuration["mmria_settings:timer_password"];

            mmria.common.model.couchdb.user user = null;
			try
			{
				string request_string = config_couchdb_url + "/_users/" + System.Web.HttpUtility.HtmlEncode("org.couchdb.user:" + email.ToLower());
				var user_curl = new mmria.server.cURL("GET", null, request_string, null, config_timer_user_name, config_timer_password);
				var responseFromServer = await user_curl.executeAsync();

				user = Newtonsoft.Json.JsonConvert.DeserializeObject<mmria.common.model.couchdb.user>(responseFromServer);
			}
			catch(Exception ex)
			{
				Console.WriteLine (ex);

			} 

            mmria.common.model.couchdb.document_put_response user_save_result = null;

            if(user == null)// if user does NOT exists create user with email
            {
                user = add_new_user(email.ToLower(), Guid.NewGuid().ToString());

                try
                {
                    Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings ();
                    settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    var object_string = Newtonsoft.Json.JsonConvert.SerializeObject(user, settings);

                    string user_db_url = config_couchdb_url + "/_users/"  + user._id;

                    var user_curl = new mmria.server.cURL("PUT", null, user_db_url, object_string, config_timer_user_name, config_timer_password);
                    var responseFromServer = await user_curl.executeAsync();
                    user_save_result = Newtonsoft.Json.JsonConvert.DeserializeObject<mmria.common.model.couchdb.document_put_response>(responseFromServer);

                }
                catch(Exception ex) 
                {
                    Console.WriteLine (ex);
                }
            }

            //create login session
            if(user_save_result == null || user_save_result.ok)
            {
                var session_data = new System.Collections.Generic.Dictionary<string,string>(StringComparer.InvariantCultureIgnoreCase);
                session_data["access_token"] = access_token;
                session_data["refresh_token"] = refresh_token;
                session_data["expires_at"] = unix_time.ToString();

                await create_user_principal(user.name, new List<string>(), unix_time.DateTime);


                var Session_Event_Message = new mmria.server.model.actor.Session_Event_Message
                (
                    DateTime.Now,
                    user.name,
                    this.GetRequestIP(),
                    mmria.server.model.actor.Session_Event_Message.Session_Event_Message_Action_Enum.successful_login
                );

                _actorSystem.ActorOf(Props.Create<mmria.server.model.actor.Record_Session_Event>()).Tell(Session_Event_Message);




                var Session_Message = new mmria.server.model.actor.Session_Message
                (
                    Guid.NewGuid().ToString(), //_id = 
                    null, //_rev = 
                    DateTime.Now, //date_created = 
                    DateTime.Now, //date_last_updated = 
                    null, //date_expired = 

                    true, //is_active = 
                    user.name, //user_id = 
                    this.GetRequestIP(), //ip = 
                    Session_Event_Message._id, // session_event_id = 
                    session_data
                );

                _actorSystem.ActorOf(Props.Create<mmria.server.model.actor.Post_Session>()).Tell(Session_Message);
                Response.Cookies.Append("sid", Session_Message._id, new CookieOptions{ HttpOnly = true });
                Response.Cookies.Append("expires_at", unix_time.ToString(), new CookieOptions{ HttpOnly = true });
                //return RedirectToAction("Index", "HOME");
                //return RedirectToAction("Index", "HOME");
            }

            return RedirectToAction("Index", "HOME");

            // Generate JWT for token request
            //var cert = new X509Certificate2(Server.MapPath("~/App_Data/cert.pfx"), "1234");
            /*
            var cert = new X509Certificate2();
            var signingCredentials = new SigningCredentials(new X509SecurityKey(cert), SecurityAlgorithms.RsaSha256);
            var header = new JwtHeader(signingCredentials);
             
            var header = new JwtHeader();
            var payload = new JwtPayload
            {
                {"iss", sams_client_id},
                {"sub", sams_client_id},
                {"aud", $"{sams_endpoint_token}"},
                {"jti", Guid.NewGuid().ToString("N")},
                {"exp", (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 5 * 60}
            };
            var securityToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(securityToken);

            // Send POST to make token request
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["client_assertion"] = tokenString;
                data["client_assertion_type"] = HttpUtility.HtmlEncode("urn:ietf:params:oauth:client-assertion-type:jwt-bearer");
                data["code"] = code;
                data["grant_type"] = "authorization_code";

                //var response = wb.UploadValues($"{IdpUrl}/api/openid_connect/token", "POST", data);
                var response = wb.UploadValues($"{sams_endpoint_token}", "POST", data);

                var responseString = Encoding.ASCII.GetString(response);
                dynamic tokenResponse = JObject.Parse(responseString);

                var token = handler.ReadToken((String)tokenResponse.id_token) as JwtSecurityToken;
                var userId = token.Claims.First(c => c.Type == "sub").Value;
                var userEmail = token.Claims.First(c => c.Type == "email").Value;

                TempData["id"] = userId;
                TempData["email"] = userEmail;
                //return RedirectToAction("Index", "HOME");
                return RedirectToAction("Index", "HOME");
            }*/
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
            if (tryUseXForwardHeader)
                ip = GetHeaderValueAs<string>("X-Forwarded-For").SplitCsv().FirstOrDefault();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhitespace() && _accessor.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (ip.IsNullOrWhitespace())
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        public T GetHeaderValueAs<T>(string headerName)
        {
            Microsoft.Extensions.Primitives.StringValues values;

            if (_accessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!rawValues.IsNullOrWhitespace())
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
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


            foreach(var role in mmria.server.util.authorization.get_current_user_role_jurisdiction_set_for(p_user_name).Select( jr => jr.role_name).Distinct())
            {

                claims.Add(new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, Issuer));
            }


            //Response.Cookies.Append("uid", p_user_name);
            //Response.Cookies.Append("roles", string.Join(",",p_role_list));
            
            var userIdentity = new ClaimsIdentity("SuperSecureLogin");
            userIdentity.AddClaims(claims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var session_idle_timeout_minutes = 30;
            
            if(_configuration["mmria_settings:session_idle_timeout_minutes"] != null)
            {
                int.TryParse(_configuration["mmria_settings:session_idle_timeout_minutes"], out session_idle_timeout_minutes);
            }


            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                new AuthenticationProperties
                {
                    ExpiresUtc = p_session_expire_date_time,
                    IsPersistent = false,
                    AllowRefresh = true,
                });

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

        private mmria.common.model.couchdb.user add_new_user(string p_name, string p_password)
        {
            return new mmria.common.model.couchdb.user(){
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
