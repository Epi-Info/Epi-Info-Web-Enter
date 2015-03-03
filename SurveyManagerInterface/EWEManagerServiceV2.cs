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
using Epi.Web.Enter.Common.Exception;
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
    }
}
