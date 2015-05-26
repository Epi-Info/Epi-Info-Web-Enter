using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Enter.Common.DTO;
using Epi.Web.Enter.Common.Message;
using Epi.Web.Enter.Common.Exception;
namespace Epi.Web.WCF.SurveyService
{
    [ServiceContract]
    public interface IEWEManagerServiceV2 : IEWEManagerService
    {
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        void UpdateRecordStatus(SurveyAnswerRequest pRequestMessage);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        FormSettingResponse GetSettings(FormSettingRequest Request);
    }
}
