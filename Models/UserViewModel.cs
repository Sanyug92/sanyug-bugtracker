using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sanyug_bugtracker.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string RoleId { get; set; }
        public MultiSelectList Roles { get; set; }
        public string[] SelectedRoles { get; set; }

      
    }
}