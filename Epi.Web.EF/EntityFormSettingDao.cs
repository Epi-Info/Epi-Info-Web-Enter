using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using Epi.Web.Enter.Common;
using Epi.Web.Enter.Common.Criteria;
using Epi.Web.Enter.Interfaces.DataInterface;
using Epi.Web.Enter.Common.BusinessObject;

namespace Epi.Web.EF
{
    public class EntityFormSettingDao : IFormSettingDao
    {
        public FormSettingBO GetFormSettings(string FormId)
        {

            FormSettingBO FormSettingBO = new FormSettingBO();
            Dictionary<int, string> ColumnNameList = new Dictionary<int, string>();
            Dictionary<int, string> AvailableUsers = new Dictionary<int, string>();
            Dictionary<int, string> SelectedUsers = new Dictionary<int, string>();
            Dictionary<int, string> AvailableOrgs = new Dictionary<int, string>();
            Dictionary<int, string> SelectedOrgs = new Dictionary<int, string>();
            Dictionary<int, string> DataAccessRuleIds = new Dictionary<int, string>();
            Dictionary<string, string> DataAccessRuleDescription = new Dictionary<string, string>();
            int selectedDataAccessRuleId ;
            try
            {
                Guid id = new Guid(FormId);
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = from response in Context.ResponseDisplaySettings
                                where response.FormId == id
                                select response;

                    var DataRow = Query;

                    foreach (var Row in DataRow)
                    {

                        ColumnNameList.Add(Row.SortOrder, Row.ColumnName);

                    }

                    


                    //var SelectedUserQuery = from FormInfo in Context.SurveyMetaDatas
                    //            join UserInfo in Context.Users
                    //            on FormInfo.OwnerId equals UserInfo.UserID
                    //            into temp
                    //            from UserInfo in temp.DefaultIfEmpty()
                    //            where FormInfo.SurveyId == id
                    //            select new { FormInfo, UserInfo };

                    SurveyMetaData SelectedUserQuery = Context.SurveyMetaDatas.First(x => x.SurveyId == id);

                    var SelectedOrgId = SelectedUserQuery.OrganizationId;

                    var query = (from user in SelectedUserQuery.Users
                                join userorg in Context.UserOrganizations
                            on user.UserID equals userorg.UserID
                                where userorg.Active == true &&
                                userorg.OrganizationID == SelectedOrgId
                               // orderby user.UserName
                                select user).Distinct().OrderBy(user => user.UserName) ;

                    // IEnumerable<User> Users = SelectedUserQuery.Users;
                    foreach (var user in query)
                    {
                        SelectedUsers.Add(user.UserID, user.UserName);
                    }

                    //foreach (var Selecteduser in SelectedUserQuery)
                    //    {
                    //    SelectedUsers.Add(Selecteduser.UserInfo.UserID, Selecteduser.UserInfo.UserName);
                    //    }


                    var UserQuery = (from user in Context.Users
                                    join userorg in Context.UserOrganizations
                                    on user.UserID equals userorg.UserID
                                     where userorg.Active == true &&
                                 userorg.OrganizationID == SelectedOrgId
                                    //orderby user.UserName
                                     select user).Distinct().OrderBy(user => user.UserName);



                    foreach (var user in UserQuery)
                    {
                        if (!SelectedUsers.ContainsValue(user.UserName) && user.UserID != SelectedUserQuery.OwnerId)
                        {
                            AvailableUsers.Add(user.UserID, user.UserName);
                        }
                    }
                   
                    //// Select Orgnization list 
                    var OrganizationQuery = Context.Organizations.Where(c => c.SurveyMetaDatas.Any(a => a.SurveyId == id)).ToList();

 
                       foreach(var org in OrganizationQuery){

                           SelectedOrgs.Add(org.OrganizationId, org.Organization1);
                       }
                       ////  Available Orgnization list 

                       IEnumerable<Organization> OrganizationList = Context.Organizations.ToList();
                       foreach (var Org in OrganizationList)
                       {
                           if (!SelectedOrgs.ContainsValue(Org.Organization1))
                           {
                               AvailableOrgs.Add(Org.OrganizationId, Org.Organization1);
                           }
                       }
                       //// Select DataAccess Rule Ids  list 
                     var MetaData = Context.SurveyMetaDatas.Where( a => a.SurveyId == id).Single();


                      selectedDataAccessRuleId = int.Parse(MetaData.DataAccessRuleId.ToString()); 
                       ////  Available DataAccess Rule Ids  list 

                       IEnumerable<DataAccessRule> RuleIDs = Context.DataAccessRules.ToList();
                       foreach (var Rule in RuleIDs)
                       {

                           DataAccessRuleIds.Add(Rule.RuleId, Rule.RuleName);
                           DataAccessRuleDescription.Add(Rule.RuleName, Rule.RuleDescription);
                          
                       }
                    FormSettingBO.ColumnNameList = ColumnNameList;
                    FormSettingBO.UserList = AvailableUsers;
                    FormSettingBO.AssignedUserList = SelectedUsers;
                    FormSettingBO.AvailableOrgList = AvailableOrgs;
                    FormSettingBO.SelectedOrgList = SelectedOrgs;
                    FormSettingBO.DataAccessRuleIds = DataAccessRuleIds;
                    FormSettingBO.SelectedDataAccessRule = selectedDataAccessRuleId;
                    FormSettingBO.DataAccessRuleDescription = DataAccessRuleDescription;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return FormSettingBO;


        }
        public void UpDateColumnNames(FormSettingBO FormSettingBO, string FormId)
        {
            Guid Id = new Guid(FormId);
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {

                    IEnumerable<ResponseDisplaySetting> ColumnList = Context.ResponseDisplaySettings.ToList().Where(x => x.FormId == Id);

                    //Delete old columns
                    foreach (var item in ColumnList)
                    {
                        Context.ResponseDisplaySettings.DeleteObject(item);
                    }
                    Context.SaveChanges();

                    //insert new columns

                    ResponseDisplaySetting ResponseDisplaySettingEntity = new ResponseDisplaySetting();
                    foreach (var item in FormSettingBO.ColumnNameList)
                    {

                        ResponseDisplaySettingEntity = Mapper.ToColumnName(item, Id);
                        Context.AddToResponseDisplaySettings(ResponseDisplaySettingEntity);
                        //Context.SaveChanges();
                    }
                    Context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }


        }
        public void UpDateFormMode(FormInfoBO FormInfoBO)
        {

            try
            {
                Guid Id = new Guid(FormInfoBO.FormId);

                //Update Form Mode
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = from response in Context.SurveyMetaDatas
                                where response.SurveyId == Id
                                select response;

                    var DataRow = Query.Single();
                    DataRow.IsDraftMode = FormInfoBO.IsDraftMode;
                    DataRow.IsShareable = FormInfoBO.IsShareable;
                    DataRow.DataAccessRuleId = FormInfoBO.DataAccesRuleId;

                    Context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        public void UpDateSettingsList(FormSettingBO FormSettingBO, string FormId)
        {

            Guid Id = new Guid(FormId);
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    SurveyMetaData Response = Context.SurveyMetaDatas.First(x => x.SurveyId == Id);
                    //Remove old Users
                
                    var _User = new HashSet<string>(Response.Users.Select(x => x.UserName));
                    var Users = Context.Users.Where(t => _User.Contains(t.UserName))  .ToList();
                  
                    foreach (User user in Users)
                    {

                        Response.Users.Remove(user);

                    }
                    Context.SaveChanges();



                    //insert new users
                    foreach (var item in FormSettingBO.AssignedUserList)
                    {
                        User User = Context.Users.FirstOrDefault(x => x.UserName == item.Value);
                        Response.Users.Add(User);

                    }
                    Context.SaveChanges();



                    //Remove old Orgs

                    var _Org = new HashSet<int>(Response.Organizations.Select(x => x.OrganizationId));
                    var Orgs = Context.Organizations.Where(t => _Org.Contains(t.OrganizationId)).ToList();

                    foreach (Organization org in Orgs)
                    {

                        Response.Organizations.Remove(org);

                    }
                    Context.SaveChanges();



                    //insert new Orgs
                    List<User> OrgAdmis = new List<User>();

                    foreach (var item in FormSettingBO.SelectedOrgList)
                    {
                        int OrgId = int.Parse(item.Value);
                        Organization Org = Context.Organizations.FirstOrDefault(x => x.OrganizationId == OrgId);
                        Response.Organizations.Add(Org);

                        

                    }
                    Context.SaveChanges();


                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        
        public List<string> GetAllColumnNames(string FormId)
        {
            Guid Id = new Guid(FormId);
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    List<string> Columns = (from c in Context.SurveyMetaDataTransforms
                                            where c.SurveyId == Id &&
                                            (c.FieldTypeId != 2 && c.FieldTypeId != 20 && c.FieldTypeId != 3 && c.FieldTypeId != 17 && c.FieldTypeId != 21) //filter non-data fields.
                                            orderby c.FieldName
                                            select c.FieldName).ToList();
                    return Columns;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public Dictionary<int, string> GetOrgAdmins(Dictionary<int, string> SelectedOrgList)
        {
            Dictionary<int, string> GetOrgAdmins = new Dictionary<int, string>();

            int i = 0;
            try
            {
                foreach (var org in SelectedOrgList)
                {
                    using (var Context = DataObjectFactory.CreateContext())
                    {
                        int OrgId = int.Parse(org.Value);

                        var AdminList = Context.UserOrganizations.Where(x => x.OrganizationID == OrgId && x.RoleId == 2 && x.Active == true).ToList();

                        foreach (var item in AdminList)
                        {
                            GetOrgAdmins.Add(i, item.User.EmailAddress);
                            i++;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }


            return GetOrgAdmins;
        
        
        }

        public List<UserBO> GetOrgAdminsByFormId(string FormId)
        {

            List<UserBO> BoList = new List<UserBO>();
            Dictionary<int, string> GetOrgAdmins = new Dictionary<int, string>();
            Guid Id = new Guid(FormId);
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    SurveyMetaData Response = Context.SurveyMetaDatas.First(x => x.SurveyId == Id);
                    var _Org = new HashSet<int>(Response.Organizations.Select(x => x.OrganizationId));
                    var Orgs = Context.Organizations.Where(t => _Org.Contains(t.OrganizationId)).ToList();


                    foreach (var Org in Orgs)
                    {
                        
                        var AdminList = Context.UserOrganizations.Where(x => x.OrganizationID == Org.OrganizationId && x.RoleId == 2 && x.Active == true);
                        foreach (var Admin in AdminList)
                        {
                            UserBO UserBO = new UserBO();
                            UserBO.EmailAddress = Admin.User.EmailAddress;
                            UserBO.UserId = Admin.User.UserID;
                            BoList.Add(UserBO);
                             
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return BoList;
        
        
        }

        public void SoftDeleteForm( string FormId)
        {
            Guid Id = new Guid(FormId);
            try
            {
                using (var Context = DataObjectFactory.CreateContext())
                {
                    var Query = from response in Context.SurveyMetaDatas
                                where response.SurveyId == Id
                                select response;

                    var DataRow = Query.Single();
                    DataRow.ParentId = Id;


                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        
        }
    }
}
