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
namespace Epi.Web.MVC.Controllers
{
    public class AdminOrganizationController : Controller
    {
        
        private Epi.Web.MVC.Facade.ISurveyFacade _isurveyFacade;
        public AdminOrganizationController(Epi.Web.MVC.Facade.ISurveyFacade isurveyFacade)
        {
            _isurveyFacade = isurveyFacade;
        }
        [HttpGet]
        public ActionResult OrgList( )
        {
              int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
        OrganizationRequest Request = new OrganizationRequest();
        Request.UserId = UserId;
        OrganizationResponse Organizations = _isurveyFacade.GetUserOrganizations(Request);

        var Model  = Mapper.ToOrganizationModelList(Organizations.OrganizationList);
        
                  return View("OrgList",Model);
             
        }
        
       
     [HttpGet]
        public ActionResult OrgInfo(int  orgid  , bool  iseditmode  )
        {
        OrganizationRequest Request = new OrganizationRequest();
        Request.Organization.OrganizationId = orgid;
       // OrganizationResponse Organizations = _isurveyFacade.GetOrganizationInfo(Request);
        OrgAdminInfoModel OrgInfo = new OrgAdminInfoModel();
        OrgInfo.IsEditMode = iseditmode;
        return View(OrgInfo);
        }
     [HttpPost]
     public ActionResult OrgInfo(OrgAdminInfoModel OrgAdminInfoModel)
         {
         if (ModelState.IsValid)
             {

             int UserId = SurveyHelper.GetDecryptUserId(Session["UserId"].ToString());
             OrganizationRequest Request = new OrganizationRequest();
             Request.UserId = UserId;
             OrganizationResponse Organizations = _isurveyFacade.GetUserOrganizations(Request);

             var Model = Mapper.ToOrganizationModelList(Organizations.OrganizationList);


             return View("OrgList", Model);

             }
         else
             {

             return View(OrgAdminInfoModel);
             }
       
         }

    }
}
