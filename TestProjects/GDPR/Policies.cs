using GDPR.Attributes;
using GDPR.Core;
using System.Collections.Generic;

namespace GDPR
{
    [GDPRPolicies]
    public class Policies : IGDPRPolicies
    {
        private void GeneratePolicies(List<Policy> list)
        {
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