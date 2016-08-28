using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sanyug_bugtracker.Models
{
    public class Projects
    {
        public Projects()
        {
            this.Tickets = new HashSet<Tickets>();
            this.Users = new HashSet<ApplicationUser>();
        }
        public int Id { get; set; }
        [AllowHtml]
        public string Name { get; set; }
        public string PManagerID { get; set; }
        

        public virtual ApplicationUser PManager { get; set; }
        public virtual ICollection<Tickets> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}