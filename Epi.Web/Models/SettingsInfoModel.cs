using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
    {
    public class SettingsInfoModel
        {
        private Dictionary<int,string> _FormControlNameList;
        private Dictionary<int, string> _SelectedControlNameList;
        private string _FormOwnerFirstName;
        private string _FormOwnerLastName;
        private bool _IsDraftMode;
        private string _FormName;
        private Dictionary<int, string> _AssignedUserList;
        private Dictionary<int, string> _UserList;
        private string _FormId;
        private bool _IsShareable;
        private Dictionary<int, string> _AvailableOrgList;
        private Dictionary<int, string> _SelectedOrgList;
        private int _SelectedDataAccessRule;
        private Dictionary<int, string> _DataAccessRuleIds;
        private string _DataAccessRuleDescription;
        private bool _HasDraftModeData;
        public string  DataAccessRuleDescription
        {
            get { return _DataAccessRuleDescription; }
            set { _DataAccessRuleDescription = value; }
        }
        public Dictionary<int, string> DataAccessRuleIds
        {
            get { return _DataAccessRuleIds; }
            set { _DataAccessRuleIds = value; }
        }

        public int SelectedDataAccessRule
        {
            get { return _SelectedDataAccessRule; }
            set { _SelectedDataAccessRule = value; }
        }
        public Dictionary<int, string> FormControlNameList
            {
            get { return _FormControlNameList; }
            set { _FormControlNameList = value; }
            }
        
        public Dictionary<int, string> SelectedControlNameList
            {
            get { return _SelectedControlNameList; }
            set { _SelectedControlNameList = value; }
            }
        
        public string FormOwnerFirstName
            {
            get { return _FormOwnerFirstName; }
            set { _FormOwnerFirstName = value; }
            }
         
        public string FormOwnerLastName
            {
            get { return _FormOwnerLastName; }
            set { _FormOwnerLastName = value; }
            }

        public bool IsDraftMode
            {
            get { return _IsDraftMode; }
            set { _IsDraftMode = value; }
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

        public string FormName
            {
            get { return _FormName; }
            set { _FormName = value; }
            }
        public string FormId
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

        
        public bool HasDraftModeData
        {
            get { return _HasDraftModeData; }
            set { _HasDraftModeData = value; }
        }
        }
    }