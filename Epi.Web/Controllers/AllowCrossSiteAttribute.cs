using System;
using System.Web.Mvc;

namespace Epi.Web.MVC.Controllers
{
	internal class AllowCrossSiteAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
			base.OnActionExecuting(filterContext);
		}
	}
}