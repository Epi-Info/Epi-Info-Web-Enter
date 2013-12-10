using System;
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
        private int _UserId;
        private string _OrganizationName;
        private int _OrganizationId;
        private bool _IsDraftMode;
        private bool _IsOwner;

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
        public int OrganizationId
        {
            get { return _OrganizationId; }
            set { _OrganizationId = value; }
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
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
    }
}
