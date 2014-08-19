using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Epi.Web.MVC;
namespace Epi.Web.MVC.Models
{
    /// <summary>
    /// Maps DTO object to Model object or Model object to DTO object
    /// </summary>
    public static class Mapper
    {



        /// <summary>
        /// Maps FormInfo DTO to FormInfo Model
        /// </summary>
        /// <param name="FormInfoDTO"></param>
        /// <returns></returns>
        public static FormInfoModel ToFormInfoModel(this Epi.Web.Enter.Common.DTO.FormInfoDTO FormInfoDTO)
        {
            return new FormInfoModel
            {
                FormId = FormInfoDTO.FormId,
                FormName = FormInfoDTO.FormName,
                FormNumber = FormInfoDTO.FormNumber,
                IsDraftMode = FormInfoDTO.IsDraftMode,
                OrganizationId = FormInfoDTO.OrganizationId,
                OrganizationName = FormInfoDTO.OrganizationName,
                UserId = FormInfoDTO.UserId,
                IsOwner = FormInfoDTO.IsOwner,
                OwnerFName =FormInfoDTO.OwnerFName,
                OwnerLName = FormInfoDTO.OwnerLName
            };
        }

        /// <summary>
        /// Maps SurveyInfo DTO to SurveyInfo Model.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>


        public static SurveyInfoModel ToSurveyInfoModel(this Epi.Web.Enter.Common.DTO.SurveyInfoDTO SurveyInfoDTO)
        {
            return new SurveyInfoModel
            {
                SurveyId = SurveyInfoDTO.SurveyId,
                SurveyNumber = SurveyInfoDTO.SurveyNumber,
                SurveyName = SurveyInfoDTO.SurveyName,
                OrganizationName = SurveyInfoDTO.OrganizationName,
                DepartmentName = SurveyInfoDTO.DepartmentName,
                IntroductionText = SurveyInfoDTO.IntroductionText,
                ExitText = SurveyInfoDTO.ExitText,
                XML = SurveyInfoDTO.XML,
                IsSuccess = SurveyInfoDTO.IsSuccess,
                SurveyType = SurveyInfoDTO.SurveyType,
                ClosingDate = SurveyInfoDTO.ClosingDate,
                UserPublishKey = SurveyInfoDTO.UserPublishKey,
                IsDraftMode = SurveyInfoDTO.IsDraftMode,
                StartDate = SurveyInfoDTO.StartDate,

            };

        }

        /// <summary>
        /// Maps SurveyInfo Model to SurveyInfo DTO.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>

        public static Epi.Web.Enter.Common.DTO.SurveyInfoDTO ToSurveyInfoDTO(SurveyInfoModel SurveyInfoModel)
        {
            return new Epi.Web.Enter.Common.DTO.SurveyInfoDTO
            {
                SurveyId = SurveyInfoModel.SurveyId,
                SurveyNumber = SurveyInfoModel.SurveyNumber,
                SurveyName = SurveyInfoModel.SurveyName,
                OrganizationName = SurveyInfoModel.OrganizationName,
                DepartmentName = SurveyInfoModel.DepartmentName,
                IntroductionText = SurveyInfoModel.IntroductionText,
                ExitText = SurveyInfoModel.ExitText,
                XML = SurveyInfoModel.XML,
                IsSuccess = SurveyInfoModel.IsSuccess,
                ClosingDate = SurveyInfoModel.ClosingDate,
                UserPublishKey = SurveyInfoModel.UserPublishKey,
                IsDraftMode = SurveyInfoModel.IsDraftMode,
                StartDate = SurveyInfoModel.StartDate,

            };
        }


        public static SurveyAnswerModel ToSurveyAnswerModel(this Epi.Web.Enter.Common.DTO.SurveyAnswerDTO SurveyAnswerDTO)
        {
            return new SurveyAnswerModel
            {
                ResponseId = SurveyAnswerDTO.ResponseId,
                SurveyId = SurveyAnswerDTO.SurveyId,
                DateUpdated = SurveyAnswerDTO.DateUpdated,
                DateCompleted = SurveyAnswerDTO.DateCompleted,
                Status = SurveyAnswerDTO.Status,
                XML = SurveyAnswerDTO.XML
            };
        }

