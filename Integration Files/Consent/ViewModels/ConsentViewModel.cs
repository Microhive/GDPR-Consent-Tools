using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GDPR.Model
{
    public class ConsentViewModel
    {
        public bool IsSelected { get; set; }

        public string Purpose { get; set; }

        public string PII { get; set; }
    }
}
