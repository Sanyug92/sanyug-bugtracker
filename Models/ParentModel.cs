using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sanyug_bugtracker.Models
{
    public class ParentModel
    {
        public IEnumerable<Tickets> tickets { get; set; }
        public IEnumerable<ticketViewModel> ticketVM { get; set; }
        public IList<ApplicationUser> DevUser { get; set; }
    }
}