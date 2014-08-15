using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Epi.Web.MVC.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Organization/

        public ActionResult Index()
        {
            return View("OrgList");
        }
        public ActionResult UserList()
            {
            return View("UserList");
            }
          public ActionResult OrgInfo()
            {
            return View("OrgInfo");
            }
         
    }
}
