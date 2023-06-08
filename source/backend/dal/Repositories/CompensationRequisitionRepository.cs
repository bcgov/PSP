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
                    .ThenInclude(y => y.PimsAcqPayeeCheques)
                .AsNoTracking()
                .FirstOrDefault(x => x.CompensationRequisitionId.Equals(compensationRequisitionId)) ?? throw new KeyNotFoundException();

            return entity;
        }

        public PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition)
        {
            var existingCompensationRequisition = Context.PimsCompensationRequisitions
                .FirstOrDefault(x => x.CompensationRequisitionId.Equals(compensationRequisition.CompensationRequisitionId)) ?? throw new KeyNotFoundException();

            Context.Entry(existingCompensationRequisition).CurrentValues.SetValues(compensationRequisition);
            Context.UpdateChild<PimsCompensationRequisition, long, PimsCompReqH120, long>(a => a.PimsCompReqH120s, compensationRequisition.CompensationRequisitionId, compensationRequisition.PimsCompReqH120s.ToArray(), true);

            return compensationRequisition;
        }

        public PimsAcquisitionPayee UpdatePayee(PimsAcquisitionPayee compensationPayee)
        {
            var existingCompensationPayee = Context.PimsAcquisitionPayees
                .FirstOrDefault(x => x.AcquisitionPayeeId.Equals(compensationPayee.AcquisitionPayeeId)) ?? throw new KeyNotFoundException();

            Context.Entry(existingCompensationPayee).CurrentValues.SetValues(compensationPayee);

            return compensationPayee;
        }

        public PimsAcqPayeeCheque UpdatePayeeCheque(PimsAcqPayeeCheque payeeCheque)
        {
            var existingPayeeCheque = Context.PimsAcqPayeeCheques
                .FirstOrDefault(x => x.AcqPayeeChequeId.Equals(payeeCheque.AcqPayeeChequeId)) ?? throw new KeyNotFoundException();

            Context.Entry(existingPayeeCheque).CurrentValues.SetValues(payeeCheque);

            return existingPayeeCheque;
        }

        public bool TryDelete(long compensationId)
        {
            var deletedEntity = Context.PimsCompensationRequisitions
                .Include(fa => fa.PimsCompReqH120s)
                .Include(cr => cr.PimsAcquisitionPayees)
                    .ThenInclude(ap => ap.PimsAcqPayeeCheques)
                .AsNoTracking()
                .FirstOrDefault(c => c.CompensationRequisitionId == compensationId);

            if (deletedEntity != null)
            {
                // Remove child entries.
                foreach (var payee in deletedEntity.PimsAcquisitionPayees)
                {
                    foreach (var cheque in payee.PimsAcqPayeeCheques)
                    {
                        Context.PimsAcqPayeeCheques.Remove(new PimsAcqPayeeCheque() { AcqPayeeChequeId = cheque.AcqPayeeChequeId });
                    }

                    Context.CommitTransaction(); // TODO: required to enforce delete order. Can be removed when cascade deletes are implemented.

                    Context.PimsAcquisitionPayees.Remove(new PimsAcquisitionPayee() { AcquisitionPayeeId = payee.AcquisitionPayeeId });
                }

                foreach(var financial in deletedEntity.PimsCompReqH120s)
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
                    .ThenInclude(ih => ih.Person)
                .Include(ap => ap.InterestHolder)
                    .ThenInclude(ih => ih.Organization)
                .Include(ap => ap.OwnerRepresentative)
                    .ThenInclude(or => or.Person)
                .Include(ap => ap.OwnerSolicitor)
                    .ThenInclude(os => os.Organization)
                .Include(ap => ap.OwnerSolicitor)
                    .ThenInclude(os => os.Person)
                .Include(ap => ap.PimsAcqPayeeCheques)
                .AsNoTracking()
                .FirstOrDefault(x => x.AcquisitionPayeeId.Equals(payeeId)) ?? throw new KeyNotFoundException();

            return payeeEntity;
        }
    }
}
