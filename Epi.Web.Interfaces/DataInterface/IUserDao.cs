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
        bool UpdateUser(UserBO User);
        bool DeleteUser(UserBO User);
        bool InsertUser(UserBO User);
        UserBO GetUserByUserId(UserBO User);
        bool UpdateUserPassword(UserBO User);
        bool UpdateUserInfo(UserBO User);
        List<UserBO> GetUserByFormId(string FormId);
        UserBO GetCurrentUser(int userId);

    }
}
