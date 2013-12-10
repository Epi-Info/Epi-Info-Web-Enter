using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epi.Web.Common.BusinessObject;

namespace Epi.Web.Interfaces.DataInterface
    {
    public interface IFormInfoDao
        {
         List<FormInfoBO> GetFormInfo(int UserId);

         List<FormInfoBO> GetAssignedFormsInfo(List<Guid> FormIds);

         List<Guid> GetAssignedFormsId(int UserId);
              


        }
    }
