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

        public List<FormInfoBO> GetFormsInfo(Guid UserId)
            {
            //Owner Forms
            List<FormInfoBO> result = this.FormInfoDao.GetFormInfo(UserId);

            //Assigned Forms 
            List<Guid> AssignedFormId = new List<Guid>();
            AssignedFormId = this.FormInfoDao.GetAssignedFormsId(UserId);


            List<FormInfoBO> AssignedForm = this.FormInfoDao.GetAssignedFormsInfo(AssignedFormId);

            foreach (var form in AssignedForm)
                {
                result.Add(form);
                }
           

            return result;
            }
        }
    }
