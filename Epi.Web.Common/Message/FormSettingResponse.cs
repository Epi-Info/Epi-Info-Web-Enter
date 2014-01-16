using System.Runtime.Serialization;
using System.Collections.Generic;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.DTO;

namespace Epi.Web.Common.Message
    {
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
   public class FormSettingResponse
        {

         public FormSettingResponse()
        {
        this.FormSetting = new FormSettingDTO();
        this.FormInfo = new FormInfoDTO();
        }
         [DataMember]
         public FormSettingDTO FormSetting;

         [DataMember]
         public FormInfoDTO FormInfo;
        }
    }
