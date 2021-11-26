using System.Security.Claims;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace Pims.Dal.Services
{
    /// <summary>
    /// BaseService abstract class, provides a generic service layer to perform CRUD operations on the datasource.
    /// TODO: Filename contains an tilde.
    /// </summary>
    /// <typeparam name="ET"></typeparam>
    public abstract class BaseService<ET> : BaseService
        where ET : class
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
        /// <param name="service"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        protected BaseService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<BaseService> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper)
        { }
        #endregion

        #region Methods
        /// <summary>
        /// Find the entity for the specified 'keyValues'.
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public ET Find(params object[] keyValues)
        {
            return this.Context.Find<ET>(keyValues);
        }
        #endregion
    }
}
