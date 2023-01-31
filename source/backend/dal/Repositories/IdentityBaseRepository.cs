using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// IdentityBaseRepository abstract class, provides a generic repository to perform CRUD operations on entities with ID.
    /// </summary>
    /// <typeparam name="T_Entity"></typeparam>
    public abstract class IdentityBaseRepository<T_Entity> : BaseRepository<T_Entity>
         where T_Entity : class, IIdentityEntity<long>, IBaseEntity
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a IdentityBaseRepository class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        protected IdentityBaseRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<IdentityBaseRepository<T_Entity>> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the entity with the specified id.
        /// NOTE: This does NOT load any relationships, only the main entity values.
        /// </summary>
        /// <param name="id">The entity Id.</param>
        /// <returns>The entity.</returns>
        public T_Entity GetById(long id)
        {
            return Context.Set<T_Entity>().AsNoTracking()
                .FirstOrDefault(x => x.Id == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Retrieves the row version of the entity with the specified id.
        /// </summary>
        /// <param name="id">The entity Id.</param>
        /// <returns>The entity row version.</returns>
        public long GetRowVersion(long id)
        {
            return Context.Set<T_Entity>().AsNoTracking()
                .Where(p => p.Id == id)?
                .Select(p => p.ConcurrencyControlNumber)?
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }
        #endregion
    }
}
