using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Keycloak
{
    public interface IPimsKeycloakService
    {
        #region Roles
        Task<IEnumerable<Entity.PimsRole>> SyncRolesAsync();
        Task<IEnumerable<Entity.PimsRole>> GetRolesAsync(int page = 1, int quantity = 10, string search = null);
        Task<Entity.PimsRole> GetRoleAsync(Guid key);
        Task<Entity.PimsRole> UpdateRoleAsync(Entity.PimsRole role);
        #endregion

        #region Users
        Task<Entity.PimsUser> SyncUserAsync(Guid key);
        Task<IEnumerable<Entity.PimsUser>> GetUsersAsync(int page = 1, int quantity = 10, string search = null);
        Task<Entity.PimsUser> GetUserAsync(Guid key);
        Task<Entity.PimsUser> UpdateUserAsync(Entity.PimsUser user);
        Task<Entity.PimsAccessRequest> UpdateAccessRequestAsync(Entity.PimsAccessRequest update);
        #endregion
    }
}
