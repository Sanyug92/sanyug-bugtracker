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
using System.IO;

namespace sanyug_bugtracker.Controllers
{
    [RequireHttps]
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachments ticketAttachments = db.TicketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Create
        public ActionResult Create(Tickets tickets)
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");

            var ticketAttachment = new TicketAttachments();
            var bp = db.Tickets.Find(ticketAttachment.TicketId);
            //var project = new Projects();
            //Tickets tickets = db.Tickets.Find(id);
            //ticket.TicketId = id;
            //ticket.UserId = User.Identity.GetUserId();

            var user = db.Users.Find(User.Identity.GetUserId());
            if (User.IsInRole("Developer"))
            {

                if (!(tickets.AssignedToId == user.Id))
                {
                    return RedirectToAction("Login", "Account");
                }
            }

            if (User.IsInRole("ProjectManager"))
            {
              
                if (!(tickets.Project.PManagerID == user.Id))
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            if (User.IsInRole("Submitter"))
            {

                if (!(tickets.OwnerUserId == user.Id))
                {
                    return RedirectToAction("Login", "Account");
                }
            }

            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");


            //return View(ticket);
            if (!User.Identity.IsAuthenticated)
            {

                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Created,TicketId,UserId,FileUrl")] TicketAttachments ticketAttachments, HttpPostedFileBase image /*int id*/)
        { 
            if (image != null && image.ContentLength > 0)
            {
                //check the file name to make sure its an image
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                    ModelState.AddModelError("image", "Invalid Format.");
            }

       

            if (image != null)
            {
                //relative server path
                var filePath = "/Uploads/";
                // path on physical drive on server
                var absPath = Server.MapPath("~" + filePath);
                // media url for relative path
                ticketAttachments.FileUrl = filePath + image.FileName;
                //save image
                image.SaveAs(Path.Combine(absPath, image.FileName));
            }
            

            if (ModelState.IsValid)
            {
                ticketAttachments.UserId = User.Identity.GetUserId();
                ticketAttachments.Created = DateTime.UtcNow;
                var T = db.Tickets.Find(ticketAttachments.TicketId);
                var user = db.Users.Find(User.Identity.GetUserId());
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
                db.TicketAttachments.Add(ticketAttachments);
              
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { Id = T.Id });
            }
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachments ticketAttachments = db.TicketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachments.UserId);
            return View(ticketAttachments);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,FilePath,Description,Created,UserId,FileUrl")] TicketAttachments ticketAttachments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachments.UserId);
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachments ticketAttachments = db.TicketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachments);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachments ticketAttachments = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachments);
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
