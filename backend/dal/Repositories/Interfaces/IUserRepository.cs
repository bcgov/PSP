using System;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IUserRepository interface, provides functions to interact with users within the datasource.
    /// </summary>
    public interface IUserRepository : IRepository<PimsUser>
    {
        int Count();
        bool UserExists(Guid keycloakUserId);
        PimsUser Activate();
        Paged<PimsUser> Get(int page, int quantity);
        Paged<PimsUser> Get(UserFilter filter = null);
        PimsUser Get(Guid keycloakUserId);
        PimsUser Get(long id);

        PimsUser GetTracking(long id);
        public PimsUser RemoveRole(PimsUser user, long roleId);
        public PimsUser RemoveRegion(PimsUser user, long regionId);
        PimsUser Add(PimsUser add);
        void AddWithoutSave(PimsUser add);
        PimsUser Update(PimsUser update);
        PimsUser UpdateOnly(PimsUser update);
        PimsUser UpdateWithoutSave(PimsUser update);
        void Delete(PimsUser delete);
        PimsUser GetUserInfo(Guid keycloakUserId);
    }
}
