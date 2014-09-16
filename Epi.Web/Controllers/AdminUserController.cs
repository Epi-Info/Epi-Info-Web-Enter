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
        [HttpGet] 
        public ActionResult UserList()
            {
            UserOrgModel UserOrgModel = GetUserList();
            return View("UserList", UserOrgModel);
            }

       
        [HttpGet]
        public ActionResult UserInfo(int userid, bool iseditmode,int orgid)
            {
            UserModel UserModel = new UserModel();
            UserRequest Request = new UserRequest();
            
            if (iseditmode)
                {

               
                Request.Organization = new OrganizationDTO();
                Request.Organization.OrganizationId = orgid;

                Request.User = new UserDTO();
                Request.User.UserId = userid;

                UserResponse Response = _isurveyFacade.GetUserInfo(Request);
                UserModel = Mapper.ToUserModelR( Response.User[0]);
                UserModel.IsEditMode = true;
                return View("UserInfo", UserModel);
               }
         
            UserModel.IsActive = true;
            return View("UserInfo", UserModel);
            }
        [HttpPost]
        public ActionResult UserInfo(UserModel UserModel)
            {
            UserOrgModel UserOrgModel =   GetUserList();  
            UserResponse Response = new UserResponse();
            UserRequest Request = new UserRequest();
            int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
            try
                {
                 
                if (UserModel.IsEditMode)
                    {
                    Request.Action = "UpDate";
                   
                    Request.User = Mapper.ToUserDTO(UserModel);
                    Request.CurrentOrg = ViewBag.SelectedOrg;
                    Request.CurrentUser = UserId;
                    Response = _isurveyFacade.SetUserInfo(Request);
                    UserOrgModel = GetUserList();
                    UserOrgModel.Message = UserModel.FirstName + " " + UserModel.LastName + "has been Updated! ";
                    }
                else 
                    {
                    Request.Action = "";
                     Request.User = Mapper.ToUserDTO(UserModel);
                     Request.CurrentOrg = ViewBag.SelectedOrg;
                     Request.CurrentUser = UserId;
                     Response = _isurveyFacade.SetUserInfo(Request);
                     UserOrgModel = GetUserList();
                     UserOrgModel.Message = UserModel.FirstName + " " + UserModel.LastName + "has been added! ";
                    }
               
               
                
                }
             catch(Exception ex)
                {
                throw ex;
                 }
            return View("UserList", UserOrgModel);
            }
  [HttpGet]
          public ActionResult GetUserList(int orgid)
         {
        
       
         OrganizationRequest Request = new OrganizationRequest();
         Request.Organization.OrganizationId = orgid;
         OrganizationResponse OrganizationUsers = _isurveyFacade.GetOrganizationUsers(Request);
         List<UserModel> UserModel = Mapper.ToUserModelList(OrganizationUsers.OrganizationUsersList);
         ViewBag.SelectedOrg = orgid;
         return PartialView("PartialUserList", UserModel);
          
       
         }
  private UserOrgModel GetUserList()
      {
      int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
      UserOrgModel UserOrgModel = new UserOrgModel();
      try
          {

         
          OrganizationRequest Request = new OrganizationRequest();
          Request.UserId = UserId;

          OrganizationResponse Organizations = _isurveyFacade.GetUserOrganizations(Request);
          List<OrganizationModel> OrgListModel = Mapper.ToOrganizationModelList(Organizations.OrganizationList);
          UserOrgModel.OrgList = OrgListModel;

          Request.Organization.OrganizationId = Organizations.OrganizationList[0].OrganizationId;
          OrganizationResponse OrganizationUsers = _isurveyFacade.GetOrganizationUsers(Request);
          List<UserModel> UserModel = Mapper.ToUserModelList(OrganizationUsers.OrganizationUsersList);

          UserOrgModel.UserList = UserModel;
          ViewBag.SelectedOrg = Organizations.OrganizationList[0].OrganizationId;
          }
      catch (Exception Ex)
          {
          throw Ex;
              
          }
      return UserOrgModel;
      }
    }
}
