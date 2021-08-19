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

namespace Pims.Dal.Services
{
    /// <summary>
    /// ClaimService class, provides a service layer to administrate users within the datasource.
    /// </summary>
    public class ClaimService : BaseService<Claim>, IClaimService
    {
        #region Variables
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ClaimService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public ClaimService(PimsContext dbContext, System.Security.Claims.ClaimsPrincipal user, IPimsService service, ILogger<ClaimService> logger) : base(dbContext, user, service, logger) { }
        #endregion

        #region Methods
        /// <summary>
        /// Get a page of users from the datasource.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="quantity"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Paged<Claim> Get(int page, int quantity, string name = null)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);

            var query = this.Context.Claims.AsNoTracking();

            if (!String.IsNullOrWhiteSpace(name))
                query = query.Where(r => EF.Functions.Like(r.Name, $"%{name}%"));

            var claims = query.Skip((page - 1) * quantity).Take(quantity);
            return new Paged<Claim>(claims.ToArray(), page, quantity, query.Count());
        }

        /// <summary>
        /// Get the claim with the specified 'key'.
        /// </summary>
        /// <param name="key"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Claim Get(Guid key)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);

            return this.Context.Claims.AsNoTracking().FirstOrDefault(c => c.Key == key) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get the claim with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Claim GetByName(string name)
        {
            return this.Context.Claims.AsNoTracking().FirstOrDefault(c => c.Name == name) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Updates the specified claim in the datasource.
        /// </summary>
        /// <param name="add"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Claim Add(Claim add)
        {
            add.ThrowIfNull(nameof(add));
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);

            this.Context.Claims.Add(add);
            this.Context.CommitTransaction();
            return add;
        }

        /// <summary>
        /// Updates the specified claim in the datasource.
        /// </summary>
        /// <param name="update"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        /// <returns></returns>
        public Claim Update(Claim update)
        {
            update.ThrowIfNull(nameof(update));
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);

            var claim = this.Context.Claims.Find(update.Id) ?? throw new KeyNotFoundException();

            this.Context.Entry(claim).CurrentValues.SetValues(update);
            this.Context.CommitTransaction();
            return claim;
        }

        /// <summary>
        /// Remove the specified claim from the datasource.
        /// </summary>
        /// <param name="delete"></param>
        /// <exception type="KeyNotFoundException">Entity does not exist in the datasource.</exception>
        public void Delete(Claim delete)
        {
            delete.ThrowIfNull(nameof(delete));
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);

            var claim = this.Context.Claims.Find(delete.Id) ?? throw new KeyNotFoundException();

            this.Context.Entry(claim).CurrentValues.SetValues(delete);
            this.Context.CommitTransaction();
        }

        /// <summary>
        /// Remove the claims from the datasource, excluding those listed.
        /// </summary>
        /// <param name="exclude"></param>
        /// <returns></returns>
        public int RemoveAll(Guid[] exclude)
        {
            this.User.ThrowIfNotAuthorized(Permissions.AdminRoles);
            var claims = this.Context.Claims
                .Include(r => r.Roles)
                .Where(r => !exclude.Contains(r.Key));
            claims.ForEach(r =>
            {
                r.Roles.Clear();
            });

            this.Context.Claims.RemoveRange(claims);
            var result = this.Context.CommitTransaction();
            return result;
        }
        #endregion
    }
}
