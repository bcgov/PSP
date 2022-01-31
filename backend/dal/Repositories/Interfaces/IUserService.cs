using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using System;
using System.Collections.Generic;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IUserService interface, provides functions to interact with users within the datasource.
    /// </summary>
    public interface IUserService : IRepository<PimsUser>
    {
        int Count();
        bool UserExists(Guid keycloakUserId);
        PimsUser Activate();
        Paged<PimsUser> Get(int page, int quantity);
        Paged<PimsUser> Get(UserFilter filter = null);
        PimsUser Get(Guid keycloakUserId);
        PimsUser Get(long id);

        PimsUser GetTracking(long id);
        void LoadOrganizations(PimsUser user);
        void LoadRoles(PimsUser user);
        public PimsUser RemoveRole(PimsUser user, long roleId);
        IEnumerable<long> GetOrganizations(Guid keycloakUserId);
        IEnumerable<PimsUser> GetAdministrators(params long[] organizationIds);
        PimsUser Add(PimsUser add);
        void AddWithoutSave(PimsUser add);
        PimsUser Update(PimsUser update);
        PimsUser UpdateOnly(PimsUser update);
        PimsUser UpdateWithoutSave(PimsUser update);
        void Delete(PimsUser delete);
    }
}
