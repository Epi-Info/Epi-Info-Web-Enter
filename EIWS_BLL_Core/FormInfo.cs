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

        public List<FormInfoBO> GetFormsInfo(int UserId)
            {
            //Owner Forms
            List<FormInfoBO> result = this.FormInfoDao.GetFormInfo(UserId);

           

            return result;
            }
        public FormInfoBO GetFormInfoByFormId(string FormId)
            {
            //Owner Forms
            FormInfoBO result = this.FormInfoDao.GetFormByFormId(FormId);



            return result;
            }
        }
    }
