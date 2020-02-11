using System;

namespace Epi.Web.MVC.Models
{
    public class User
    {
		public string _id { get; set; } // ": "org.couchdb.user:user1",
		public string _rev { get; set; } // ": "3-a8db5e34c488c3516220de7e033ba794",
		public string password_scheme { get; set; } // ": "pbkdf2",
		public string password { get; set; }
		public int? iterations { get; set; } // ": 10,
		public string name { get; set; } // ": "user1",
		public string[] roles { get; set; } /* ": ["abstractor", "form_designer"],*/
		public string type { get; set; } // ": "user",
		public string derived_key { get; set; } // ": "1e3ce523e927775812b51c76918c93b62bde4b4c",
		public string salt { get; set; } // ": "143c420b868e430cb929bd779b581a82"

		public bool is_active { get; set; } 
		public bool is_enabled { get; set; } 
		public string open_id { get; set; } 
		public string email { get; set; } 
		public string first_name { get; set; } 
		public string last_name { get; set; } 


		public User()
		{
			
		}
	}
}

