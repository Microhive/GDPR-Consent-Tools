using GDPR.Attributes;
using GDPR.Model;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Infrastructure.Identity
{
    [PII("Username, Email, CPR")]
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Consent> Consents { get; set; }
    }
}
