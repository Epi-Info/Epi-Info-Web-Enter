using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;
using Epi.Web.Interfaces.DataInterface;

namespace Epi.Web.Interfaces.DataInterface
    {
   public  interface IFormSettingDao
        {
       FormSettingBO GetResponseColumnNames(string FormId);

        }
    }
