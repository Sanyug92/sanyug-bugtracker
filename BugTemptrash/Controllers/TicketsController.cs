using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sanyug_bugtracker.Models;
using Microsoft.AspNet.Identity;
//using sanyug_Bugtracker;
using System.Web.Security;
using System.Threading.Tasks;

namespace sanyug_bugtracker.Controllers
{
    //[RequireHttps]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       
        [Authorize(Roles = "Admin, DemoAdmin, ProjectManager, DemoProjectManager, Developer, DemoDeveloper, Submitter, DemoSubmitter")]
        public ActionResult Index()
        {

            var tickets = db.Tickets.Include(t => t.AssignedTo).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);        
            return View(tickets.ToList());
        }
        [Authorize(Roles = "Admin, DemoAdmin, ProjectManager, DemoProjectManager, Developer, DemoDeveloper, Submitter, DemoSubmitter")]
        public ActionResult myIndex()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
           
            if (User.IsInRole("Submitter"))
            {
                var myT = db.Tickets.Where(t => t.OwnerUserId == user.Id).Include(t => t.AssignedTo).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
                return View(myT.ToList());
            }
            else if (User.IsInRole("Developer, DemoDeveloper"))
            {
                var myTicForDev = db.Tickets.Where(t => t.AssignedToId == user.Id).Include(t => t.AssignedTo).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
                return View(myTicForDev.ToList());
            }
            else if (User.IsInRole("ProjectManager, DemoProjectManager"))
            {
                var parentM = new ParentModel();
                var Devs = db.Roles.FirstOrDefault(r => r.Name == "Developer");
                var uList = db.Users.Where(u => u.Roles.Any(r => r.RoleId == Devs.Id)).ToList();
                ViewBag.Developer = new SelectList(uList, "Id", "FirstName");
                var myTicForPM = db.Tickets.Where(u => u.Project.PManagerID == user.Id).Include(t => t.AssignedTo).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
                parentM.DevUser = uList;
                parentM.tickets = myTicForPM;
                return View(parentM);
            }
           
            //ViewData["Developer"] = new SelectList(uList, "Id", "Name");
            var tickets = db.Tickets.Include(t => t.AssignedTo).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());

        }


        public ActionResult DevProjects()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            //var myProjectsForDev = db.Tickets.Where(t => t.Project.Users.Any(a => a.Id != user.Id));
            //return View(myProjectsForDev.ToList());

            var myTicForDev = db.Tickets.Where(t => t.AssignedToId != user.Id).Where(p =>p.Project.Tickets.Any(a => a.AssignedToId != user.Id)).Include(t => t.AssignedTo).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            //var myTicForDev = db.Project.Tickets.Where(t => t.Tickets.Any(a => a.AssignedToId != user.Id)).Include(t => t.AssignedTo).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(myTicForDev.ToList());
        }

        // Test two models
        //public ActionResult MyModels()
        //{
        //    var Tickets = new List<Tickets>();
        //    Tickets = db.Tickets.ToList();
        //    var ProjectM = new List<ProjectManagerViewModel>();

        //    return View("myview", new ParentModel { Tickets = Tickets, ProjectM = ProjectM });


        //}

        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        public ActionResult myUnassignedT()
        {
            var tickets = db.Tickets.Where(t => t.AssignedToId == null).Include(t => t.AssignedTo).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());

        }


        //GET: Get List of Developer
        [Authorize(Roles = "ProjectManager, Admin") ]
        [HttpGet]
        public ActionResult AssignTicket(int id, int pId)
        {
            //List<ProjectManagerViewModel> ListPMModel = new List<ProjectManagerViewModel>();
            var PModel = new ProjectManagerViewModel();
            var project = new Projects();
            var  uList = new List<ApplicationUser>();
            if (id != null)
            {
            var currTic = db.Tickets.Find(id);
                var currproject = db.Projects.Find(pId);
                PModel.Tid = id;
                PModel.Pid = pId;
                project.Id = pId;
                var Devs = db.Roles.FirstOrDefault(r => r.Name == "Developer");
                uList = db.Users.Where(u => u.Roles.Any(r => r.RoleId == Devs.Id)).ToList();
                PModel.Developers = new MultiSelectList(uList, "id", "FirstName", PModel.SelectedDevelopers);
                return View(PModel);

             

            }
            var Devss = db.Roles.FirstOrDefault(r => r.Name == "Developer");
            ViewBag.Developer = new SelectList(Devss.Id, "Id", "Name");
            ViewData["Developer"] = new SelectList(Devss.Id, "Id", "Name");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "ProjectManager, Admin")]
        public async Task<ActionResult> AssignTicket(ProjectManagerViewModel model, int tId, int pId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var PModel = new ProjectManagerViewModel();
            ProjectHelper projectHelp = new ProjectHelper(db);
            model.Pid = pId;
            var ticket = db.Tickets.Find(tId);
            var currDev = db.Users.Find(model.Id);

            if (ModelState.IsValid)
            {
               
                foreach (var DevId in model.SelectedDevelopers)
                {
                    projectHelp.AddDevToTicket(DevId, tId);

                    projectHelp.RemoveT(tId, DevId);


                    //Send Email
                    var svc = new EmailService();
                    var msg = new IdentityMessage();
                    msg.Subject = "New Ticket Assignment";
                    msg.Body = "\r\n You have recieved a new ticket assignment," + ticket.Title + "with the follow description" + ticket.Description + "and Id," + ticket.Id + "\r\n";
                             
                    msg.Destination = currDev.Email;

                    await svc.SendAsync(msg);
                }
            }
            var Devs = db.Roles.FirstOrDefault(r => r.Name == "Developer");
            ViewBag.Developer = new SelectList(Devs.Id, "Id", "Name");
            //ViewBag.AssignedToId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToId);
            return RedirectToAction("Details", "Projects", new { id = pId } );



        }
        [HttpPost]
        [Authorize(Roles = "ProjectManager, Admin")]
        public ActionResult AssignTickets(string userid, int tId, int pId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            
            ProjectHelper projectHelp = new ProjectHelper(db);
  
                
                    projectHelp.AddDevToTicket(userid, tId);

                    projectHelp.RemoveT(tId, userid);

            //var Devs = db.Roles.FirstOrDefault(r => r.Name == "Developer");
            //ViewBag.Developer = new SelectList(Devs.Id, "Id", "Name");
            ////ViewBag.AssignedToId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToId);
            //return RedirectToAction("Details", "Projects", new { id = pId });

            return Content(userid + " " + tId + " " + pId);


        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }

            var user = db.Users.Find(User.Identity.GetUserId());
            //ViewData["ownerTo"] = tickets.OwnerUserId;
            //ViewData["AssignedTo"] = tickets.AssignedToId;
            ViewBag.userid = user.Id;
            return View(tickets);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter, Admin, ProjectManager, Developer")]
        public ActionResult Create()
        {
            //ViewBag.AssignedToId = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.OwnerUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Submitter, Admin, ProjectManager, Developer")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToId")] Tickets tickets, UserViewModel model)
        {
            var currUser = db.Users.Find(model.Id);

            var currproject = db.Projects.Find(tickets.ProjectId);
            

            if (ModelState.IsValid)
            {
                //tickets.OwnerUser = db.Users.Find(model.FirstName + " " + model.LastName);
                //tickets.AssignedToId = User.Identity.GetUserId();
                tickets.OwnerUserId = User.Identity.GetUserId();
                tickets.Created = DateTime.Now;
                db.Tickets.Add(tickets);
                db.SaveChanges();

                //Send Email
                var svc = new EmailService();
                var msg = new IdentityMessage();
                msg.Subject = "New Ticket Created";
                msg.Body = "\r\n You have a new ticket from," + tickets.OwnerUser.FirstName + "titled,"+ tickets.Title + "with the follow description:" + tickets.Description + "and Id," + tickets.Id + "\r\n";

                msg.Destination = currproject.PManager.Email;

                await svc.SendAsync(msg);

                var bp = db.Projects.Find(tickets.ProjectId);
                return RedirectToAction("myIndex", "Tickets");
            }

           


            //ViewBag.AssignedToId = new SelectList(db.ApplicationUsers, "Id", "FirstName", tickets.AssignedToId);
            //ViewBag.OwnerUserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.AssignedToId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToId);
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
           //if(User.IsInRole("Submitter, Admin, Developer, ProjectManager"))
           // {
           //     return View("myIndex", "Tickets");
           // }

            return View(tickets);
        }

        // GET: Tickets/Edit/5
       
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tickets tickets = db.Tickets.Find(id);
          
           
            if (tickets == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AssignedToId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToId);
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);
            if (User.IsInRole("Developer"))
            {
                
                if (!(tickets.AssignedToId == user.Id))
                {
                    return RedirectToAction("Login","Account");
                }
            }
            if (User.IsInRole("Submitter"))
            {

                if (!(tickets.OwnerUserId == user.Id))
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            if (!User.Identity.IsAuthenticated)
            {
           
                   return RedirectToAction("Login", "Account");
                
            }

            return View(tickets);      
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToId")] Tickets model)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ProjectHelper HistoryHelp = new ProjectHelper(db);
         
            var tickets = db.Tickets.Find(model.Id);
            var noty = new TicketNotifications();
            noty.open = false;

            if (!(tickets.Title == model.Title) || (tickets.TicketPriorityId == model.TicketPriorityId) || (tickets.TicketStatusId == model.TicketStatusId)
                || (tickets.AssignedToId == model.AssignedToId) || (tickets.ProjectId == model.ProjectId) || (tickets.TicketTypeId == model.TicketTypeId))
            {

                HistoryHelp.AddHistory(tickets.Id, user.Id,tickets.Title,model.Title, "Title");
                HistoryHelp.AddHistory(tickets.Id, user.Id, tickets.TicketPriorityId.ToString(), model.TicketPriorityId.ToString(), "TicketPriority");
                HistoryHelp.AddHistory(tickets.Id, user.Id, tickets.TicketStatusId.ToString(), model.TicketStatusId.ToString(), "TicketStatus");
                HistoryHelp.AddHistory(tickets.Id, user.Id, tickets.TicketTypeId.ToString(), model.TicketTypeId.ToString(), "TicketType");
                HistoryHelp.AddHistory(tickets.Id, user.Id, tickets.ProjectId.ToString(), model.ProjectId.ToString(), "Project");
                HistoryHelp.AddHistory(tickets.Id, user.Id, tickets.Description, model.Description, "Description");
                HistoryHelp.AddHistory(tickets.Id, user.Id, tickets.AssignedToId, model.AssignedToId, "AssignedTo");
                HistoryHelp.AddNoty(tickets.Id, user.Id, noty.open);
            }


            tickets.TicketPriorityId = model.TicketPriorityId;
            tickets.TicketStatusId = model.TicketStatusId;
            tickets.OwnerUserId = user.Id;
            tickets.Created = tickets.Created = DateTime.Now;
            db.Tickets.Add(model);
            //tickets.Id = model.Id;
            var currproject = db.Projects.Find(tickets.ProjectId);
            

            if (ModelState.IsValid)
            {
               
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();

                //Send Email
                var svc = new EmailService();
                var msg = new IdentityMessage();
                msg.Subject = "Edits";
                msg.Body = "\r\n You're ticket titled," + tickets.Title + "was edited." + "\r\n";

                msg.Destination = currproject.Users.Select(u => u.Email).ToString();

                await svc.SendAsync(msg);

                return RedirectToAction("myIndex", "Tickets");
               
            }
            //ViewBag.AssignedToId = new SelectList(db.Users, "Id", "FirstName", tickets.AssignedToId);
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", tickets.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", tickets.TicketTypeId);


            return View("myIndex","Tickets");
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tickets tickets = db.Tickets.Find(id);
            db.Tickets.Remove(tickets);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
