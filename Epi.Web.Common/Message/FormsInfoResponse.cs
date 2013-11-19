﻿using System.Collections.Generic;
using System.Runtime.Serialization;

using Epi.Web.Common.MessageBase;
using Epi.Web.Common.DTO;

namespace Epi.Web.Common.Message
    {
    /// <summary>
    /// Represents a SurveyInfo response message to client
    /// </summary>    
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
   public class FormsInfoResponse
        {
        /// <summary>
        /// Single SurveyInfo
        /// </summary>
        [DataMember]
        public List<FormInfoDTO> SurveyInfoList;
        }
    }
