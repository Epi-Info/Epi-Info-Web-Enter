using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Interfaces.DataInterface;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
namespace Epi.Web.EF
{
    public class EntityUserDao : IUserDao
    {
        public UserBO GetUser(UserBO User)
        {
            var Context = DataObjectFactory.CreateContext();
            var UserQuery = from user in Context.Users
                            where user.UserName == User.UserName && user.PasswordHash == User.PasswordHash
                            select user;
            UserBO Result = new UserBO();

            foreach (var user in UserQuery)
            {
                Mapper.MapToUserBO(Result, user);
                return Result;
            }

            return null;
        }



        public void UpdateUser(UserBO User)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(UserBO User)
        {
            throw new NotImplementedException();
        }

        public void InsertUser(UserBO User)
        {
            throw new NotImplementedException();
        }

    }
}
