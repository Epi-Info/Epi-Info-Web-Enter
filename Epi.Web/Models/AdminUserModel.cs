using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using System.ComponentModel;

namespace Epi.Web.MVC.Models
{
    public class AdminUserModel
    {
         
        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [DisplayName("Address")]
        public string Address { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [DisplayName("DOB")]
        public DateTime DOB { get; set; }
        [DisplayName("Salary")]
        public decimal Salary { get; set; }
        [DisplayName("Turn On  Email Notifications")]
        [Required]
        public Boolean Notifications { get; set; }

        [DisplayName("Organization Name")]
        [Required(ErrorMessage = "Organization Name is required")]
        public string OrgName { get; set; }

        [DisplayName("Org Key")]
        [Required(ErrorMessage = "Org Key is required")]
        public string OrgKey { get; set; }

        [DisplayName("Expiry Date")]
        [Required(ErrorMessage = "Expiry Date is required")]
        public DateTime ExpDate { get; set; }

        [DisplayName("Host Organization")]
        [Required]
        public Boolean HostOrganization { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required")]
        public string AdminFirstName { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        public string AdminLastName { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Email is required")]
        public string AdminEmail { get; set; }
        [DisplayName("Turn On Email Notifications")]
        [Required]
        public Boolean AdminNotifications { get; set; }

        [DisplayName("Survey Title")]
        [Required(ErrorMessage = "Survey Title is required")]
        public string SurveyTitle { get; set; }

        [DisplayName("Survey Id")]
        [Required(ErrorMessage = "Survey Id is required")]
        public string SurveyId { get; set; }

        [DisplayName("Security Key")]
        [Required(ErrorMessage = "Security Key is required")]
        public string SecurityKey { get; set; }

        [DisplayName("Start Date-Time")]
        [Required(ErrorMessage = "Start Date-Time is required")]
        public DateTime StartDateTime { get; set; }

        [DisplayName("Closing Date-Time")]
        [Required(ErrorMessage = "Closing Date-Time is required")]
        public DateTime ClosingDateTime { get; set; }

    }
    public class Users
    {
        public Users()
        {
            _userList.Add(new AdminUserModel
            {
                FirstName = "Anuja",
                LastName = "Pawar",
                Address = "Indore MP",
                Email = "test@test.com",
                DOB = Convert.ToDateTime("6/22/1976"),
                Salary = 40000

            });
            _userList.Add(new AdminUserModel
            {
                FirstName = "Deerghika",
                LastName = "Pawar",
                Address = "Indore MP",
                Email = "test1@test.com",
                DOB = Convert.ToDateTime("7/11/2001"),
                Salary = 7000

            });
            _userList.Add(new AdminUserModel
            {
                FirstName = "Arnav",
                LastName = "Pawar",
                Address = "Indore MP",
                Email = "test2@test.com",
                DOB = Convert.ToDateTime("3/12/2010"),
                Salary = 5000

            });
        }

        public List<AdminUserModel> _userList = new List<AdminUserModel>();

        public void CreateUser(AdminUserModel userModel)
        {
            _userList.Add(userModel);
        }

        public void UpdateUser(AdminUserModel userModel)
        {
            foreach (AdminUserModel usrlst in _userList)
            {
                if (usrlst.Email == userModel.Email)
                {
                    _userList.Remove(usrlst);
                    _userList.Add(userModel);
                    break;
                }
            }
        }
        public AdminUserModel GetUser(string Email)
        {
            AdminUserModel usrMdl = null;

            foreach (AdminUserModel um in _userList)
                if (um.Email == Email)
                    usrMdl = um;

            return usrMdl;
        }


    }
}