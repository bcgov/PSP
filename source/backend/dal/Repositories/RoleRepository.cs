using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Http.Configuration;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// RoleRepository class, provides a service layer to administrate roles within the datasource.
    /// </summary>
    public class RoleRepository : BaseRepository<PimsRole>, IRoleRepository
    {
        #region Variables
        private readonly IOptionsMonitor<AuthClientOptions> _keycloakOptions;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a RoleRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public RoleRepository(PimsContext dbContext, ClaimsPrincipal user, IOptionsMonitor<AuthClientOptions> options, ILogger<RoleRepository> logger)
            : base(dbContext, user, logger)
        {
            this._keycloakOptions = options;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a page of roles from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Paged<PimsRole> GetPage(int page, int quantity, string name = null)
        {
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.AdminRoles, _keycloakOptions);

            var query = this.Context.PimsRoles.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(r => EF.Functions.Like(r.Name, $"%{name}%"));
            }

            var roles = query.Skip((page - 1) * quantity).Take(quantity);
            return new Paged<PimsRole>(roles.ToArray(), page, quantity, query.Count());
        }

        /// <summary>
        /// Get the role with the specified 'id'.
        /// </summary>
        /// <param name="key"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public PimsRole GetByKey(Guid key)
        {
            return this.Context.PimsRoles
                .Include(r => r.PimsRoleClaims).ThenInclude(c => c.Claim)
                .AsNoTracking()
                .FirstOrDefault(u => u.RoleUid == key) ?? throw new KeyNotFoundException();
        }
        #endregion
    }
}
