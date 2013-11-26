using System.Collections.Generic;
using System.Runtime.Serialization;
using Epi.Web.Common.MessageBase;
using Epi.Web.Common.DTO;
namespace Epi.Web.Common.Message
    {
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class FormResponseInfoResponse
        {
           /// <summary>
        /// Default Constructor for SurveyInfoResponse.
        /// </summary>
        public FormResponseInfoResponse() 
        {
        this.FormResponseInfoList = new List<FormResponseInfoDTO>(); 
        }
        /// <summary>
        /// Single SurveyInfo
        /// </summary>
        [DataMember]
        public List<FormResponseInfoDTO> FormResponseInfoList;
        }
    }
