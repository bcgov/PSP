using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Pims.Dal.Services
{
    /// <summary>
    /// BaseService abstract class, provides a generic service layer to perform business logic.
    /// It can access the datastore via available repositories.
    /// </summary>
    public abstract class BaseService
    {
        #region Properties

        /// <summary>
        /// get - The current user accessing the service.
        /// </summary>
        protected ClaimsPrincipal User { get; }

        /// <summary>
        /// get - The logger.
        /// </summary>
        protected ILogger<BaseService> Logger { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a BaseService class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        protected BaseService(ClaimsPrincipal user, ILogger<BaseService> logger)
        {
            this.User = user;
            this.Logger = logger;
        }
        #endregion

        #region Methods

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
