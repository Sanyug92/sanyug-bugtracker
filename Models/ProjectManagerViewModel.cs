using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sanyug_bugtracker.Models
{
    public class ProjectManagerViewModel
    {
        public string Id { get; set; }
        public int Pid { get; set; }
        public int Tid { get; set; }
        public ApplicationUser user { get; set; }
        public IList<string> Bugs { get; set; }
        public IdentityRole Name { get; set; }
        //public IList<string> AssignTickets { get; set; }
        public MultiSelectList Developers { get; set; }
        public string[] SelectedDevelopers { get; set; }
        public MultiSelectList ProjectManager { get; set; }
        public string[] SelectedProjectManager{ get; set; }
        public MultiSelectList MSTickets { get; set; }
        public IList<Tickets> SelectedTickets { get; set; }
        public string ProjectN { get; set; }
        
    }
}