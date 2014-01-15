using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
    {
    public class SettingsInfoModel
        {
        private Dictionary<int,string> _FormControlNameList;
        public Dictionary<int, string> FormControlNameList
            {
            get { return _FormControlNameList; }
            set { _FormControlNameList = value; }
            }
        private Dictionary<int, string> _SelectedControlNameList;
        public Dictionary<int, string> SelectedControlNameList
            {
            get { return _SelectedControlNameList; }
            set { _SelectedControlNameList = value; }
            }
        }
    }