using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Http.Configuration;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Core.Security;
using Pims.Core.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ClaimRepository class, provides a service layer to administrate users within the datasource.
    /// </summary>
    public class ClaimRepository : BaseRepository<PimsClaim>, IClaimRepository
    {
        #region Variables
        private readonly IOptionsMonitor<AuthClientOptions> _keycloakOptions;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a ClaimRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ClaimRepository(PimsContext dbContext, System.Security.Claims.ClaimsPrincipal user, IOptionsMonitor<AuthClientOptions> options, ILogger<ClaimRepository> logger)
            : base(dbContext, user, logger)
        {
            _keycloakOptions = options;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get a page of users from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Paged<PimsClaim> GetPage(int page, int quantity, string name = null)
        {
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.AdminRoles, _keycloakOptions);

            var query = this.Context.PimsClaims.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(r => EF.Functions.Like(r.Name, $"%{name}%"));
            }

            var claims = query.Skip((page - 1) * quantity).Take(quantity);
            return new Paged<PimsClaim>(claims.ToArray(), page, quantity, query.Count());
        }

        /// <summary>
        /// Get the claim with the specified 'key'.
        /// </summary>
        /// <param name="key"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public PimsClaim GetByKey(Guid key)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);

            return this.Context.PimsClaims.AsNoTracking().FirstOrDefault(c => c.ClaimUid == key) ?? throw new KeyNotFoundException();
        }
        #endregion
    }
}
