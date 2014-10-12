namespace Broker.Data.Migrations
{
    using Broker.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;

    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Broker.Data.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            SeedRoles(context);
            SeedUsers(context);
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "admin" };

                manager.Create(user, "123456");
                manager.AddToRole(user.Id, "admin");
            }
        }

        private void SeedRoles(ApplicationDbContext context)
        {
            if (!context.Roles.Any(c => c.Name == "admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                manager.Create(new IdentityRole { Name = "admin" });
                manager.Create(new IdentityRole { Name = "company participant" });
                manager.Create(new IdentityRole { Name = "company creator" });
            }
        }
    }
}
