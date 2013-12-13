using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;

using Epi.Web.Interfaces.DataInterface;
namespace Epi.Web.BLL
    {
  public  class FormSetting
        {


      private IFormSettingDao FormSettingDao;



      public FormSetting(IFormSettingDao pFormSettingDao)
        {
        this.FormSettingDao = pFormSettingDao;
        }

      public FormSettingBO GetResponseColumnNames(string FormId)
            {
             
          FormSettingBO result = this.FormSettingDao.GetResponseColumnNames(FormId);

           

            return result;
            }

        }
    }
