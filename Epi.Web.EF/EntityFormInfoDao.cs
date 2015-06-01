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
        public List<FormInfoBO> GetFormInfo(int UserId)
            {
        List<FormInfoBO> FormList = new List<FormInfoBO>();
        FormInfoBO FormInfoBO;
     
            try
                {

                int Id = UserId;

                    using (var Context = DataObjectFactory.CreateContext())
                        {
                    


                      


                       IEnumerable<SurveyMetaData> AllForms = Context.SurveyMetaDatas.Where(x => x.ParentId == null);
                       List<string> Assigned = new List<string>();
                      // Dictionary<int,string> Shared = new Dictionary<int,string>();
                       List<KeyValuePair<int, string>> Shared = new List<KeyValuePair<int, string>>();
                       User CurrentUser = Context.Users.Single(x => x.UserID == Id);

                        
                       var UserOrganizations = CurrentUser.UserOrganizations.Where(x=> x.RoleId == 2);
                       //SurveyMetaData Response = Context.SurveyMetaDatas.First(x => x.SurveyId == Id);
                       //var _Org = new HashSet<int>(Response.Organizations.Select(x => x.OrganizationId));
                       //var Orgs = Context.Organizations.Where(t => _Org.Contains(t.OrganizationId)).ToList();
                       List<string> SharedForms = new List<string>();
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
                               SharedForms.Add(item.Value);
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
                                    if (SharedForms.Contains(FormInfoBO.FormId))
                                    {
                                        FormInfoBO.IsShared = true;
                                        FormInfoBO.UserId = Id;
                                        FormInfoBO.OrganizationId = Shared.FirstOrDefault(x => x.Value.Equals(FormInfoBO.FormId)).Key;
                                        FormList.Add(FormInfoBO);
                                    }
                                    else if (Assigned.Contains(FormInfoBO.FormId))
                                     {
                                   FormInfoBO.IsOwner = false;
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
            EWECommand.CommandText = "usp_read_ewav_toggle_switch";
            EWECommand.CommandType = CommandType.StoredProcedure;
            EWECommand.Parameters.Add(new SqlParameter("FormId", FormId));
            EWECommand.Parameters.Add(new SqlParameter("UserId", UserId));

            object rows = EWECommand.ExecuteScalar();
            EWEConnection.Close();
            if ((int)rows >0)
            {
                return true;
            }

            return false;

        }
        }
    }
