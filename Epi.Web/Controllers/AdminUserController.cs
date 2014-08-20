using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Epi.Web.MVC.Controllers
{
    public class AdminUserController : Controller
    {
        //
        // GET: /Organization/

         
        public ActionResult UserList()
            {
            return View("UserList");
            }
          
          public ActionResult UserInfo()
          {
              return View("UserInfo");
          }
         
    }
}
