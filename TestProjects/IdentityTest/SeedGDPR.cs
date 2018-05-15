using GDPR.Model;
using IdentityTest.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace GDPR
{
    public static class SeedGDPR
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();

            var purposes = context.Purposes.ToList();
            var policies = (new Policies()).GetPolicies();

            foreach (var purpose in policies)
            {
                if (!purposes.Any(x => x.Title.ToLower() == purpose.Purpose.ToLower()))
                {
                    var entity = new Purpose
                    {
                        Id = Guid.NewGuid(),
                        Title = purpose.Purpose,
                        Description = purpose.PII
                    };

                    context.Purposes.Add(entity);
                }
            }

            context.SaveChanges();
        }
    }
}
