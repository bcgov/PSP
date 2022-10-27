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
            var euser = _userRepository.GetTracking(user.Id);

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
            var euser = _userRepository.GetTracking(update.Id);

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

            var accessRequest = _accessRequestRepository.Get(update.AccessRequestId);
            var user = _userRepository.Get(accessRequest.UserId);

            if (update.AccessRequestStatusTypeCode == AccessRequestStatusTypes.APPROVED)
            {

                user.PimsUserRoles.Clear();
                user.IsDisabled = false;
                user.PimsUserRoles.Add(new Entity.PimsUserRole() { UserId = user.Id, RoleId = update.RoleId.Value });
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
                euser.PimsRegionUsers.ForEach(region => _userRepository.RemoveRegion(euser, region.Id));
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

            euser = _userRepository.UpdateOnly(euser);

            // Now update keycloak
            var kmodel = _mapper.Map<KModel.UserModel>(update);

            if (resetRoles)
            {
                var userGroups = await _keycloakService.GetUserGroupsAsync(euser.GuidIdentifierValue.Value);
                foreach (var group in userGroups)
                {
                    try
                    {
                        var matchingPimsRole = _roleRepository.GetByKeycloakId(group.Id);
                        if (matchingPimsRole != null)
                        {
                            await _keycloakService.RemoveGroupFromUserAsync(update.GuidIdentifierValue.Value, group.Id);
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        // ignore any roles that are in keycloak but not in PIMS
                    }
                }
            }

            var roleIds = update.PimsUserRoles.Select(r => r.RoleId);
            foreach (var roleId in roleIds)
            {
                var role = _roleRepository.Find(roleId) ?? throw new KeyNotFoundException("Cannot assign a role to a user, when the role does not exist.");
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

            return _userRepository.Get(euser.Id);
        }
    }
    #endregion
}
