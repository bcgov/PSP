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
        /// Get the original value of the specified 'propertyName'.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public T OriginalValue<T>(object entity, string propertyName)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.");
            }

            return (T)this.Context.Entry(entity).OriginalValues[propertyName];
        }

        /// <summary>
        /// Commit all saved changes as a single transaction.
        /// </summary>
        public void CommitTransaction()
        {
            this.Context.CommitTransaction();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Provides a way to fetch the context within the assembly.
        /// </summary>
        /// <returns></returns>
        internal PimsContext GetContext()
        {
            return this.Context;
        }

        /// <summary>
        /// Provides a way to fetch the user within the assembly.
        /// </summary>
        /// <returns></returns>
        internal ClaimsPrincipal GetUser()
        {
            return this.User;
        }
        #endregion
    }
}
