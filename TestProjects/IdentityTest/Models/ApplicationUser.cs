using System;
using System.Collections.Generic;
using GDPR.Attributes;
using GDPR.Model;
using Microsoft.AspNetCore.Identity;

namespace IdentityTest.Models
{
    [PII("Name, DateOfBirth, CPR")]
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string CPR { get; set; }

        public string Remark { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Consent> Consents { get; set; }
    }
}
