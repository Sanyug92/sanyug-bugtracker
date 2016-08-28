using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sanyug_bugtracker.Models
{
    public class AdminUserViewModel
    {
        public ApplicationUser user { get; set; }
        public IList<string> roles { get; set; }
        public IdentityRole Name { get; set; }
    }
}