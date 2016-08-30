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


namespace sanyug_bugtracker.Controllers
{
    //[RequireHttps]
    public class TicketHistoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketHistories
        public ActionResult Index()
        {

            var ticketHistories = db.TicketHistories.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketHistories.ToList());


        }

        // GET: TicketHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistories ticketHistories = db.TicketHistories.Find(id);
            if (ticketHistories == null)
            {
                return HttpNotFound();
            }
            return View(ticketHistories);
        }

        // GET: TicketHistories/Create
        public ActionResult Create()
        {       
            ViewBag.UserId = db.Users.Find(User.Identity.GetUserId());
            //tickets = db.Tickets.Find(tId);
            //var user = db.Users.Find(User.Identity.GetUserId());     
            return View();
        }

        // POST: TicketHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,Property,OldValue,NewValue,Changed,UserId")] TicketHistories ticketHistories, Tickets tickets)
        {

                if (ModelState.IsValid)
                {

                    db.TicketHistories.Add(ticketHistories);
                    db.SaveChanges();
                    return RedirectToAction("myIndex", "Tickets");
                }

        
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketHistories.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketHistories.UserId);
            return View(ticketHistories);
        }

        //// POST: TicketHistories/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,TicketId,Property,OldValue,NewValue,Changed,UserId")] ticketViewModel model, Tickets tickets, TicketHistories ticketHistories)
        //{
        //    //tickets = db.Tickets.Find(tId);

        //    //if (!(tickets.Title == model.Title) || (tickets.TicketPriorityId == model.TicketPriorityId) || (tickets.TicketStatusId == model.TicketStatusId)
        //    //    || (tickets.AssignedToId == model.AssignedToId) || (tickets.ProjectId == model.ProjectId) || (tickets.OwnerUserId == model.OwnerUserId))
        //    //    {

        //        ticketHistories.Changed = true;

        //        //}

        //    if (ticketHistories.Changed == true)
        //    {
                
        //        if (ModelState.IsValid)
        //        {
                    
        //            db.TicketHistories.Add(ticketHistories);
        //            db.SaveChanges();
        //            return RedirectToAction("myIndex", "Tickets");
        //        }
        //    }
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketHistories.TicketId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketHistories.UserId);
        //    return View(ticketHistories);
        //}


        // GET: TicketHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistories ticketHistories = db.TicketHistories.Find(id);
            if (ticketHistories == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketHistories.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketHistories.UserId);
            return View(ticketHistories);
        }

        // POST: TicketHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,Property,OldValue,NewValue,Changed,UserId")] TicketHistories ticketHistories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketHistories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketHistories.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketHistories.UserId);
            return View(ticketHistories);
        }

        // GET: TicketHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistories ticketHistories = db.TicketHistories.Find(id);
            if (ticketHistories == null)
            {
                return HttpNotFound();
            }
            return View(ticketHistories);
        }

        // POST: TicketHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketHistories ticketHistories = db.TicketHistories.Find(id);
            db.TicketHistories.Remove(ticketHistories);
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
