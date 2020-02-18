using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Epi.Web.MVC
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			// -----------------
			// BEGIN IGNORE ROUTE
			// -----------------
			routes.IgnoreRoute(
				"{resource}.axd/{*pathInfo}"
			);
			routes.IgnoreRoute(
				"{resource}.axd/{*pathInfo}"
			);
			routes.IgnoreRoute(
				"{*staticfile}",
				new { staticfile = @".*\.(jpg|gif|jpeg|png|js|css|htm|html|htc|php)$" }
			);

			// -----------------
			// BEGIN MAP HTTP ROUTE
			// -----------------
			routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			// -----------------
			// BEGIN MAP ROUTE
			//
			// Route name
			// URL with parameters
			// Parameter defaults
			// -----------------
			routes.MapRoute(
				null,
				"Home/ReadSortedResponseInfo",
				new { controller = "Home", action = "ReadSortedResponseInfo", formid = UrlParameter.Optional, page = UrlParameter.Optional, sort = UrlParameter.Optional, sortfield = UrlParameter.Optional, orgid = UrlParameter.Optional, reset = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Home/ReadResponseInfo",
				new { controller = "Home", action = "ReadResponseInfo" }
			);
			routes.MapRoute(
				null,
				"Home/GetSettings",
				new { controller = "Home", action = "GetSettings", formid = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Home/SaveSettings",
				new { controller = "Home", action = "SaveSettings", formid = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Home/Edit",
				new { controller = "Home", action = "Edit", ResId = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Home/LogOut",
				new { controller = "Home", action = "LogOut", ResId = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Home/Delete/{ResponseId}",
				new { controller = "Home", action = "Delete", ResponseId = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Home/CheckForConcurrency",
				new { controller = "Home", action = "CheckForConcurrency", responseid = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Home/Notify",
				new { controller = "Home", action = "Notify", responseid = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Home/Unlock/{ResponseId}/{RecoverLastRecordVersion}",
				new { controller = "Home", action = "Unlock", ResponseId = UrlParameter.Optional, RecoverLastRecordVersion = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"FormResponse/Unlock/{ResponseId}/{RecoverLastRecordVersion}",
				new { controller = "FormResponse", action = "Unlock", ResponseId = UrlParameter.Optional, RecoverLastRecordVersion = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"FormResponse/CheckForConcurrency",
				new { controller = "FormResponse", action = "CheckForConcurrency" }
			);
			routes.MapRoute(
				null,
				"FormResponse/Delete/{ResponseId}",
				new { controller = "FormResponse", action = "Delete", ResponseId = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"FormResponse/DeleteBranch/{ResponseId}",
				new { controller = "FormResponse", action = "DeleteBranch", ResponseId = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Home/{surveyid}/{orgid}",
				new { controller = "Home", action = "Index", surveyid = UrlParameter.Optional, orgid = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Home/Index/{surveyid}/{AddNewFormId}",
				new { controller = "Home", action = "Index", surveyid = UrlParameter.Optional, AddNewFormId = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Home/Index/{surveyid}/{EditForm}",
				new { controller = "Home", action = "Index", surveyid = UrlParameter.Optional, EditForm = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"FormResponse/LogOut",
				new { controller = "FormResponse", action = "LogOut", ResId = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"FormResponse/ReadResponseInfo",
				new { controller = "FormResponse", action = "ReadResponseInfo", ResId = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"FormResponse/{formid}/{responseid}",
				new { controller = "FormResponse", action = "Index", formid = UrlParameter.Optional, responseid = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"FormResponse/{formid}",
				new { controller = "FormResponse", action = "Index", formid = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"EIWST/Notify/{id}",
				new { controller = "EIWST", action = "Notify", id = "" }
			);
			routes.MapRoute(
				null,
				"EIWST/DataService/{surveyid}",
				new { controller = "EIWST", action = "Index", surveyid = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Survey/UpdateResponseXml/{id}",
				new { controller = "Survey", action = "UpdateResponseXml", id = "" }
			);
			routes.MapRoute(
				null,
				"Survey/SaveSurvey/{id}",
				new { controller = "Survey", action = "SaveSurvey", id = "" }
			);
			routes.MapRoute(
				null,
				"Survey/Delete/{responseid}",
				new { controller = "Survey", action = "Delete", responseid = UrlParameter.Optional, PageNumber = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Survey/DeleteBranch/{responseid}",
				new { controller = "Survey", action = "DeleteBranch", responseid = UrlParameter.Optional, PageNumber = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Survey/AddChild",
				new { controller = "Survey", action = "AddChild" }
			);
			routes.MapRoute(
				null,
				"Survey/GetCodesValue",
				new { controller = "Survey", action = "GetCodesValue" }
			);
			routes.MapRoute(
				null,
				"Survey/GetAutoCompleteList",
				new { controller = "Survey", action = "GetAutoCompleteList" }
			);
			routes.MapRoute(
				null,
				"Survey/HasResponse",
				new { controller = "Survey", action = "HasResponse" }
			);
			routes.MapRoute(
				null,
				"Survey/UpDateGrid",
				new { controller = "Survey", action = "UpDateGrid" }
			);
			routes.MapRoute(
				null,
				"Survey/ReadResponseInfo",
				new { controller = "Survey", action = "ReadResponseInfo" }
			);
			routes.MapRoute(
				 null,
				 "Survey/LogOut",
				 new { controller = "Survey", action = "LogOut", ResId = UrlParameter.Optional }
			 );
			routes.MapRoute(
				null,
				"Survey/{responseid}/{PageNumber}",
				new { controller = "Survey", action = "Index", responseid = UrlParameter.Optional, PageNumber = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Login/ForgotPassword",
				new { controller = "Login", action = "ForgotPassword" }
			 );
			routes.MapRoute(
				null,
				"Login/ResetPassword",
				new { controller = "Login", action = "ResetPassword" }
			);
			routes.MapRoute(
				null,
				"Login/Index",
				new { controller = "Login", action = "Index" }
			);
			routes.MapRoute(
				null,
				"Login",
				new { controller = "Login", action = "Index" }
			);
			routes.MapRoute(
				null,
				"Login/Callback",
				new { controller = "Login", action = "SignInCallback" }
			);
			routes.MapRoute(
				null,
				"Final/{surveyid}",
				new { controller = "Final", action = "Index", surveyid = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"Post/Notify/{id}",
				new { controller = "Post", action = "Notify", id = "" }
			);
			routes.MapRoute(
				null,
				"Post/SignOut/{id}",
				new { controller = "Post", action = "SignOut", id = "" }
			);
			routes.MapRoute(
				null,
				"AdminOrganization/AutoComplete",
				new { controller = "AdminOrganization", action = "AutoComplete" }
			);
			routes.MapRoute(
				null,
				"AdminOrganization/GetUserInfoAD",
				new { controller = "AdminOrganization", action = "GetUserInfoAD", email = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"AdminOrganization/OrgList",
				new { controller = "AdminOrganization", action = "OrgList" }
			);
			routes.MapRoute(
				null,
				"AdminOrganization/{orgkey}/{iseditmode}",
				new { controller = "AdminOrganization", action = "OrgInfo", orgkey = UrlParameter.Optional, iseditmode = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"AdminOrganization/Cancel",
				new { controller = "AdminOrganization", action = "Cancel" }
			);
			routes.MapRoute(
				null,
				"AdminUser/GetUserInfoAD",
				new { controller = "AdminUser", action = "GetUserInfoAD", email = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"AdminUser/GetUserList",
				new { controller = "AdminUser", action = "GetUserList", orgid = UrlParameter.Optional }
			);
			routes.MapRoute(
				null,
				"AdminUser/UserList",
				new { controller = "AdminUser", action = "UserList" }
			);
			routes.MapRoute(
				null,
				"AdminUser/{userid}/{iseditmode}/{orgid}",
				new { controller = "AdminUser", action = "UserInfo", userid = UrlParameter.Optional, iseditmode = UrlParameter.Optional, orgid = UrlParameter.Optional }
			);
		}
	}
}
