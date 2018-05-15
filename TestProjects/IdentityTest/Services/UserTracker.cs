using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTest.Services
{
    public class UserTracker : IUserTracker
    {
        public Task SaveUserLocation(string id)
        {
            return Task.CompletedTask;
        }
    }
}
