using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Common.DTO
    {
  public   class FormSettingDTO
        {
        private Guid _FormId;
        private Dictionary<int,string> _ColumnNameList;
        


        public Guid FormId
            {
            get { return _FormId; }
            set { _FormId = value; }
            }


        public Dictionary<int, string> ColumnNameList
            {
            get { return _ColumnNameList; }
            set { _ColumnNameList = value; }
            }


      
        }
    }
