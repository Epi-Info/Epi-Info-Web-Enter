using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Epi.Web.Common.BusinessObject
    {
    public class FormInfoBO
        {
        private string _FormId;
        private string _FormNumber;
        private string _FormName;
        private int _UserId;
        private string _OrganizationName;
        private int _Organizationid;
        private bool _IsDraftMode;
        private bool _IsOwner;
        [DataMember]
        public string FormId
            {
            get { return _FormId; }
            set { _FormId = value; }
            }
        [DataMember]
        public string FormNumber
            {
            get { return _FormNumber; }
            set { _FormNumber = value; }
            }

        [DataMember]
        public string FormName
            {
            get { return _FormName; }
            set { _FormName = value; }
            }
        [DataMember]
        public string OrganizationName
            {
            get { return _OrganizationName; }
            set { _OrganizationName = value; }
            }
        [DataMember]
        public int OrganizationId
            {
            get { return _Organizationid; }
            set { _Organizationid = value; }
            }
        [DataMember]
        public bool IsDraftMode
            {
            get { return _IsDraftMode; }
            set { _IsDraftMode = value; }
            }

        [DataMember]
        public int UserId
            {
            get { return _UserId; }
            set { _UserId = value; }
            }
        [DataMember]
        public bool IsOwner
            {
            get { return _IsOwner; }
            set { _IsOwner = value; }
            }
        }
    }
