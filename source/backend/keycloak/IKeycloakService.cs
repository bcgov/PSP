using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pims.Keycloak.Models;

namespace Pims.Keycloak
{
    public interface IKeycloakService
    {
        #region Users

        Task<Models.UserModel> GetUserAsync(Guid id);

        Task<Models.RoleModel[]> GetUserGroupsAsync(Guid id);

        Task ModifyUserRoleMappings(IEnumerable<UserRoleOperation> operations);
        #endregion
    }
}
