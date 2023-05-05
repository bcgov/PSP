using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with compensation requisitions within the datasource.
    /// </summary>
    public class CompensationRepository : BaseRepository<PimsCompensationRequisition>, ICompensationRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a CompensationRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public CompensationRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<CompensationRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods
        public IList<PimsCompensationRequisition> GetAllByAcquisitionFileId(long acquisitionFileId)
        {
            return Context.PimsCompensationRequisitions.Include(c => c.PimsCompReqH120s).AsNoTracking().Where(c => c.AcquisitionFileId == acquisitionFileId).ToList();
        }

        public bool TryDelete(long compensationId)
        {
            var deletedEntity = Context.PimsCompensationRequisitions.FirstOrDefault(c => c.CompensationRequisitionId == compensationId);
            if (deletedEntity != null)
            {
                Context.Remove(deletedEntity);
                return true;
            }
            return false;
        }
        #endregion
    }
}
