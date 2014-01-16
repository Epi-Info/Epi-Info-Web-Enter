using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.DTO;

namespace Epi.Web.EF
{
    /// <summary>
    /// Maps Entity Framework entities to business objects and vice versa.
    /// </summary>
    public class Mapper
    {
        /// <summary>
        /// Maps SurveyMetaData entity to SurveyInfoBO business object.
        /// </summary>
        /// <param name="entity">A SurveyMetaData entity to be transformed.</param>
        /// <returns>A SurveyInfoBO business object.</returns>
        internal static SurveyInfoBO Map(SurveyMetaData entity)
        {
            SurveyInfoBO result =  new SurveyInfoBO();
            
                result.SurveyId = entity.SurveyId.ToString();
                result.SurveyName = entity.SurveyName;
                result.SurveyNumber = entity.SurveyNumber;
                result.XML = entity.TemplateXML;
                result.IntroductionText = entity.IntroductionText;
                result.ExitText = entity.ExitText;
                result.OrganizationName = entity.OrganizationName;
                result.DepartmentName = entity.DepartmentName;
                result.ClosingDate = entity.ClosingDate;
                result.TemplateXMLSize = (long) entity.TemplateXMLSize;
                result.DateCreated = entity.DateCreated;
                result.IsDraftMode = entity.IsDraftMode;
                result.StartDate = entity.StartDate;

                if (entity.UserPublishKey != null)
                    {
                    // result.UserPublishKey = (Guid)entity.UserPublishKey.Value;
                    result.UserPublishKey = entity.UserPublishKey;
                    }
                result.SurveyType = entity.SurveyTypeId; 
            


            return result;
        }

        public static FormInfoBO ToFormInfoBO(SurveyMetaData entity)
            {
            return new FormInfoBO
            {
                FormId =  entity.SurveyId.ToString(),
                FormNumber = entity.SurveyNumber,
                FormName = entity.SurveyName,
                OrganizationName = entity.OrganizationName,
                OrganizationId = entity.OrganizationId,
                IsDraftMode = entity.IsDraftMode,
                UserId = entity.OwnerId,


            };
            }

        /// <summary>
        /// Maps the Entity User to BO
        /// </summary>
        /// <param name="Result"></param>
        /// <param name="user"></param>
        public static void MapToUserBO(UserBO Result, User user)
        {
            Result.UserId = user.UserID;
            Result.UserName = user.UserName;
            Result.EmailAddress = user.EmailAddress;
            Result.FirstName = user.FirstName;
            Result.LastName = user.LastName;
            Result.PhoneNumber = user.PhoneNumber;
            Result.ResetPassword = user.ResetPassword;
            //Result.Role = user.role
        }



        /// <summary>
        /// Maps SurveyMetaData entity to FormInfoBO business object.
        /// </summary>
        /// <param name="entity">A SurveyMetaData entity to be transformed.</param>
        /// <returns>A FormInfoBO business object.</returns>
        internal static FormInfoBO MapToFormInfoBO(SurveyMetaData entity,User UserEntity,bool GetXml=false )
            {
            FormInfoBO result = new FormInfoBO();

            result.FormId = entity.SurveyId.ToString();
            result.FormName = entity.SurveyName;
            result.FormNumber = entity.SurveyNumber;
            result.OrganizationName = entity.OrganizationName;
            result.OrganizationId = entity.OrganizationId;
            result.IsDraftMode = entity.IsDraftMode;
            result.UserId = entity.OwnerId;
            result.OwnerFName = UserEntity.FirstName;
            result.OwnerLName = UserEntity.LastName;
            if (GetXml)
                {
                result.Xml = entity.TemplateXML;
                }
             return result;
            }

        internal static List<SurveyInfoBO> Map(List<SurveyMetaData> entities)
        {
            List<SurveyInfoBO> result = new List<SurveyInfoBO>();
            foreach (SurveyMetaData surveyMetaData in entities)
            {
                result.Add(Map(surveyMetaData));
            }

            return result;
        }

        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyMetaData entity.
        /// </summary>
        /// <param name="businessobject">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyMetaData entity.</returns>
        internal static SurveyMetaData Map(SurveyInfoBO businessobject)
        {
            return new SurveyMetaData
            {
                SurveyId = new Guid(businessobject.SurveyId),
                SurveyName = businessobject.SurveyName,
                SurveyNumber = businessobject.SurveyNumber,
                TemplateXML = businessobject.XML,
                IntroductionText = businessobject.IntroductionText,
                ExitText = businessobject.ExitText,
                OrganizationName = businessobject.OrganizationName,
                DepartmentName = businessobject.DepartmentName,
                ClosingDate = businessobject.ClosingDate ,
                UserPublishKey=businessobject.UserPublishKey,
                SurveyTypeId = businessobject.SurveyType,
                TemplateXMLSize = businessobject.TemplateXMLSize,
                DateCreated = businessobject.DateCreated,
                IsDraftMode = businessobject.IsDraftMode,
                StartDate = businessobject.StartDate,
               
               
                

            };
        }


