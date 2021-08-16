using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Core.Helpers;
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
        public async Task<Entity.User> SyncUserAsync(Guid key)
        {
            var kuser = await _keycloakService.GetUserAsync(key) ?? throw new KeyNotFoundException();
            var kgroups = await _keycloakService.GetUserGroupsAsync(key);

            var euser = _pimsAdminService.User.Get(key);
            if (euser == null)
            {
                // The user does not exist in PIMS, it needs to be added.
                euser = _mapper.Map<Entity.User>(kuser);
                foreach (var group in kgroups)
                {
                    var erole = _pimsAdminService.Role.Get(group.Id);

                    // If the role doesn't exist, create it.
                    if (erole == null)
                    {
                        erole = _mapper.Map<Entity.Role>(group);
                        _pimsAdminService.Role.AddOne(erole);
                    }

                    euser.RolesManyToMany.Add(new Entity.UserRole(euser, erole));
                }
                _pimsAdminService.User.AddOne(euser);
            }
            else
            {
                // The user exists in PIMS, it only needs to be updated.
                var roles = euser?.Roles.ToArray();
                _mapper.Map(kuser, euser);
                foreach (var group in kgroups)
                {
                    var erole = _pimsAdminService.Role.Get(group.Id);

                    // If the role doesn't exist, create it.
                    if (erole == null)
                    {
                        erole = _mapper.Map<Entity.Role>(group);
                        _pimsAdminService.Role.AddOne(erole);
                    }

                    // If the user isn't associated with the role, add a link.
                    if (!roles.Any(r => r.Key == group.Id))
                    {
                        euser.RolesManyToMany.Add(new Entity.UserRole(euser, erole));
                    }
                }
                _pimsAdminService.User.UpdateOne(euser);
            }
            _pimsAdminService.CommitTransaction();

            return euser;
        }

        /// <summary>
        /// Get an array of users from keycloak and PIMS.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Entity.User>> GetUsersAsync(int page = 1, int quantity = 10, string search = null)
        {
            var kusers = await _keycloakService.GetUsersAsync((page - 1) * quantity, quantity, search);

            // TODO: Need better performing solution.
            return kusers.Select(u => ExceptionHelper.HandleKeyNotFound(() => _pimsAdminService.User.Get(u.Id)) ?? _mapper.Map<Entity.User>(u));
        }

        /// <summary>
        /// Get the user specified for the 'key', only if they exist in Keycloak and PIMS.
        /// </summary>
        /// <param name="key"></param>
        /// <exception type="KeyNotFoundException">User does not exist in keycloak or PIMS.</exception>
        /// <returns></returns>
        public async Task<Entity.User> GetUserAsync(Guid key)
        {
            var kuser = await _keycloakService.GetUserAsync(key) ?? throw new KeyNotFoundException();
            return _pimsAdminService.User.Get(kuser.Id);
        }

        /// <summary>
        /// Update the specified user in keycloak and PIMS.
        /// </summary>
        /// <param name="user"></param>
        /// <exception type="KeyNotFoundException">User does not exist in keycloak or PIMS.</exception>
        /// <returns></returns>
        public async Task<Entity.User> UpdateUserAsync(Entity.User user)
        {
            var kuser = await _keycloakService.GetUserAsync(user.Key) ?? throw new KeyNotFoundException("User does not exist in Keycloak");
            var euser = _pimsAdminService.User.Get(user.Id);

            IEnumerable<long> addRoleIds;
            IEnumerable<long> removeRoleIds;
            if (user.Roles.Any() || euser.Roles.Any())
            {
                addRoleIds = user.Roles.Except(euser.Roles, new RoleRoleIdComparer()).Select(r => r.Id).ToArray();
                removeRoleIds = euser.Roles.Except(user.Roles, new RoleRoleIdComparer()).Select(r => r.Id).ToArray();
            }
            else
            {
                addRoleIds = user.RolesManyToMany.Except(euser.RolesManyToMany, new UserRoleRoleIdComparer()).Select(r => r.RoleId).ToArray();
                removeRoleIds = euser.RolesManyToMany.Except(user.RolesManyToMany, new UserRoleRoleIdComparer()).Select(r => r.RoleId).ToArray();
            }

            IEnumerable<long> addAgencyIds;
            IEnumerable<long> removeAgencyIds;
            if (user.Agencies.Any() || euser.Agencies.Any())
            {
                addAgencyIds = user.Agencies.Except(euser.Agencies, new AgencyAgencyIdComparer()).Select(a => a.Id).ToArray();
                removeAgencyIds = euser.Agencies.Except(user.Agencies, new AgencyAgencyIdComparer()).Select(a => a.Id).ToArray();
                // Make sure child agencies are included.
                if (!addAgencyIds.Any())
                {
                    user.Agencies.ForEach(a =>
                    {
                        addAgencyIds = addAgencyIds.Concat(_pimsAdminService.Agency.GetChildren(a.Id).Select(a => a.Id).ToArray()).ToArray();
                    });
                }
            }
            else
            {
                addAgencyIds = user.AgenciesManyToMany.Except(euser.AgenciesManyToMany, new UserAgencyAgencyIdComparer()).Select(r => r.AgencyId).ToArray();
                removeAgencyIds = euser.AgenciesManyToMany.Except(user.AgenciesManyToMany, new UserAgencyAgencyIdComparer()).Select(r => r.AgencyId).ToArray();
                // Make sure child agencies are included.
                if (!addAgencyIds.Any())
                {
                    user.AgenciesManyToMany.ForEach(a =>
                    {
                        addAgencyIds = addAgencyIds.Concat(_pimsAdminService.Agency.GetChildren(a.AgencyId).Select(a => a.Id).ToArray()).ToArray();
                    });
                }
            }
            // Each parent agency should add children agencies.
            addAgencyIds.ToArray().ForEach(id =>
            {
                var childAgencies = _pimsAdminService.Agency.GetChildren(id).Select(a => a.Id).ToArray();
                addAgencyIds = addAgencyIds.Concat(childAgencies).Distinct().ToArray();
            });
            // Don't incorrectly remove child agencies.
            removeAgencyIds = removeAgencyIds.Except(addAgencyIds).ToArray();

            // Update PIMS
            euser.Username = kuser.Username; // PIMS must use whatever username is set in keycloak.
            euser.FirstName = user.FirstName;
            euser.LastName = user.LastName;
            euser.Email = user.Email;
            euser.Position = user.Position;
            euser.Note = user.Note;
            euser.IsDisabled = user.IsDisabled;
            euser.RowVersion = user.RowVersion;

            // Remove all keycloak groups from user.  // TODO: Only add/remove the ones that should be removed.
            var userGroups = await _keycloakService.GetUserGroupsAsync(euser.Key);
            foreach (var group in userGroups)
            {
                await _keycloakService.RemoveGroupFromUserAsync(user.Key, group.Id);
            }

            var roleIds = user.Roles.Any() ? user.Roles.Select(r => r.Id) : user.RolesManyToMany.Select(r => r.RoleId);
            foreach (var roleId in roleIds)
            {
                var role = _pimsAdminService.Role.Find(roleId) ?? throw new KeyNotFoundException("Cannot assign a role to a user, when the role does not exist.");
                if (role.KeycloakGroupId == null) throw new KeyNotFoundException("PIMS has not been synced with Keycloak.");
                _logger.LogInformation($"Adding keycloak group '{role.Name}' to user '{euser.Username}'.");
                await _keycloakService.AddGroupToUserAsync(user.Key, role.KeycloakGroupId.Value);
            }

            // Update Roles.
            removeRoleIds.ForEach(r =>
            {
                var role = _pimsAdminService.Role.Find(r) ?? throw new KeyNotFoundException("Cannot remove a role from a user, when the role does not exist.");
                if (role.KeycloakGroupId == null) throw new KeyNotFoundException("PIMS has not been synced with Keycloak.");
                if (euser.Roles.Any())
                {
                    euser.Roles.Remove(role);
                }
                else
                {
                    var userRole = euser.RolesManyToMany.FirstOrDefault(r => r.RoleId == role.Id);
                    euser.RolesManyToMany.Remove(userRole);
                }
            });
            addRoleIds.ForEach(r =>
            {
                var role = _pimsAdminService.Role.Find(r) ?? throw new KeyNotFoundException("Cannot assign a role to a user, when the role does not exist.");
                if (role.KeycloakGroupId == null) throw new KeyNotFoundException("PIMS has not been synced with Keycloak.");
                if (euser.Roles.Any())
                {
                    euser.Roles.Add(role);
                }
                else
                {
                    euser.RolesManyToMany.Add(new Entity.UserRole(euser, role));
                }
            });

            // Update Agencies
            addAgencyIds.ForEach(a =>
            {
                var agency = _pimsAdminService.Agency.Find(a) ?? throw new KeyNotFoundException("Cannot assign an agency to a user, when the agency does not exist.");
                if (euser.Agencies.Any())
                {
                    euser.Agencies.Add(agency);
                }
                else
                {
                    euser.AgenciesManyToMany.Add(new Entity.UserAgency(euser, agency));
                }
            });
            removeAgencyIds.ForEach(a =>
            {
                var agency = _pimsAdminService.Agency.Find(a) ?? throw new KeyNotFoundException("Cannot remove an agency from a user, when the agency does not exist.");
                if (euser.Agencies.Any())
                {
                    euser.Agencies.Remove(agency);
                }
                else
                {
                    var userAgency = euser.AgenciesManyToMany.FirstOrDefault(r => r.AgencyId == agency.Id);
                    euser.AgenciesManyToMany.Remove(userAgency);
                }
            });

            _pimsAdminService.User.Update(euser);

            // Now update keycloak
            var kmodel = _mapper.Map<KModel.UserModel>(user);
            kmodel.Attributes = new Dictionary<string, string[]>
            {
                ["agencies"] = _pimsService.User.GetAgencies(euser.Key).Select(a => a.ToString()).ToArray(),
                ["displayName"] = new[] { user.DisplayName }
            };
            _logger.LogInformation($"Updating keycloak agency attribute '{kmodel.Attributes["agencies"]}' for user '{euser.Username}'.");
            await _keycloakService.UpdateUserAsync(kmodel);  // TODO: Fix issue where EmailVerified will be set to false.

            return _pimsAdminService.User.Get(euser.Id);
        }

        /// <summary>
        /// Updates the specified access request in the datasource. if the request is granted, update the associated user as well.
        /// </summary>
        /// <param name="accessRequest"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public async Task<Entity.AccessRequest> UpdateAccessRequestAsync(Entity.AccessRequest accessRequest)
        {
            accessRequest.ThrowIfNull(nameof(accessRequest));
            accessRequest.ThrowIfNull(nameof(accessRequest.UserId));

            _user.ThrowIfNotAuthorized(Permissions.AdminUsers, Permissions.AgencyAdmin);
            var existingAccessRequest = _pimsAdminService.User.GetAccessRequest(accessRequest.Id);
            if (existingAccessRequest.Status != Entity.AccessRequestStatus.Approved && accessRequest.Status == Entity.AccessRequestStatus.Approved)
            {
                var user = _pimsAdminService.User.Get(existingAccessRequest.UserId);
                accessRequest.AgenciesManyToMany.ForEach((accessRequestAgency) =>
                {
                    if (!user.AgenciesManyToMany.Any(a => a.AgencyId == accessRequestAgency.AgencyId))
                    {
                        user.AgenciesManyToMany.Add(new Entity.UserAgency()
                        {
                            User = user,
                            AgencyId = accessRequestAgency.AgencyId
                        });
                    }
                });
                accessRequest.RolesManyToMany.ForEach((accessRequestRole) =>
                {
                    if (!user.RolesManyToMany.Any(r => r.RoleId == accessRequestRole.RoleId))
                    {
                        user.RolesManyToMany.Add(new Entity.UserRole()
                        {
                            User = user,
                            RoleId = accessRequestRole.RoleId
                        });
                    }
                });
                await UpdateUserAsync(user);
            }

            return _pimsAdminService.User.UpdateAccessRequest(accessRequest);
        }
    }
    #endregion
}
