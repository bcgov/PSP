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
        Paged<User> Get(UserFilter filter);
        User Get(Guid key);
        User Get(long id);
        void LoadOrganizations(User user);
        void LoadRoles(User user);
        IEnumerable<long> GetOrganizations(Guid keycloakUserId);
        IEnumerable<User> GetAdmininstrators(params long[] organizationId);
        User Add(User add);
        void AddWithoutSave(User add);
        User Update(User update);
        User UpdateWithoutSave(User update);
        void Delete(User delete);
    }
}
