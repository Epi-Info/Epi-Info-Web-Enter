using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
using System.Xml;
using System.Xml.Linq;
using Epi.Web.Interfaces.DataInterface;
using System.Configuration;
namespace Epi.Web.BLL
    {
    public class FormSetting
        {


        private IFormSettingDao FormSettingDao;

        private IUserDao UserDao;
        private IFormInfoDao FormInfoDao;
        public FormSetting(IFormSettingDao pFormSettingDao, IUserDao pUserDao, IFormInfoDao pFormInfoDao)
            {
            this.FormSettingDao = pFormSettingDao;
            this.UserDao = pUserDao;
            this.FormInfoDao = pFormInfoDao;
            }

        public FormSettingBO GetFormSettings(string FormId, string Xml)
            {

            FormSettingBO result = this.FormSettingDao.GetFormSettings(FormId);
            if (!string.IsNullOrEmpty(Xml))
                {
                result.FormControlNameList = GetFormColumnNames(Xml, result.ColumnNameList);
                }
            return result;
            }

        public Dictionary<int, string> GetFormColumnNames(string Xml, Dictionary<int,string> Selected)
            {
            Dictionary<int, string> List = new Dictionary<int, string>();

            XDocument xdoc = XDocument.Parse(Xml);


            var _FieldsTypeIDs = from _FieldTypeID in
                                     xdoc.Descendants("Field")

                                 select _FieldTypeID;
            int Count = 0;
            foreach (var _FieldTypeID in _FieldsTypeIDs)
                {
                if (!Selected.ContainsValue(_FieldTypeID.Attribute("Name").Value.ToString()) && _FieldTypeID.Attribute("FieldTypeId").Value !="2")
                    {
                    List.Add(Count, _FieldTypeID.Attribute("Name").Value.ToString());
                    Count++;
                    }
                }
            return List;

            }


        public FormSettingBO SaveSettings(Dictionary<int, string> ColumnList, bool IsDraftMode)
            {
            throw new NotImplementedException();
            }





        public string SaveSettings(bool IsDraftMode, Dictionary<int, string> ColumnNameList, Dictionary<int, string> AssignedUserList,string FormId)
            {
            string Message="";
            FormSettingBO FormSettingBO = new FormSettingBO();
            FormSettingBO.ColumnNameList = ColumnNameList;
            FormSettingBO.AssignedUserList = AssignedUserList;
            FormInfoBO FormInfoBO = new FormInfoBO();
            FormInfoBO.FormId = FormId;
            FormInfoBO.IsDraftMode = IsDraftMode;
            try
                {

                this.FormSettingDao.UpDateColumnNames(FormSettingBO, FormId);
                this.FormSettingDao.UpDateFormMode(FormInfoBO);
                this.FormSettingDao.UpDateAssignedUserList(FormSettingBO, FormId);

               if(ConfigurationManager.AppSettings["SEND_EMAIL_TO_ASSIGNED_USERS"].ToUpper() =="TRUE")
                   {
                    SendEmail(AssignedUserList, FormId) ;

                   }
                  
                


                Message = "Success";
                }
            catch (Exception Ex){
                Message = "Error";
                throw Ex;
                
                }
            return Message;
            }


        private void SendEmail(Dictionary<int,String> AssignedUserList,string FormId) 
            { 
            
             //GetFormCurrentUsersList
                List<UserBO> FormCurrentUsersList = this.UserDao.GetUserByFormId(FormId);
                FormInfoBO FormInfoBO = this.FormInfoDao.GetFormByFormId(FormId);
                UserBO UserBO = this.UserDao.GetCurrentUser(FormInfoBO.UserId);
                List<string> UsersEmail = new List<string>();
                foreach (UserBO User in FormCurrentUsersList)
                    {
                    if (!AssignedUserList.ContainsValue(User.UserName))
                        {
                        /// send email to user
                        UsersEmail.Add(User.EmailAddress);
                        }

                    }
           if (UsersEmail.Count()>0)
             {
                Epi.Web.Common.Email.Email Email = new Web.Common.Email.Email();
                Email.Body = "The following form has been assigned to You:\n Title:" + FormInfoBO.FormName + " \n Form ID:" + FormInfoBO.FormId  + " \n \n \n  Thank you.";
                Email.From = UserBO.EmailAddress;
                Email.To = UsersEmail;
                Email.Subject = "Form -" + FormInfoBO.FormName + "has been assigned to You";
                Epi.Web.Common.Email.EmailHandler.SendMessage(Email);
             }
           }
        }
    }