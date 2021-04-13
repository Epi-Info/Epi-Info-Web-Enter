using Epi.Web.Common.DTO;
using Epi.Web.Enter.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Epi.Web.Enter.Common.Message
{
    [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class PublishReportResponse : Epi.Web.Enter.Common.MessageBase.RequestBase
    {
        public PublishReportResponse (){

            this.Reports = new List<ReportInfoDTO>();

         }

        [DataMember]
        public List<ReportInfoDTO> Reports;
        [DataMember]
        public string Message;
    }
}
