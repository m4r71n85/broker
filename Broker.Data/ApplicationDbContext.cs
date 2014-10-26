using Broker.Data.Migrations;
using Broker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Broker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public IDbSet<Agency> Agencies { get; set; }
        public IDbSet<AgencyCandidacy> AgencyCandidacies { get; set; }
        public IDbSet<Mail> Mails { get; set; }
    }
}
