using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Core.Helpers;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Comparers;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// Sync the user for the specified 'key' from keycloak to PIMS.
        /// If the user doesn't exist in PIMS it will add it.
        /// </summary>
        /// <param name="key"></param>
        /// <exception type="KeyNotFoundException">User does not exist in keycloak or PIMS.</exception>
        /// <returns></returns>
        public async Task<Entity.PimsUser> SyncUserAsync(Guid key)
        {
            var kuser = await _keycloakService.GetUserAsync(key) ?? throw new KeyNotFoundException();
            var kgroups = await _keycloakService.GetUserGroupsAsync(key);

            var euser = _pimsService.User.Get(key);
            if (euser == null)
            {
                // The user does not exist in PIMS, it needs to be added.
                euser = _mapper.Map<Entity.PimsUser>(kuser);
                foreach (var group in kgroups)
                {
                    var erole = _pimsService.Role.Get(group.Id);

                    // If the role doesn't exist, create it.
                    if (erole == null)
                    {
                        erole = _mapper.Map<Entity.PimsRole>(group);
                        _pimsService.Role.AddWithoutSave(erole);
                    }

                    euser.PimsUserRoles.Add(new Entity.PimsUserRole(euser, erole));
                }
                _pimsService.User.AddWithoutSave(euser);
            }
            else
            {
                // The user exists in PIMS, it only needs to be updated.
                var roles = euser.GetRoles();
                _mapper.Map(kuser, euser);
                foreach (var group in kgroups)
                {
                    var erole = _pimsService.Role.Get(group.Id);

                    // If the role doesn't exist, create it.
                    if (erole == null)
                    {
                        erole = _mapper.Map<Entity.PimsRole>(group);
                        _pimsService.Role.AddWithoutSave(erole);
                    }

                    // If the user isn't associated with the role, add a link.
                    if (!roles.Any(r => r.RoleUid == group.Id))
                    {
                        euser.PimsUserRoles.Add(new Entity.PimsUserRole(euser, erole));
                    }
                }
                _pimsService.User.UpdateWithoutSave(euser);
            }
            _pimsService.User.CommitTransaction();

            return euser;
        }

        /// <summary>
        /// Get an array of users from keycloak and PIMS.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Entity.PimsUser>> GetUsersAsync(int page = 1, int quantity = 10, string search = null)
        {
            var kusers = await _keycloakService.GetUsersAsync((page - 1) * quantity, quantity, search);

            // TODO: Need better performing solution.
            return kusers.Select(u => ExceptionHelper.HandleKeyNotFound(() => _pimsService.User.Get(u.Id)) ?? _mapper.Map<Entity.PimsUser>(u));
        }

        /// <summary>
        /// Get the user specified for the 'key', only if they exist in Keycloak and PIMS.
        /// </summary>
        /// <param name="key"></param>
        /// <exception type="KeyNotFoundException">User does not exist in keycloak or PIMS.</exception>
        /// <returns></returns>
        public async Task<Entity.PimsUser> GetUserAsync(Guid key)
        {
            var kuser = await _keycloakService.GetUserAsync(key) ?? throw new KeyNotFoundException();
            return _pimsService.User.Get(kuser.Id);
        }

        /// <summary>
        /// Update the specified user in keycloak and PIMS.
        /// </summary>
        /// <param name="user"></param>
        /// <exception type="KeyNotFoundException">User does not exist in keycloak or PIMS.</exception>
        /// <returns></returns>
        public async Task<Entity.PimsUser> UpdateUserAsync(Entity.PimsUser user)
        {
            var kuser = await _keycloakService.GetUserAsync(user.GuidIdentifierValue.Value) ?? throw new KeyNotFoundException("User does not exist in Keycloak");
            var euser = _pimsService.User.GetTracking(user.Id);

            IEnumerable<long> addRoleIds;
            IEnumerable<long> removeRoleIds;
            addRoleIds = user.PimsUserRoles.Except(euser.PimsUserRoles, new UserRoleRoleIdComparer()).Select(r => r.RoleId).ToArray();
            removeRoleIds = euser.PimsUserRoles.Except(user.PimsUserRoles, new UserRoleRoleIdComparer()).Select(r => r.RoleId).ToArray();

            IEnumerable<long> addOrganizationIds;
            IEnumerable<long> removeOrganizationIds;
            addOrganizationIds = user.PimsUserOrganizations.Except(euser.PimsUserOrganizations, new UserOrganizationOrganizationIdComparer()).Select(r => r.OrganizationId).ToArray();
            removeOrganizationIds = euser.PimsUserOrganizations.Except(user.PimsUserOrganizations, new UserOrganizationOrganizationIdComparer()).Select(r => r.OrganizationId).ToArray();
            // Make sure child organizations are included.
            if (!addOrganizationIds.Any())
            {
                user.PimsUserOrganizations.ForEach(a =>
                {
                    addOrganizationIds = addOrganizationIds.Concat(_pimsService.Organization.GetChildren(a.OrganizationId).Select(a => a.Id).ToArray()).ToArray();
                });
            }
            // Each parent organization should add children organizations.
            addOrganizationIds.ForEach(id =>
            {
                var childOrganizations = _pimsService.Organization.GetChildren(id).Select(a => a.Id).ToArray();
                addOrganizationIds = addOrganizationIds.Concat(childOrganizations).Distinct().ToArray();
            });
            // Don't incorrectly remove child organizations.
            removeOrganizationIds = removeOrganizationIds.Except(addOrganizationIds).ToArray();

            // Update Roles.
            removeRoleIds.ForEach(r =>
            {
                var role = _pimsService.Role.Find(r) ?? throw new KeyNotFoundException("Cannot remove a role from a user, when the role does not exist.");
                if (role.KeycloakGroupId == null) throw new KeyNotFoundException("PIMS has not been synced with Keycloak.");
                euser = _pimsService.User.RemoveRole(euser, role.RoleId);
            });
            addRoleIds.ForEach(r =>
            {
                var role = _pimsService.Role.Find(r) ?? throw new KeyNotFoundException("Cannot assign a role to a user, when the role does not exist.");
                if (role.KeycloakGroupId == null) throw new KeyNotFoundException("PIMS has not been synced with Keycloak.");
                euser.PimsUserRoles.Add(new Entity.PimsUserRole(euser, role));
            });

            // Update Organizations
            addOrganizationIds.ForEach(oId =>
            {
                var organization = _pimsService.Organization.Find(oId) ?? throw new KeyNotFoundException("Cannot assign an organization to a user, when the organization does not exist.");
                var roleId = user.PimsUserOrganizations.FirstOrDefault(o => o.OrganizationId == oId).RoleId;
                var role = _pimsService.Role.Find(roleId);
                euser.PimsUserOrganizations.Add(new Entity.PimsUserOrganization() { User = euser, Organization = organization, Role = role });
            });
            removeOrganizationIds.ForEach(oId =>
            {
                var organization = _pimsService.Organization.Find(oId) ?? throw new KeyNotFoundException("Cannot remove an organization from a user, when the organization does not exist.");
                var userOrganization = euser.PimsUserOrganizations.FirstOrDefault(r => r.OrganizationId == organization.Id);
                euser.PimsUserOrganizations.Remove(userOrganization);
            });

            return await SaveUserChanges(user, euser, kuser, true);
        }

        /// <summary>
        /// Update the specified user in keycloak and PIMS, only add new organizations and roles.
        /// </summary>
        /// <param name="user"></param>
        /// <exception type="KeyNotFoundException">User does not exist in keycloak or PIMS.</exception>
        /// <returns></returns>
        public async Task<Entity.PimsUser> AppendToUserAsync(Entity.PimsUser update)
        {
            var kuser = await _keycloakService.GetUserAsync(update.GuidIdentifierValue.Value) ?? throw new KeyNotFoundException("User does not exist in Keycloak");
            var euser = _pimsService.User.GetTracking(update.Id);

            IEnumerable<long> addRoleIds = update.PimsUserRoles.Except(euser.PimsUserRoles, new UserRoleRoleIdComparer()).Select(r => r.RoleId).ToArray();
            IEnumerable<long> addOrganizationIds = update.PimsUserOrganizations.Except(euser.PimsUserOrganizations, new OrganizationOrganizationIdComparer()).Select(a => a.OrganizationId).ToArray();
            addOrganizationIds = update.PimsUserOrganizations.Except(euser.PimsUserOrganizations, new UserOrganizationOrganizationIdComparer()).Select(r => r.OrganizationId).ToArray();
            // Each parent organization should add children organizations.
            addOrganizationIds.ForEach(id =>
            {
                var childOrganizations = _pimsService.Organization.GetChildren(id).Select(a => a.Id).ToArray();
                addOrganizationIds = addOrganizationIds.Concat(childOrganizations).Distinct().ToArray();
            });

            var roleIds = update.PimsUserRoles.Select(r => r.RoleId);
            foreach (var roleId in roleIds)
            {
                var role = _pimsService.Role.Find(roleId) ?? throw new KeyNotFoundException("Cannot assign a role to a user, when the role does not exist.");
                if (role.KeycloakGroupId == null) throw new KeyNotFoundException("PIMS has not been synced with Keycloak.");
                _logger.LogInformation($"Adding keycloak group '{role.Name}' to user '{euser.BusinessIdentifierValue}'.");
                await _keycloakService.AddGroupToUserAsync(update.GuidIdentifierValue.Value, role.KeycloakGroupId.Value);
            }

            addRoleIds.ForEach(r =>
            {
                var role = _pimsService.Role.Find(r) ?? throw new KeyNotFoundException("Cannot assign a role to a user, when the role does not exist.");
                if (role.KeycloakGroupId == null) throw new KeyNotFoundException("PIMS has not been synced with Keycloak.");
                euser.PimsUserRoles.Add(new Entity.PimsUserRole(euser, role));
            });

            // Update Organizations
            addOrganizationIds.ForEach(oId =>
            {
                var organization = _pimsService.Organization.Find(oId) ?? throw new KeyNotFoundException("Cannot assign an organization to a user, when the organization does not exist.");
                var roleId = update.PimsUserOrganizations.FirstOrDefault(o => o.OrganizationId == oId).RoleId;
                var role = _pimsService.Role.Find(roleId);
                euser.PimsUserOrganizations.Add(new Entity.PimsUserOrganization() { UserId = update.UserId, Organization = organization, Role = role });
            });

            return await SaveUserChanges(update, euser, kuser);
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

            // Update PIMS
            euser.BusinessIdentifierValue = kuser.Username; // PIMS must use whatever username is set in keycloak.
            euser.Person.FirstName = update.Person.FirstName;
            euser.Person.MiddleNames = update.Person.MiddleNames;
            euser.Person.Surname = update.Person.Surname;
            euser.Position = update.Position;
            euser.Note = update.Note;
            euser.IsDisabled = update.IsDisabled;
            euser.ConcurrencyControlNumber = update.ConcurrencyControlNumber;

            //TODO: currently the PIMS contact method screen does not support the concept of multiple contact methods, so for now simply overwrite any work email addresses.
            euser.Person.PimsContactMethods.RemoveAll(c => c.ContactMethodTypeCode == ContactMethodTypes.WorkEmail);
            update.Person.PimsContactMethods.ForEach(c =>
            {
                euser.Person.PimsContactMethods.Add(new PimsContactMethod(c.Person, c.Organization, c.ContactMethodTypeCode, c.ContactMethodValue));
            });


            euser = _pimsService.User.UpdateOnly(euser);

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
                var role = _pimsService.Role.Find(roleId) ?? throw new KeyNotFoundException("Cannot assign a role to a user, when the role does not exist.");
                if (role.KeycloakGroupId == null) throw new KeyNotFoundException("PIMS has not been synced with Keycloak.");
                _logger.LogInformation($"Adding keycloak group '{role.Name}' to user '{euser.BusinessIdentifierValue}'.");
                await _keycloakService.AddGroupToUserAsync(update.GuidIdentifierValue.Value, role.KeycloakGroupId.Value);
            }

            kmodel.Attributes = new Dictionary<string, string[]>
            {
                ["organizations"] = _pimsService.User.GetOrganizations(euser.GuidIdentifierValue.Value).Select(a => a.ToString()).ToArray(),
                ["displayName"] = new[] { update.BusinessIdentifierValue }
            };
            _logger.LogInformation($"Updating keycloak organization attribute '{kmodel.Attributes["organizations"]}' for user '{euser.BusinessIdentifierValue}'.");
            await _keycloakService.UpdateUserAsync(kmodel);

            return _pimsService.User.Get(euser.Id);
        }

        /// <summary>
        /// Approve the access request and grant the user a role and organization.
        /// </summary>
        /// <param name="accessRequest"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public async Task<Entity.PimsAccessRequest> UpdateAccessRequestAsync(Entity.PimsAccessRequest update)
        {
            update.ThrowIfNull(nameof(update));
            _user.ThrowIfNotAuthorized(Permissions.AdminUsers, Permissions.OrganizationAdmin);

            var accessRequest = _pimsService.AccessRequest.Get(update.AccessRequestId);
            var user = _pimsService.User.Get(accessRequest.UserId);

            if (update.AccessRequestStatusTypeCode == AccessRequestStatusTypes.APPROVED) {

                user.PimsUserRoles.Clear();
                user.PimsUserRoles.Add(new Entity.PimsUserRole() { UserId = user.Id, RoleId = update.RoleId.Value});
                update.PimsAccessRequestOrganizations.ToArray().ForEach(aro =>
                {
                    if (!user.PimsUserOrganizations.Any(a => a.OrganizationId == aro.OrganizationId))
                    {
                        user.PimsUserOrganizations.Add(new Entity.PimsUserOrganization() { UserId = update.UserId, OrganizationId = aro.OrganizationId.Value, RoleId = update.RoleId.Value });
                    }
                });
                await AppendToUserAsync(user);
                
            }
            update.User = user;
            update.Role = _pimsService.Role.Find(update.RoleId);
            
            return _pimsService.AccessRequest.Update(update);
        }
    }
    #endregion
}
