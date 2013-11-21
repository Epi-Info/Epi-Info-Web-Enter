﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
{
    public class FormInfoModel
    {
        private string _FormId;
        private string _FormNumber;
        private string _FormName;
        private Guid _UserId;
        private string _OrganizationName;
        private Guid _OrganizationKey;
        private bool _IsDraftMode;
        private bool _IsSelected;
        private bool _IsOwner;
        private string _CssClassName;

        public string FormId
        {
            get { return _FormId; }
            set { _FormId = value; }
        }
        public string FormNumber
        {
            get { return _FormNumber; }
            set { _FormNumber = value; }
        }

        public string FormName
        {
            get { return _FormName; }
            set { _FormName = value; }
        }
        public string OrganizationName
        {
            get { return _OrganizationName; }
            set { _OrganizationName = value; }
        }
        public Guid OrganizationKey
        {
            get { return _OrganizationKey; }
            set { _OrganizationKey = value; }
        }
        public bool IsDraftMode
        {
            get { return _IsDraftMode; }
            set { _IsDraftMode = value; }
        }
        public bool IsOwner
        {
            get { return _IsOwner; }
            set { _IsOwner = value; }
        }
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; }
        }
        public Guid UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        public string CssClassName 
        {
            get { return _CssClassName; }
            set { _CssClassName = value; }
        }
    }
}