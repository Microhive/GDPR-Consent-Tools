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
            list.Add(new Policy(purpose: "Marketing", pii: "Name, DateOfBirth, CPR"));
            list.Add(new Policy(purpose: "Marketing", pii: "Name, DateOfBirth, CPR"));
            list.Add(new Policy(purpose: "Security", pii: "Name, DateOfBirth, CPR"));
        }

        public List<Policy> GetPolicies()
        {
            var test = new List<Policy>();
            GeneratePolicies(test);
            return test;
        }
    }
}
