using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityTest.Models;
using GDPR.Model;
using System.Linq;

namespace IdentityTest.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>//, IGDPRDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public IQueryable<ApplicationUser> ApplicationUserPurpose(string purpose)
        {
            var p = this.Purposes.FirstOrDefault(x => x.Title.ToLower() == purpose.ToLower());
            return this.ApplicationUser.Where(x => x.Consents.Select(c => c.Id).Contains(p.Id));
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<Consent> Consents { get; set; }

        public DbSet<Purpose> Purposes { get; set; }

        public string Purpose { get; set; }
    }
}
