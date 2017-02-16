using System.Runtime.Serialization;
using System.Collections.Generic;
using Epi.Web.Enter.Common.MessageBase;
using Epi.Web.Enter.Common.Criteria;
using Epi.Web.Enter.Common.DTO;
using System;


namespace Epi.Web.Enter.Common.Message
    {
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class FormSettingRequest : Epi.Web.Enter.Common.MessageBase.RequestBase
        {
       
          public FormSettingRequest()
                {
                this.FormSetting = new List<FormSettingDTO>();
                this.FormInfo = new FormInfoDTO();
                }
         [DataMember]
            public List<FormSettingDTO> FormSetting;
         [DataMember]
         public FormInfoDTO FormInfo;
         [DataMember]
         public bool GetXml;
         [DataMember]
         public int CurrentOrgId;
         [DataMember]
         public bool FormInfoOnly;
        }
    }
