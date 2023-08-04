using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with compensation requisition h120s within the datasource.
    /// </summary>
    public class CompReqFinancialRepository : BaseRepository<PimsCompReqFinancial>, ICompReqFinancialRepository
    {
        #region Constructors
        private readonly ClaimsPrincipal _user;

        /// <summary>
        /// Creates a new instance of a CompReqFinancialRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public CompReqFinancialRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<BaseRepository> logger)
            : base(dbContext, user, logger)
        {
            this._user = user;
        }
        #endregion

        public IEnumerable<PimsCompReqFinancial> GetAllByAcquisitionFileId(long acquisitionFileId, bool? finalOnly)
        {
            this._user.ThrowIfNotAllAuthorized(Security.Permissions.CompensationRequisitionView);

            var query = Context.PimsCompReqFinancials
                .Include(c => c.CompensationRequisition)
                .Where(c => c.CompensationRequisition.AcquisitionFileId == acquisitionFileId);

            if (finalOnly == true)
            {
                query = query.Where(c => c.CompensationRequisition.IsDraft == false);
            }
            return query.ToArray();
        }
    }
}
