using System;
using System.Threading.Tasks;

namespace Pims.Keycloak
{
    public interface IKeycloakService
    {
        #region Attack Detection
        Task DeleteAttackDetectionAsync();
        #endregion

        #region Users
        Task<int> GetUserCountAsync();

        Task<Models.UserModel[]> GetUsersAsync(int first = 0, int max = 10, string search = null);

        Task<Models.UserModel> GetUserAsync(Guid id);

        Task<Models.UserModel> CreateUserAsync(Models.UserModel user);

        Task<Guid> UpdateUserAsync(Models.UserModel user);

        Task<Guid> DeleteUserAsync(Guid id);

        Task<Models.GroupModel[]> GetUserGroupsAsync(Guid id);

        Task<int> GetUserGroupCountAsync(Guid id);

        Task<Guid> AddGroupToUserAsync(Guid userKey, Guid groupKey);

        Task<Guid> RemoveGroupFromUserAsync(Guid userKey, Guid groupKey);
        #endregion

        #region Groups
        Task<int> GetGroupCountAsync();

        Task<Models.GroupModel[]> GetGroupsAsync(int first = 0, int max = 10, string search = null);

        Task<Models.GroupModel> GetGroupAsync(Guid id);

        Task<Models.GroupModel> CreateGroupAsync(Models.GroupModel group);

        Task<Models.GroupModel> CreateSubGroupAsync(Guid parentKey, Models.GroupModel group);

        Task<Models.GroupModel> UpdateGroupAsync(Models.GroupModel group);

        Task<Guid> DeleteGroupAsync(Guid id);

        Task<Models.UserModel[]> GetGroupMembersAsync(Guid id, int first = 0, int max = 10);
        #endregion

        #region Roles
        #region By ID
        Task<Models.RoleModel> GetRoleAsync(Guid key);

        Task<Models.RoleModel> UpdateRoleAsync(Models.RoleModel role);

        Task<Guid> DeleteRoleAsync(Guid key);

        Task<Models.RoleModel[]> CreateCompositeRolesAsync(Guid parentKey, Models.RoleModel[] roles);

        Task<Models.RoleModel[]> GetCompositeRolesAsync(Guid parentKey);

        Task<Models.RoleModel[]> DeleteCompositeRolesAsync(Guid parentKey, Models.RoleModel[] roles);

        Task<Models.RoleModel[]> GetClientCompositeRolesAsync(Guid parentKey, string clientName);

        Task<Models.RoleModel[]> GetRealmCompositeRolesAsync(Guid parentKey);
        #endregion

        #region Realm
        Task<Models.RoleModel[]> GetRolesAsync();

        Task<Models.RoleModel> GetRoleAsync(string name);

        Task<Models.RoleModel> CreateRoleAsync(Models.RoleModel role);

        Task<Models.RoleModel> UpdateRoleAsync(string name, Models.RoleModel role);

        Task<string> DeleteRoleAsync(string name);

        Task<Models.RoleModel> CreateCompositeRoleAsync(string parentName, Models.RoleModel role);

        Task<Models.RoleModel[]> GetCompositeRolesAsync(string parentName);

        Task<Models.RoleModel[]> DeleteCompositeRolesAsync(string parentName, Models.RoleModel[] roles);

        Task<Models.RoleModel[]> GetClientCompositeRolesAsync(string parentName, string clientName);

        Task<Models.RoleModel[]> GetRealmCompositeRolesAsync(string parentName);

        Task<Models.UserModel[]> GetRoleMembersAsync(string parentName, int first = 0, int max = 10);
        #endregion

        #region Client
        Task<Models.RoleModel[]> GetRolesAsync(Guid clientKey);

        Task<Models.RoleModel> GetRoleAsync(Guid clientKey, string name);

        Task<Models.RoleModel> CreateRoleAsync(Guid clientKey, Models.RoleModel role);

        Task<Models.RoleModel> UpdateRoleAsync(Guid clientKey, Models.RoleModel role);

        Task<string> DeleteRoleAsync(Guid clientKey, string name);

        Task<Models.RoleModel> CreateCompositeRoleAsync(Guid clientKey, string parentName, Models.RoleModel role);

        Task<Models.RoleModel[]> GetCompositeRolesAsync(Guid clientKey, string parentName);

        Task<Models.RoleModel[]> DeleteCompositeRoleAsync(Guid clientKey, string parentName, Models.RoleModel[] roles);

        Task<Models.RoleModel[]> GetClientCompositeRolesAsync(Guid clientKey, string parentName, string clientName);

        Task<Models.RoleModel[]> GetRealmCompositeRolesAsync(Guid clientKey, string parentName);

        Task<Models.UserModel[]> GetRoleMembersAsync(Guid clientKey, string parentName, int first = 0, int max = 10);
        #endregion
        #endregion
    }
}

