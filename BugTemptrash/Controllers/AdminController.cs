using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using sanyug_bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sanyug_bugtracker.Controllers
{
    [Authorize(Roles = "Admin")]
    //[RequireHttps]
    public class AdminController : Controller
    {
        // GET: Admin
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            List<AdminUserViewModel> roleusers = new List<AdminUserViewModel>();
            UserRolesHelper userRole = new UserRolesHelper(db);
            var AdminModel = new UserViewModel();
            foreach (var RoleUser in db.Users.ToList())
            {
                var roleuser = new AdminUserViewModel();
                roleuser.roles = userRole.ListUseRoles(RoleUser.Id);
                roleuser.Name = db.Roles.Find(roleuser.roles.FirstOrDefault());
                roleuser.user = RoleUser;
                roleusers.Add(roleuser);
               
            }


            return View(roleusers);
        }
        //Get action for userrole change
        [Authorize(Roles = "Admin")]
        public ActionResult ChangeRole(string id)
        {
            if(id != null)
            { 
            var FindUser = db.Users.Find(id);
            var RolesRoom = new UserViewModel();
            UserRolesHelper userRole = new UserRolesHelper(db);
            RolesRoom.FirstName = FindUser.FirstName;
            RolesRoom.LastName = FindUser.LastName;
            RolesRoom.Id = FindUser.Id;
            RolesRoom.SelectedRoles = userRole.ListUseRoles(id).ToArray();
            RolesRoom.Roles = new MultiSelectList(db.Roles, "Name", "Name", RolesRoom.SelectedRoles);
            return View(RolesRoom);
            }
            var projects = db.Projects.Include(t => t.Name).Include(t => t.Id);    

            return RedirectToAction("Index");
        }
        //Post Action for User Roles
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public ActionResult ChangeRole(UserViewModel model)
        {
            var FindUserToPost = db.Users.Find(model.Id);
            UserRolesHelper userRolePost = new UserRolesHelper(db);
            foreach(var RemoveRole in db.Roles.Select(r => r.Name).ToList())
            {
                userRolePost.RemoveUserFromRole(FindUserToPost.Id, RemoveRole);
            }
            foreach (var AddRoles in model.SelectedRoles)
            {
                userRolePost.AddUserToRole(FindUserToPost.Id, AddRoles);
            }
            
            return RedirectToAction("Index", "Admin");
        }

    

        // GET: Admin/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
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

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
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
