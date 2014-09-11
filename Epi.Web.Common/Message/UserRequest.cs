using System.Runtime.Serialization;
using Epi.Web.Enter.Common.MessageBase;
using Epi.Web.Enter.Common.DTO;

namespace Epi.Web.Enter.Common.Message
    {
   public class UserRequest
        {
       [DataMember]
        public UserDTO User;
       [DataMember]
       public OrganizationDTO  Organization;
        }
    }
