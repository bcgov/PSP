using Pims.Core.Helpers;
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
        /// Sync all the groups in keycloak with PIMS.
        /// This will delete any additional roles within PIMS.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Entity.PimsRole>> SyncRolesAsync()
        {
            var gcount = await _keycloakService.GetGroupCountAsync();

            var roles = new List<Entity.PimsRole>();
            for (var i = 0; i < gcount; i += 10)
            {
                var kgroups = await _keycloakService.GetGroupsAsync(i, 10);

                foreach (var kgroup in kgroups)
                {
                    var erole = _pimsRepository.Role.GetByName(kgroup.Name);

                    if (erole == null)
                    {
                        // Need to add the group as a role within PIMS.
                        erole = _mapper.Map<Entity.PimsRole>(kgroup);
                        _pimsRepository.Role.AddWithoutSave(erole);
                    }
                    else
                    {
                        _mapper.Map(kgroup, erole);
                        _pimsRepository.Role.UpdateWithoutSave(erole);
                    }

                    roles.Add(erole);
                }

                _pimsRepository.Role.CommitTransaction();
            }

            // Remove groups in PIMS that don't exist in keycloak.
            var roleIds = roles.Select(g => g.RoleUid).ToArray();
            _pimsRepository.Role.RemoveAll(roleIds);

            return roles;
        }

        /// <summary>
        /// Get an array of roles from keycloak and PIMS.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Entity.PimsRole>> GetRolesAsync(int page = 1, int quantity = 10, string search = null)
        {
            var kgroups = await _keycloakService.GetGroupsAsync((page - 1) * quantity, quantity, search);

            // TODO: Need better performing solution.
            return kgroups.Select(g => ExceptionHelper.HandleKeyNotFound(() => _pimsRepository.Role.GetByKeycloakId(g.Id)) ?? _mapper.Map<Entity.PimsRole>(g));
        }

        /// <summary>
        /// Get the role specified by the 'key', if they exist in keycloak and PIMS.
        /// </summary>
        /// <param name="key"></param>
        /// <exception type="KeyNotFoundException">User does not exist in keycloak or PIMS.</exception>
        /// <returns></returns>
        public async Task<Entity.PimsRole> GetRoleAsync(Guid key)
        {
            var role = ExceptionHelper.HandleKeyNotFound(() => _pimsRepository.Role.Get(key));
            if (role == null)
            {
                var kgroup = await _keycloakService.GetGroupAsync(key) ?? throw new KeyNotFoundException();
                return _mapper.Map<Entity.PimsRole>(kgroup);
            }
            return role;
        }

        /// <summary>
        /// Update the specified role in keycloak and PIMS.
        /// This will add the role to PIMS if it does not current exist.
        /// </summary>
        /// <param name="role"></param>
        /// <exception type="KeyNotFoundException">User does not exist in keycloak or PIMS.</exception>
        /// <returns></returns>
        public async Task<Entity.PimsRole> UpdateRoleAsync(Entity.PimsRole role)
        {
            if (await _keycloakService.GetGroupAsync(role.RoleUid) == null)
            {
                throw new KeyNotFoundException();
            }

            // Role does not exist in PIMS, it needs to be added.
            if (_pimsRepository.Role.Find(role.RoleId) == null)
            {
                _pimsRepository.Role.Add(role);
            }
            else
            {
                _pimsRepository.Role.Update(role);
            }

            var kmodel = _mapper.Map<KModel.GroupModel>(role);
            await _keycloakService.UpdateGroupAsync(kmodel);

            return role;
        }
    }
    #endregion
}
