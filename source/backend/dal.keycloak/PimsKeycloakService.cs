using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Core.Security;
using Pims.Keycloak;
using Pims.Keycloak.Models;
using Entity = Pims.Dal.Entities;
using KModel = Pims.Keycloak.Models;

namespace Pims.Dal.Keycloak
{
    /// <summary>
    /// PimsKeycloakService class, provides a way to integrate both PIMS and Keycloak datasources.
    /// </summary>
    public class PimsKeycloakService : IPimsKeycloakService
    {
        #region Variable
        private readonly IKeycloakRepository _keycloakRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAccessRequestRepository _accessRequestRepository;
        private readonly ClaimsPrincipal _user;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PimsKeycloakService object, initializes with the specified arguments.
        /// </summary>
        /// <param name="keycloakRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="accessRequestRepository"></param>
        /// <param name="user"></param>
        public PimsKeycloakService(
            IKeycloakRepository keycloakRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IAccessRequestRepository accessRequestRepository,
            ClaimsPrincipal user)
        {
            _keycloakRepository = keycloakRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _accessRequestRepository = accessRequestRepository;
            _user = user;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Update the specified user in keycloak and PIMS.
        /// </summary>
        /// <param name="user"></param>
        /// <exception type="KeyNotFoundException">User does not exist in keycloak or PIMS.</exception>
        /// <returns></returns>
        public async Task<Entity.PimsUser> UpdateUserAsync(Entity.PimsUser user)
        {
            var kuser = await _keycloakRepository.GetUserAsync(user.GuidIdentifierValue.Value);
            var euser = _userRepository.GetTrackingById(user.Internal_Id);

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
            var kuser = await _keycloakRepository.GetUserAsync(update.GuidIdentifierValue.Value) ?? throw new KeyNotFoundException("User does not exist in Keycloak");
            var euser = _userRepository.GetTrackingById(update.Internal_Id);

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

            var accessRequest = _accessRequestRepository.GetById(update.AccessRequestId);
            var user = _userRepository.GetById(accessRequest.UserId);

            if (update.AccessRequestStatusTypeCode == AccessRequestStatusTypes.APPROVED)
            {
                // Copy access request notes to the user's notes on approval
                user.Note = update.Note;
                user.PimsUserRoles.Clear();
                user.PimsRegionUsers.Clear();
                user.IsDisabled = false;
                user.PimsUserRoles.Add(new Entity.PimsUserRole() { UserId = user.Internal_Id, RoleId = update.RoleId.Value });
                user.PimsRegionUsers.Add(new Entity.PimsRegionUser() { UserId = user.Internal_Id, RegionCode = update.RegionCode });
                await AppendToUserAsync(user);
            }
            update.User = user;
            update.Role = _roleRepository.Find(update.RoleId);

            return _accessRequestRepository.Update(update);
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
                var roleIds = euser.PimsUserRoles.Select(ur => ur.RoleId).ToArray();
                for (int i = 0; i < roleIds.Length; i++)
                {
                    _userRepository.RemoveRole(euser, roleIds[i]);
                }
            }

            // Update PIMS
            if (kuser != null)
            {
                var idirUsername = kuser.Attributes?.FirstOrDefault(a => a.Key == "idir_username").Value.FirstOrDefault();
                if (idirUsername == null)
                {
                    throw new KeyNotFoundException("keycloak user missing required idir_username attribute");
                }
                euser.BusinessIdentifierValue = idirUsername; // PIMS must use whatever username is set in keycloak.
            }
            euser.Person.FirstName = update.Person.FirstName;
            euser.Person.MiddleNames = update.Person.MiddleNames;
            euser.Person.Surname = update.Person.Surname;
            euser.Position = update.Position;
            euser.UserTypeCode = update.UserTypeCode;
            euser.Note = update.Note;
            euser.IsDisabled = update.IsDisabled;
            euser.ConcurrencyControlNumber = update.ConcurrencyControlNumber;

            _userRepository.UpdateAllRolesForUser(euser.Internal_Id, update.PimsUserRoles);
            _userRepository.UpdateAllRegionsForUser(euser.Internal_Id, update.PimsRegionUsers);

            var newWorkEmail = update.Person.PimsContactMethods?.OrderByDescending(cm => cm.IsPreferredMethod).FirstOrDefault(cm => cm.ContactMethodTypeCode == ContactMethodTypes.WorkEmail);
            if (newWorkEmail != null)
            {
                var existingWorkEmail = euser.Person.PimsContactMethods?.OrderByDescending(cm => cm.IsPreferredMethod).FirstOrDefault(cm => cm.ContactMethodTypeCode == ContactMethodTypes.WorkEmail || cm.ContactMethodTypeCode == ContactMethodTypes.PerseEmail);
                if (existingWorkEmail != null)
                {
                    existingWorkEmail.ContactMethodValue = update.Person.GetEmail();
                }
                else
                {
                    euser.Person.PimsContactMethods.Add(newWorkEmail);
                }
            }

            euser = _userRepository.UpdateOnly(euser);

            var roles = update.IsDisabled.HasValue && update.IsDisabled.Value ? System.Array.Empty<PimsRole>() : euser.PimsUserRoles.Select(ur => _roleRepository.Find(ur.RoleId));

            // Now update keycloak
            if (kuser != null)
            {
                var keycloakUserGroups = await _keycloakRepository.GetUserGroupsAsync(euser.GuidIdentifierValue.Value);
                var newRolesToAdd = roles.Where(r => keycloakUserGroups.All(crr => crr.Name != r.Name));
                var rolesToRemove = keycloakUserGroups.Where(r => roles.All(crr => crr.Name != r.Name));
                var addOperations = newRolesToAdd.Select(nr => new UserRoleOperation() { Operation = "add", RoleName = nr.Name, Username = update.GetIdirUsername() });
                var removeOperations = rolesToRemove.Select(rr => new UserRoleOperation() { Operation = "del", RoleName = rr.Name, Username = update.GetIdirUsername() });


                await _keycloakRepository.ModifyUserRoleMappings(addOperations.Concat(removeOperations));
            }
            _userRepository.CommitTransaction();

            return _userRepository.GetById(euser.Internal_Id);
        }
    }
    #endregion
}
