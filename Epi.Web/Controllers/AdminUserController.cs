using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Epi.Web.Enter.Common.Message;
using Epi.Web.MVC.Utility;
using Epi.Web.Enter.Common.DTO;
using System.Web.Configuration;
using Epi.Web.MVC.Models;
using System.Text.RegularExpressions;
namespace Epi.Web.MVC.Controllers
{
    public class AdminUserController : Controller
    {
        //
        // GET: /Organization/
        private Epi.Web.MVC.Facade.ISurveyFacade _isurveyFacade;

         public AdminUserController(Epi.Web.MVC.Facade.ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
         
        public ActionResult UserList()
            {
            int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
             
           

            UserOrgModel UserOrgModel = new UserOrgModel();
            OrganizationRequest Request = new OrganizationRequest();
            Request.UserId = UserId;
             
            OrganizationResponse Organizations = _isurveyFacade.GetUserOrganizations(Request);
            List<OrganizationModel> OrgListModel = Mapper.ToOrganizationModelList(Organizations.OrganizationList);
            UserOrgModel.OrgList = OrgListModel;

            Request.Organization.OrganizationId = Organizations.OrganizationList[0].OrganizationId;
            OrganizationResponse OrganizationUsers = _isurveyFacade.GetOrganizationUsers(Request);
            List<UserModel> UserModel = Mapper.ToUserModelList(OrganizationUsers.OrganizationUsersList);

           UserOrgModel.UserList = UserModel;
           
            return View("UserList", UserOrgModel);
            }
          
          public ActionResult UserInfo()
          {
              return View("UserInfo");
          }
  [HttpGet]
          public ActionResult GetUserList(int orgid)
         {
        
       
         OrganizationRequest Request = new OrganizationRequest();
         Request.Organization.OrganizationId = orgid;
         OrganizationResponse OrganizationUsers = _isurveyFacade.GetOrganizationUsers(Request);
         List<UserModel> UserModel = Mapper.ToUserModelList(OrganizationUsers.OrganizationUsersList);
         return PartialView("PartialUserList", UserModel);
          
       
         }
     
    }
}
