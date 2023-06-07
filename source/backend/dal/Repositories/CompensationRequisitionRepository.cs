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
            Context.UpdateChild<PimsCompensationRequisition, long, PimsCompReqH120, long>(a => a.PimsCompReqH120s, compensationRequisition.CompensationRequisitionId, compensationRequisition.PimsCompReqH120s.ToArray(), false);

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
                        this.Context.PimsAcqPayeeCheques.Remove(new PimsAcqPayeeCheque() { AcqPayeeChequeId = cheque.AcqPayeeChequeId });
                    }

                    this.Context.CommitTransaction(); // TODO: required to enforce delete order. Can be removed when cascade deletes are implemented.

                    this.Context.PimsAcquisitionPayees.Remove(new PimsAcquisitionPayee() { AcquisitionPayeeId = payee.AcquisitionPayeeId });
                }

                this.Context.CommitTransaction(); // TODO: required to enforce delete order. Can be removed when cascade deletes are implemented.

                this.Context.PimsCompensationRequisitions.Remove(new PimsCompensationRequisition() { CompensationRequisitionId = deletedEntity.CompensationRequisitionId });
                return true;
            }
            return false;
        }

        public PimsAcquisitionPayee GetPayee(long compensationRequisitionId)
        {
            var compensationRequisition = GetById(compensationRequisitionId);
            var compensationPayee = compensationRequisition.PimsAcquisitionPayees?.FirstOrDefault();
            if (compensationRequisition is not null && compensationPayee is null)
            {
                throw new KeyNotFoundException();
            }

            var payeeEntity = Context.PimsAcquisitionPayees
                 .Include(x => x.AcquisitionFilePerson)
                 .Include(x => x.AcquisitionOwner)
                 .Include(x => x.InterestHolder)
                 .Include(x => x.OwnerRepresentative)
                 .Include(x => x.PimsAcqPayeeCheques)
                 .Include(x => x.OwnerSolicitor)
                 .AsNoTracking()
                .FirstOrDefault(x => x.AcquisitionPayeeId.Equals(compensationPayee.AcquisitionPayeeId));

            if (payeeEntity.PimsAcqPayeeCheques?.FirstOrDefault() is not null)
            {
                payeeEntity.PimsAcqPayeeCheques.FirstOrDefault().PretaxAmt = compensationRequisition.PayeeChequesPreTaxTotalAmount;
                payeeEntity.PimsAcqPayeeCheques.FirstOrDefault().TaxAmt = compensationRequisition.PayeeChequesTaxTotalAmount;
                payeeEntity.PimsAcqPayeeCheques.FirstOrDefault().TotalAmt = compensationRequisition.PayeeChequesTotalAmount;
            }

            return payeeEntity;
        }
    }
}
