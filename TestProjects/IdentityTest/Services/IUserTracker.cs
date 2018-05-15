using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityTest.Services
{
    public interface IUserTracker
    {
        Task SaveUserLocation(string id);
    }
}
