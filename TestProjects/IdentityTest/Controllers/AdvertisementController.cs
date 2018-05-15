using System.Linq;
using GDPR.Attributes;
using IdentityTest.Data;
using IdentityTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTest.Controllers
{
    public class FriendsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public FriendsController(
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Purpose("Marketing", "Name, DateOfBirth, CPR")]
        public string[] SendAdvertisement()
        {
            var users = _dbContext.ApplicationUser.ToList();
            ApplicationUser user = users.FirstOrDefault();
            return users.Select(x => x.Email).ToArray();
        }
    }
}