using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using System;
using System.Collections.Generic;

namespace Pims.Dal.Services
{
    /// <summary>
    /// IUserService interface, provides functions to interact with users within the datasource.
    /// </summary>
    public interface IUserService : IService<User>
    {
        int Count();
        bool UserExists(Guid keycloakUserId);
        User Activate();
        Paged<User> Get(int page, int quantity);
        Paged<User> Get(UserFilter filter = null);
        User Get(Guid keycloakUserId);
        User Get(long id);

        User GetTracking(long id);
        void LoadOrganizations(User user);
        void LoadRoles(User user);
        IEnumerable<long> GetOrganizations(Guid keycloakUserId);
        IEnumerable<User> GetAdmininstrators(params long[] organizationIds);
        User Add(User add);
        void AddWithoutSave(User add);
        User Update(User update);
        User UpdateOnly(User update);
        User UpdateWithoutSave(User update);
        void Delete(User delete);
    }
}
