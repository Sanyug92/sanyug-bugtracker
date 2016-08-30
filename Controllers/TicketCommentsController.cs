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
using System.Threading.Tasks;

namespace sanyug_bugtracker.Controllers
{
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketComments
        public ActionResult Index()
        {
            var ticketComments = db.TicketComments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketComments.ToList());
        }

        // GET: TicketComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComments ticketComments = db.TicketComments.Find(id);
            if (ticketComments == null)
            {
                return HttpNotFound();
            }
            return View(ticketComments);
        }

        // GET: TicketComments/Create
        [HttpGet]        
        public ActionResult Create()
        {
            //var ticket = new TicketComments();
            //var project = new Projects();
            
            //ticket.TicketId = id;
            //ticket.UserId = User.Identity.GetUserId();
            
            var user = db.Users.Find(User.Identity.GetUserId());
            var ticketComments = new TicketComments();
            var T = db.Tickets.Find(ticketComments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");

            if (User.IsInRole("Developer"))
            {

                if (!(T.AssignedToId == user.Id))
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            if (User.IsInRole("ProjectManager"))
            {

                if (!(T.Project.PManagerID == user.Id))
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            if (User.IsInRole("Submitter"))
            {

                if (!(T.OwnerUserId == user.Id))
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");

            //return View(ticket);
            if (!User.Identity.IsAuthenticated) {

                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Comment,Created,TicketId,UserId")] TicketComments ticketComments /*int id*/)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                var T = db.Tickets.Find(ticketComments.TicketId);


                ticketComments.UserId = User.Identity.GetUserId();
                ticketComments.Created = DateTime.UtcNow;
                if (User.IsInRole("Developer"))
                {

                    if (!(T.AssignedToId == user.Id))
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                if (User.IsInRole("ProjectManager"))
                {

                    if (!(T.Project.PManagerID == user.Id))
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                if (User.IsInRole("Submitter"))
                {

                    if (!(T.OwnerUserId == user.Id))
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }
                db.TicketComments.Add(ticketComments);              
                db.SaveChanges();


                var svc = new EmailService();
                var msg = new IdentityMessage();
                msg.Subject = "New Comment";
                msg.Body = "\r\n You have a new Attachment on the ticket titled," + ticketComments.Ticket.Title + "with the following description:" + ticketComments.Ticket.Description + "\r\n";


                msg.Destination = ticketComments.Ticket.AssignedToId;

                await svc.SendAsync(msg);

                return RedirectToAction("Details", "Tickets", new { Id = T.Id });
                //ticketComments.TicketId = id;


                //ticketComments.Created = DateTime.Now;

                //db.TicketComments.Add(ticketComments);
                //db.SaveChanges();               
                //return RedirectToAction("Details", "Tickets", new { id = id });
            }

            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComments.TicketId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketComments.UserId);
            return View(ticketComments);
        }

        // GET: TicketComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComments ticketComments = db.TicketComments.Find(id);
            if (ticketComments == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketComments.UserId);
            return View(ticketComments);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Comment,Created,TicketId,UserId")] TicketComments ticketComments)
        {
            if (ModelState.IsValid)
            {
              
                db.Entry(ticketComments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("details", "Tickets", new {id = ticketComments.TicketId });
            }

            var svc = new EmailService();
            var msg = new IdentityMessage();
            msg.Subject = "Comment Edited";
            msg.Body = "\r\n You have a new edit on the ticket titled," + ticketComments.Ticket.Title + "with the following description:" + ticketComments.Ticket.Description + "\r\n";

            
            msg.Destination = ticketComments.Ticket.AssignedToId;

            await svc.SendAsync(msg);

            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketComments.UserId);
            return View(ticketComments);
        }

        // GET: TicketComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComments ticketComments = db.TicketComments.Find(id);
            if (ticketComments == null)
            {
                return HttpNotFound();
            }
            return View(ticketComments);
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketComments ticketComments = db.TicketComments.Find(id);
            db.TicketComments.Remove(ticketComments);
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
