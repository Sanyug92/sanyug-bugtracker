using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sanyug_bugtracker.Models
{
    public class ticketViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public int? TicketTypeId { get; set; }
        public int? TicketPriorityId { get; set; }
        public int? TicketStatusId { get; set; }
        public string OwnerUserId { get; set; }
        public string AssignedToId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

    }
}