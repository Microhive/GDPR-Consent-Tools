using GDPR.Core;
using System;

namespace GDPR.Attributes
{
    public class PurposeAttribute : Attribute
    {
        public string Purpose { get; set; }

        public string PII { get; set; }

        public PurposeAttribute(string name, string pii)
        {
            Purpose = name;
            PII = pii;
        }
    }
}
