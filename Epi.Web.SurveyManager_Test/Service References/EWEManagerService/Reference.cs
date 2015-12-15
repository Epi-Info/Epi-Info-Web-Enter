﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Epi.Web.SurveyManager.Client.EWEManagerService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="EWEManagerService.IEWEManagerService")]
    public interface IEWEManagerService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/PublishSurvey", ReplyAction="http://tempuri.org/IEWEManagerService/PublishSurveyResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/PublishSurveyCustomFaultExceptionFault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.PublishResponse PublishSurvey(Epi.Web.Enter.Common.Message.PublishRequest pRequestMessage);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/GetSurveyInfo", ReplyAction="http://tempuri.org/IEWEManagerService/GetSurveyInfoResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/GetSurveyInfoCustomFaultExceptionFault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.SurveyInfoResponse GetSurveyInfo(Epi.Web.Enter.Common.Message.SurveyInfoRequest pRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/GetOrganization", ReplyAction="http://tempuri.org/IEWEManagerService/GetOrganizationResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/GetOrganizationCustomFaultExceptionFault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.OrganizationResponse GetOrganization(Epi.Web.Enter.Common.Message.OrganizationRequest pRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/GetOrganizationInfo", ReplyAction="http://tempuri.org/IEWEManagerService/GetOrganizationInfoResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/GetOrganizationInfoCustomFaultExceptionFaul" +
            "t", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.OrganizationResponse GetOrganizationInfo(Epi.Web.Enter.Common.Message.OrganizationRequest pRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/GetOrganizationNames", ReplyAction="http://tempuri.org/IEWEManagerService/GetOrganizationNamesResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/GetOrganizationNamesCustomFaultExceptionFau" +
            "lt", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.OrganizationResponse GetOrganizationNames(Epi.Web.Enter.Common.Message.OrganizationRequest pRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/GetOrganizationByKey", ReplyAction="http://tempuri.org/IEWEManagerService/GetOrganizationByKeyResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/GetOrganizationByKeyCustomFaultExceptionFau" +
            "lt", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.OrganizationResponse GetOrganizationByKey(Epi.Web.Enter.Common.Message.OrganizationRequest pRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/SetSurveyInfo", ReplyAction="http://tempuri.org/IEWEManagerService/SetSurveyInfoResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/SetSurveyInfoCustomFaultExceptionFault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.SurveyInfoResponse SetSurveyInfo(Epi.Web.Enter.Common.Message.SurveyInfoRequest pRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/GetSurveyAnswer", ReplyAction="http://tempuri.org/IEWEManagerService/GetSurveyAnswerResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/GetSurveyAnswerCustomFaultExceptionFault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.SurveyAnswerResponse GetSurveyAnswer(Epi.Web.Enter.Common.Message.SurveyAnswerRequest pRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/SetOrganization", ReplyAction="http://tempuri.org/IEWEManagerService/SetOrganizationResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/SetOrganizationCustomFaultExceptionFault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.OrganizationResponse SetOrganization(Epi.Web.Enter.Common.Message.OrganizationRequest pRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/UpdateOrganizationInfo", ReplyAction="http://tempuri.org/IEWEManagerService/UpdateOrganizationInfoResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/UpdateOrganizationInfoCustomFaultExceptionF" +
            "ault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.OrganizationResponse UpdateOrganizationInfo(Epi.Web.Enter.Common.Message.OrganizationRequest pRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/IsValidOrgKey", ReplyAction="http://tempuri.org/IEWEManagerService/IsValidOrgKeyResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/IsValidOrgKeyCustomFaultExceptionFault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        bool IsValidOrgKey(Epi.Web.Enter.Common.Message.SurveyInfoRequest pRequest);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/RePublishSurvey", ReplyAction="http://tempuri.org/IEWEManagerService/RePublishSurveyResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/RePublishSurveyCustomFaultExceptionFault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.PublishResponse RePublishSurvey(Epi.Web.Enter.Common.Message.PublishRequest pRequestMessage);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/UserLogin", ReplyAction="http://tempuri.org/IEWEManagerService/UserLoginResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/UserLoginCustomFaultExceptionFault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.UserAuthenticationResponse UserLogin(Epi.Web.Enter.Common.Message.UserAuthenticationRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/UpdateUser", ReplyAction="http://tempuri.org/IEWEManagerService/UpdateUserResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/UpdateUserCustomFaultExceptionFault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        bool UpdateUser(Epi.Web.Enter.Common.Message.UserAuthenticationRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/GetUser", ReplyAction="http://tempuri.org/IEWEManagerService/GetUserResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/GetUserCustomFaultExceptionFault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.UserAuthenticationResponse GetUser(Epi.Web.Enter.Common.Message.UserAuthenticationRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEWEManagerService/SetSurveyAnswer", ReplyAction="http://tempuri.org/IEWEManagerService/SetSurveyAnswerResponse")]
        [System.ServiceModel.FaultContractAttribute(typeof(Epi.Web.Enter.Common.Exception.CustomFaultException), Action="http://tempuri.org/IEWEManagerService/SetSurveyAnswerCustomFaultExceptionFault", Name="CustomFaultException", Namespace="http://www.yourcompany.com/types/")]
        Epi.Web.Enter.Common.Message.PreFilledAnswerResponse SetSurveyAnswer(Epi.Web.Enter.Common.Message.PreFilledAnswerRequest pRequestMessage);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEWEManagerServiceChannel : Epi.Web.SurveyManager.Client.EWEManagerService.IEWEManagerService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EWEManagerServiceClient : System.ServiceModel.ClientBase<Epi.Web.SurveyManager.Client.EWEManagerService.IEWEManagerService>, Epi.Web.SurveyManager.Client.EWEManagerService.IEWEManagerService {
        
        public EWEManagerServiceClient() {
        }
        
        public EWEManagerServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EWEManagerServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EWEManagerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EWEManagerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Epi.Web.Enter.Common.Message.PublishResponse PublishSurvey(Epi.Web.Enter.Common.Message.PublishRequest pRequestMessage) {
            return base.Channel.PublishSurvey(pRequestMessage);
        }
        
        public Epi.Web.Enter.Common.Message.SurveyInfoResponse GetSurveyInfo(Epi.Web.Enter.Common.Message.SurveyInfoRequest pRequest) {
            return base.Channel.GetSurveyInfo(pRequest);
        }
        
        public Epi.Web.Enter.Common.Message.OrganizationResponse GetOrganization(Epi.Web.Enter.Common.Message.OrganizationRequest pRequest) {
            return base.Channel.GetOrganization(pRequest);
        }
        
        public Epi.Web.Enter.Common.Message.OrganizationResponse GetOrganizationInfo(Epi.Web.Enter.Common.Message.OrganizationRequest pRequest) {
            return base.Channel.GetOrganizationInfo(pRequest);
        }
        
        public Epi.Web.Enter.Common.Message.OrganizationResponse GetOrganizationNames(Epi.Web.Enter.Common.Message.OrganizationRequest pRequest) {
            return base.Channel.GetOrganizationNames(pRequest);
        }
        
        public Epi.Web.Enter.Common.Message.OrganizationResponse GetOrganizationByKey(Epi.Web.Enter.Common.Message.OrganizationRequest pRequest) {
            return base.Channel.GetOrganizationByKey(pRequest);
        }
        
        public Epi.Web.Enter.Common.Message.SurveyInfoResponse SetSurveyInfo(Epi.Web.Enter.Common.Message.SurveyInfoRequest pRequest) {
            return base.Channel.SetSurveyInfo(pRequest);
        }
        
        public Epi.Web.Enter.Common.Message.SurveyAnswerResponse GetSurveyAnswer(Epi.Web.Enter.Common.Message.SurveyAnswerRequest pRequest) {
            return base.Channel.GetSurveyAnswer(pRequest);
        }
        
        public Epi.Web.Enter.Common.Message.OrganizationResponse SetOrganization(Epi.Web.Enter.Common.Message.OrganizationRequest pRequest) {
            return base.Channel.SetOrganization(pRequest);
        }
        
        public Epi.Web.Enter.Common.Message.OrganizationResponse UpdateOrganizationInfo(Epi.Web.Enter.Common.Message.OrganizationRequest pRequest) {
            return base.Channel.UpdateOrganizationInfo(pRequest);
        }
        
        public bool IsValidOrgKey(Epi.Web.Enter.Common.Message.SurveyInfoRequest pRequest) {
            return base.Channel.IsValidOrgKey(pRequest);
        }
        
        public Epi.Web.Enter.Common.Message.PublishResponse RePublishSurvey(Epi.Web.Enter.Common.Message.PublishRequest pRequestMessage) {
            return base.Channel.RePublishSurvey(pRequestMessage);
        }
        
        public Epi.Web.Enter.Common.Message.UserAuthenticationResponse UserLogin(Epi.Web.Enter.Common.Message.UserAuthenticationRequest request) {
            return base.Channel.UserLogin(request);
        }
        
        public bool UpdateUser(Epi.Web.Enter.Common.Message.UserAuthenticationRequest request) {
            return base.Channel.UpdateUser(request);
        }
        
        public Epi.Web.Enter.Common.Message.UserAuthenticationResponse GetUser(Epi.Web.Enter.Common.Message.UserAuthenticationRequest request) {
            return base.Channel.GetUser(request);
        }
        
        public Epi.Web.Enter.Common.Message.PreFilledAnswerResponse SetSurveyAnswer(Epi.Web.Enter.Common.Message.PreFilledAnswerRequest pRequestMessage) {
            return base.Channel.SetSurveyAnswer(pRequestMessage);
        }
    }
}
