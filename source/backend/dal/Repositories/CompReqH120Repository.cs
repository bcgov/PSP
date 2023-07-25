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
    public class CompReqH120Repository : BaseRepository<PimsCompReqH120>, ICompReqH120Repository
    {
        #region Constructors
        private readonly ClaimsPrincipal _user;

        /// <summary>
        /// Creates a new instance of a CompReqH120Repository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public CompReqH120Repository(PimsContext dbContext, ClaimsPrincipal user, ILogger<BaseRepository> logger)
            : base(dbContext, user, logger)
        {
            this._user = user;
        }
        #endregion

        public IEnumerable<PimsCompReqH120> GetAllByAcquisitionFileId(long acquisitionFileId, bool? finalOnly)
        {
            this._user.ThrowIfNotAllAuthorized(Security.Permissions.CompensationRequisitionView);

            var query = Context.PimsCompReqH120s
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
