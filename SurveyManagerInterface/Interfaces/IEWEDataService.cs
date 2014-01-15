using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Epi.Web.Common.DTO;
using Epi.Web.Common.Message;
using Epi.Web.Common.Exception;
using Epi.Web.WCF.SurveyService;

namespace Epi.Web.WCF.SurveyService
{
    [ServiceContract]
    public interface IEWEDataService
    {
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]

        SurveyInfoResponse GetSurveyInfo(SurveyInfoRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyAnswerResponse GetSurveyAnswer(SurveyAnswerRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyAnswerResponse SetSurveyAnswer(SurveyAnswerRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserAuthenticationResponse PassCodeLogin(UserAuthenticationRequest pRequest);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserAuthenticationResponse SetPassCode(UserAuthenticationRequest pRequest);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserAuthenticationResponse GetAuthenticationResponse(UserAuthenticationRequest pRequest);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]

        FormsInfoResponse GetFormsInfo(FormsInfoRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]

        FormResponseInfoResponse GetFormResponseInfo(FormResponseInfoRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyAnswerResponse GetFormResponseList(SurveyAnswerRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        FormSettingResponse GetResponseColumnNames(FormSettingRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        SurveyAnswerResponse DeleteResponse(SurveyAnswerRequest pRequest);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserAuthenticationResponse UserLogin(UserAuthenticationRequest pRequest);
        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        UserAuthenticationResponse GetUser(UserAuthenticationRequest request);

        [OperationContract]
        [FaultContract(typeof(CustomFaultException))]
        bool UpdateUser(UserAuthenticationRequest request);
    }

}
