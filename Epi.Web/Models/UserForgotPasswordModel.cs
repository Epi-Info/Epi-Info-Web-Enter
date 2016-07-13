using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Epi.Web.MVC.Models
{
    public class UserForgotPasswordModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "The email address you entered is not in the proper format.")]
        public string UserName { get; set; }
    }
}