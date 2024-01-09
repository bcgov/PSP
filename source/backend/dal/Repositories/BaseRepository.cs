using System;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// BaseService abstract class, provides a generic service layer to perform CRUD operations on the datasource.
    /// </summary>
    public abstract class BaseRepository : IRepository
    {
        #region Variables
        #endregion

        #region Properties

        /// <summary>
        /// get - The current user accessing the service.
        /// </summary>
        protected ClaimsPrincipal User { get; }

        /// <summary>
        /// get - The datasource context object.
        /// </summary>
        protected PimsContext Context { get; }

        /// <summary>
        /// get - The logger.
        /// </summary>
        protected ILogger<BaseRepository> Logger { get; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a BaseService class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        protected BaseRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<BaseRepository> logger)
        {
            this.Context = dbContext;
            this.User = user;
            this.Logger = logger;
        }

        /// <summary>
        /// Commit all saved changes as a single transaction.
        /// </summary>
        public void CommitTransaction()
        {
            this.Context.CommitTransaction();
        }

        #endregion
    }
}
