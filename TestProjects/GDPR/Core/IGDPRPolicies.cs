using System.Collections.Generic;

namespace GDPR.Core
{
    public interface IGDPRPolicies
    {
        List<Policy> GetPolicies();
    }
}
