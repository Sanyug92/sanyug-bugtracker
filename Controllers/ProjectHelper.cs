﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using sanyug_bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sanyug_bugtracker.Controllers
{
    /// <summary>
    ///  Code  for assigning tickets to Developers by PM and assign projects to PM by Admin
    /// </summary>
    /// <autogeneratedoc />
    /// 
    public class ProjectHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();       
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;


        public ProjectHelper(ApplicationDbContext context)
        {
            userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(context));
            roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(context));
            db = context;

        }

        public void AddUserToProject(string userId, int projectId)
        {
            var getProject = db.Projects.Find(projectId);

            getProject.Users.Add(db.Users.Find(userId));         
            
            db.SaveChanges();
        }
        public void RemoveUserFromProject(string userId, int projectId)
        {
            
            
            var getProject = db.Projects.Find(projectId);
            //getProject.PManagerID = userId;

            getProject.Users.Remove(db.Users.Find(userId));
            db.SaveChanges();

        }
        //public bool RemoveUserFromProject(string userid, int projectId)
        //{
        //    var result = userManager.RemoveFromRole(userid, projectId);
        //    return result.Succeeded;
        //}
        public void AddDevToTicket(string userId, int tId)
        {

            var getUser = db.Users.Find(userId);
            var getTickets = db.Tickets.Find(tId);

            if(getTickets.AssignedToId != userId)
            {
                getTickets.AssignedToId = userId;
            }
            
            //db.Tickets.Add(getTickets);

            db.SaveChanges();
        }
        public void RemoveT(int tId, string userId)
        {
            //var getUser = db.Users.Find(userId);
            var getTickets = db.Tickets.Find(tId);

            if (getTickets.AssignedToId != userId)
            {
                db.Tickets.Remove(getTickets);
            }        
            db.SaveChanges();
        }
        public void AddHistory(int tId, string userID, string old, string newVal, string property)
        {

            TicketHistories TH = new TicketHistories();
            TH.TicketId = tId;
            TH.UserId = userID;
            TH.OldValue = old;
            TH.NewValue = newVal;
            TH.Property = property;

            db.TicketHistories.Add(TH);
            db.SaveChanges();

        }
        public void AddNoty(int tId, string userID, bool open)
        {

            TicketNotifications noty = new TicketNotifications();
            noty.TicketId = tId;
            noty.UserId = userID;
            noty.open = open;

            db.TicketNotifications.Add(noty);
            db.SaveChanges();
        }

        public IList<ApplicationUser> UsersInRole(string roleName)
        {
            var UserIDs = roleManager.FindByName(roleName).Users.Select(r => r.UserId);
            return userManager.Users.Where(u => UserIDs.Contains(u.Id)).ToList();

        }
        public IList<ApplicationUser> UserNotInRole(string roleName)
        {
            var userIDs = System.Web.Security.Roles.GetUsersInRole(roleName);
            return userManager.Users.Where(u => !userIDs.Contains(u.Id)).ToList();
        }
    }
}