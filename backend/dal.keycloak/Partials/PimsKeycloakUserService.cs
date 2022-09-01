using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using Entity = Pims.Dal.Entities;
using KModel = Pims.Keycloak.Models;

namespace Pims.Dal.Keycloak
{
    /// <summary>
    /// PimsKeycloakService class, provides a way to integrate both PIMS and Keycloak datasources.
    /// </summary>
    public partial class PimsKeycloakService : IPimsKeycloakService
    {
        #region Methods

        /// <summary>
        /// Update the specified user in keycloak and PIMS.
        /// </summary>
        /// <param name="user"></param>
        /// <exception type="KeyNotFoundException">User does not exist in keycloak or PIMS.</exception>
        /// <returns></returns>
        public async Task<Entity.PimsUser> UpdateUserAsync(Entity.PimsUser user)
        {
            var kuser = await _keycloakService.GetUserAsync(user.GuidIdentifierValue.Value) ?? throw new KeyNotFoundException("User does not exist in Keycloak");
            var euser = _pimsRepository.User.GetTracking(user.Id);

            return await SaveUserChanges(user, euser, kuser, true);
        }

        /// <summary>
        /// Update the specified user in keycloak and PIMS, only add and roles.
        /// </summary>
        /// <param name="update"></param>
        /// <exception type="KeyNotFoundException">User does not exist in keycloak or PIMS.</exception>
        /// <returns></returns>
        public async Task<Entity.PimsUser> AppendToUserAsync(Entity.PimsUser update)
        {
            var kuser = await _keycloakService.GetUserAsync(update.GuidIdentifierValue.Value) ?? throw new KeyNotFoundException("User does not exist in Keycloak");
            var euser = _pimsRepository.User.GetTracking(update.Id);

            return await SaveUserChanges(update, euser, kuser, true);
        }

        /// <summary>
        /// Approve the access request and grant the user a role and organization.
        /// </summary>
        /// <param name="update"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public async Task<Entity.PimsAccessRequest> UpdateAccessRequestAsync(Entity.PimsAccessRequest update)
        {
            update.ThrowIfNull(nameof(update));
            _user.ThrowIfNotAuthorized(Permissions.AdminUsers);

            var accessRequest = _pimsRepository.AccessRequest.Get(update.AccessRequestId);
            var user = _pimsRepository.User.Get(accessRequest.UserId);

            if (update.AccessRequestStatusTypeCode == AccessRequestStatusTypes.APPROVED)
            {

                user.PimsUserRoles.Clear();
                user.IsDisabled = false;
                user.PimsUserRoles.Add(new Entity.PimsUserRole() { UserId = user.Id, RoleId = update.RoleId.Value });
                await AppendToUserAsync(user);
            }
            update.User = user;
            update.Role = _pimsRepository.Role.Find(update.RoleId);

            return _pimsRepository.AccessRequest.Update(update);
        }

        /// <summary>
        /// Save the updated user in keycloak and database.
        /// </summary>
        /// <param name="update"></param>
        /// <param name="euser"></param>
        /// <param name="kuser"></param>
        /// <returns></returns>
        private async Task<Entity.PimsUser> SaveUserChanges(Entity.PimsUser update, Entity.PimsUser euser, KModel.UserModel kuser, bool resetRoles = false)
        {
            if (resetRoles)
            {
                euser.PimsUserRoles.ForEach(role => _pimsRepository.User.RemoveRole(euser, role.RoleId));
                euser.PimsRegionUsers.ForEach(region => _pimsRepository.User.RemoveRegion(euser, region.Id));
            }

            // Update PIMS
            euser.BusinessIdentifierValue = kuser.Username; // PIMS must use whatever username is set in keycloak.
            euser.Person.FirstName = update.Person.FirstName;
            euser.Person.MiddleNames = update.Person.MiddleNames;
            euser.Person.Surname = update.Person.Surname;
            euser.Position = update.Position;
            euser.Note = update.Note;
            euser.IsDisabled = update.IsDisabled;
            euser.ConcurrencyControlNumber = update.ConcurrencyControlNumber;
            euser.PimsUserRoles = update.PimsUserRoles;
            euser.PimsRegionUsers = update.PimsRegionUsers;

            euser.Person.PimsContactMethods.RemoveAll(c => c.ContactMethodTypeCode == ContactMethodTypes.WorkEmail);
            update.Person.PimsContactMethods.ForEach(c =>
            {
                euser.Person.PimsContactMethods.Add(new PimsContactMethod(update.Person, c.Organization, c.ContactMethodTypeCode, c.ContactMethodValue));
            });

            euser = _pimsRepository.User.UpdateOnly(euser);

            // Now update keycloak
            var kmodel = _mapper.Map<KModel.UserModel>(update);

            if (resetRoles)
            {
                // Remove all keycloak groups from user.  // TODO: Only add/remove the ones that should be removed.
                var userGroups = await _keycloakService.GetUserGroupsAsync(euser.GuidIdentifierValue.Value);
                foreach (var group in userGroups)
                {
                    await _keycloakService.RemoveGroupFromUserAsync(update.GuidIdentifierValue.Value, group.Id);
                }
            }

            var roleIds = update.PimsUserRoles.Select(r => r.RoleId);
            foreach (var roleId in roleIds)
            {
                var role = _pimsRepository.Role.Find(roleId) ?? throw new KeyNotFoundException("Cannot assign a role to a user, when the role does not exist.");
                if (role.KeycloakGroupId == null)
                {
                    throw new KeyNotFoundException("PIMS has not been synced with Keycloak.");
                }

                _logger.LogInformation($"Adding keycloak group '{role.Name}' to user '{euser.BusinessIdentifierValue}'.");
                await _keycloakService.AddGroupToUserAsync(update.GuidIdentifierValue.Value, role.KeycloakGroupId.Value);
            }

            kmodel.Attributes = new Dictionary<string, string[]>
            {
                ["displayName"] = new[] { update.BusinessIdentifierValue },
            };
            _logger.LogInformation($"Updating keycloak user '{euser.BusinessIdentifierValue}'.");
            await _keycloakService.UpdateUserAsync(kmodel);

            return _pimsRepository.User.Get(euser.Id);
        }
    }
    #endregion
}
