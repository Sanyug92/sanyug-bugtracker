using Microsoft.AspNet.Identity;
using sanyug_bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sanyug_bugtracker.Controllers
{ 
 [Authorize(Roles = "ProjectManager")]
 
    public class ProjectManagerController : Controller
    {
        // GET: ProjectManager
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {         
            IList<ApplicationUser> uList = new List<ApplicationUser>();
            UserRolesHelper RolesHelper = new UserRolesHelper(db);

            //foreach (var DevTeam in db.Users.ToList())
            //{
            //    //if(RolesHelper.IsUserInRole(DevTeam.Id, "Developer"))
            //    //uList.Add(DevTeam);

            //    var dev = db.Roles.FirstOrDefault(r => r.Name == "Developer");
            //    uList = db.Users.Where(u => u.Roles.Any(r => r.RoleId == dev.Id)).ToList();
            //}
            return View(uList);
            
        }

        // GET: ProjectManager/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProjectManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectManager/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectManager/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProjectManager/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProjectManager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjectManager/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
