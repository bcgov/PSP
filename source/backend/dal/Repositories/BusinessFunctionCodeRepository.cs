using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with financial codes within the datasource.
    /// </summary>
    public class BusinessFunctionCodeRepository : BaseRepository<PimsBusinessFunctionCode>, IBusinessFunctionCodeRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a BusinessFunctionCodeRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public BusinessFunctionCodeRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<BusinessFunctionCodeRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves all business function codes.
        /// </summary>
        /// <returns></returns>
        public IList<PimsBusinessFunctionCode> GetAllBusinessFunctionCodes()
        {
            return this.Context.PimsBusinessFunctionCodes.AsNoTracking()
                .ToList();
        }

        #endregion
    }
}
