using System.Runtime.Serialization;
using System.Collections.Generic;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.DTO;
using System;


namespace Epi.Web.Common.Message
    {
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
   public class FormSettingRequest
        {
       
          public FormSettingRequest()
                {
                this.FormSetting = new FormSettingDTO();
                }
         [DataMember]
            public FormSettingDTO FormSetting;
 
        }
    }
