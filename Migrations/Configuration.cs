namespace sanyug_Bugtracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using sanyug_bugtracker.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<sanyug_bugtracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(sanyug_bugtracker.Models.ApplicationDbContext context)
        {
  
        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "DemoAdmin"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoAdmin" });
            }


            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "DemoProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "DemoDeveloper"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoDeveloper" });
            }
            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            if (!context.Roles.Any(r => r.Name == "DemoSubmitter"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoSubmitter" });
            }



            //context.TicketPriorities.AddOrUpdate(
            //new TicketPriorities() {Name = "Low" },
            //new TicketPriorities() {Name = "Medium" },
            //new TicketPriorities() {Name = "High" }
            //);

            //context.TicketStatuses.AddOrUpdate(
            //new TicketStatuses() { Name = "Created" },
            //new TicketStatuses() { Name = "Assigned" },
            //new TicketStatuses() { Name = "In Progress" },
            //new TicketStatuses() { Name = "Resolved" }
            //);



            //context.TicketTypes.AddOrUpdate(
            // new TicketTypes() { Name = "User Interface Defects"},
            // new TicketTypes() { Name = "Boundary Related Defects" },
            // new TicketTypes() { Name = "Error Handling Defects" },
            // new TicketTypes() { Name = "Calculation Defects" },
            // new TicketTypes() { Name = "Improper Service Levels (Control flow defects)" },
            // new TicketTypes() { Name = " Interpreting Data Defects" },
            // new TicketTypes() { Name = " Race Conditions (Compatibility and Intersystem defects)" },
            // new TicketTypes() { Name = "Load Conditions (Memory Leakages under load)" },
            // new TicketTypes() { Name = "Hardware Failures"}
            //);



            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "sanyugichie@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sanyugichie@gmail.com",
                    Email = "sanyugichie@gmail.com",
                    FirstName = "Sanyu",
                    LastName = "Gichie",
                    DisplayName = "sgichie"
                }, "@33781slG@");
            }
            if (!context.Users.Any(u => u.Email == "Admin@demo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Admin@demo.com",
                    Email = "Admin@demo.com",
                    FirstName = "Addy",
                    LastName = "TheAdmin",
                    DisplayName = "Addy"
                }, "@33781slG@");
            }
            if (!context.Users.Any(u => u.Email == "Dev@demo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Dev@demo.com",
                    Email = "Dev@demo.com",
                    FirstName = "Denny",
                    LastName = "TheDeveloper",
                    DisplayName = "Denny"
                }, "@33781slG@");
            }

            if (!context.Users.Any(u => u.Email == "sub@demo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sub@demo.com",
                    Email = "sub@demo.com",
                    FirstName = "Subby",
                    LastName = "TheSubmitter",
                    DisplayName = "Subby"
                }, "@33781slG@");
            }

            if (!context.Users.Any(u => u.Email == "PM@demo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "PM@demo.com",
                    Email = "PM@demo.com",
                    FirstName = "Penny",
                    LastName = "TheProjectManager",
                    DisplayName = "sgichie"
                }, "@33781slG@");
            }

            if (!context.Users.Any(u => u.Email == "sanyugichie@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sanyugichie@gmail.com",
                    Email = "sanyugichie@gmail.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "J-Twich"
                }, "Abc@123!");
            }

            if (!context.Users.Any(u => u.Email == "sanyugichie@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sanyugichie@gmail.com",
                    Email = "sanyugichie@gmail.com",
                    FirstName = "Ernie",
                    LastName = "Campbell",
                    DisplayName = "E-Campbell"
                }, "Abc@123!");
            }

            if (!context.Users.Any(u => u.Email == "sanyugichie@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "sanyugichie@gmail.com",
                    Email = "sanyugichie@gmail.com",
                    FirstName = "Noah",
                    LastName = "King",
                    DisplayName = "N-King"
                }, "Abc@123!");
            }
            var userId = userManager.FindByEmail("sanyugichie@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            var ProjectMuserId = userManager.FindByEmail("PM@demo.com").Id;
            userManager.AddToRole(ProjectMuserId, "ProjectManager");

            var DeveloperuserId = userManager.FindByEmail("Dev@demo.com").Id;
            userManager.AddToRole(DeveloperuserId, "Developer");

            var SubmitteruserId = userManager.FindByEmail("sub@demo.com").Id;
            userManager.AddToRole(SubmitteruserId, "Submitter");
        }
    }
}
