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
            User.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);

            return Context.PimsCompensationRequisitions
                .Include(c => c.PimsCompReqH120s)
                .AsNoTracking()
                .Where(c => c.AcquisitionFileId == acquisitionFileId).ToList();
        }

        public PimsCompensationRequisition Add(PimsCompensationRequisition compensationRequisition)
        {
            User.ThrowIfNotAuthorized(Permissions.CompensationRequisitionAdd);
            Context.PimsCompensationRequisitions.Add(compensationRequisition);

            return compensationRequisition;
        }

        public PimsCompensationRequisition GetById(long compensationRequisitionId)
        {
            User.ThrowIfNotAuthorized(Permissions.CompensationRequisitionView);
            var entity = Context.PimsCompensationRequisitions
                .Include(x => x.YearlyFinancial)
                .Include(x => x.ChartOfAccounts)
                .Include(x => x.Responsibility)
                .Include(c => c.PimsCompReqH120s)
                    .ThenInclude(y => y.FinancialActivityCode)
                .Include(x => x.PimsAcquisitionPayees)
                    .ThenInclude(y => y.AcquisitionOwner)
                .Include(x => x.PimsAcquisitionPayees)
                    .ThenInclude(y => y.PimsAcqPayeeCheques)
                .AsNoTracking()
                .FirstOrDefault(x => x.CompensationRequisitionId.Equals(compensationRequisitionId)) ?? throw new KeyNotFoundException();

            return entity;
        }

        public PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition)
        {
            User.ThrowIfNotAuthorized(Permissions.CompensationRequisitionEdit);
            var existingCompensationRequisition = Context.PimsCompensationRequisitions
                .FirstOrDefault(x => x.CompensationRequisitionId.Equals(compensationRequisition.Internal_Id)) ?? throw new KeyNotFoundException();

            Context.Entry(existingCompensationRequisition).CurrentValues.SetValues(compensationRequisition);
            Context.UpdateChild<PimsCompensationRequisition, long, PimsCompReqH120, long>(p => p.PimsCompReqH120s, compensationRequisition.Internal_Id, compensationRequisition.PimsCompReqH120s.ToArray());
            //Context.UpdateGrandchild<PimsCompensationRequisition, long, PimsAcquisitionPayee>(p => p.PimsAcquisitionPayees, ch => ch.PimsAcqPayeeCheques, compensationRequisition.Internal_Id, compensationRequisition.PimsAcquisitionPayees.ToArray());

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
