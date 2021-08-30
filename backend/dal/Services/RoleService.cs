using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pims.Dal.Services
{
    /// <summary>
    /// RoleService class, provides a service layer to administrate users within the datasource.
    /// </summary>
    public class RoleService : BaseService<Role>, IRoleService
    {
        #region Variables
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a RoleService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public RoleService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<RoleService> logger) : base(dbContext, user, service, logger) { }
        #endregion

        #region Methods
        /// <summary>
        /// Get a page of users from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Paged<Role> Get(int page, int quantity, string name = null)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);

            var query = this.Context.Roles.AsNoTracking();

            if (!String.IsNullOrWhiteSpace(name))
                query = query.Where(r => EF.Functions.Like(r.Name, $"%{name}%"));

            var roles = query.Skip((page - 1) * quantity).Take(quantity);
            return new Paged<Role>(roles.ToArray(), page, quantity, query.Count());
        }

        /// <summary>
        /// Get the user with the specified 'id'.
        /// </summary>
        /// <param name="key"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Role Get(Guid key)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);

            return this.Context.Roles
                .Include(r => r.ClaimsManyToMany).ThenInclude(c => c.Claim)
                .AsNoTracking()
                .FirstOrDefault(u => u.Key == key) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get the role with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Role GetByName(string name)
        {
            return this.Context.Roles
                .Include(r => r.ClaimsManyToMany).ThenInclude(c => c.Claim)
                .AsNoTracking()
                .FirstOrDefault(r => r.Name == name) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get the user with the specified keycloak group 'key'.
        /// </summary>
        /// <param name="key"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Role GetByKeycloakId(Guid key)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);

            return this.Context.Roles
                .Include(r => r.ClaimsManyToMany).ThenInclude(c => c.Claim)
                .AsNoTracking()
                .FirstOrDefault(u => u.KeycloakGroupId == key) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Add the specified role to the datasource.
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public Role Add(Role add)
        {
            AddWithoutSave(add);
            this.Context.CommitTransaction();
            return add;
        }

        /// <summary>
        /// Add the specified role to the datasource.
        /// </summary>
        /// <param name="add"></param>
        /// <returns></returns>
        public void AddWithoutSave(Role add)
        {
            add.ThrowIfNull(nameof(add));
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);

            this.Context.Roles.Add(add);
        }

        /// <summary>
        /// Updates the specified role in the datasource.
        /// </summary>
        /// <param name="role"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Role Update(Role update)
        {
            var role = UpdateWithoutSave(update);
            this.Context.CommitTransaction();
            return role;
        }

        /// <summary>
        /// Updates the specified role in the datasource.
        /// </summary>
        /// <param name="role"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Role UpdateWithoutSave(Role update)
        {
            update.ThrowIfNull(nameof(update));
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);

            var role = this.Context.Roles.Find(update.Id) ?? throw new KeyNotFoundException();

            this.Context.Entry(role).CurrentValues.SetValues(update);
            this.Context.SetOriginalRowVersion(role);
            return role;
        }

        /// <summary>
        /// Remove the specified role from the datasource.
        /// </summary>
        /// <param name="entity"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        public void Delete(Role delete)
        {
            delete.ThrowIfNull(nameof(delete));
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);

            var role = this.Context.Roles.Find(delete.Id) ?? throw new KeyNotFoundException();

            role.RowVersion = delete.RowVersion;
            this.Context.SetOriginalRowVersion(role);
            this.Context.Roles.Remove(role);
            this.Context.CommitTransaction();
        }

        /// <summary>
        /// Remove the roles from the datasource, excluding those listed.
        /// </summary>
        /// <param name="exclude"></param>
        /// <returns></returns>
        public int RemoveAll(Guid[] exclude)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);
            var roles = this.Context.Roles.Include(r => r.Claims).Include(r => r.Users).Where(r => !exclude.Contains(r.Key));
            roles.ForEach(r =>
            {
                r.Claims.Clear();
                r.Users.Clear();
            });

            this.Context.Roles.RemoveRange(roles);
            var result = this.Context.CommitTransaction();
            return result;
        }
        #endregion
    }
}
