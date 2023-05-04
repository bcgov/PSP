using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with compensation requisitions within the datasource.
    /// </summary>
    public class CompensationRequisitionRepository : BaseRepository<PimsCompensationRequisition>, ICompensationRequisitionRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a CompensationRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public CompensationRequisitionRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<BaseRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        public IList<PimsCompensationRequisition> GetAllByAcquisitionFileId(long acquisitionFileId)
        {
            return Context.PimsCompensationRequisitions.Include(c => c.PimsCompReqH120s).AsNoTracking().Where(c => c.AcquisitionFileId == acquisitionFileId).ToList();
        }

        public PimsCompensationRequisition Add(PimsCompensationRequisition compensationRequisition)
        {
            User.ThrowIfNotAuthorized(Permissions.CompensationRequisitionAdd);
            Context.PimsCompensationRequisitions.Add(compensationRequisition);

            return compensationRequisition;
        }

        public void Delete(long compensationRequisitionId)
        {
            User.ThrowIfNotAuthorized(Permissions.CompensationRequisitionDelete);
            var entity = Context.PimsCompensationRequisitions.FirstOrDefault(d => d.CompensationRequisitionId == compensationRequisitionId);
            if (entity is not null)
            {
                Context.PimsCompensationRequisitions.Remove(entity);
            }

            return;
        }

        public PimsCompensationRequisition GetById(long compensationRequisitionId)
        {
            User.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);
            var entity = Context.PimsCompensationRequisitions
                .AsNoTracking()
                .FirstOrDefault(x => x.CompensationRequisitionId.Equals(compensationRequisitionId)) ?? throw new KeyNotFoundException();

            return entity;
        }

        public PimsCompensationRequisition Update(long compensationRequisitionId, PimsCompensationRequisition compensationRequisition)
        {
            User.ThrowIfNotAuthorized(Permissions.CompensationRequisitionEdit);
            var existingCompensationRequisition = Context.PimsCompensationRequisitions
                .FirstOrDefault(x => x.CompensationRequisitionId.Equals(compensationRequisitionId)) ?? throw new KeyNotFoundException();

            Context.Entry(existingCompensationRequisition).CurrentValues.SetValues(compensationRequisition);

            return compensationRequisition;
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
    }
}
