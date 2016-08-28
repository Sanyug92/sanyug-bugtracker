using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sanyug_bugtracker.Models
{
    public class TicketStatuses

    {
        public TicketStatuses()
        {
            //this.Tickets = new HashSet<Tickets>();
            //this.Users = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }       
        public string Name { get; set; }

        //public virtual ICollection<Tickets> Tickets { get; set; }
        //public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}