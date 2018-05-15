using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GDPR.Model;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppIdentityDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(AppIdentityDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var purposes = _db.Purposes.ToList();
            var consent = _db.Consents.Where(x => x.ApplicationUserId == user.Id);

            Purposes = new List<SelectListItem>();
            foreach (var purpose in purposes)
            {
                var entry = new SelectListItem
                {
                    Selected = consent.Any(x => x.Purpose.Id == purpose.Id),
                    Text = $"{purpose.Title} requires {purpose.Description}",
                    Value = purpose.Id.ToString()
                };
                Purposes.Add(entry);
            }
        }

        [BindProperty]
        public IList<SelectListItem> Purposes { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var dbPurpose = _db.Purposes.ToList();
            var user = await _userManager.GetUserAsync(User);

            var delete = _db.Consents.Where(x => x.ApplicationUserId == user.Id);
            _db.Consents.RemoveRange(delete);

            var purposes = Purposes.Where(x => x.Selected);
            foreach (var purpose in purposes)
            {
                Guid.TryParse(purpose.Value, out var id);
                _db.Consents.Add(new Consent
                {
                    Id = Guid.NewGuid(),
                    Purpose = dbPurpose.First(x => x.Id == id),
                    ApplicationUserId = user.Id
                });
            }

            await _db.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}