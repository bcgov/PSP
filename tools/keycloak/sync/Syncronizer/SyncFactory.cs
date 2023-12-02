using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Claim;
using Pims.Api.Models.Concepts.Role;
using Pims.Api.Models.Concepts.User;
using Pims.Core.Exceptions;
using Pims.Keycloak;
using KModel = Pims.Keycloak.Models;

namespace Pims.Tools.Keycloak.Sync
{

    /// <summary>
    /// SyncFactory class, provides a way to sync Keycloak roles, groups and users with PIMS.
    /// </summary>
    public class SyncFactory : ISyncFactory
    {
        #region Variables
        private static readonly int MAXPAGES = 20;
        private readonly IKeycloakRepository _keycloakRepository;
        private readonly IPimsRequestClient _pimsClient;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an Factory class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="keycloakRepository"></param>
        /// <param name="logger"></param>
        /// <param name="pimsClient"></param>
        public SyncFactory(IKeycloakRepository keycloakRepository, IPimsRequestClient pimsClient, ILogger<SyncFactory> logger)
        {
            _keycloakRepository = keycloakRepository;
            _pimsClient = pimsClient;
            _logger = logger;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Sync roles, groups and users in keycloak and PIMS.
        /// </summary>
        /// <returns></returns>
        public async Task<int> SyncAsync()
        {
            var log = new StringBuilder();
            var errorLog = new StringBuilder();

            await ActivateAccountAsync();

            await SyncClaimsAsync(log, errorLog);
            await SyncRolesAsync(log, errorLog);
            await SyncUsersAsync(log, errorLog);

            _logger.LogInformation($"---------------------{Environment.NewLine}Summary{Environment.NewLine}---------------------{Environment.NewLine}{log}");

            if (errorLog.Length > 0)
            {
                _logger.LogError($"---------------------{Environment.NewLine}Summary{Environment.NewLine}---------------------{Environment.NewLine}{errorLog}");
            }

            return 0;
        }

        #region Helpers

        /// <summary>
        /// Activate the service account.
        /// </summary>
        /// <returns></returns>
        private async Task ActivateAccountAsync()
        {
            var aRes = await _pimsClient.HandleRequestAsync<HttpResponseMessage>(HttpMethod.Post, "auth/activate");
            if (!aRes.IsSuccessStatusCode)
            {
                throw new HttpClientRequestException(aRes);
            }
        }

        #region Claim/Role

        /// <summary>
        /// Fetch all Claims in PIMS and link or create them in keycloak.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="errorLog"></param>
        /// <returns></returns>
        private async Task SyncClaimsAsync(StringBuilder log, StringBuilder errorLog)
        {
            IEnumerable<ClaimModel> claims = new List<ClaimModel>();
            PageModel<ClaimModel> claimsPage = null;
            int page = 1;
            do
            {
                claimsPage = await _pimsClient.HandleRequestAsync<PageModel<ClaimModel>>(HttpMethod.Get, $"admin/claims?page={page++}&quantity=50");
                claims = claims.Concat(claimsPage.Items);
            }
            while (claimsPage != null && claimsPage.Items.Any() && page < MAXPAGES);

            var allKeycloakRoles = await _keycloakRepository.GetAllRoles();
            var keycloakRoles = allKeycloakRoles.Data.Where(x => x.Composite.HasValue && x.Composite.Value == false);
            var keycloakRolesToDelete = keycloakRoles.Where(r => claims.All(crr => crr.Name != r.Name));

            var addTask = AddClaimsFromPims(claims.Where(c => keycloakRoles.All(kr => c.Name != kr.Name)), log, errorLog);
            var removeTask = RemoveRolesFromPims(keycloakRolesToDelete, log, errorLog);
            await Task.WhenAll(addTask, removeTask);
        }

        private async Task AddClaimsFromPims(IEnumerable<ClaimModel> claims, StringBuilder log, StringBuilder errorLog)
        {
            foreach (var claim in claims)
            {
                try
                {
                    await AddKeycloakRoleAsync(claim);
                    LogInfo(log, $"Keycloak role synchronized: '{claim.Name}'.");
                }
                catch (HttpClientRequestException ex)
                {
                    LogError(errorLog, $"Failed to Synchronize PIMS claim '{claim.Name}' with keycloak. Original error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Get the keycloak role that matches the PIMS claim.
        /// If it doesn't exist, create it in keycloak.
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        private async Task AddKeycloakRoleAsync(ClaimModel claim)
        {
            var krole = new KModel.RoleModel()
            {
                Name = claim.Name,
            };

            // Add the role to keycloak and sync with PIMS.
            _logger.LogInformation($"Adding keycloak role: {claim.Name}");
            var response = await _keycloakRepository.AddKeycloakRole(krole);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new HttpClientRequestException(response, $"Failed to add the role '{claim.Name}' to keycloak.");
            }
            _logger.LogInformation($"Keycloak role: {claim.Name} added");
            return;
        }
        #endregion

        #region Role/Group

        /// <summary>
        /// Fetch all Roles in PIMS and link or create them in keycloak.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="errorLog"></param>
        /// <returns></returns>
        private async Task SyncRolesAsync(StringBuilder log, StringBuilder errorLog)
        {
            IEnumerable<RoleModel> roles = new List<RoleModel>();
            PageModel<RoleModel> rolesPage = null;
            int page = 1;
            do
            {
                rolesPage = await _pimsClient.HandleRequestAsync<PageModel<RoleModel>>(HttpMethod.Get, $"admin/roles?page={page++}&quantity=50");
                roles = roles.Concat(rolesPage.Items);
            }
            while (rolesPage != null && rolesPage.Items.Any() && page < MAXPAGES);

            var keycloakRoles = await _keycloakRepository.GetAllRoles();
            var keycloakCompositeRoles = keycloakRoles.Data.Where(x => x.Composite.HasValue && x.Composite.Value);
            var keycloakRolesToDelete = keycloakCompositeRoles.Where(r => roles.All(crr => crr.Name != r.Name));

            var addTask = AddRolesFromPims(roles, log, errorLog);
            var removeTask = RemoveRolesFromPims(keycloakRolesToDelete, log, errorLog);
            await Task.WhenAll(addTask, removeTask);
        }

        private async Task AddRolesFromPims(IEnumerable<RoleModel> roles, StringBuilder log, StringBuilder errorLog)
        {
            var keycloakRoles = await _keycloakRepository.GetAllRoles();
            foreach (var role in roles)
            {
                try
                {
                    var prole = await _pimsClient.HandleRequestAsync<RoleModel>(HttpMethod.Get, $"admin/roles/{role.RoleUid}");
                    if (prole.RoleClaims.Count() == 0)
                    {
                        continue;
                    }

                    _logger.LogInformation($"Adding/updating keycloak composite role '{prole.Name}'");
                    var matchingKeycloakGroup = keycloakRoles.Data.FirstOrDefault(r => r.Name == prole.Name);
                    if (matchingKeycloakGroup != null)
                    {
                        await SyncGroupInKeycloak(matchingKeycloakGroup, prole);
                    }
                    else
                    {
                        await AddGroupToKeycloak(prole);
                    }
                    LogInfo(log, $"Added/updated keycloak composite role '{prole.Name}'");
                }
                catch (HttpClientRequestException ex)
                {
                    LogError(errorLog, $"Failed to Synchronize PIMS role '{role.Name}' with keycloak. Original error: {ex.Message}");
                }
            }
        }

        private async Task RemoveRolesFromPims(IEnumerable<KModel.RoleModel> roles, StringBuilder log, StringBuilder errorLog)
        {
            foreach (var role in roles)
            {
                try
                {
                    _logger.LogInformation($"Deleting keycloak role '{role.Name}'");
                    await _keycloakRepository.DeleteRole(role.Name);
                    LogInfo(log, $"Deleted keycloak role '{role.Name}'");
                }
                catch (HttpClientRequestException ex)
                {
                    LogError(errorLog, $"Failed to Delete PIMS role '{role.Name}' from keycloak. Original error: {ex.Message}");
                }
            }
        }

        #endregion

        /// <summary>
        /// Add a group to keycloak.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        private async Task AddGroupToKeycloak(RoleModel role)
        {
            var krole = new KModel.RoleModel()
            {
                Name = role.Name,
            };

            // Add the group to keycloak and sync with PIMS.
            var response = await _keycloakRepository.AddKeycloakRole(krole);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                await AddRolesToGroupInKeycloak(krole, role);
            }
            else
            {
                throw new HttpClientRequestException(response, $"Failed to add the group '{role.Name}' to keycloak");
            }
        }

        /// <summary>
        /// Ensure that a group in keycloak has exactly the same roles as a group in pims.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private async Task SyncGroupInKeycloak(KModel.RoleModel group, RoleModel role)
        {
            await RemoveRolesFromGroupInKeycloak(group, role);
            await AddRolesToGroupInKeycloak(group, role);
        }

        /// <summary>
        /// Add roles to the group in keycloak that are part of the group roles in pims.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private async Task AddRolesToGroupInKeycloak(KModel.RoleModel group, RoleModel role)
        {
            var allPimsGroupRoles = role.RoleClaims.Select(c => new KModel.RoleModel()
            {
                Name = c.Claim.Name,
            }).ToArray();

            var allKeycloakGroupRolesResponse = await _keycloakRepository.GetAllGroupRoles(group.Name);
            var newRolesToAdd = allPimsGroupRoles.Where(r => allKeycloakGroupRolesResponse.Data.All(crr => crr.Name != r.Name));
            if (newRolesToAdd.Any())
            {
                _logger.LogInformation($"Adding the following roles to the composite role {role.Name}: {string.Join(',', newRolesToAdd.Select(r => r.Name))}");
                var response = await _keycloakRepository.AddKeycloakRolesToGroup(group.Name, newRolesToAdd);
                _logger.LogInformation($"Added the following roles to the composite role {role.Name}: {string.Join(',', newRolesToAdd.Select(r => r.Name))}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpClientRequestException(response, $"Failed to update the group '{group.Name}' with the roles '{string.Join(',', newRolesToAdd.Select(r => r.Name))}' in keycloak");
                }
            }
        }

        /// <summary>
        /// Remove roles from the group in keycloak that are not part of the group roles in pims.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private async Task RemoveRolesFromGroupInKeycloak(KModel.RoleModel group, RoleModel role)
        {
            var allPimsGroupRoles = role.RoleClaims.Select(c => new KModel.RoleModel()
            {
                Name = c.Claim.Name,
            }).ToArray();

            var allKeycloakGroupRolesResponse = await _keycloakRepository.GetAllGroupRoles(group.Name);
            var rolesToRemove = allKeycloakGroupRolesResponse.Data.Where(r => allPimsGroupRoles.All(crr => crr.Name != r.Name));
            foreach (var roleToRemove in rolesToRemove)
            {
                _logger.LogInformation($"Deleting the following roles to the composite role {role.Name}: {roleToRemove}");

                // Update the group in keycloak.
                var response = await _keycloakRepository.DeleteRoleFromGroup(group.Name, roleToRemove.Name);
                _logger.LogInformation($"Deleting the following roles to the composite role {role.Name}: {roleToRemove}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpClientRequestException(response, $"Failed to delete the role '{roleToRemove.Name}' from group '{group.Name}' in keycloak");
                }
            }
        }

        #region User

        /// <summary>
        /// Fetch all Users in PIMS and update keycloak so they have the same roles and claims.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="errorLog"></param>
        /// <returns></returns>
        private async Task SyncUsersAsync(StringBuilder log, StringBuilder errorLog)
        {
            var page = 1;
            var quantity = 50;
            var users = new List<UserModel>();
            var pageOfUsers = await _pimsClient.HandleRequestAsync<PageModel<UserModel>>(HttpMethod.Get, $"admin/users?page={page}&quantity={quantity}");
            users.AddRange(pageOfUsers.Items);

            // Keep asking for pages of users until we have them all.
            while (pageOfUsers.Items.Count() == quantity && page < MAXPAGES)
            {
                pageOfUsers = await _pimsClient.HandleRequestAsync<PageModel<UserModel>>(HttpMethod.Get, $"admin/users?page={++page}&quantity={quantity}");
                users.AddRange(pageOfUsers.Items);
            }

            foreach (var user in users)
            {
                try
                {
                    var kuser = await GetUserAsync(user);

                    // Ignore users that only exist in PIMS.
                    if (kuser == null)
                    {
                        LogError(errorLog, $"Ignoring user: '{user.Person.FirstName} {user.Person.Surname}' as they were not found in keycloak. guid: {user.GuidIdentifierValue.ToString()}");
                        continue;
                    }

                    await SyncUserRoles(user, log);
                    LogInfo(log, $"Synchronized PIMS user '{user.Person.FirstName} {user.Person.Surname}' with guid: {user.GuidIdentifierValue.ToString()}");
                }
                catch (Exception ex)
                {
                    LogError(errorLog, $"Failed to Synchronize PIMS user '{user.Person.FirstName} {user.Person.Surname}' with keycloak. Original error: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Add all roles that are associated to a user in PIMS but not associated to that user in keycloak. Remove all roles that are associated to a user in keycloak that are not associated to that user in PIMS.
        /// </summary>
        /// <param name="user">The pims user to sync.</param>
        /// <param name="log"></param>
        /// <returns></returns>
        private async Task SyncUserRoles(UserModel user, StringBuilder log)
        {
            var username = user.GuidIdentifierValue.ToString().Replace("-", string.Empty) + "@idir";
            var response = await _keycloakRepository.GetUserRoles(username);
            var kUserRoles = response.Data;

            IEnumerable<KModel.RoleModel> userRolesToAdd = user.UserRoles.Where(ur => kUserRoles.All(kr => kr.Name != ur.Role.Name && ur.Role.IsDisabled == false))
                                                                .Select(ur => new KModel.RoleModel() { Name = ur.Role.Name });
            IEnumerable<KModel.RoleModel> userRolesToRemove = kUserRoles.Where(kur => user.UserRoles.All(ur => ur.Role.Name != kur.Name))
                                                                .Select(ur => new KModel.RoleModel() { Name = ur.Name });

            if (userRolesToAdd.Any())
            {
                _logger.LogInformation($"Executing operation 'add' on roles '{string.Join(',', userRolesToAdd.Select(r => r.Name))}' to user '{username}'");
                var addResponse = await _keycloakRepository.AddRolesToUser(username, userRolesToAdd);
                if (!addResponse.IsSuccessStatusCode)
                {
                    throw new HttpClientRequestException(addResponse, $"Failed to update the user role mappings for '{username}' during operation 'add' on roles '{string.Join(',', userRolesToAdd.Select(r => r.Name))}'");
                }
                LogInfo(log, $"Executed operation 'add' on roles '{string.Join(',', userRolesToAdd.Select(r => r.Name))}' to user '{username}'");
            }

            foreach (var userRoleToRemove in userRolesToRemove)
            {
                _logger.LogInformation($"Executing operation 'delete' on role '{userRoleToRemove.Name}' to user '{username}'");

                var deleteResponse = await _keycloakRepository.DeleteRoleFromUsers(username, userRoleToRemove.Name);
                if (!deleteResponse.IsSuccessStatusCode)
                {
                    throw new HttpClientRequestException(deleteResponse, $"Failed to update the user role mappings for '{username}' during operation 'delete' on role '{userRoleToRemove.Name}'");
                }
                LogInfo(log, $"Executed operation 'delete' on role '{userRoleToRemove.Name}' to user '{username}'");
            }
        }

        /// <summary>
        /// Get the keycloak user that matches the PIMS user.
        /// If it doesn't exist return 'null'.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<KModel.UserModel> GetUserAsync(UserModel user)
        {
            try
            {
                // Make a request to keycloak to find a matching user.
                var users = await _keycloakRepository.GetUsersAsync(user.GuidIdentifierValue);
                if (users.Count() > 1)
                {
                    throw new HttpClientRequestException($"Found multiple users in keycloak for GUID: user: {user.GuidIdentifierValue.ToString()}");
                }
                return users.FirstOrDefault();
            }
            catch (HttpClientRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw ex;
            }
        }

        private void LogInfo(StringBuilder log, string message)
        {
            _logger.LogInformation(message);
            log.Append($"{message}{Environment.NewLine}");
        }

        private void LogError(StringBuilder errorLog, string message)
        {
            _logger.LogError(message);
            errorLog.Append($"{message}{Environment.NewLine}");
        }
        #endregion
        #endregion
        #endregion
    }
}
