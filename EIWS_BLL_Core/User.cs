using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Interfaces.DataInterface;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Security;
using System.Configuration;
using Epi.Web.Common.Constants;
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
            string KeyForUserPasswordSalt = ReadSalt();
            PasswordHasher PasswordHasher = new Web.Common.Security.PasswordHasher(KeyForUserPasswordSalt);
            string salt = PasswordHasher.CreateSalt(User.UserName);

            User.PasswordHash = PasswordHasher.HashPassword(salt, User.PasswordHash);

            UserResponseBO = UserDao.GetUser(User);

            return UserResponseBO;
        }

        private string ReadSalt()
        {
            return ConfigurationManager.AppSettings["KeyForUserPasswordSalt"];
        }

        public UserBO GetUserByUserId(UserBO User)
        {
            UserBO UserResponseBO;

            UserResponseBO = UserDao.GetUserByUserId(User);

            return UserResponseBO;
        }

        public bool UpdateUser(UserBO User)
        {
            switch (User.Operation)
            {
                case Constant.OperationMode.UpdatePassword:
                    PasswordGenerator passGen = new PasswordGenerator();
                    string password = passGen.Generate();

                    string KeyForUserPasswordSalt = ReadSalt();
                    PasswordHasher PasswordHasher = new Web.Common.Security.PasswordHasher(KeyForUserPasswordSalt);
                    string salt = PasswordHasher.CreateSalt(User.UserName);

                    User.PasswordHash = PasswordHasher.HashPassword(salt, password);
                    
                    bool success = UserDao.UpdateUser(User);
                    if (success)
                    {
                        SendEmail(User.UserName, password);
                    }
                    return success;

                case Constant.OperationMode.UpdateUserInfo:
                    break;
                case Constant.OperationMode.UpdateUser:
                    break;
                default:
                    break;
            }
            return false;
        }

        private void SendEmail(string emailAddress, string password)
        {

            Epi.Web.Common.Email.Email Email = new Web.Common.Email.Email();
            Email.Body = "Your password has been updated. Please used temporary password" + password;
            Email.From = ConfigurationManager.AppSettings["EMAIL_FROM"];
            List<string> EmailList = new List<string>();
            EmailList.Add(emailAddress);
            Email.To = EmailList;
            Email.Subject = "WEB Enter password reset.";
            bool success = Epi.Web.Common.Email.EmailHandler.SendMessage(Email);

        }
    }
}
