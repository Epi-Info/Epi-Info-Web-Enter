using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;

namespace Epi.Web.Interfaces.DataInterface
{
    public interface IUserDao
    {
        UserBO GetUser(UserBO User);
        void UpdateUser(UserBO User);
        void DeleteUser(UserBO User);
        void InsertUser(UserBO User);
    }
}
