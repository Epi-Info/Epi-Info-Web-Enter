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

        public bool IsUserExistsInOrganizaion(UserBO User, OrganizationBO OrgBO)
        {
            bool Exists = false;
            Exists = UserDao.IsUserExistsInOrganizaion(User, OrgBO);

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

        public bool UpdateUser(UserBO User, OrganizationBO OrgBO)
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
                    //if (success)
                    //{
                    //    //List<string> EmailList = new List<string>();
                    //    //EmailList.Add(User.EmailAddress);
                    //    Email email = new Email();
                    //    email.To = new List<string>();
                    //    email.To.Add(User.EmailAddress);
                    //    success = SendEmail(email, Constant.EmailCombinationEnum.UpdateUserInfo);


                    //}
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
                    email.Body = string.Format("You recently accessed our Forgot Password service for  Epi Web Enter. \n \n Your new temporary password is: {0}\n \n If you have not accessed password help, please contact the administrator. \n \nLog in with your temporary password. You will then be asked to create a new password.", email.Password);
                    break;
                case Constant.EmailCombinationEnum.PasswordChanged:
                    email.Subject = "Your Epi Web Enter Password has been updated";
                    email.Body = " You recently updated your password for Epi Web Enter. \n \n If you have not accessed password help, please contact the administrator for you organization. \n \n ";
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

            email.Body = email.Body.ToString() + " \n \nPlease click the link below to launch Epi Web Enter. \n" + ConfigurationManager.AppSettings["BaseURL"] + "\nThank you.";
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
            //UserBO ExistingUser; //= GetUser(UserBO);
            //ExistingUser = UserDao.GetUserByEmail(UserBO);
            //ExistingUser.Role = UserDao.GetUserHighestRole(ExistingUser.UserId);

            bool success;
            if (UserBO.UserName == null)
            {
                string KeyForUserPasswordSalt = ReadSalt();
                PasswordHasher PasswordHasher = new Web.Enter.Common.Security.PasswordHasher(KeyForUserPasswordSalt);
                string salt = PasswordHasher.CreateSalt(UserBO.EmailAddress);
                UserBO.ResetPassword = true;
                PasswordGenerator PassGen = new PasswordGenerator();
                string tempPassword = PassGen.Generate();
                UserBO.PasswordHash = PasswordHasher.HashPassword(salt, tempPassword);// "PassWord1");
                //UserBO.PasswordHash = PasswordHasher.HashPassword(salt, "PassWord1");
                success = UserDao.InsertUser(UserBO, OrgBO);
                StringBuilder Body = new StringBuilder();
                if (success)
                {                
                    Email email = new Email();
                    Body.Append("Your account has now been created for " + OrgBO.Organization + "\n" + "Email: " + UserBO.EmailAddress + "\n" + "Password: " + tempPassword);// +
                    email.To = new List<string>();
                    email.To.Add(UserBO.EmailAddress);
                    email.Body = Body.ToString();
                    success = SendEmail(email, Constant.EmailCombinationEnum.InsertUser);
                }
            }
            else
            {
                //UserBO.Role = UserBO.Role;
                //UserBO.IsActive = UserBO.IsActive;
                success = UserDao.UpdateUserOrganization(UserBO, OrgBO);
                if (success)
                {
                    Email email = new Email();

                    StringBuilder Body = new StringBuilder();

                    Body.Append("Welcome to Epi Web Enter.  Your account has now been created for " + OrgBO.Organization);
                    // var OrgKey = OrgBO.OrganizationKey;
                    var OrgKey = Epi.Web.Enter.Common.Security.Cryptography.Decrypt(OrgBO.OrganizationKey);
                    Body.Append("\nOrganization Key: " + OrgKey);
                    email.Body = Body.ToString();
                    email.To = new List<string>();
                    email.To.Add(UserBO.EmailAddress);

                    success = SendEmail(email, Constant.EmailCombinationEnum.InsertUser);
                }

            }




            return success;
        }
    }
}
