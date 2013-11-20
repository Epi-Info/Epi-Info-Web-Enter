using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;

using Epi.Web.Interfaces.DataInterface;
namespace Epi.Web.BLL
    {
   public class FormInfo
        {
       private IFormInfoDao FormInfoDao;

 

        public FormInfo( IFormInfoDao pSurveyInfoDao)
        {
        this.FormInfoDao = pSurveyInfoDao;
        }

        public List<FormInfoBO> GetFormsInfoByUserId(Guid UserId)
            {
            List<FormInfoBO> result = this.FormInfoDao.GetFormInfo(UserId);
            return result;
            }
        }
    }
