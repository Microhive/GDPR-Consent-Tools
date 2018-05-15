using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GDPR.Attributes
{
    public class PIIAttribute : Attribute
    {
        public PIIAttribute(string pii) { }
    }
}
