using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.BusinessObject
    {
      [DataContract(Namespace = "http://www.yourcompany.com/types/")]
    public class FormSettingBO
        {
        private Guid _FormId;
        private Dictionary<int, string> _ColumnNameList;


         [DataMember]
        public Guid FormId
            {
            get { return _FormId; }
            set { _FormId = value; }
            }

        [DataMember]
        public Dictionary<int, string> ColumnNameList
            {
            get { return _ColumnNameList; }
            set { _ColumnNameList = value; }
            }



        }
    }
