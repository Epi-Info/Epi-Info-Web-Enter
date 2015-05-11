using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Enter.Common.DTO
    {
  public   class FormSettingDTO
        {
       
        private Dictionary<int,string> _ColumnNameList;
        private Dictionary<int, string> _FormControlNameList;
        private Dictionary<int, string> _AssignedUserList;
        private Dictionary<int, string> _UserList;
        private string _FormId;
        private bool _IsShareable;
        private Dictionary<int, string> _AvailableOrgList;
        private Dictionary<int, string> _SelectedOrgList;
        private bool _ShowAllRecords;
    
        public bool ShowAllRecords
        {
            get { return _ShowAllRecords; }
            set { _ShowAllRecords = value; }
        }
        public Dictionary<int, string> ColumnNameList
            {
            get { return _ColumnNameList; }
            set { _ColumnNameList = value; }
            }
        public Dictionary<int, string> FormControlNameList
            {
            get { return _FormControlNameList; }
            set { _FormControlNameList = value; }
            }

        public Dictionary<int, string> AssignedUserList
            {
            get { return _AssignedUserList; }
            set { _AssignedUserList = value; }
            }
        public Dictionary<int, string> UserList
            {
            get { return _UserList; }
            set { _UserList = value; }
            }

        public   string FormId
            {
            get { return _FormId; }
            set { _FormId = value; }
            }
      
        public bool IsShareable
        {
            get { return _IsShareable; }
            set { _IsShareable = value; }
        }
    
        public Dictionary<int, string> AvailableOrgList
        {
            get { return _AvailableOrgList; }
            set { _AvailableOrgList = value; }
        }
        public Dictionary<int, string> SelectedOrgList
        {
            get { return _SelectedOrgList; }
            set { _SelectedOrgList = value; }
        }
        }
    }
