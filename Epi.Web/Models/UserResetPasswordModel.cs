using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Epi.Web.MVC.Models
{
    public class UserResetPasswordModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}