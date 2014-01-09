using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Interfaces.DataInterface;
using Epi.Web.Common.BusinessObject;
namespace Epi.Web.BLL
{
   public class User
    {
       public IUserDao UserDao;

       public User(IUserDao pUserDao) 
       {
           UserDao = pUserDao;
       }

       public UserBO GetUser(UserBO User) 
       {
           UserBO UserResponseBO;

           UserResponseBO =  UserDao.GetUser(User);

           return UserResponseBO;
       }
    }
}
