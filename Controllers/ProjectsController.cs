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

namespace sanyug_bugtracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {

            return View(db.Projects.ToList());
        }

        // GET: Projects
        [Authorize(Roles = "Admin, ProjectManager, Developer")]
        public ActionResult myProjects()
        {
            var user = db.Users.Find(User.Identity.GetUserId());


           

            //.Include(p => p.Name);
            //var PManagers = db.Roles.FirstOrDefault(r => r.Name == "ProjectManager");
            //ListPModel = db.Users.Where(u => u.Roles.Any(r => r.RoleId == user.Id)).ToList();
            //if (User.IsInRole("Developer"))
            //{
            //    var myProjectsForDev = db.Projects.Where(t => t.Tickets.Any(a => a.AssignedToId != user.Id));
            //    return View(myProjectsForDev.ToList());
            //}

            if (User.IsInRole("ProjectManager"))

            {
                var myProjects = db.Projects.Where(p => p.PManagerID == user.Id);
                return View(myProjects.ToList());
            }  

            return View(db.Projects.ToList());
        }

        //Get action for assign project 

        public ActionResult AssignProject(int id)
        {
            if (id != null)
            {
                ViewBag.PManagerID = new SelectList(db.Users, "Id", "FirstName");

                var ListPModel = new List<ApplicationUser>();

                ////var FindUser = db.Users.Find(id);
                var PModel = new ProjectManagerViewModel();
                //var PModel = new Projects();
                
                //var projects = new Projects();
                //UserRolesHelper userRole = new UserRolesHelper(db);
                PModel.Pid = id;
                var PManagers = db.Roles.FirstOrDefault(r => r.Name == "ProjectManager");
                ListPModel = db.Users.Where(u => u.Roles.Any(r => r.RoleId == PManagers.Id)).ToList();
                PModel.ProjectManager = new MultiSelectList(ListPModel, "id", "FirstName", PModel.SelectedProjectManager);
                return View(PModel);

            }
            
            
            return RedirectToAction("Index");
        }
        //Post Action for assign project
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AssignProject(ProjectManagerViewModel model, int projectId)
        {
            //var FindUserToPost = db.Users.Find(model.Id);
            //var projectID = db.Projects.Find(projectId);           
            ProjectHelper projectHelp = new ProjectHelper(db);
            //foreach (var RemoveProject in db.Projects.Select(r => r.Name).ToList())
            //{
            //    projectHelp.RemoveUserToProject(model.Id, projectId);
            //}
            //var ListPModel = new List<ApplicationUser>();
            //var PManagers = db.Roles.All(r => r.Name == "ProjectManager");
            var project = new Projects();

            

            foreach (var PMid in model.SelectedProjectManager)
            {
                project.PManagerID = PMid;
                
                projectHelp.AddUserToProject(PMid, projectId);
                //db.Projects.Add(project);
                //db.SaveChanges();
            }
             
            
            return RedirectToAction("Index", "Projects");
        }
        // Unassign Project  
        [Authorize(Roles = "Admin")]
        public ActionResult UnassignProject(int id)
        {
            if (id != null)
            {
                var ListPModel = new List<ApplicationUser>();
                //var FindUser = db.Users.Find(id);
                var PModel = new ProjectManagerViewModel();
                UserRolesHelper userRole = new UserRolesHelper(db);
                PModel.Pid = id;
                var PManagers = db.Roles.FirstOrDefault(r => r.Name == "ProjectManager");
                ListPModel = db.Users.Where(u => u.Roles.Any(r => r.RoleId == PManagers.Id)).ToList();
                PModel.ProjectManager = new MultiSelectList(ListPModel, "id", "FirstName", PModel.SelectedProjectManager);
                return View(PModel);
            }

            return RedirectToAction("Index");
        }
        //Post Action for unassign project
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult UnassignProject(ProjectManagerViewModel model, int projectId)
        {
            //var FindUserToPost = db.Users.Find(model.Id);
            //var projectID = db.Projects.Find(projectId);           
            ProjectHelper projectHelp = new ProjectHelper(db);
            //foreach (var RemoveProject in db.Projects.Select(r => r.Name).ToList())
            //{
            //    projectHelp.RemoveUserToProject(model.Id, projectId);
            //}
            var ListPModel = new List<ApplicationUser>();
            var PManagers = db.Roles.All(r => r.Name == "ProjectManager");
            var project = new Projects();

            foreach (var PMid in model.SelectedProjectManager)
            {
                projectHelp.RemoveUserFromProject(PMid, projectId);
                project.PManagerID = PMid;
                db.Projects.Add(project);
                db.SaveChanges();


            }
            return RedirectToAction("Index", "Projects");
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.RolesId = new SelectList(db.Roles, "Id", "FirstName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager,Developer")]
        public ActionResult Create([Bind(Include = "Id,Name")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                
                db.Projects.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }                      
            return View(projects);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager,Developer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projects);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = db.Projects.Find(id);
            db.Projects.Remove(projects);
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