        /// <summary>
        /// Maps SurveyInfo Model to SurveyInfo DTO.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>

        public static Epi.Web.Enter.Common.DTO.SurveyAnswerDTO ToSurveyAnswerDTO(SurveyAnswerModel SurveyAnswerModel)
        {
            return new Epi.Web.Enter.Common.DTO.SurveyAnswerDTO
            {
                ResponseId = SurveyAnswerModel.ResponseId,
                SurveyId = SurveyAnswerModel.SurveyId,
                DateUpdated = SurveyAnswerModel.DateUpdated,
                DateCompleted = SurveyAnswerModel.DateCompleted,
                Status = SurveyAnswerModel.Status,
                XML = SurveyAnswerModel.XML
            };
        }

        public static Epi.Web.Enter.Common.Message.UserAuthenticationRequest ToUserAuthenticationObj(Epi.Web.Enter.Common.DTO.PassCodeDTO pDTO)
        {
            return new Epi.Web.Enter.Common.Message.UserAuthenticationRequest
            {
                SurveyResponseId = pDTO.ResponseId,
                PassCode = pDTO.PassCode

            };
        }


        internal static List<RelateModel> ToRelateModel(List<Enter.Common.DTO.FormsHierarchyDTO> FormsHierarchy, string FormId)
            {
            List<RelateModel> List = new List<RelateModel>();
           
            // Common.DTO.FormsHierarchyDTO FormsHierarchyDTO = FormsHierarchy.Single(X => X.FormId == FormId);
           foreach(var Obj in FormsHierarchy)
               {
               RelateModel RelateModel = new RelateModel();
               RelateModel.FormId = Obj.FormId;
               RelateModel.ViewId = Obj.ViewId;
               RelateModel.ResponseIds = Mapper.ToSurveyAnswerModel(Obj.ResponseIds);
             List.Add(RelateModel);
               }
           return List;
            }

        internal static List<SurveyAnswerModel> ToSurveyAnswerModel(List<Enter.Common.DTO.SurveyAnswerDTO> list)
            {
            List<SurveyAnswerModel> ModelList = new List<SurveyAnswerModel>();
            foreach(var Obj in list)
                {
             SurveyAnswerModel SurveyAnswerModel = new Models.SurveyAnswerModel();
                SurveyAnswerModel.ResponseId = Obj.ResponseId;
                SurveyAnswerModel.SurveyId = Obj.SurveyId;
                SurveyAnswerModel.DateUpdated = Obj.DateUpdated;
                SurveyAnswerModel.DateCompleted = Obj.DateCompleted;
                SurveyAnswerModel.Status = Obj.Status;
                SurveyAnswerModel.XML = Obj.XML;
                SurveyAnswerModel.ParentRecordId = Obj.ParentRecordId;
                SurveyAnswerModel.RelateParentId = Obj.RelateParentId;
                ModelList.Add(SurveyAnswerModel);
              }
            return ModelList;
            }

        internal static  OrganizationModel  ToOrganizationModel ( Enter.Common.DTO.OrganizationDTO  DTO)
            {
             OrganizationModel  ModelList = new  OrganizationModel();

             ModelList.IsEnabled = DTO.IsEnabled;
             ModelList.Organization = DTO.Organization;
             ModelList.OrganizationId = DTO.OrganizationId;

            return ModelList;
            }

        internal static List<OrganizationModel> ToOrganizationModelList(List<Enter.Common.DTO.OrganizationDTO> list)
            {
            List<OrganizationModel> ModelList = new List<OrganizationModel>();

            foreach (var item in list)
                {
                ModelList.Add(Mapper.ToOrganizationModel(item));
                }

            return ModelList;
            }
    }
}