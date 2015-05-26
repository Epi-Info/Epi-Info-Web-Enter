using Epi.Web.Enter.Common.Exception;
using Epi.Web.BLL;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using Epi.Web.Enter.Common.Security;
using System;
using Epi.Web.Enter.Common.DTO;
using Epi.Web.Enter.Common.Message;
using Epi.Web.Enter.Common.MessageBase;
using Epi.Web.Enter.Common.Criteria;
using Epi.Web.Enter.Common.ObjectMapping;
using Epi.Web.Enter.Common.BusinessObject;

namespace Epi.Web.WCF.SurveyService
{
    public class EWEManagerServiceV2 : EWEManagerService, IEWEManagerServiceV2
    {
        public void UpdateRecordStatus(SurveyAnswerRequest pRequestMessage)
        {
            try
            {
                Epi.Web.Enter.Interfaces.DataInterfaces.ISurveyResponseDao SurveyResponseDao = new EF.EntitySurveyResponseDao();
                Epi.Web.BLL.SurveyResponse Implementation = new Epi.Web.BLL.SurveyResponse(SurveyResponseDao);
                foreach (SurveyAnswerDTO DTO in pRequestMessage.SurveyAnswerList)
                {
                    Implementation.UpdateRecordStatus(Mapper.ToBusinessObject(DTO));
                }

            }
            catch (Exception ex)
            {

                throw ex;


            }
        }


        public FormSettingResponse GetSettings(FormSettingRequest pRequest) 
        {
        FormSettingResponse Response = new FormSettingResponse();
        try
        {
            Epi.Web.Enter.Interfaces.DataInterfaces.IDaoFactory entityDaoFactory = new EF.EntityDaoFactory();


            Epi.Web.Enter.Interfaces.DataInterface.IFormInfoDao FormInfoDao = entityDaoFactory.FormInfoDao;
            Epi.Web.BLL.FormInfo FormInfoImplementation = new Epi.Web.BLL.FormInfo(FormInfoDao);
            FormInfoBO FormInfoBO = FormInfoImplementation.GetFormInfoByFormId(pRequest.FormInfo.FormId, pRequest.GetXml, pRequest.FormInfo.UserId);
            Response.FormInfo = Mapper.ToFormInfoDTO(FormInfoBO);


            Epi.Web.Enter.Interfaces.DataInterface.IFormSettingDao IFormSettingDao = entityDaoFactory.FormSettingDao;
            Epi.Web.Enter.Interfaces.DataInterface.IUserDao IUserDao = entityDaoFactory.UserDao;
            Epi.Web.Enter.Interfaces.DataInterface.IFormInfoDao IFormInfoDao = entityDaoFactory.FormInfoDao;
            Epi.Web.BLL.FormSetting SettingsImplementation = new Epi.Web.BLL.FormSetting(IFormSettingDao, IUserDao, IFormInfoDao);
            Response.FormSetting = Mapper.ToDataTransferObject(SettingsImplementation.GetFormSettings(pRequest.FormInfo.FormId.ToString(), FormInfoBO.Xml));

        }
        catch (Exception ex)
        {

            throw ex;


        }

                return Response;
        
        
        }
    }
}
