using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
{
    public class FormResponseInfoModel
    {
        public List<ResponseInfoModel> ResponseInfoModels { get; set; }
        public FormInfoModel FormInfoModel { get; set; }

        public FormResponseInfoModel()
        {
            ResponseInfoModels = new List<ResponseInfoModel>();
            FormInfoModel = new FormInfoModel();
        }
    }
}