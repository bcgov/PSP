using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Pims.Keycloak.Models;

namespace Pims.Keycloak
{
    public interface IKeycloakRepository
    {
        #region Users

        Task<UserModel> GetUserAsync(Guid id);

        Task<List<UserModel>> GetUsersAsync(Guid id);

        Task<HttpResponseMessage> AddRolesToUser(string username, IEnumerable<RoleModel> roles);

        Task<HttpResponseMessage> DeleteRoleFromUsers(string username, string roleName);

        Task<ResponseWrapper<RoleModel>> GetAllRoles();

        Task<ResponseWrapper<RoleModel>> GetAllGroupRoles(string groupName);

        Task<ResponseWrapper<RoleModel>> GetUserRoles(string username);

        Task<HttpResponseMessage> AddKeycloakRole(RoleModel role);

        Task<HttpResponseMessage> AddKeycloakRolesToGroup(string groupName, IEnumerable<RoleModel> roles);

        Task<HttpResponseMessage> DeleteRole(string roleName);

        Task<HttpResponseMessage> DeleteRoleFromGroup(string groupName, string roleName);
        #endregion
    }
}
