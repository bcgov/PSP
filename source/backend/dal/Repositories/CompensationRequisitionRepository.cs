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
                .Include(c => c.PimsCompReqH120s)
                .Include(x => x.PimsAcquisitionPayees)
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
                .Include(c => c.PimsCompReqH120s)
                    .ThenInclude(y => y.FinancialActivityCode)
                .Include(x => x.PimsAcquisitionPayees)
                    .ThenInclude(y => y.AcquisitionOwner)
                .Include(x => x.PimsAcquisitionPayees)
                .AsNoTracking()
                .FirstOrDefault(x => x.CompensationRequisitionId.Equals(compensationRequisitionId)) ?? throw new KeyNotFoundException();

            return entity;
        }

        public PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition)
        {
            var existingCompensationRequisition = Context.PimsCompensationRequisitions.Include(c => c.PimsAcquisitionPayees)
                .FirstOrDefault(x => x.CompensationRequisitionId.Equals(compensationRequisition.CompensationRequisitionId)) ?? throw new KeyNotFoundException();

            Context.Entry(existingCompensationRequisition).CurrentValues.SetValues(compensationRequisition);
            Context.UpdateChild<PimsCompensationRequisition, long, PimsCompReqH120, long>(a => a.PimsCompReqH120s, compensationRequisition.CompensationRequisitionId, compensationRequisition.PimsCompReqH120s.ToArray(), true);

            if (compensationRequisition.PimsAcquisitionPayees.FirstOrDefault() is not null)
            {
                if (existingCompensationRequisition.PimsAcquisitionPayees.FirstOrDefault() is not null)
                {
                    UpdatePayee(compensationRequisition.PimsAcquisitionPayees.FirstOrDefault());
                }
                else
                {
                    Context.PimsAcquisitionPayees.Add(compensationRequisition.PimsAcquisitionPayees.FirstOrDefault());
                }
            }

            return compensationRequisition;
        }

        public PimsAcquisitionPayee UpdatePayee(PimsAcquisitionPayee compensationPayee)
        {
            var existingCompensationPayee = Context.PimsAcquisitionPayees
                .FirstOrDefault(x => x.AcquisitionPayeeId.Equals(compensationPayee.Internal_Id)) ?? throw new KeyNotFoundException();

            Context.Entry(existingCompensationPayee).CurrentValues.SetValues(compensationPayee);

            return compensationPayee;
        }

        public bool TryDelete(long compensationId)
        {
            var deletedEntity = Context.PimsCompensationRequisitions
                .Include(fa => fa.PimsCompReqH120s)
                .Include(cr => cr.PimsAcquisitionPayees)
                .AsNoTracking()
                .FirstOrDefault(c => c.CompensationRequisitionId == compensationId);

            if (deletedEntity != null)
            {
                foreach (var payee in deletedEntity.PimsAcquisitionPayees)
                {
                    Context.PimsAcquisitionPayees.Remove(new PimsAcquisitionPayee() { AcquisitionPayeeId = payee.AcquisitionPayeeId });
                }

                foreach (var financial in deletedEntity.PimsCompReqH120s)
                {
                    Context.PimsCompReqH120s.Remove(new PimsCompReqH120() { CompReqFinActivity = financial.CompReqFinActivity });
                }

                Context.CommitTransaction(); // TODO: required to enforce delete order. Can be removed when cascade deletes are implemented.

                Context.PimsCompensationRequisitions.Remove(new PimsCompensationRequisition() { CompensationRequisitionId = deletedEntity.CompensationRequisitionId });
                return true;
            }
            return false;
        }

        public PimsAcquisitionPayee GetPayee(long payeeId)
        {

            var payeeEntity = Context.PimsAcquisitionPayees
                .Include(ap => ap.AcquisitionFilePerson)
                    .ThenInclude(afp => afp.Person)
                .Include(ap => ap.AcquisitionOwner)
                .Include(ap => ap.InterestHolder)
                    .ThenInclude(ih => ih.InterestHolderTypeCodeNavigation)
                .Include(ap => ap.InterestHolder)
                    .ThenInclude(ih => ih.Person)
                .Include(ap => ap.InterestHolder)
                    .ThenInclude(ih => ih.Organization)
                .AsNoTracking()
                .FirstOrDefault(x => x.AcquisitionPayeeId.Equals(payeeId)) ?? throw new KeyNotFoundException();

            return payeeEntity;
        }
    }
}
