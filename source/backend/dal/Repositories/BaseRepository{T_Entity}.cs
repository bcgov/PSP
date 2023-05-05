using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// BaseService abstract class, provides a generic service layer to perform CRUD operations on the datasource.
    /// </summary>
    /// <typeparam name="T_Entity"></typeparam>
    public abstract class BaseRepository<T_Entity> : BaseRepository
        where T_Entity : class
    {
        #region Variables
        #endregion

        #region Properties
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a BaseService class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        protected BaseRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<BaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Find the entity for the specified 'keyValues'.
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public T_Entity Find(params object[] keyValues)
        {
            return this.Context.Find<T_Entity>(keyValues);
        }

        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
        #endregion
    }
}
