using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using Epi.Web.Enter.Common.BusinessObject;
using Epi.Web.Enter.Common.Criteria;
using Epi.Web.Enter.Interfaces.DataInterface;
using System.Data.SqlClient;
using System.Data;
namespace Epi.Web.EF
    {
    public class EntityFormInfoDao: IFormInfoDao
        {
        public List<FormInfoBO> GetFormInfo(int UserId , int CurrentOrgId)
            {
        List<FormInfoBO> FormList = new List<FormInfoBO>();
        FormInfoBO FormInfoBO;
     
            try
                {

                int Id = UserId;

                    using (var Context = DataObjectFactory.CreateContext())
                        {




                       User CurrentUser = Context.Users.Single(x => x.UserID == Id);

                       //IEnumerable<SurveyMetaData> AllForms = Context.SurveyMetaDatas.Where(x => x.ParentId == null );
                       
                   IEnumerable<SurveyMetaData> AllForms = Context.SurveyMetaDatas.Where(x => x.ParentId == null && x.Users.Any(r=> r.UserID == CurrentUser.UserID)).Distinct();

                       


                       List<string> Assigned = new List<string>();
                      // Dictionary<int,string> Shared = new Dictionary<int,string>();
                       List<KeyValuePair<int, string>> Shared = new List<KeyValuePair<int, string>>();
                       

                        
                       var UserOrganizations = CurrentUser.UserOrganizations.Where(x=> x.RoleId == 2);
                       //SurveyMetaData Response = Context.SurveyMetaDatas.First(x => x.SurveyId == Id);
                       //var _Org = new HashSet<int>(Response.Organizations.Select(x => x.OrganizationId));
                       //var Orgs = Context.Organizations.Where(t => _Org.Contains(t.OrganizationId)).ToList();
                       //List<string> SharedForms = new List<string>();
                       List<KeyValuePair<int, string>> SharedForms = new List<KeyValuePair<int, string>>();
                       foreach (var form in AllForms)
                           {
                           if (form.Users.Contains(CurrentUser) )
                               {
                                  Assigned.Add(form.SurveyId.ToString());
                               }

                           // checking if the form is shared with any organization
                           SurveyMetaData Response = Context.SurveyMetaDatas.First(x => x.SurveyId == form.SurveyId);
                           var _Org = new HashSet<int>(Response.Organizations.Select(x => x.OrganizationId));
                           var Orgs = Context.Organizations.Where(t => _Org.Contains(t.OrganizationId)).ToList();
                           //if form is shared 
                           if (Orgs.Count> 0)
                           {
                               foreach (var org in Orgs)
                               {
                                   KeyValuePair<int, string> Item = new KeyValuePair<int, string>(org.OrganizationId,form.SurveyId.ToString());
                                  
                                   Shared.Add(Item);
                               }
                              
                              }

                           
                           }
                        // find the forms that are shared with the current user 
                      
                       foreach (var item in Shared)
                       {

                           if (UserOrganizations.Where(x => x.OrganizationID == item.Key).Count()>0)
                           {
                               KeyValuePair<int, string> Item = new KeyValuePair<int, string>(item.Key, item.Value);
                               SharedForms.Add(Item);
                           }
                       }


                       var items = from FormInfo in Context.SurveyMetaDatas
                                   join UserInfo in Context.Users
                                   on FormInfo.OwnerId equals UserInfo.UserID
                                   into temp
                                   where FormInfo.ParentId == null
                                   
                                   from UserInfo in temp.DefaultIfEmpty()
                                   select new { FormInfo, UserInfo }; 
                     

                        foreach (var item in items)
                            {
                            
                        FormInfoBO = Mapper.MapToFormInfoBO(item.FormInfo,item.UserInfo,false);
                        //if (string.IsNullOrEmpty(FormInfoBO.ParentId))
                        //    {
                               

                            if(item.UserInfo.UserID == Id)
                                {
                                    FormInfoBO.IsOwner = true;
                                    FormList.Add(FormInfoBO);
                                   
                                }
                            else
                                {

                                //Only Share or Assign
                                    if (SharedForms.Where(x=>x.Value == FormInfoBO.FormId).Count()>0)
                                    {
                                        FormInfoBO.IsShared = true;
                                        FormInfoBO.UserId = Id;
                                        //FormInfoBO.OrganizationId = Shared.FirstOrDefault(x => x.Value.Equals(FormInfoBO.FormId)).Key;
                                        FormInfoBO.OrganizationId = SharedForms.FirstOrDefault(x => x.Value.Equals(FormInfoBO.FormId)).Key;
                                        FormList.Add(FormInfoBO);
                                    }
                                    else if (Assigned.Contains(FormInfoBO.FormId))
                                     {
                                      FormInfoBO.IsOwner = false;
                                      var UserOrgId =this.GetUserOrganization(FormInfoBO.FormId, UserId);
                                      if (UserOrgId > -1)
                                        {
                                          FormInfoBO.OrganizationId = this.GetUserOrganization(FormInfoBO.FormId, UserId);
                                        }
                                   FormList.Add(FormInfoBO);
                                     }
                                   
                                }

                           // FormList.Add(FormInfoBO);
                              // }
                            }
                         }
                }
            catch (Exception ex)
                {
                throw (ex);
                }
           



        return FormList;
            }

        private int GetUserOrganization(string SurveyId, int UserId)
        {
            try
            {
               int OrgId = -1;
                    using (var Context = DataObjectFactory.CreateContext())
                    {

                   

                        // get UserOrganization
                        var items = from UserOrgInfo in Context.UserOrganizations 
                                    join OrgInfo in Context.Organizations on UserOrgInfo.OrganizationID equals OrgInfo.OrganizationId into temp
                                   
                                    where UserOrgInfo.UserID == UserId
                                    
                                    from query in temp.DefaultIfEmpty()
                                    select new { query };
                        foreach (var Item in items)
                        {

                            var OId = Item.query.SurveyMetaDatas.Where(x => x.SurveyId == new Guid(SurveyId));
                            if (OId.Count()>0)
                            {
                                OrgId = Item.query.OrganizationId;
                                break;
                            }
                        }


                        //var OId = items.Where(x => x.query.SurveyMetaDatas.Where(z => z.SurveyId == new Guid(SurveyId)) == items1);

                    }

                return OrgId;
             }
            catch (Exception ex)
                {
                throw (ex);
                }
        }

        public FormInfoBO GetFormByFormId(string FormId, bool GetXml, int UserId)
            {

       
                      FormInfoBO FormInfoBO = new FormInfoBO();
                    
                      try
                          {

                          Guid Id = new Guid(FormId);

                          using (var Context = DataObjectFactory.CreateContext())
                              {
                                  


                              var items = from FormInfo in Context.SurveyMetaDatas
                                          join UserInfo in Context.Users
                                          on FormInfo.OwnerId equals UserInfo.UserID
                                          into temp
                                          from UserInfo in temp.DefaultIfEmpty()
                                          where FormInfo.SurveyId == Id
                                          select new { FormInfo, UserInfo };
                              SurveyMetaData Response = Context.SurveyMetaDatas.First(x => x.SurveyId == Id);
                              var _Org = new HashSet<int>(Response.Organizations.Select(x => x.OrganizationId));
                              var Orgs = Context.Organizations.Where(t => _Org.Contains(t.OrganizationId)).ToList();



                              bool IsShared = false;

                              foreach(var org in Orgs)
                              {
                                  

                                  var UserInfo = Context.UserOrganizations.Where(x => x.OrganizationID == org.OrganizationId && x.UserID == UserId && x.RoleId == 2);
                                  if (UserInfo.Count()>0)
                                 {
                                     IsShared = true;
                                     break;
                                 
                                 }
                            
                              }




                              foreach (var item in items)
                                  {
                                 
                                  FormInfoBO = Mapper.MapToFormInfoBO(item.FormInfo, item.UserInfo,GetXml);
                                  FormInfoBO.IsShared = IsShared;

                                  if (item.UserInfo.UserID == UserId)
                                      {
                                      FormInfoBO.IsOwner = true;
                                      }
                                  else
                                      {
                                      FormInfoBO.IsOwner = false;
                                      }

                                  }
                              }
                          }
                      catch (Exception ex)
                          {
                          throw (ex);
                          }




                      return FormInfoBO;
            
            
            }

        public FormInfoBO GetFormByFormId(string FormId)
        {

        FormInfoBO FormInfoBO = new FormInfoBO();

        try
            {

            Guid Id = new Guid(FormId);

            using (var Context = DataObjectFactory.CreateContext())
                {
                 
                SurveyMetaData SurveyMetaData = Context.SurveyMetaDatas.Single(x => x.SurveyId == Id);
                FormInfoBO = Mapper.ToFormInfoBO(SurveyMetaData);
                }
            }
        catch (Exception ex)
            {
            throw (ex);
            }




        return FormInfoBO;
            
            }

        public bool GetEwavLiteToggleSwitch(string FormId, int UserId) 
        {
            string EWEConnectionString = DataObjectFactory.EWEADOConnectionString;
            SqlConnection EWEConnection = new SqlConnection(EWEConnectionString);
            EWEConnection.Open();
            SqlCommand EWECommand = new SqlCommand(EWEConnectionString, EWEConnection);
            SqlDataAdapter DataAdapter = new SqlDataAdapter(EWECommand);
            DataSet DS = new DataSet();
            EWECommand.CommandText = "usp_read_canvases_for_lite";
            EWECommand.CommandType = CommandType.StoredProcedure;
            EWECommand.Parameters.Add(new SqlParameter("FormId", FormId));
            EWECommand.Parameters.Add(new SqlParameter("UserId", UserId));

            DataAdapter.Fill(DS);

            object numberOfRows = DS.Tables[0].Rows.Count;
            EWEConnection.Close();
            if ((int)numberOfRows >0)
            {
                return true;
            }

            return false;

        }


        public bool HasDraftRecords(string FormId) {

            try
            {

                Guid Id = new Guid(FormId);
                bool _HasDraftRecords = false;
                using (var Context = DataObjectFactory.CreateContext())
                {

                    var DraftRecords = Context.SurveyResponses.Where(x => x.SurveyId == Id && x.IsDraftMode == true);
                   if (DraftRecords.Count() > 0)
                {
                    _HasDraftRecords = true;
                
                }
                }
                
                return _HasDraftRecords;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
           
        }

        }
    }
