using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using System.ComponentModel;

namespace Epi.Web.MVC.Models
{
    public class UserModel
    {
         
       
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
       
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
      
        
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
         
    }
   
}