using Infrastructure.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace GDPR.Model
{
    public class Consent
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Purpose Purpose { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
