using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
using Epi.Web.Interfaces.DataInterface;
namespace Epi.Web.EF
    {
    public class EntityFormInfoDao: IFormInfoDao
        {
        public List<FormInfoBO> GetFormInfo(int UserId) {
        List<FormInfoBO> FormList = new List<FormInfoBO>();
        FormInfoBO FormInfoBO;
     
            try
                {

                int Id = UserId;

                    using (var Context = DataObjectFactory.CreateContext())
                        {
                    


                       var items = from FormInfo in Context.SurveyMetaDatas
                                   join UserInfo in Context.Users
                                   on FormInfo.OwnerId equals UserInfo.UserID
                                   into temp
                                   from UserInfo in temp.DefaultIfEmpty()
                                   select new { FormInfo, UserInfo };


                        foreach (var item in items)
                            {
                        FormInfoBO = Mapper.MapToFormInfoBO(item.FormInfo,item.UserInfo);
                           
                            if(item.UserInfo.UserID == Id)
                                {
                                    FormInfoBO.IsOwner = true;
                                }
                            else
                                {
                                   FormInfoBO.IsOwner = false;
                                }
                            FormList.Add(FormInfoBO);
                            
                            }
                         }
                }
            catch (Exception ex)
                {
                throw (ex);
                }
           



        return FormList;
            }

      public  FormInfoBO GetFormByFormId(string FormId) {

       
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


              foreach (var item in items)
                  {
                  FormInfoBO = Mapper.MapToFormInfoBO(item.FormInfo, item.UserInfo);

                  

                  }
              }
          }
      catch (Exception ex)
          {
          throw (ex);
          }




      return FormInfoBO;
            
            
            }
        

        }
    }
