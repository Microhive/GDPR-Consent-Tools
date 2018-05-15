using GDPR.Attributes;
using GDPR.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace GDPR
{
    [GDPRPolicies]
    public class Policies : IGDPRPolicies
    {
        private void GeneratePolicies(List<Policy> list)
        {
        }

        public List<Policy> GetPolicies()
        {
            var test = new List<Policy>();
            GeneratePolicies(test);
            return test;
        }
    }
}