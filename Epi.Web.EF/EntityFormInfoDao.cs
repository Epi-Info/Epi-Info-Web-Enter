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
        }
    }
