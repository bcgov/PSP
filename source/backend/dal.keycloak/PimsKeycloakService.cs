using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
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
        private readonly IKeycloakService _keycloakService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAccessRequestRepository _accessRequestRepository;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _user;
        private readonly ILogger<IPimsKeycloakService> _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PimsKeycloakService object, initializes with the specified arguments.
        /// </summary>
        /// <param name="keycloakService"></param>
        /// <param name="userRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="user"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public PimsKeycloakService(
            IKeycloakService keycloakService,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IAccessRequestRepository accessRequestRepository,
            ClaimsPrincipal user,
            IMapper mapper,
            ILogger<IPimsKeycloakService> logger)
        {
            _keycloakService = keycloakService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _accessRequestRepository = accessRequestRepository;
            _mapper = mapper;
            _user = user;
            _logger = logger;
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
            var kuser = await _keycloakService.GetUserAsync(user.GuidIdentifierValue.Value) ?? throw new KeyNotFoundException("User does not exist in Keycloak");
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
            var kuser = await _keycloakService.GetUserAsync(update.GuidIdentifierValue.Value) ?? throw new KeyNotFoundException("User does not exist in Keycloak");
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

                user.PimsUserRoles.Clear();
                user.IsDisabled = false;
                user.PimsUserRoles.Add(new Entity.PimsUserRole() { UserId = user.Internal_Id, RoleId = update.RoleId.Value });
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
                euser.PimsUserRoles.ForEach(role => _userRepository.RemoveRole(euser, role.RoleId));
                euser.PimsRegionUsers.ForEach(region => _userRepository.RemoveRegion(euser, region.Internal_Id));
            }

            // Update PIMS
            var idirUsername = kuser.Attributes?.FirstOrDefault(a => a.Key == "idir_username").Value.FirstOrDefault();
            if(idirUsername == null)
            {
                throw new KeyNotFoundException("keycloak user missing required idir_username attribute");
            }
            euser.BusinessIdentifierValue = idirUsername; // PIMS must use whatever username is set in keycloak.
            euser.Person.FirstName = update.Person.FirstName;
            euser.Person.MiddleNames = update.Person.MiddleNames;
            euser.Person.Surname = update.Person.Surname;
            euser.Position = update.Position;
            euser.Note = update.Note;
            euser.IsDisabled = update.IsDisabled;
            euser.ConcurrencyControlNumber = update.ConcurrencyControlNumber;
            euser.PimsUserRoles = update.PimsUserRoles;
            euser.PimsRegionUsers = update.PimsRegionUsers;

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
            var keycloakUserGroups = await _keycloakService.GetUserGroupsAsync(euser.GuidIdentifierValue.Value);
            var newRolesToAdd = roles.Where(r => keycloakUserGroups.All(crr => crr.Name != r.Name));
            var rolesToRemove = keycloakUserGroups.Where(r => roles.All(crr => crr.Name != r.Name));
            var addOperations = newRolesToAdd.Select(nr => new UserRoleOperation() { Operation = "add", RoleName = nr.Name, Username = update.GetIdirUsername() });
            var removeOperations = rolesToRemove.Select(rr => new UserRoleOperation() { Operation = "del", RoleName = rr.Name, Username = update.GetIdirUsername() });

            await _keycloakService.ModifyUserRoleMappings(addOperations.Concat(removeOperations));
            _userRepository.CommitTransaction();

            return _userRepository.GetById(euser.Internal_Id);
        }
    }
    #endregion
}
