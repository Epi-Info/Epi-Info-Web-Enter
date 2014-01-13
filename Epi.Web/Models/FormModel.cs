using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
    {
    public class FormModel
        {
        private List<FormInfoModel> _FormList;
        private string  _UserLastName;
        private string _UserFirstName;
        public List<FormInfoModel> FormList
            {
            get { return _FormList; }
            set { _FormList = value; }
            }
        public string UserLastName
            {
            get { return _UserLastName; }
            set { _UserLastName = value; }
            }
        public string UserFirstName
            {
            get { return _UserFirstName; }
            set { _UserFirstName = value; }
            }
        }
    }