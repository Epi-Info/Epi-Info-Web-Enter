using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Epi.Web.MVC.Models
{
    public class UserLoginModel
    {

        private string _UserName;
        private string _Password;
		private bool _SAMS;

        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "The email address you entered is not in the proper format.")]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value.Trim(); ; }
        }
        [Required(ErrorMessage = "Password is required.")]
        public string Password
        {
            get { return _Password; }
            set { _Password = value.Trim();  }
        }
		public bool SAMS
		{
			get =>  true;
			set { _SAMS = value; }
		}

		public bool ViewValidationSummary { get; set; }
    }
}