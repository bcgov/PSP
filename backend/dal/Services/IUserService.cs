using Pims.Dal.Entities;
using System;
using System.Collections.Generic;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IUserService interface, provides functions to interact with users within the datasource.
    /// </summary>
    public interface IUserService : IService
    {
        bool UserExists(Guid key);
        User Activate();
        IEnumerable<long> GetAgencies(Guid userKey);
        AccessRequest GetAccessRequest();
        AccessRequest GetAccessRequest(long id);
        AccessRequest DeleteAccessRequest(AccessRequest accessRequest);
        AccessRequest AddAccessRequest(AccessRequest request);
        AccessRequest UpdateAccessRequest(AccessRequest request);
        IEnumerable<User> GetAdmininstrators(params long[] agencyId);
    }
}
