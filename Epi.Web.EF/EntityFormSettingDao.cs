using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using Epi.Web.Common;
using Epi.Web.Common.Criteria;
using Epi.Web.Interfaces.DataInterface;
using Epi.Web.Common.BusinessObject;

namespace Epi.Web.EF
    {
   public  class EntityFormSettingDao : IFormSettingDao
        {
       public FormSettingBO GetFormSettings(string FormId) 
           
           {

           FormSettingBO FormSettingBO = new FormSettingBO();
           Dictionary<int, string> ColumnNameList = new Dictionary<int, string>();
           try{
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
               FormSettingBO.ColumnNameList =  ColumnNameList;
               }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
           return FormSettingBO;
           
           
           }
       public void SaveSettings(FormSettingBO FormSettingBO) 
           {

           int OrganizationId = 0;
           try
               {
               using (var Context = DataObjectFactory.CreateContext())
                   {

                   //retrieve OrganizationId based on OrganizationKey
                   using (var ContextOrg = DataObjectFactory.CreateContext())
                       {
                   //    string OrgKey = Epi.Web.Common.Security.Cryptography.Encrypt(SurveyInfo.OrganizationKey.ToString());
                   //    OrganizationId = ContextOrg.Organizations.FirstOrDefault(x => x.OrganizationKey == OrgKey).OrganizationId;
                      }

                   //SurveyInfo.TemplateXMLSize = RemoveWhitespace(SurveyInfo.XML).Length;
                   //SurveyInfo.DateCreated = DateTime.Now;

                   //var SurveyMetaDataEntity = Mapper.Map(SurveyInfo);
                   //SurveyMetaDataEntity.OrganizationId = OrganizationId;
                   //Context.AddToSurveyMetaDatas(SurveyMetaDataEntity);

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
