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
            return Context.PimsCompensationRequisitions
                .Include(c => c.PimsCompReqFinancials)
                .AsNoTracking()
                .Where(c => c.AcquisitionFileId == acquisitionFileId).ToList();
        }

        public PimsCompensationRequisition Add(PimsCompensationRequisition compensationRequisition)
        {
            Context.PimsCompensationRequisitions.Add(compensationRequisition);

            return compensationRequisition;
        }

        public PimsCompensationRequisition GetById(long compensationRequisitionId)
        {
            var entity = Context.PimsCompensationRequisitions
                .Include(x => x.YearlyFinancial)
                .Include(x => x.ChartOfAccounts)
                .Include(x => x.Responsibility)
                .Include(c => c.PimsCompReqFinancials)
                    .ThenInclude(y => y.FinancialActivityCode)
                .Include(x => x.AcquisitionOwner)
                .Include(x => x.AcquisitionFilePerson)
                .Include(x => x.InterestHolder)
                .AsNoTracking()
                .FirstOrDefault(x => x.CompensationRequisitionId.Equals(compensationRequisitionId)) ?? throw new KeyNotFoundException();

            return entity;
        }

        public PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition)
        {
            var existingCompensationRequisition = Context.PimsCompensationRequisitions
                .FirstOrDefault(x => x.CompensationRequisitionId.Equals(compensationRequisition.CompensationRequisitionId)) ?? throw new KeyNotFoundException();

            Context.Entry(existingCompensationRequisition).CurrentValues.SetValues(compensationRequisition);
            Context.UpdateChild<PimsCompensationRequisition, long, PimsCompReqFinancial, long>(a => a.PimsCompReqFinancials, compensationRequisition.CompensationRequisitionId, compensationRequisition.PimsCompReqFinancials.ToArray(), true);

            // Todo: fix this
            /*if (compensationRequisition.PimsAcquisitionPayees.FirstOrDefault() is not null)
            {
                if (existingCompensationRequisition.PimsAcquisitionPayees.FirstOrDefault() is not null)
                {
                    UpdatePayee(compensationRequisition.PimsAcquisitionPayees.FirstOrDefault());
                }
                else
                {
                    Context.PimsAcquisitionPayees.Add(compensationRequisition.PimsAcquisitionPayees.FirstOrDefault());
                }
            }*/

            return compensationRequisition;
        }

        public bool TryDelete(long compensationId)
        {
            var deletedEntity = Context.PimsCompensationRequisitions
                .Include(fa => fa.PimsCompReqFinancials)
                .AsNoTracking()
                .FirstOrDefault(c => c.CompensationRequisitionId == compensationId);

            if (deletedEntity != null)
            {
                // TODO: Fix this;
                /*foreach (var payee in deletedEntity.PimsAcquisitionPayees)
                {
                    Context.PimsAcquisitionPayees.Remove(new PimsAcquisitionPayee() { AcquisitionPayeeId = payee.AcquisitionPayeeId });
                }*/

                foreach (var financial in deletedEntity.PimsCompReqFinancials)
                {
                    Context.PimsCompReqFinancials.Remove(new PimsCompReqFinancial() { FinancialActivityCode = financial.FinancialActivityCode });
                }

                Context.CommitTransaction(); // TODO: required to enforce delete order. Can be removed when cascade deletes are implemented.

                Context.PimsCompensationRequisitions.Remove(new PimsCompensationRequisition() { CompensationRequisitionId = deletedEntity.CompensationRequisitionId });
                return true;
            }
            return false;
        }


    }
}
