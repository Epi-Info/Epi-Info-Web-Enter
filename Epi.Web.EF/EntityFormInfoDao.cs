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

     
            try
                {

                Guid Id = UserId;

                    using (var Context = DataObjectFactory.CreateContext())
                        {
                        FormList.Add(Mapper.MapToFormInfoBO(Context.SurveyMetaDatas.FirstOrDefault(x => x.SurveyId == Id)));
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