        /// <summary>
        /// Maps SurveyMetaData entity to SurveyInfoBO business object.
        /// </summary>
        /// <param name="entity">A SurveyMetaData entity to be transformed.</param>
        /// <returns>A SurveyInfoBO business object.</returns>
        internal static SurveyResponseBO Map(SurveyAnswerDTO entity)
        {
            return new SurveyResponseBO
            {
                SurveyId = entity.SurveyId.ToString(),
                ResponseId = entity.ResponseId.ToString(),
                XML = entity.XML,
                Status = entity.Status,
                UserPublishKey = entity.UserPublishKey,
                DateUpdated = entity.DateUpdated,
                DateCompleted = entity.DateCompleted
            };
        }

        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyMetaData entity.
        /// </summary>
        /// <param name="businessobject">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyMetaData entity.</returns>
        internal static SurveyAnswerDTO Map(SurveyResponseBO businessobject)
        {
            return new SurveyAnswerDTO
            {
                SurveyId = businessobject.SurveyId,
                ResponseId = businessobject.ResponseId,
                XML = businessobject.XML,
                Status = businessobject.Status,
                UserPublishKey = businessobject.UserPublishKey,
                DateUpdated = businessobject.DateUpdated,
                DateCompleted = businessobject.DateCompleted

            };
        }
         
        /// <summary>
        /// Maps SurveyMetaData entity to SurveyInfoBO business object.
        /// </summary>
        /// <param name="entity">A SurveyMetaData entity to be transformed.</param>
        /// <returns>A SurveyInfoBO business object.</returns>
        internal static SurveyResponseBO Map(SurveyResponse entity)
        {
        SurveyResponseBO SurveyResponseBO = new SurveyResponseBO();
           
                SurveyResponseBO.SurveyId = entity.SurveyId.ToString();
                SurveyResponseBO.ResponseId = entity.ResponseId.ToString();
                SurveyResponseBO.XML = entity.ResponseXML;
                SurveyResponseBO.Status = entity.StatusId;
                SurveyResponseBO.DateUpdated = entity.DateUpdated;
                SurveyResponseBO.DateCompleted = entity.DateCompleted;
                SurveyResponseBO.TemplateXMLSize = (long)entity.ResponseXMLSize;
                SurveyResponseBO.DateCreated = entity.DateCreated;
                SurveyResponseBO.IsDraftMode = entity.IsDraftMode ;
                SurveyResponseBO.IsLocked = entity.IsLocked;
            if (entity.ParentRecordId != null)
                {
                        SurveyResponseBO.ParentRecordId = entity.ParentRecordId.ToString();
                }
            
            return SurveyResponseBO;
        }

        internal static List<SurveyResponseBO> Map(List<SurveyResponse> entities)
        {
            List<SurveyResponseBO> result = new List<SurveyResponseBO>();
            foreach (SurveyResponse surveyResponse in entities)
            {
                result.Add(Map(surveyResponse));
            }

            return result;
        }
        internal static OrganizationBO Map(Organization entity)
        {
            return new OrganizationBO
            {
                Organization = entity.Organization1,
                IsEnabled = entity.IsEnabled,
                OrganizationKey = entity.OrganizationKey


            };
        }
        internal static OrganizationBO Map(string Organization)
        {
            return new OrganizationBO
            {
                Organization = Organization
                 


            };
        }
        internal static Organization ToEF(OrganizationBO pBo)
        {
            return new Organization
            {
                Organization1 = pBo.Organization,
                IsEnabled = pBo.IsEnabled,
                OrganizationKey = pBo.OrganizationKey,


            };
        }
        /// <summary>
        /// Maps SurveyInfoBO business object to SurveyMetaData entity.
        /// </summary>
        /// <param name="businessobject">A SurveyInfoBO business object.</param>
        /// <returns>A SurveyMetaData entity.</returns>
        internal static SurveyResponse ToEF(SurveyResponseBO pBO)
        {

                SurveyResponse SurveyResponse = new SurveyResponse();
             
                SurveyResponse.SurveyId = new Guid(pBO.SurveyId);
                SurveyResponse.ResponseId = new Guid(pBO.ResponseId);
                SurveyResponse.ResponseXML = pBO.XML;
                SurveyResponse.StatusId = pBO.Status;
                SurveyResponse.ResponseXMLSize = pBO.TemplateXMLSize;
                SurveyResponse.DateUpdated = pBO.DateUpdated;
                SurveyResponse.DateCompleted = pBO.DateCompleted;
                SurveyResponse.DateCreated = pBO.DateCreated;
                SurveyResponse.IsDraftMode = pBO.IsDraftMode;
                
                if (!string.IsNullOrEmpty(pBO.ParentRecordId))
                {
                SurveyResponse.ParentRecordId = new Guid(pBO.ParentRecordId);
                }
               return  SurveyResponse;
        }
        internal static UserAuthenticationResponseBO ToAuthenticationResponseBO(UserAuthenticationRequestBO AuthenticationRequestBO)
        {


            return new UserAuthenticationResponseBO
            {
                PassCode = AuthenticationRequestBO.PassCode,

            };
        
        }
    }
}
