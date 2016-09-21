using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.DTO;
using Epi.Web.Enter.Common.Criteria;
using System.Xml;
using System.Xml.Linq;
using Epi.Web.Enter.Interfaces.DataInterface;
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
        public FormSetting(IFormSettingDao pFormSettingDao )
        {
            this.FormSettingDao = pFormSettingDao;
            
        }

        public FormSettingBO GetFormSettings(string FormId, string Xml,int CurrentOrgId = -1)
        {
            FormSettingBO result = this.FormSettingDao.GetFormSettings(FormId, CurrentOrgId);
            if (!string.IsNullOrEmpty(Xml))
            {
                result.FormControlNameList = GetFormColumnNames(Xml, result.ColumnNameList);
            }
            else
            {
                result.FormControlNameList = new Dictionary<int, string>();
                var Columns = GetAllColumns(FormId);

                for (int i = 0; i < Columns.Count; i++)
                {
                    result.FormControlNameList.Add(i, Columns[i]);
                }
            }
            return result;
        }

        private List<string> GetAllColumns(string FormId)
        {
            return FormSettingDao.GetAllColumnNames(FormId);
        }

        public Dictionary<int, string> GetFormColumnNames(string Xml, Dictionary<int, string> Selected)
        {
            Dictionary<int, string> List = new Dictionary<int, string>();

            XDocument xdoc = XDocument.Parse(Xml);


            var _FieldsTypeIDs = from _FieldTypeID in
                                     xdoc.Descendants("Field")

                                 select _FieldTypeID;
            int Count = 0;
            foreach (var _FieldTypeID in _FieldsTypeIDs)
            {
                if (!Selected.ContainsValue(_FieldTypeID.Attribute("Name").Value.ToString()) && _FieldTypeID.Attribute("FieldTypeId").Value != "2" && _FieldTypeID.Attribute("FieldTypeId").Value != "21" && _FieldTypeID.Attribute("FieldTypeId").Value != "3"
                    && _FieldTypeID.Attribute("FieldTypeId").Value != "4" && _FieldTypeID.Attribute("FieldTypeId").Value != "13" && _FieldTypeID.Attribute("FieldTypeId").Value != "20")
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





       // public string SaveSettings(bool IsDraftMode, Dictionary<int, string> ColumnNameList, Dictionary<int, string> AssignedUserList, string FormId, Dictionary<int, string> SelectedOrgList, bool IsShareable)
        public string SaveSettings(bool IsDraftMode, FormSettingDTO FormSettingDTO)
        {
            string Message = "";
            FormSettingBO FormSettingBO = new FormSettingBO();
           // FormSettingBO.ColumnNameList = FormSettingDTO.ColumnNameList;
            FormSettingBO.AssignedUserList = FormSettingDTO.AssignedUserList;
            FormSettingBO.SelectedOrgList = FormSettingDTO.SelectedOrgList;
            FormSettingBO.DeleteDraftData = FormSettingDTO.DeleteDraftData;
            //FormInfoBO FormInfoBO = new FormInfoBO();
            //FormInfoBO.FormId = FormSettingDTO.FormId;
            //FormInfoBO.IsDraftMode =IsDraftMode;
            //FormInfoBO.IsShareable = FormSettingDTO.IsShareable;
            try
            {
                List<UserBO> FormCurrentUsersList = this.UserDao.GetUserByFormId(FormSettingDTO.FormId);
                //this.FormSettingDao.UpDateColumnNames(FormSettingBO, FormSettingDTO.FormId);
               // this.FormSettingDao.UpDateFormMode(FormInfoBO);
                Dictionary<int, string> AssignedOrgAdminList = this.FormSettingDao.GetOrgAdmins(FormSettingDTO.SelectedOrgList);// about to share with
                List<UserBO> CurrentOrgAdminList = this.FormSettingDao.GetOrgAdminsByFormId(FormSettingDTO.FormId);// shared with 
                this.FormSettingDao.UpDateSettingsList(FormSettingBO, FormSettingDTO.FormId);

                // Clear all Draft records
                if (FormSettingDTO.DeleteDraftData)
                { 
                this.FormSettingDao.DeleteDraftRecords(FormSettingDTO.FormId);
                }

               List<UserBO> AdminList =  this.UserDao.GetAdminsBySelectedOrgs(FormSettingBO, FormSettingDTO.FormId);

                if (ConfigurationManager.AppSettings["SEND_EMAIL_TO_ASSIGNED_USERS"].ToUpper() == "TRUE" && FormSettingDTO.AssignedUserList.Count() > 0)
                {
                    SendEmail(FormSettingDTO.AssignedUserList, FormSettingDTO.FormId, FormCurrentUsersList);

                }

                // Send Email to organization admin when a form is shared with that organization
             SendEmail(AssignedOrgAdminList, FormSettingDTO.FormId, CurrentOrgAdminList,true);


                Message = "Success";
            }
            catch (Exception Ex)
            {
                Message = "Error";
                throw Ex;

            }
            return Message;
        }

        public void UpDateColumnNames(bool IsDraftMode, FormSettingDTO FormSettingDTO) 
        {
            FormSettingBO FormSettingBO = new FormSettingBO();
            FormSettingBO.ColumnNameList = FormSettingDTO.ColumnNameList;
            FormInfoBO FormInfoBO = new FormInfoBO();
            FormInfoBO.FormId = FormSettingDTO.FormId;
            FormInfoBO.IsDraftMode = IsDraftMode;
            FormInfoBO.IsShareable = FormSettingDTO.IsShareable;
            FormInfoBO.DataAccesRuleId = FormSettingDTO.SelectedDataAccessRule;
            this.FormSettingDao.UpDateColumnNames(FormSettingBO, FormSettingDTO.FormId);
            this.FormSettingDao.UpDateFormMode(FormInfoBO);
            if (FormSettingDTO.IsDisabled)
            {

                this.FormSettingDao.SoftDeleteForm(FormSettingDTO.FormId);
            
            }

        }
        private void SendEmail(Dictionary<int, String> AssignedUserList, string FormId, List<UserBO> FormCurrentUsersList,bool ShareForm = false)
        {

            try
            {

                FormInfoBO FormInfoBO = this.FormInfoDao.GetFormByFormId(FormId);
                if (!string.IsNullOrEmpty(FormInfoBO.ParentId))
                {
                    return;
                }
                UserBO UserBO = this.UserDao.GetCurrentUser(FormInfoBO.UserId);
                List<string> UsersEmail = new List<string>();
                List<string> CurrentUsersEmail = new List<string>();

                foreach (UserBO User in FormCurrentUsersList)
                {
                    CurrentUsersEmail.Add(User.EmailAddress);
                }


                if (CurrentUsersEmail.Count() > 0)
                {

                    foreach (var User in AssignedUserList)
                    {
                        if (!CurrentUsersEmail.Contains(User.Value))
                        {

                            UsersEmail.Add(User.Value);
                        }

                    }
                }
                else
                {
                    foreach (var User in AssignedUserList)
                    {

                        UsersEmail.Add(User.Value);

                    }
                }
               
                if (UsersEmail.Count() > 0)
                {
                    Epi.Web.Enter.Common.Email.Email Email = new Web.Enter.Common.Email.Email();
                     if(!ShareForm ){
                   
                    Email.Body = UserBO.FirstName + " " + UserBO.LastName + " has assigned the following form  to you in Epi Info™ Web Enter.\n\nTitle: " + FormInfoBO.FormName + " \n \n \nPlease click the link below to launch Epi Info™ Web Enter.";
                    Email.Body = Email.Body.ToString() + " \n \n" + ConfigurationManager.AppSettings["BaseURL"];
                    Email.From = UserBO.EmailAddress;
                    Email.To = UsersEmail;
                    Email.Subject = "An Epi Info Web Enter Form - " + FormInfoBO.FormName + " has been assigned to You";
                   
                     }
                     else
                     {
                          
                    Email.Body = UserBO.FirstName + " " + UserBO.LastName + " has shared the following form  with your organization in Epi Info™ Web Enter.\n\nTitle: " + FormInfoBO.FormName + " \n \n \nPlease click the link below to launch Epi Info™ Web Enter.";
                    Email.Body = Email.Body.ToString() + " \n \n" + ConfigurationManager.AppSettings["BaseURL"];
                    Email.From = UserBO.EmailAddress;
                    Email.To = UsersEmail;
                    Email.Subject = "An Epi Info Web Enter Form - " + FormInfoBO.FormName + " has been shered with your organization.";
                          
                     
                     }
                     Epi.Web.Enter.Common.Email.EmailHandler.SendMessage(Email);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FormSettingBO GetFormSettings()
        {
            FormSettingBO result = this.FormSettingDao.GetFormSettings();
            return result;
        }
    }
}
