using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
{
    public class FormResponseInfoModel
    {
        public FormInfoModel FormInfoModel;
        public List<ResponseModel> ResponsesList;
        public List<string> ColumnNames;
        public List<KeyValuePair<int, string>> Columns;


        public FormResponseInfoModel()
        {
            FormInfoModel = new FormInfoModel();
            ResponsesList = new List<ResponseModel>();
            ColumnNames = new List<string>();
            Columns = new List<KeyValuePair<int, string>>();
        }
    }

}