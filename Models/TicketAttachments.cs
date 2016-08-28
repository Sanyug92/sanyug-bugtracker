using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sanyug_bugtracker.Models
{
    public class TicketAttachments
    {

        public int Id { get; set; }
        public int TicketId { get; set; }       
        public string FilePath { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public string UserId { get; set; }
        public string FileUrl { get; set; }

       
        public virtual Tickets Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}