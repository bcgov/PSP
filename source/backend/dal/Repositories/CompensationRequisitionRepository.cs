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

        public IList<PimsCompensationRequisition> GetAllByLeaseFileId(long leaseFileId)
        {
            return Context.PimsCompensationRequisitions
                .Include(c => c.PimsCompReqFinancials)
                .AsNoTracking()
                .Where(c => c.LeaseId == leaseFileId).ToList();
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
                .Include(x => x.AcquisitionFileTeam)
                .Include(x => x.InterestHolder)
                .Include(x => x.AlternateProject)
                .Include(x => x.PimsLeaseStakeholderCompReqs)
                    .ThenInclude(y => y.LeaseStakeholder)
                        .ThenInclude(z => z.LeaseStakeholderTypeCodeNavigation)
                .Include(x => x.PimsLeaseStakeholderCompReqs)
                    .ThenInclude(y => y.LeaseStakeholder)
                        .ThenInclude(z => z.LessorTypeCodeNavigation)
                .Include(x => x.PimsPropAcqFlCompReqs)
                    .ThenInclude(y => y.PropertyAcquisitionFile)
                        .ThenInclude(z => z.Property)
                .Include(x => x.PimsPropLeaseCompReqs)
                    .ThenInclude(y => y.PropertyLease)
                        .ThenInclude(z => z.Property)
                .AsNoTracking()
                .FirstOrDefault(x => x.CompensationRequisitionId.Equals(compensationRequisitionId)) ?? throw new KeyNotFoundException();

            return entity;
        }

        public PimsCompensationRequisition Update(PimsCompensationRequisition compensationRequisition)
        {
            var existingCompensationRequisition = Context.PimsCompensationRequisitions
                .FirstOrDefault(x => x.CompensationRequisitionId.Equals(compensationRequisition.CompensationRequisitionId)) ?? throw new KeyNotFoundException();

            // Don't let the frontend override the legacy payee - this is only intended to be populated via ETL
            compensationRequisition.LegacyPayee = existingCompensationRequisition.LegacyPayee;

            Context.Entry(existingCompensationRequisition).CurrentValues.SetValues(compensationRequisition);
            Context.UpdateChild<PimsCompensationRequisition, long, PimsCompReqFinancial, long>(a => a.PimsCompReqFinancials, compensationRequisition.CompensationRequisitionId, compensationRequisition.PimsCompReqFinancials.ToArray(), true);
            Context.UpdateChild<PimsCompensationRequisition, long, PimsPropAcqFlCompReq, long>(a => a.PimsPropAcqFlCompReqs, compensationRequisition.CompensationRequisitionId, compensationRequisition.PimsPropAcqFlCompReqs.ToArray(), true);
            Context.UpdateChild<PimsCompensationRequisition, long, PimsPropLeaseCompReq, long>(a => a.PimsPropLeaseCompReqs, compensationRequisition.CompensationRequisitionId, compensationRequisition.PimsPropLeaseCompReqs.ToArray(), true);
            Context.UpdateChild<PimsCompensationRequisition, long, PimsLeaseStakeholderCompReq, long>(a => a.PimsLeaseStakeholderCompReqs, compensationRequisition.CompensationRequisitionId, compensationRequisition.PimsLeaseStakeholderCompReqs.ToArray(), true);

            return compensationRequisition;
        }

        public bool TryDelete(long compensationId)
        {
            var deletedEntity = Context.PimsCompensationRequisitions
                .Include(fa => fa.PimsCompReqFinancials)
                .Include(p => p.PimsPropAcqFlCompReqs)
                .Include(l => l.PimsPropLeaseCompReqs)
                .Include(s => s.PimsLeaseStakeholderCompReqs)
                .AsNoTracking()
                .FirstOrDefault(c => c.CompensationRequisitionId == compensationId);

            if (deletedEntity != null)
            {
                foreach (var financial in deletedEntity.PimsCompReqFinancials)
                {
                    Context.PimsCompReqFinancials.Remove(new PimsCompReqFinancial() { CompReqFinancialId = financial.CompReqFinancialId });
                }

                foreach(var propAcqFile in deletedEntity.PimsPropAcqFlCompReqs)
                {
                    Context.PimsPropAcqFlCompReqs.Remove(new PimsPropAcqFlCompReq() { PropAcqFlCompReqId = propAcqFile.PropAcqFlCompReqId });
                }

                foreach (var propLeaseFile in deletedEntity.PimsPropLeaseCompReqs)
                {
                    Context.PimsPropLeaseCompReqs.Remove(new PimsPropLeaseCompReq() { PropLeaseCompReqId = propLeaseFile.PropLeaseCompReqId });
                }

                foreach (var compReqLeaseStakeholder in deletedEntity.PimsLeaseStakeholderCompReqs)
                {
                    Context.PimsLeaseStakeholderCompReqs.Remove(new PimsLeaseStakeholderCompReq() { LeaseStakeholderCompReqId = compReqLeaseStakeholder.LeaseStakeholderCompReqId });
                }

                Context.CommitTransaction(); // TODO: required to enforce delete order. Can be removed when cascade deletes are implemented.

                Context.PimsCompensationRequisitions.Remove(new PimsCompensationRequisition() { CompensationRequisitionId = deletedEntity.CompensationRequisitionId });
                return true;
            }
            return false;
        }

        public List<PimsPropertyAcquisitionFile> GetAcquisitionCompReqPropertiesById(long compensationRequisitionId)
        {
            return Context.PimsPropAcqFlCompReqs
                .Where(x => x.CompensationRequisitionId == compensationRequisitionId)
                    .Include(pa => pa.PropertyAcquisitionFile)
                    .ThenInclude(p => p.Property)
                        .ThenInclude(rp => rp.RegionCodeNavigation)
                .Include(pa => pa.PropertyAcquisitionFile)
                    .ThenInclude(p => p.Property)
                        .ThenInclude(rp => rp.DistrictCodeNavigation)
                .Include(pa => pa.PropertyAcquisitionFile)
                    .ThenInclude(p => p.Property)
                        .ThenInclude(rp => rp.Address)
                            .ThenInclude(a => a.Country)
                .Include(pa => pa.PropertyAcquisitionFile)
                    .ThenInclude(p => p.Property)
                        .ThenInclude(rp => rp.Address)
                            .ThenInclude(a => a.Country)
                .AsNoTracking()
                .Select(x => x.PropertyAcquisitionFile)
                .ToList();
        }

        public List<PimsPropertyLease> GetLeaseCompReqPropertiesById(long compensationRequisitionId)
        {
            return Context.PimsPropLeaseCompReqs
                .Where(x => x.CompensationRequisitionId == compensationRequisitionId)
                    .Include(l => l.PropertyLease)
                    .ThenInclude(p => p.Property)
                        .ThenInclude(rp => rp.RegionCodeNavigation)
                .Include(pa => pa.PropertyLease)
                    .ThenInclude(p => p.Property)
                        .ThenInclude(rp => rp.DistrictCodeNavigation)
                .Include(pa => pa.PropertyLease)
                    .ThenInclude(p => p.Property)
                        .ThenInclude(rp => rp.Address)
                            .ThenInclude(a => a.Country)
                .Include(pa => pa.PropertyLease)
                    .ThenInclude(p => p.Property)
                        .ThenInclude(rp => rp.Address)
                            .ThenInclude(a => a.Country)
                .AsNoTracking()
                .Select(x => x.PropertyLease)
                .ToList();
        }

        public IEnumerable<PimsCompReqFinancial> GetCompensationRequisitionFinancials(long id)
        {
            return Context.PimsCompReqFinancials
                .AsNoTracking()
                .Include(y => y.FinancialActivityCode)
                .Where(x => x.CompensationRequisitionId == id)
                .ToList();
        }
    }
}
