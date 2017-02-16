using Epi.Web.Enter.Common.DTO;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Epi.Web.Enter.Common.MessageBase;
using Epi.Web.Enter.Common.DTO;


namespace Epi.Web.Enter.Common.Message
{
     [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class SourceTablesResponse : RequestBase
    {
         public SourceTablesResponse() {
             this.List = new List<SourceTableDTO>();
         }

          
         public List<SourceTableDTO> List{get;set;}
    }
}
