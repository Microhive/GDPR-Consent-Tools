using System.Linq;
using System.Threading.Tasks;
using GDPR.Attributes;
using GDPR.Core;
using IdentityTest.Data;
using IdentityTest.Models;
using IdentityTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityTest.Controllers
{
    public class NotificationController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailSender _emailSender;

        public NotificationController(
            ApplicationDbContext dbContext,
            IEmailSender emailSender)
        {
            _dbContext = dbContext; 
            _emailSender = emailSender;
        }

        [Purpose("Marketing", "Name, DateOfBirth, CPR")]
        public async Task<string[]> MarketingNotification()
        {
            var users = _dbContext.ApplicationUserPurpose("Marketing").ToList();
            foreach (var user in users)
            {
                await _emailSender.SendEmailAsync(
                    user.Email, 
                    "Marketing Email Subject",  
                    "Marketing Email Message!");
            }

            return users.Select(x => x.Email).ToArray();
        }

        [Purpose("Security", "Name, DateOfBirth, CPR")]
        public async Task<string[]> SecurityNotification()
        {
            var users = _dbContext.ApplicationUserPurpose("Security").ToList();
            foreach (var user in users)
            {
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Security Email Subject",
                    "Security Email Message!");
            }

            return users.Select(x => x.Email).ToArray();
        }
    }
}