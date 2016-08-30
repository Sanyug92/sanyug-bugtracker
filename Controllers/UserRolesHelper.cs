using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using sanyug_bugtracker;
using sanyug_bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace sanyug_bugtracker.Controllers
{
    public class UserRolesHelper
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public UserRolesHelper(ApplicationDbContext context)
        {
                userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
                roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            db = context;

        }

        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
      
        }

        public IList<string> ListUseRoles(string userId)
        {
            return userManager.GetRoles(userId);

        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }
        public bool AddUserToRoleid(string userId, string roleId)
        {
            var result = userManager.AddToRole(userId, roleId);
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userid, string roleName)
        {
            var result= userManager.RemoveFromRole(userid, roleName);
            return result.Succeeded;
        }

        public IList<ApplicationUser>UsersInRole(string roleName)
        {
            var UserIDs = roleManager.FindByName(roleName).Users.Select(r => r.UserId);
            return userManager.Users.Where(u => UserIDs.Contains(u.Id)).ToList();

        }
        public IList<ApplicationUser>UserNotInRole(string roleName)
        {
            var userIDs = System.Web.Security.Roles.GetUsersInRole(roleName);
            return userManager.Users.Where(u  => !userIDs.Contains(u.Id)).ToList();
        }
    }
}