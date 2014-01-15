using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Interfaces.DataInterface;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
using Epi.Web.Common.Constants;
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



        public bool UpdateUser(UserBO User)
        {
            var Context = DataObjectFactory.CreateContext();
            switch (User.Operation)
            {
                case Constant.OperationMode.UpdatePassword:
                    var user = Context.Users.Single(a => a.UserName == User.UserName);
                    user.PasswordHash = User.PasswordHash;
                    Context.SaveChanges();
                    return true;
                case Constant.OperationMode.UpdateUserInfo:
                    break;
                case Constant.OperationMode.UpdateUser:
                    break;
                default:
                    break;
            }
            return false;
        }

        public bool DeleteUser(UserBO User)
        {
            throw new NotImplementedException();
        }

        public bool InsertUser(UserBO User)
        {
            throw new NotImplementedException();
        }
        public UserBO GetUserByUserId(UserBO User)
        {
            var Context = DataObjectFactory.CreateContext();
            var UserQuery = from Users in Context.Users
                            where Users.UserID == User.UserId
                            select Users;
            UserBO Result = new UserBO();

            foreach (var user in UserQuery)
            {
                Mapper.MapToUserBO(Result, user);
                return Result;
            }

            return null;
        }
    }
}
