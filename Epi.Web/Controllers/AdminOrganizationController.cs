using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Epi.Web.MVC.Controllers
{
    public class AdminOrganizationController : Controller
    {
        //
        // GET: /AdminOrganization/

        public ActionResult OrgList()
        {
        return View("OrgList");
        }
    
        public ActionResult OrgInfo()
        {
        return View("OrgInfo");
        }
     

    }
}
