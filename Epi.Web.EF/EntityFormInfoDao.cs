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
        public List<FormInfoBO> GetFormInfo(Guid UserId) {
        List<FormInfoBO> FormList = new List<FormInfoBO>();
        FormInfoBO FormInfoBO;
     
            try
                {

                Guid Id = UserId;

                    using (var Context = DataObjectFactory.CreateContext())
                        {
                        var items =   Context.SurveyMetaDatas.Where(x => x.OwnerId == Id).ToList();
                        foreach (var item in items)
                            {
                            FormInfoBO = Mapper.MapToFormInfoBO(item);
                            FormInfoBO.IsOwner = true;
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


        public List<FormInfoBO> GetAssignedFormsInfo(List<Guid> FormIds) 
            
            {
            List<FormInfoBO> FormList = new List<FormInfoBO>();
            FormInfoBO FormInfoBO; ;
            try
                {


                foreach (var Id in FormIds)
                    {

                    using (var Context = DataObjectFactory.CreateContext())
                        {
                        var items = Context.SurveyMetaDatas.Where(x => x.SurveyId == Id).ToList();
                        foreach (var item in items)
                            {
                            FormInfoBO = Mapper.MapToFormInfoBO(item);
                            FormInfoBO.IsOwner = false;
                            FormList.Add(FormInfoBO);

                            }
                        }
                    }
                }
            catch (Exception ex)
                {
                throw (ex);
                }
           


            return FormList;
            }
        public List<Guid> GetAssignedFormsId(Guid UserId) 
            {
            List<Guid> FormId = new List<Guid>();

             try
                {

                Guid Id = UserId;

                    using (var Context = DataObjectFactory.CreateContext())
                        {
                        var items =   Context.SurveyMetaDataUsers.Where(x => x.UserId == Id).ToList();
                        foreach (var item in items)
                            {
                            FormId.Add(item.FormId);
                            }
                         }
                }
            catch (Exception ex)
                {
                throw (ex);
                }


            return FormId;
            
            
            }

        }
    }
