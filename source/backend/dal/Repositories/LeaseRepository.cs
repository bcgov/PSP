using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// LeaseRepository class, provides a service layer to interact with leases within the datasource.
    /// </summary>
    public class LeaseRepository : BaseRepository<PimsLease>, ILeaseRepository
    {
        private readonly ISequenceRepository _sequenceRepository;
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public LeaseRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LeaseRepository> logger, ISequenceRepository sequenceRepository)
            : base(dbContext, user, logger)
        {
            _sequenceRepository = sequenceRepository;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Returns the total number of leases in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Context.PimsLeases.Count();
        }

        /// <summary>
        /// Get an array of leases within the specified filters.
        /// Note that the 'leaseFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<PimsLease> GetAllByFilter(LeaseFilter filter, HashSet<short> regionCodes, bool loadPayments = false)
        {
            this.User.ThrowIfNotAuthorized(Permissions.LeaseView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            var query = GenerateLeaseQuery(filter, regionCodes, loadPayments);

            // Getting all by the filter will ignore the order by passed and instead use the lease id.
            var leases = query.OrderBy(l => l.LeaseId).ToArray();

            return leases;
        }

        public long GetRowVersion(long id)
        {
            User.ThrowIfNotAuthorized(Permissions.LeaseView);

            return Context.PimsLeases.Where(l => l.LeaseId == id)?.Select(l => l.ConcurrencyControlNumber)?.FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public PimsLease Get(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.LeaseView);

            PimsLease lease = this.Context.PimsLeases.AsSplitQuery().AsNoTracking()
                .Include(l => l.PimsPropertyLeases)
                .Include(l => l.RegionCodeNavigation)
                .Include(l => l.LeaseProgramTypeCodeNavigation)
                .Include(l => l.LeasePayRvblTypeCodeNavigation)
                .Include(l => l.LeaseLicenseTypeCodeNavigation)
                .Include(l => l.LeaseResponsibilityTypeCodeNavigation)
                .Include(l => l.LeaseInitiatorTypeCodeNavigation)
                .Include(l => l.PimsLeaseLeasePurposes)
                    .ThenInclude(p => p.LeasePurposeTypeCodeNavigation)
                .Include(l => l.LeaseStatusTypeCodeNavigation)
                .Include(l => l.PimsLeaseStakeholders)
                .Include(t => t.PimsPropertyImprovements)
                .Include(l => l.PimsInsurances)
                .Include(l => l.PimsSecurityDeposits)
                .Include(l => l.PimsLeasePeriods)
                .Include(l => l.Project)
                    .ThenInclude(x => x.WorkActivityCode)
                .Include(r => r.Project)
                    .ThenInclude(x => x.CostTypeCode)
                .Include(r => r.Project)
                    .ThenInclude(x => x.BusinessFunctionCode)
                .Include(r => r.Product)
                .FirstOrDefault(l => l.LeaseId == id) ?? throw new KeyNotFoundException();

            lease.PimsPropertyImprovements = lease.PimsPropertyImprovements.OrderBy(i => i.PropertyImprovementTypeCode).ToArray();
            lease.PimsLeasePeriods = lease.PimsLeasePeriods.OrderBy(t => t.PeriodStartDate).ThenBy(t => t.LeasePeriodId).Select(t =>
            {
                t.PimsLeasePayments = t.PimsLeasePayments.OrderBy(p => p.PaymentReceivedDate).ThenBy(p => p.LeasePaymentId).ToArray();
                return t;
            }).ToArray();
            return lease;
        }

        public IEnumerable<PimsLease> GetAllByIds(IEnumerable<long> leaseIds)
        {
            this.User.ThrowIfNotAuthorized(Permissions.LeaseView);

            IEnumerable<PimsLease> leases = this.Context.PimsLeases.AsSplitQuery().AsNoTracking()
                .Include(l => l.PimsPropertyLeases)
                .Include(l => l.RegionCodeNavigation)
                .Include(l => l.LeaseProgramTypeCodeNavigation)
                .Include(l => l.LeasePayRvblTypeCodeNavigation)
                .Include(l => l.LeaseLicenseTypeCodeNavigation)
                .Include(l => l.LeaseResponsibilityTypeCodeNavigation)
                .Include(l => l.LeaseInitiatorTypeCodeNavigation)
                .Include(l => l.PimsLeaseLeasePurposes)
                    .ThenInclude(p => p.LeasePurposeTypeCodeNavigation)
                .Include(l => l.LeaseStatusTypeCodeNavigation)
                .Include(l => l.PimsLeaseStakeholders)
                .Include(t => t.PimsPropertyImprovements)
                .Include(l => l.PimsInsurances)
                .Include(l => l.PimsSecurityDeposits)
                .Include(l => l.PimsLeasePeriods)
                    .ThenInclude(l => l.PimsLeasePayments)
                .Include(l => l.Project)
                    .ThenInclude(x => x.WorkActivityCode)
                .Include(r => r.Project)
                    .ThenInclude(x => x.CostTypeCode)
                .Include(r => r.Project)
                    .ThenInclude(x => x.BusinessFunctionCode)
                .Include(r => r.Product)
                .Where(l => leaseIds.Any(leaseId => leaseId == l.LeaseId)) ?? throw new KeyNotFoundException();
            return leases;
        }

        /// <summary>
        /// Retrieves the lease with the specified id last update information.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <returns></returns>
        public LastUpdatedByModel GetLastUpdateBy(long leaseId)
        {
            // Lease details
            var lastUpdatedByAggregate = new List<LastUpdatedByModel>();
            var leaseDetailsLastUpdatedBy = this.Context.PimsLeases.AsNoTracking()
                .Where(a => a.LeaseId == leaseId)
                .Select(a => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = a.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = a.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = a.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(leaseDetailsLastUpdatedBy);

            // Lease Properties
            var propertiesLastUpdatedBy = this.Context.PimsPropertyLeases.AsNoTracking()
                .Where(ap => ap.LeaseId == leaseId)
                .Select(ap => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = ap.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = ap.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = ap.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(propertiesLastUpdatedBy);

            // Lease Deleted Properties
            // This is needed to get the properties last-updated-by when deleted
            var deletedProperties = this.Context.PimsPropertyLeaseHists.AsNoTracking()
            .Where(aph => aph.LeaseId == leaseId)
            .GroupBy(aph => aph.PropertyLeaseId)
            .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var propertiesHistoryLastUpdatedBy = deletedProperties
            .Select(aph => new LastUpdatedByModel()
            {
                ParentId = leaseId,
                AppLastUpdateUserid = aph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = aph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = aph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(propertiesHistoryLastUpdatedBy);

            // Lease Consultations
            var consultationsLastUpdatedBy = this.Context.PimsLeaseConsultations.AsNoTracking()
                .Where(ap => ap.LeaseId == leaseId)
                .Select(ap => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = ap.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = ap.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = ap.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(consultationsLastUpdatedBy);

            // Lease Deleted Consultations
            // This is needed to get the consultations last-updated-by when deleted
            var deletedConsultations = this.Context.PimsLeaseConsultationHists.AsNoTracking()
            .Where(aph => aph.LeaseId == leaseId)
            .GroupBy(aph => aph.LeaseConsultationId)
            .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var consultationHistoryLastUpdatedBy = deletedConsultations
            .Select(aph => new LastUpdatedByModel()
            {
                ParentId = leaseId,
                AppLastUpdateUserid = aph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = aph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = aph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(consultationHistoryLastUpdatedBy);

            // Lease Stakeholders
            var leaseStakeholdersLastUpdatedBy = this.Context.PimsLeaseStakeholders.AsNoTracking()
                .Where(ap => ap.LeaseId == leaseId)
                .Select(ap => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = ap.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = ap.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = ap.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(leaseStakeholdersLastUpdatedBy);

            // Lease Deleted stakeholders
            // This is needed to get the stakeholders last-updated-by when deleted
            var deletedLeaseStakeholders = this.Context.PimsLeaseStakeholderHists.AsNoTracking()
                .Where(aph => aph.LeaseId == leaseId)
                .GroupBy(aph => aph.LeaseStakeholderId)
                .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var leaseStakeholdersHistoryLastUpdatedBy = deletedLeaseStakeholders
            .Select(aph => new LastUpdatedByModel()
            {
                ParentId = leaseId,
                AppLastUpdateUserid = aph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = aph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = aph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(leaseStakeholdersHistoryLastUpdatedBy);

            // Lease Improvements
            var improvementLastUpdatedBy = this.Context.PimsPropertyImprovements.AsNoTracking()
                .Where(ap => ap.LeaseId == leaseId)
                .Select(ap => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = ap.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = ap.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = ap.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(improvementLastUpdatedBy);

            // Lease Deleted Improvements
            // This is needed to get the property improvements last-updated-by when deleted
            var lastImprovementHistory = this.Context.PimsPropertyImprovementHists.AsNoTracking()
                .Where(aph => aph.LeaseId == leaseId)
                .GroupBy(aph => aph.PropertyImprovementId)
                .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var improvementsHistoryLastUpdatedBy = lastImprovementHistory
            .Select(aph => new LastUpdatedByModel()
            {
                ParentId = leaseId,
                AppLastUpdateUserid = aph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = aph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = aph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(improvementsHistoryLastUpdatedBy);

            // Lease Insurance
            var insuranceLastUpdatedBy = this.Context.PimsInsurances.AsNoTracking()
                .Where(ap => ap.LeaseId == leaseId)
                .Select(ap => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = ap.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = ap.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = ap.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(insuranceLastUpdatedBy);

            // Lease Deleted Insurance
            // This is needed to get the property insurance last-updated-by when deleted
            var lastInsuranceHistory = this.Context.PimsInsuranceHists.AsNoTracking()
                .Where(aph => aph.LeaseId == leaseId)
                .GroupBy(aph => aph.InsuranceId)
                .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var insuranceHistoryLastUpdatedBy = lastInsuranceHistory
            .Select(aph => new LastUpdatedByModel()
            {
                ParentId = leaseId,
                AppLastUpdateUserid = aph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = aph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = aph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(insuranceHistoryLastUpdatedBy);

            // Lease Deposits
            var depositsLastUpdatedBy = this.Context.PimsSecurityDeposits.AsNoTracking()
                .Where(ap => ap.LeaseId == leaseId)
                .Select(ap => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = ap.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = ap.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = ap.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(depositsLastUpdatedBy);

            // Lease Deleted Deposits
            // This is needed to get the lease deposits last-updated-by when deleted
            var lastSDHistory = this.Context.PimsSecurityDepositHists.AsNoTracking()
                .Where(aph => aph.LeaseId == leaseId)
                .GroupBy(aph => aph.SecurityDepositId)
                .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var depositsHistoryLastUpdatedBy = lastSDHistory
            .Select(aph => new LastUpdatedByModel()
            {
                ParentId = leaseId,
                AppLastUpdateUserid = aph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = aph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = aph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(depositsHistoryLastUpdatedBy);

            // Lease Return Deposits
            var depositReturnsLastUpdatedBy = this.Context.PimsSecurityDepositReturns.AsNoTracking()
                .Include(l => l.SecurityDeposit)
                .Where(ap => ap.SecurityDeposit.LeaseId == leaseId)
                .Select(ap => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = ap.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = ap.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = ap.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(depositReturnsLastUpdatedBy);

            // Lease Deleted Return Deposits
            // This is needed to get the returned deposits last-updated-by when deleted
            var returnsHistory = this.Context.PimsSecurityDepositReturnHists.AsNoTracking();
            var returnsDepositHistory = this.Context.PimsSecurityDepositHists.AsNoTracking()
                .Where(at => at.LeaseId == leaseId)
                .Join(
                    returnsHistory,
                    securityDepositHist => securityDepositHist.SecurityDepositId,
                    securityDepositReturnHolderHist => securityDepositReturnHolderHist.SecurityDepositId,
                    (securityDepositHist, securityDepositReturnHist) => securityDepositReturnHist)
                .GroupBy(aph => aph.SecurityDepositId)
                .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var returnsHistoryLastUpdatedBy = returnsDepositHistory
            .Select(rdh => new LastUpdatedByModel()
            {
                ParentId = leaseId,
                AppLastUpdateUserid = rdh.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = rdh.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = rdh.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(returnsHistoryLastUpdatedBy);

            // Lease Deposit Holder
            var depositHolderReturnsLastUpdatedBy = this.Context.PimsSecurityDepositHolders.AsNoTracking()
                .Include(l => l.SecurityDeposit)
                .Where(ap => ap.SecurityDeposit.LeaseId == leaseId)
                .Select(ap => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = ap.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = ap.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = ap.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(depositHolderReturnsLastUpdatedBy);

            // Lease Deleted Deposit Holder
            // This is needed to get the returned deposits last-updated-by when deleted
            var returnHoldersHistory = this.Context.PimsSecurityDepositHolderHists.AsNoTracking();
            var securityDepositHolderHist = this.Context.PimsSecurityDepositHists.AsNoTracking()
                .Where(at => at.LeaseId == leaseId)
                .Join(
                    returnHoldersHistory,
                    securityDepositHist => securityDepositHist.SecurityDepositId,
                    securityDepositHolderHist => securityDepositHolderHist.SecurityDepositId,
                    (securityDepositHist, securityDepositHolderHist) => securityDepositHolderHist)
                .GroupBy(aph => aph.SecurityDepositHolderId)
                .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var returnHoldersHistoryLastUpdatedBy = securityDepositHolderHist
            .Select(sdh => new LastUpdatedByModel()
            {
                ParentId = leaseId,
                AppLastUpdateUserid = sdh.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = sdh.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = sdh.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(returnHoldersHistoryLastUpdatedBy);

            // Lease Periods
            var periodLastUpdatedBy = this.Context.PimsLeasePeriods.AsNoTracking()
                .Where(ap => ap.LeaseId == leaseId)
                .Select(ap => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = ap.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = ap.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = ap.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(periodLastUpdatedBy);

            // Lease Deleted Periods
            // This is needed to get the lease periods last-updated-by when deleted
            var deletedTerms = this.Context.PimsLeasePeriodHists.AsNoTracking()
                .Where(aph => aph.LeaseId == leaseId)
                .GroupBy(aph => aph.LeasePeriodId)
                .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var periodHistoryLastUpdatedBy = deletedTerms
            .Select(aph => new LastUpdatedByModel()
            {
                ParentId = leaseId,
                AppLastUpdateUserid = aph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = aph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = aph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(periodHistoryLastUpdatedBy);

            // Lease Period Payment
            var periodPaymentReturnsLastUpdatedBy = this.Context.PimsLeasePayments.AsNoTracking()
                .Include(l => l.LeasePeriod)
                .Where(ap => ap.LeasePeriod.LeaseId == leaseId)
                .Select(ap => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = ap.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = ap.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = ap.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(periodPaymentReturnsLastUpdatedBy);

            // Lease Deleted Period Payments
            // This is needed to get the period payments last-updated-by when deleted
            var periodPaymentHistory = this.Context.PimsLeasePaymentHists.AsNoTracking();
            var deletedPeriodPaymentHistory = this.Context.PimsLeasePeriodHists.AsNoTracking()
                .Where(at => at.LeaseId == leaseId)
                .Join(
                    periodPaymentHistory,
                    leasePeriodHist => leasePeriodHist.LeasePeriodId,
                    leasePaymentHist => leasePaymentHist.LeasePeriodId,
                    (leasePeriodHist, leasePaymentHist) => leasePaymentHist)
                .GroupBy(aph => aph.LeasePaymentId)
                .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var periodPaymentHistoryLastUpdatedBy = deletedPeriodPaymentHistory
            .Select(rdh => new LastUpdatedByModel()
            {
                ParentId = leaseId,
                AppLastUpdateUserid = rdh.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = rdh.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = rdh.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(periodPaymentHistoryLastUpdatedBy);

            // Lease Documents
            var documentsUpdatedBy = this.Context.PimsLeaseDocuments.AsNoTracking()
                .Where(ap => ap.LeaseId == leaseId)
                .Select(ap => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = ap.Document.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = ap.Document.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = ap.Document.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(documentsUpdatedBy);

            // Lease Deleted Documents
            // This is needed to get the lease documents last-updated-by when deleted
            var deletedDocuments = this.Context.PimsLeaseDocumentHists.AsNoTracking()
                .Where(aph => aph.LeaseId == leaseId)
                .GroupBy(aph => aph.LeaseDocumentId)
                .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var documentsHistoryLastUpdatedBy = deletedDocuments
            .Select(aph => new LastUpdatedByModel()
            {
                ParentId = leaseId,
                AppLastUpdateUserid = aph.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                AppLastUpdateUserGuid = aph.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                AppLastUpdateTimestamp = aph.EndDateHist ?? DateTime.UnixEpoch,
            })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(documentsHistoryLastUpdatedBy);

            // Lease Notes
            var notesUpdatedBy = this.Context.PimsLeaseNotes.AsNoTracking()
                .Where(ap => ap.LeaseId == leaseId)
                .Include(a => a.Note)
                .Select(ap => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = ap.Note.AppLastUpdateUserid,
                    AppLastUpdateUserGuid = ap.Note.AppLastUpdateUserGuid,
                    AppLastUpdateTimestamp = ap.Note.AppLastUpdateTimestamp,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(notesUpdatedBy);

            // Lease Deleted Notes
            // This is needed to get the notes last-updated-by from the document that where deleted
            var deletedNotes = this.Context.PimsLeaseNoteHists.AsNoTracking()
                .Where(aph => aph.LeaseId == leaseId)
                .GroupBy(aph => aph.LeaseNoteId)
                .Select(gaph => gaph.OrderByDescending(a => a.EffectiveDateHist).FirstOrDefault()).ToList();

            var notesHistoryLastUpdatedBy = deletedNotes
                .Select(anh => new LastUpdatedByModel()
                {
                    ParentId = leaseId,
                    AppLastUpdateUserid = anh.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                    AppLastUpdateUserGuid = anh.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                    AppLastUpdateTimestamp = anh.EndDateHist ?? DateTime.UnixEpoch,
                })
                .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
                .Take(1)
                .ToList();
            lastUpdatedByAggregate.AddRange(notesHistoryLastUpdatedBy);

            return lastUpdatedByAggregate.OrderByDescending(x => x.AppLastUpdateTimestamp).FirstOrDefault();
        }

        // TODO: original Get method should have AsNoTracking() but that breaks a number of existing workflows. Added this as a temporary fix until lease logic is refactored.
        public PimsLease GetNoTracking(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.LeaseView);

            PimsLease lease = this.Context.PimsLeases.AsSplitQuery()
                .AsNoTracking()
                .Include(l => l.PimsPropertyLeases)
                .ThenInclude(p => p.Property)
                    .ThenInclude(p => p.Address)
                    .ThenInclude(p => p.Country)
                .Include(l => l.PimsPropertyLeases)
                    .ThenInclude(p => p.Property)
                    .ThenInclude(p => p.Address)
                    .ThenInclude(p => p.ProvinceState)
                .Include(l => l.PimsPropertyLeases)
                    .ThenInclude(p => p.AreaUnitTypeCodeNavigation)
                .Include(l => l.PimsPropertyLeases)
                    .ThenInclude(p => p.Property)
                    .ThenInclude(s => s.SurplusDeclarationTypeCodeNavigation)
                .Include(l => l.PimsPropertyLeases)
                    .ThenInclude(p => p.Property)
                    .ThenInclude(p => p.PropertyAreaUnitTypeCodeNavigation)
                .Include(l => l.RegionCodeNavigation)

                .Include(l => l.Project)
                .Include(l => l.LeaseProgramTypeCodeNavigation)
                .Include(l => l.LeasePayRvblTypeCodeNavigation)
                .Include(l => l.LeaseLicenseTypeCodeNavigation)
                .Include(l => l.LeaseResponsibilityTypeCodeNavigation)
                .Include(l => l.LeaseInitiatorTypeCodeNavigation)
                .Include(l => l.PimsLeaseLeasePurposes)
                    .ThenInclude(p => p.LeasePurposeTypeCodeNavigation)
                .Include(l => l.LeaseStatusTypeCodeNavigation)

                .Include(l => l.PimsLeaseStakeholders)
                    .ThenInclude(l => l.LeaseStakeholderTypeCodeNavigation)
                .Include(l => l.PimsLeaseStakeholders)
                    .ThenInclude(t => t.Person)
                    .ThenInclude(o => o.PimsPersonOrganizations)
                    .ThenInclude(o => o.Organization)
                .Include(l => l.PimsLeaseStakeholders)
                    .ThenInclude(t => t.Person)
                    .ThenInclude(o => o.PimsPersonAddresses)
                    .ThenInclude(o => o.Address)
                .Include(l => l.PimsLeaseStakeholders)
                    .ThenInclude(t => t.Person)
                    .ThenInclude(o => o.PimsPersonAddresses)
                    .ThenInclude(o => o.AddressUsageTypeCodeNavigation)
                .Include(l => l.PimsLeaseStakeholders)
                    .ThenInclude(t => t.Person)
                    .ThenInclude(o => o.PimsContactMethods)
                    .ThenInclude(cm => cm.ContactMethodTypeCodeNavigation)

                .Include(l => l.PimsLeaseStakeholders)
                    .ThenInclude(t => t.Organization)
                    .ThenInclude(o => o.PimsPersonOrganizations)
                    .ThenInclude(o => o.Person)
                .Include(l => l.PimsLeaseStakeholders)
                    .ThenInclude(t => t.Organization)
                    .ThenInclude(o => o.PimsOrganizationAddresses)
                    .ThenInclude(o => o.AddressUsageTypeCodeNavigation)
                .Include(l => l.PimsLeaseStakeholders)
                    .ThenInclude(t => t.Organization)
                    .ThenInclude(o => o.PimsOrganizationAddresses)
                    .ThenInclude(o => o.Address)
                .Include(l => l.PimsLeaseStakeholders)
                    .ThenInclude(t => t.Organization)
                    .ThenInclude(o => o.PimsContactMethods)
                    .ThenInclude(cm => cm.ContactMethodTypeCodeNavigation)

                .Include(l => l.PimsLeaseStakeholders)
                    .ThenInclude(t => t.PrimaryContact)

                .Include(l => l.PimsLeaseStakeholders)
                    .ThenInclude(t => t.LessorTypeCodeNavigation)

                .Include(t => t.PimsPropertyImprovements)

                .Include(l => l.PimsInsurances)
                    .ThenInclude(i => i.InsuranceTypeCodeNavigation)

                .Include(l => l.PimsSecurityDeposits)
                    .ThenInclude(s => s.SecurityDepositTypeCodeNavigation)
                .Include(l => l.PimsSecurityDeposits)
                    .ThenInclude(s => s.PimsSecurityDepositHolder)
                    .ThenInclude(h => h.Person)
                .Include(l => l.PimsSecurityDeposits)
                    .ThenInclude(s => s.PimsSecurityDepositHolder)
                    .ThenInclude(h => h.Organization)
                .Include(l => l.PimsSecurityDeposits)
                    .ThenInclude(s => s.PimsSecurityDepositReturns)
                    .ThenInclude(r => r.PimsSecurityDepositReturnHolder)
                    .ThenInclude(h => h.Person)
                .Include(l => l.PimsSecurityDeposits)
                    .ThenInclude(s => s.PimsSecurityDepositReturns)
                    .ThenInclude(r => r.PimsSecurityDepositReturnHolder)
                    .ThenInclude(s => s.Organization)

                .Include(l => l.PimsLeasePeriods)
                     .ThenInclude(t => t.LeasePmtFreqTypeCodeNavigation)
                .Include(t => t.PimsLeasePeriods)
                     .ThenInclude(t => t.LeasePeriodStatusTypeCodeNavigation)
                .Include(t => t.PimsLeasePeriods)
                    .ThenInclude(t => t.PimsLeasePayments)
                    .ThenInclude(t => t.LeasePaymentMethodTypeCodeNavigation)
                .Include(t => t.PimsLeasePeriods)
                    .ThenInclude(t => t.PimsLeasePayments)
                    .ThenInclude(t => t.LeasePaymentStatusTypeCodeNavigation)

                .Include(t => t.PimsLeaseConsultations)
                    .ThenInclude(t => t.ConsultationTypeCodeNavigation)
                .Include(t => t.PimsLeaseConsultations)
                    .ThenInclude(t => t.ConsultationStatusTypeCodeNavigation)
                .FirstOrDefault(l => l.LeaseId == id) ?? throw new KeyNotFoundException();

            lease.PimsPropertyImprovements = lease.PimsPropertyImprovements.OrderBy(i => i.PropertyImprovementTypeCode).ToArray();
            lease.PimsLeasePeriods = lease.PimsLeasePeriods.OrderBy(t => t.PeriodStartDate).ThenBy(t => t.LeasePeriodId).Select(t =>
            {
                t.PimsLeasePayments = t.PimsLeasePayments.OrderBy(p => p.PaymentReceivedDate).ThenBy(p => p.LeasePaymentId).ToArray();
                return t;
            }).ToArray();

            return lease;
        }

        /// <summary>
        /// Get a page with an array of leases within the specified filters.
        /// Note that the 'leaseFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<PimsLease> GetPage(LeaseFilter filter, HashSet<short> regions)
        {
            this.User.ThrowIfNotAuthorized(Permissions.LeaseView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            var skip = (filter.Page - 1) * filter.Quantity;
            var query = GenerateLeaseQuery(filter, regions);
            var items = query
                .Skip(skip)
                .Take(filter.Quantity)
                .ToArray();

            return new Paged<PimsLease>(items, filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Get All Documents by Lease Id.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <returns></returns>
        public IList<PimsLeaseDocument> GetAllLeaseDocuments(long leaseId)
        {
            return Context.PimsLeaseDocuments
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentStatusTypeCodeNavigation)
                .Include(ad => ad.Document)
                    .ThenInclude(d => d.DocumentType)
                .Where(x => x.LeaseId == leaseId)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Add the passed lease to the database assuming the user has the require claims.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public PimsLease Add(PimsLease lease)
        {
            if (lease == null)
            {
                throw new ArgumentNullException(nameof(lease), "lease cannot be null.");
            }

            User.ThrowIfNotAuthorized(Permissions.LeaseAdd);

            if (lease.PimsPropertyLeases.Any(x => x.Property != null && x.Property.IsRetired.HasValue && x.Property.IsRetired.Value))
            {
                throw new BusinessRuleViolationException("Retired property can not be selected.");
            }

            lease = GenerateLFileNo(lease);

            Context.PimsLeases.Add(lease);
            Context.CommitTransaction();
            return Get(lease.LeaseId);
        }

        /// <summary>
        /// Add new LeaseDocument.
        /// </summary>
        /// <param name="leaseDocument"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Check for EntityState.</exception>
        public PimsLeaseDocument AddLeaseDocument(PimsLeaseDocument leaseDocument)
        {
            leaseDocument.ThrowIfNull(nameof(leaseDocument));

            var newEntry = Context.PimsLeaseDocuments.Add(leaseDocument);
            if (newEntry.State == EntityState.Added)
            {
                return newEntry.Entity;
            }
            else
            {
                throw new InvalidOperationException("Could not create document");
            }
        }

        /// <summary>
        /// Deletes the Laease Document by DocumentId.
        /// </summary>
        /// <param name="leaseDocumentId"></param>
        public void DeleteLeaseDocument(long leaseDocumentId)
        {
            var entity = Context.PimsLeaseDocuments.FirstOrDefault(d => d.LeaseDocumentId == leaseDocumentId);
            if (entity is not null)
            {
                Context.PimsLeaseDocuments.Remove(entity);
            }

            return;
        }

        /// <summary>
        /// Update the passed lease in the database assuming the user has the require claims.
        /// </summary>
        /// <param name="lease"></param>
        /// <returns></returns>
        public PimsLease Update(PimsLease lease, bool commitTransaction = true)
        {
            if (lease == null)
            {
                throw new ArgumentNullException(nameof(lease), "lease cannot be null.");
            }

            this.User.ThrowIfNotAuthorized(Permissions.LeaseEdit);
            var existingLease = this.Context.PimsLeases.Where(l => l.LeaseId == lease.LeaseId).FirstOrDefault()
                 ?? throw new KeyNotFoundException();

            Context.Entry(existingLease).CurrentValues.SetValues(lease);
            Context.UpdateChild<PimsLease, long, PimsLeaseLeasePurpose, long>(p => p.PimsLeaseLeasePurposes, lease.LeaseId, lease.PimsLeaseLeasePurposes.ToArray());

            if (commitTransaction)
            {
                this.Context.CommitTransaction();
            }

            return existingLease;
        }

        /// <summary>
        /// Update the renewals of a lease.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <param name="rowVersion"></param>
        /// <param name="renewals"></param>
        /// <returns></returns>
        public PimsLease UpdateLeaseRenewals(long leaseId, long? rowVersion, ICollection<PimsLeaseRenewal> renewals)
        {
            var existingLease = this.Context.PimsLeases.Include(l => l.PimsLeaseRenewals).AsNoTracking().FirstOrDefault(l => l.LeaseId == leaseId)
                 ?? throw new KeyNotFoundException();
            if (existingLease.ConcurrencyControlNumber != rowVersion)
            {
                throw new DbUpdateConcurrencyException("Unable to save. Please refresh your page and try again");
            }

            this.Context.UpdateChild<PimsLease, long, PimsLeaseRenewal, long>(l => l.PimsLeaseRenewals, leaseId, renewals.ToArray());

            return GetNoTracking(existingLease.LeaseId);
        }

        /// <summary>
        /// Generate a query for the specified 'filter'.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="regionCodes"></param>
        /// <param name="loadPayments"></param>
        /// <returns></returns>
        public IQueryable<PimsLease> GenerateLeaseQuery(LeaseFilter filter, HashSet<short> regionCodes, bool loadPayments = false)
        {
            filter.ThrowIfNull(nameof(filter));

            var query = Context.PimsLeases
                        .Include(l => l.PimsPropertyLeases)
                            .ThenInclude(p => p.Property)
                            .ThenInclude(p => p.Address)
                        .Include(l => l.PimsPropertyLeases)
                            .ThenInclude(p => p.AreaUnitTypeCodeNavigation)
                        .Include(l => l.PimsPropertyLeases)
                            .ThenInclude(p => p.Property)
                            .ThenInclude(n => n.PimsHistoricalFileNumbers)
                            .ThenInclude(t => t.HistoricalFileNumberTypeCodeNavigation)
                        .Include(l => l.PimsPropertyImprovements)
                        .Include(l => l.LeaseProgramTypeCodeNavigation)
                        .Include(l => l.LeaseStatusTypeCodeNavigation)
                        .Include(l => l.LeaseLicenseTypeCodeNavigation)
                        .Include(l => l.PimsLeaseStakeholders)
                            .ThenInclude(t => t.Person)
                        .Include(l => l.PimsLeaseStakeholders)
                            .ThenInclude(t => t.Organization)
                        .Include(l => l.RegionCodeNavigation)
                        .Include(l => l.PimsLeasePeriods)
                        .Include(l => l.PimsLeaseRenewals)
                        .AsNoTracking();

            if (loadPayments)
            {
                query = query.Include(l => l.PimsLeasePeriods)
                    .ThenInclude(l => l.PimsLeasePayments);
            }

            var predicate = GenerateCommonLeaseQuery(filter, regionCodes);
            query = query.Where(predicate);

            if (filter.Sort?.Any() == true)
            {
                var sortList = filter.Sort.ToList();
                MapSortField("ExpiryDate", "OrigExpiryDate", sortList);
                MapSortField("FileStatusTypeCode", "LeaseStatusTypeCodeNavigation.Description", sortList);
                MapSortField("ProgramName", "LeaseProgramTypeCodeNavigation.Description", sortList);

                query = query.OrderByProperty(true, sortList.ToArray());
            }
            else
            {
                query = query.OrderBy(l => l.LFileNo);
            }

            return query;
        }

        /// <summary>
        /// Get the next available id from the PIMS_LEASE_ID_SEQ.
        /// </summary>
        public long GetNextLeaseSequenceValue()
        {
            return _sequenceRepository.GetNextSequenceValue("dbo.PIMS_LEASE_ID_SEQ");
        }

        /// <summary>
        /// Generate a new L File in format L-XXX-YYY using the lease id. Add the lease id and lfileno to the passed lease.
        /// </summary>
        /// <param name="lease">The lease file entity.</param>
        public PimsLease GenerateLFileNo(PimsLease lease)
        {
            long leaseId = GetNextLeaseSequenceValue();
            lease.LeaseId = leaseId;
            lease.LFileNo = $"L-{lease.LeaseId.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0').Insert(3, "-")}";
            return lease;
        }

        /// <summary>
        /// Get all Checklist Items for a Lease.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <returns></returns>
        public List<PimsLeaseChecklistItem> GetAllChecklistItemsByLeaseId(long leaseId)
        {
            using var scope = Logger.QueryScope();

            return Context.PimsLeaseChecklistItems
                .Where(ci => ci.LeaseId == leaseId)
                .Include(ci => ci.ChklstItemStatusTypeCodeNavigation)
                .Include(ci => ci.LeaseChklstItemTypeCodeNavigation)
                    .ThenInclude(it => it.LeaseChklstSectionTypeCodeNavigation)
                .AsNoTracking()
                .ToList();
        }

        /// <summary>
        /// Get Checklist Item Types for Lease.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PimsLeaseChklstItemType> GetAllChecklistItemTypes()
        {
            return Context.PimsLeaseChklstItemTypes
                    .Include(it => it.LeaseChklstSectionTypeCodeNavigation)
                    .OrderBy(it => it.DisplayOrder)
                    .AsNoTracking()
                    .ToList();
        }

        /// <summary>
        /// Add a new checklist item to the lease.
        /// </summary>
        /// <param name="checklistItem"></param>
        /// <returns>New Checklist Item.</returns>
        public PimsLeaseChecklistItem AddChecklistItem(PimsLeaseChecklistItem checklistItem)
        {
            using var scope = Logger.QueryScope();
            Context.PimsLeaseChecklistItems.Add(checklistItem);

            return checklistItem;
        }

        /// <summary>
        /// Uppdate a lease checklist item.
        /// </summary>
        /// <param name="checklistItem"></param>
        /// <returns>The updated checklist item.</returns>
        public PimsLeaseChecklistItem UpdateChecklistItem(PimsLeaseChecklistItem checklistItem)
        {
            checklistItem.ThrowIfNull(nameof(checklistItem));

            Context.Entry(checklistItem).CurrentValues.SetValues(checklistItem);
            Context.Entry(checklistItem).State = EntityState.Modified;

            return checklistItem;
        }

        /// <summary>
        /// Gets all stakeholder types
        /// </summary>
        /// <returns>all stakeholder types.</returns>
        public IEnumerable<PimsLeaseStakeholderType> GetAllLeaseStakeholderTypes()
        {
            return Context.PimsLeaseStakeholderTypes
                    .OrderBy(it => it.DisplayOrder)
                    .AsNoTracking()
                    .ToList();
        }

        /// <summary>
        /// Generate an SQL statement for the specified 'user' and 'filter'.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private static ExpressionStarter<PimsLease> GenerateCommonLeaseQuery(LeaseFilter filter, HashSet<short> regions)
        {
            filter.ThrowIfNull(nameof(filter));

            var predicateBuilder = PredicateBuilder.New<PimsLease>(l => true);

            predicateBuilder = predicateBuilder.And(l => !l.RegionCode.HasValue || regions.Contains(l.RegionCode.Value));

            if (!string.IsNullOrWhiteSpace(filter.TenantName))
            {
                predicateBuilder = predicateBuilder.And(l => l.PimsLeaseStakeholders.Any(stakeholder => stakeholder.Person != null && EF.Functions.Like(
                    ((!string.IsNullOrWhiteSpace(stakeholder.Person.FirstName) ? stakeholder.Person.FirstName + " " : string.Empty) +
                    (!string.IsNullOrWhiteSpace(stakeholder.Person.MiddleNames) ? stakeholder.Person.MiddleNames + " " : string.Empty) +
                    (stakeholder.Person.Surname ?? string.Empty)).Trim(), $"%{filter.TenantName}%"))
                 || l.PimsLeaseStakeholders.Any(stakeholder => stakeholder.Organization != null && EF.Functions.Like(stakeholder.Organization.OrganizationName, $"%{filter.TenantName}%")));
            }

            if (!string.IsNullOrWhiteSpace(filter.PinOrPid))
            {
                var pinOrPidValue = filter.PinOrPid.Replace("-", string.Empty).Trim().TrimStart('0');
                predicateBuilder = predicateBuilder.And(l => l.PimsPropertyLeases.Any(pl => pl != null && (EF.Functions.Like(pl.Property.Pid.ToString(), $"%{pinOrPidValue}%") || EF.Functions.Like(pl.Property.Pin.ToString(), $"%{pinOrPidValue}%"))));
            }

            if (!string.IsNullOrWhiteSpace(filter.LFileNo))
            {
                predicateBuilder = predicateBuilder.And(l => EF.Functions.Like(l.LFileNo, $"%{filter.LFileNo}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.Historical))
            {
                predicateBuilder = predicateBuilder.And(l => EF.Functions.Like(l.PsFileNo, $"%{filter.Historical}%") || EF.Functions.Like(l.TfaFileNumber, $"%{filter.Historical}%"));

                predicateBuilder = predicateBuilder.Or(l => l.PimsPropertyLeases.Any(x => x.Property.PimsHistoricalFileNumbers.Any(y => y.HistoricalFileNumber.Contains(filter.Historical))));
            }

            if (!string.IsNullOrWhiteSpace(filter.Address))
            {
                predicateBuilder = predicateBuilder.And(l => l.PimsPropertyLeases.Any(pl => pl != null &&
                    (EF.Functions.Like(pl.Property.Address.StreetAddress1, $"%{filter.Address}%") ||
                    EF.Functions.Like(pl.Property.Address.StreetAddress2, $"%{filter.Address}%") ||
                    EF.Functions.Like(pl.Property.Address.StreetAddress3, $"%{filter.Address}%") ||
                    EF.Functions.Like(pl.Property.Address.MunicipalityName, $"%{filter.Address}%"))));
            }

            if (filter.IsReceivable == true)
            {
                predicateBuilder = predicateBuilder.And(l => l.LeasePayRvblTypeCode == "RCVBL");
            }

            if (filter.NotInStatus.Count > 0)
            {
                predicateBuilder = predicateBuilder.And(l => !filter.NotInStatus.Contains(l.LeaseStatusTypeCode));
            }

            if (filter.ExpiryAfterDate.HasValue)
            {
                predicateBuilder = predicateBuilder.And(l => (l.OrigExpiryDate >= filter.ExpiryAfterDate.Value || l.OrigExpiryDate == null)
                    || l.PimsLeasePeriods.Any(t => t.PeriodExpiryDate >= filter.ExpiryAfterDate.Value || t.PeriodExpiryDate == null));
            }

            if (filter.StartBeforeDate.HasValue)
            {
                predicateBuilder = predicateBuilder.And(l => l.OrigStartDate <= filter.StartBeforeDate);
            }

            if (filter.Programs.Count > 0)
            {
                predicateBuilder = predicateBuilder.And(l => filter.Programs.Any(p => p == l.LeaseProgramTypeCode));
            }

            if (filter.LeaseStatusTypes.Count > 0)
            {
                predicateBuilder = predicateBuilder.And(l => filter.LeaseStatusTypes.Any(p => p == l.LeaseStatusTypeCode));
            }

            var expiryStartDate = filter.ExpiryStartDate.ToNullableDateTime();
            var expiryEndDate = filter.ExpiryEndDate.ToNullableDateTime();
            if (expiryStartDate != null && expiryEndDate != null)
            {
                predicateBuilder = predicateBuilder.And(l => l.PimsLeaseRenewals.Where(r => r.IsExercised == true).Select(rf => rf.ExpiryDt).Max().HasValue ?
                        l.PimsLeaseRenewals.Where(r => r.IsExercised == true).Select(rf => rf.ExpiryDt).Max() >= expiryStartDate &&
                          l.PimsLeaseRenewals.Where(r => r.IsExercised == true).Select(rf => rf.ExpiryDt).Max() <= expiryEndDate :
                        l.OrigExpiryDate >= expiryStartDate &&
                          l.OrigExpiryDate <= expiryEndDate);
            }
            else if (expiryStartDate != null)
            {
                predicateBuilder = predicateBuilder.And(l => l.PimsLeaseRenewals.Where(r => r.IsExercised == true).Select(rf => rf.ExpiryDt).Max().HasValue ?
                        l.PimsLeaseRenewals.Where(r => r.IsExercised == true).Select(rf => rf.ExpiryDt).Max() >= expiryStartDate :
                        l.OrigExpiryDate >= expiryStartDate);
            }
            else if (expiryEndDate != null)
            {
                predicateBuilder = predicateBuilder.And(l => l.PimsLeaseRenewals.Where(r => r.IsExercised == true).Select(rf => rf.ExpiryDt).Max().HasValue ?
                        l.PimsLeaseRenewals.Where(r => r.IsExercised == true).Select(rf => rf.ExpiryDt).Max() <= expiryEndDate :
                        l.OrigExpiryDate <= expiryEndDate);
            }

            if (filter.RegionType.HasValue)
            {
                predicateBuilder = predicateBuilder.And(l => l.RegionCode == filter.RegionType);
            }

            if (!string.IsNullOrWhiteSpace(filter.Details))
            {
                predicateBuilder = predicateBuilder.And(l => EF.Functions.Like(l.LeaseDescription, $"%{filter.Details}%") || EF.Functions.Like(l.LeaseNotes, $"%{filter.Details}%"));
            }

            return predicateBuilder;
        }

        /// <summary>
        /// Checks for given source field name and if found replaces it with target field name for sorting the data in given sort array.
        /// </summary>
        /// <param name="sourceField">Sort field name from model.</param>
        /// <param name="targetField">Sort field name from entity.</param>
        /// <param name="sortDef">Find and replaces the soft field in this list.</param>
        private static void MapSortField(string sourceField, string targetField, List<string> sortDef)
        {
            var sortFieldIndex = sortDef.FindIndex(s => s.Contains(sourceField));
            if (sortFieldIndex > -1)
            {
                sortDef[sortFieldIndex] = sortDef[sortFieldIndex].Replace(sourceField, targetField);
            }
        }

        #endregion
    }
}
