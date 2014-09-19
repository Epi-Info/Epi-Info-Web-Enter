using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Interfaces.DataInterface;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Security;
using System.Configuration;
using Epi.Web.Enter.Common.Constants;
using Epi.Web.Enter.Common.Email;

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
            PasswordHasher PasswordHasher = new Web.Enter.Common.Security.PasswordHasher(KeyForUserPasswordSalt);
            string salt = PasswordHasher.CreateSalt(User.UserName);

            User.PasswordHash = PasswordHasher.HashPassword(salt, User.PasswordHash);

            UserResponseBO = UserDao.GetUser(User);
            UserResponseBO.Role = UserDao.GetUserHighestRole(UserResponseBO.UserId);

            return UserResponseBO;
        }
        public bool GetExistingUser(UserBO User)
            {
            bool Exists = false;
            Exists = UserDao.GetExistingUser(User);

            return Exists;
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

        public bool UpdateUser(UserBO User,OrganizationBO OrgBO)
        {
        bool success = false;
            switch (User.Operation)
            {
                case Constant.OperationMode.UpdatePassword:
                    string password = string.Empty;

                    if (User.ResetPassword)
                    {
                        password = User.PasswordHash;
                        User.ResetPassword = false;
                    }
                    else
                    {
                        PasswordGenerator passGen = new PasswordGenerator();
                        password = passGen.Generate();
                        User.ResetPassword = true;
                    }


                    string KeyForUserPasswordSalt = ReadSalt();
                    PasswordHasher PasswordHasher = new Web.Enter.Common.Security.PasswordHasher(KeyForUserPasswordSalt);
                    string salt = PasswordHasher.CreateSalt(User.UserName);

                    User.PasswordHash = PasswordHasher.HashPassword(salt, password);
                      success = UserDao.UpdateUserPassword(User);

                    if (success)
                    {
                        List<string> EmailList = new List<string>();
                        EmailList.Add(User.UserName);
                        Email email = new Email()
                        {
                            To = EmailList,
                            Password = password
                        };

                        if (User.ResetPassword)
                        {
                            success = SendEmail(email, Constant.EmailCombinationEnum.ResetPassword);
                        }
                        else
                        {
                            success = SendEmail(email, Constant.EmailCombinationEnum.PasswordChanged);
                        }

                    }
                    return success;

                case Constant.OperationMode.UpdateUserInfo:
                    success = UserDao.UpdateUserInfo(User, OrgBO);
                    if (success)
                        {
                        List<string> EmailList = new List<string>();
                        EmailList.Add(User.UserName);
                        Email email = new Email();
                          success = SendEmail(email, Constant.EmailCombinationEnum.UpdateUserInfo);
                  

                        }
                      return success;
                    
                default:
                    break;
            }
            return false;
        }

        private bool SendEmail(Email email, Constant.EmailCombinationEnum Combination)
        {

         //   Epi.Web.Enter.Common.Email.Email Email = new Web.Common.Email.Email();

            switch (Combination)
            {
                case Constant.EmailCombinationEnum.ResetPassword:
                    email.Subject = "Your Epi Web Enter Password";
                    email.Body = string.Format("You recently accessed our Forgot Password service for  Epi Web Enter. \n \n Your new temporary password is: {0}\n \n If you have not accessed password help, please contact the administrator. \n \n Please click the link below and log in with your temporary password. You will then be asked to create a new password.", email.Password);
                    break;
                case Constant.EmailCombinationEnum.PasswordChanged:
                    email.Subject = "Your Epi Web Enter Password has been updated";
                    email.Body = " You recently updated your password for Epi Web Enter. \n \n If you have not accessed password help, please contact the administrator for you organization. \n \n Please click the link below to launch Epi Web Enter.";
                    break;
                case Constant.EmailCombinationEnum.UpdateUserInfo:
                    email.Subject = "Your Epi Web Enter Account info has been updated";
                    email.Body = " You account info has been updated in Epi Web Enter system.";
                    break;
                case Constant.EmailCombinationEnum.InsertUser:
                    email.Subject = "Epi Web Enter account.";
                
                    break;
                default:
                    break;
            }

            email.Body = email.Body.ToString() + " \n \n" + ConfigurationManager.AppSettings["BaseURL"];
            email.From = ConfigurationManager.AppSettings["EMAIL_FROM"];

            return Epi.Web.Enter.Common.Email.EmailHandler.SendMessage(email);

        }

        public UserBO GetUserByEmail(UserBO User)
            {
            UserBO UserResponseBO;

            UserResponseBO = UserDao.GetUserByEmail(User);

            return UserResponseBO;
            }

        public List<UserBO> GetUsersByOrgId(int OrgId)
            {
            List<UserBO> List = new List<UserBO>();
            List = UserDao.GetUserByOrgId(OrgId);
            return List;
            }

        public UserBO GetUserByUserIdAndOrgId(UserBO UserBO, OrganizationBO OrgBO)
            {
            UserBO UserResponseBO;

            UserResponseBO = UserDao.GetUserByUserIdAndOrgId(UserBO, OrgBO);

            return UserResponseBO;
            }
        public bool SetUserInfo(UserBO UserBO, OrganizationBO OrgBO)
            {
            bool success;
             string KeyForUserPasswordSalt = ReadSalt();
                    PasswordHasher PasswordHasher = new Web.Enter.Common.Security.PasswordHasher(KeyForUserPasswordSalt);
                    string salt = PasswordHasher.CreateSalt(UserBO.EmailAddress);
                    UserBO.ResetPassword = true;
                    UserBO.PasswordHash = PasswordHasher.HashPassword(salt, "PassWord1");
            success = UserDao.InsertUser(UserBO, OrgBO);
            if (success)
                {
                
                List<string> EmailList = new List<string>();
                EmailList.Add(UserBO.UserName);
                Email email = new Email();
                email.Body = "Your account has now been created for" + OrgBO.Organization + "/n" + "Please click the link below to launch Epi Web Enter." + "/n" + ConfigurationManager.AppSettings["BaseURL"] + "/nThank you"; 
                success = SendEmail(email, Constant.EmailCombinationEnum.InsertUser);


                }
            return success;
            }
    }
}
