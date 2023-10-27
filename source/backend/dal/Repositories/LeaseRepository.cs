using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public LeaseRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LeaseRepository> logger)
            : base(dbContext, user, logger)
        {
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

            var query = this.Context.GenerateLeaseQuery(filter, regionCodes, loadPayments);

            var leases = query.OrderBy(l => l.LeaseId).ToArray();

            return leases;
        }

        public long GetRowVersion(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.LeaseView);
            return this.Context.PimsLeases.Where(l => l.LeaseId == id)?.Select(l => l.ConcurrencyControlNumber)?.FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public PimsLease Get(long id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.LeaseView);

            PimsLease lease = this.Context.PimsLeases.AsSplitQuery()
                .Include(l => l.PimsPropertyLeases)
                .Include(l => l.RegionCodeNavigation)
                .Include(l => l.LeaseProgramTypeCodeNavigation)
                .Include(l => l.LeasePayRvblTypeCodeNavigation)
                .Include(l => l.LeaseLicenseTypeCodeNavigation)
                .Include(l => l.LeaseResponsibilityTypeCodeNavigation)
                .Include(l => l.LeaseInitiatorTypeCodeNavigation)
                .Include(l => l.LeasePurposeTypeCodeNavigation)
                .Include(l => l.LeaseCategoryTypeCodeNavigation)
                .Include(l => l.LeaseStatusTypeCodeNavigation)
                .Include(l => l.PimsLeaseTenants)
                .Include(t => t.PimsPropertyImprovements)
                .Include(l => l.PimsInsurances)
                .Include(l => l.PimsSecurityDeposits)
                .Include(l => l.PimsLeaseTerms)
                .Include(l => l.PimsLeaseConsultations)
                    .ThenInclude(lc => lc.ConsultationStatusTypeCodeNavigation)
                .Include(l => l.PimsLeaseConsultations)
                    .ThenInclude(lc => lc.ConsultationTypeCodeNavigation)
                .Include(l => l.Project)
                .FirstOrDefault(l => l.LeaseId == id) ?? throw new KeyNotFoundException();

            lease.LeasePurposeTypeCodeNavigation = this.Context.PimsLeasePurposeTypes.Single(type => type.LeasePurposeTypeCode == lease.LeasePurposeTypeCode);
            lease.PimsPropertyImprovements = lease.PimsPropertyImprovements.OrderBy(i => i.PropertyImprovementTypeCode).ToArray();
            lease.PimsLeaseTerms = lease.PimsLeaseTerms.OrderBy(t => t.TermStartDate).ThenBy(t => t.LeaseTermId).Select(t =>
            {
                t.PimsLeasePayments = t.PimsLeasePayments.OrderBy(p => p.PaymentReceivedDate).ThenBy(p => p.LeasePaymentId).ToArray();
                return t;
            }).ToArray();
            return lease;
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
            var propertiesHistoryLastUpdatedBy = this.Context.PimsPropertyLeaseHists.AsNoTracking()
            .Where(aph => aph.LeaseId == leaseId)
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
            var consultationHistoryLastUpdatedBy = this.Context.PimsLeaseConsultationHists.AsNoTracking()
            .Where(aph => aph.LeaseId == leaseId)
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

            // Lease Tenants
            var tenantsLastUpdatedBy = this.Context.PimsLeaseTenants.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(tenantsLastUpdatedBy);

            // Lease Deleted Tenants
            // This is needed to get the tenants last-updated-by when deleted
            var tenantsHistoryLastUpdatedBy = this.Context.PimsLeaseTenantHists.AsNoTracking()
            .Where(aph => aph.LeaseId == leaseId)
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
            lastUpdatedByAggregate.AddRange(tenantsHistoryLastUpdatedBy);

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
            var improvementsHistoryLastUpdatedBy = this.Context.PimsPropertyImprovementHists.AsNoTracking()
            .Where(aph => aph.LeaseId == leaseId)
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
            var insuranceHistoryLastUpdatedBy = this.Context.PimsInsuranceHists.AsNoTracking()
            .Where(aph => aph.LeaseId == leaseId)
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
            var depositsHistoryLastUpdatedBy = this.Context.PimsSecurityDepositHists.AsNoTracking()
            .Where(aph => aph.LeaseId == leaseId)
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
            var returnsHistoryLastUpdatedBy = this.Context.PimsSecurityDepositHists.AsNoTracking()
                .Where(at => at.LeaseId == leaseId)
                .Join(
                    returnsHistory,
                    securityDepositHist => securityDepositHist.SecurityDepositId,
                    securityDepositReturnHolderHist => securityDepositReturnHolderHist.SecurityDepositId,
                    (securityDepositHist, securityDepositReturnHist) => new LastUpdatedByModel()
                    {
                        ParentId = leaseId,
                        AppLastUpdateUserid = securityDepositReturnHist.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                        AppLastUpdateUserGuid = securityDepositReturnHist.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                        AppLastUpdateTimestamp = securityDepositReturnHist.EndDateHist ?? DateTime.UnixEpoch,
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
            var returnHoldersHistoryLastUpdatedBy = this.Context.PimsSecurityDepositHists.AsNoTracking()
                .Where(at => at.LeaseId == leaseId)
                .Join(
                    returnHoldersHistory,
                    securityDepositHist => securityDepositHist.SecurityDepositId,
                    securityDepositHolderHist => securityDepositHolderHist.SecurityDepositId,
                    (securityDepositHist, securityDepositHolderHist) => new LastUpdatedByModel()
                    {
                        ParentId = leaseId,
                        AppLastUpdateUserid = securityDepositHolderHist.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                        AppLastUpdateUserGuid = securityDepositHolderHist.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                        AppLastUpdateTimestamp = securityDepositHolderHist.EndDateHist ?? DateTime.UnixEpoch,
                    })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(returnHoldersHistoryLastUpdatedBy);

            // Lease Terms
            var termLastUpdatedBy = this.Context.PimsLeaseTerms.AsNoTracking()
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
            lastUpdatedByAggregate.AddRange(termLastUpdatedBy);

            // Lease Deleted Terms
            // This is needed to get the lease terms last-updated-by when deleted
            var termHistoryLastUpdatedBy = this.Context.PimsLeaseTermHists.AsNoTracking()
            .Where(aph => aph.LeaseId == leaseId)
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
            lastUpdatedByAggregate.AddRange(termHistoryLastUpdatedBy);

            // Lease Term Payment
            var termPaymentReturnsLastUpdatedBy = this.Context.PimsLeasePayments.AsNoTracking()
                .Include(l => l.LeaseTerm)
                .Where(ap => ap.LeaseTerm.LeaseId == leaseId)
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
            lastUpdatedByAggregate.AddRange(termPaymentReturnsLastUpdatedBy);

            // Lease Deleted Term Payment
            // This is needed to get the term payments last-updated-by when deleted
            var termPaymentHistory = this.Context.PimsLeasePaymentHists.AsNoTracking();
            var termPaymentHistoryLastUpdatedBy = this.Context.PimsLeaseTermHists.AsNoTracking()
                .Where(at => at.LeaseId == leaseId)
                .Join(
                    termPaymentHistory,
                    leaseTermHist => leaseTermHist.LeaseTermId,
                    leasePaymentHist => leasePaymentHist.LeaseTermId,
                    (leaseTermHist, leasePaymentHist) => new LastUpdatedByModel()
                    {
                        ParentId = leaseId,
                        AppLastUpdateUserid = leasePaymentHist.AppLastUpdateUserid, // TODO: Update this once the DB tracks the user
                        AppLastUpdateUserGuid = leasePaymentHist.AppLastUpdateUserGuid, // TODO: Update this once the DB tracks the user
                        AppLastUpdateTimestamp = leasePaymentHist.EndDateHist ?? DateTime.UnixEpoch,
                    })
            .OrderByDescending(lu => lu.AppLastUpdateTimestamp)
            .Take(1)
            .ToList();
            lastUpdatedByAggregate.AddRange(termPaymentHistoryLastUpdatedBy);

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
            var documentsHistoryLastUpdatedBy = this.Context.PimsLeaseDocumentHists.AsNoTracking()
            .Where(aph => aph.LeaseId == leaseId)
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
            var notesHistoryLastUpdatedBy = this.Context.PimsLeaseNoteHists.AsNoTracking()
                .Where(anh => anh.LeaseId == leaseId)
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
                .Include(l => l.LeasePurposeTypeCodeNavigation)
                .Include(l => l.LeaseCategoryTypeCodeNavigation)
                .Include(l => l.LeaseStatusTypeCodeNavigation)

                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(l => l.TenantTypeCodeNavigation)
                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Person)
                    .ThenInclude(o => o.PimsPersonOrganizations)
                    .ThenInclude(o => o.Organization)
                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Person)
                    .ThenInclude(o => o.PimsPersonAddresses)
                    .ThenInclude(o => o.Address)
                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Person)
                    .ThenInclude(o => o.PimsPersonAddresses)
                    .ThenInclude(o => o.AddressUsageTypeCodeNavigation)
                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Person)
                    .ThenInclude(o => o.PimsContactMethods)
                    .ThenInclude(cm => cm.ContactMethodTypeCodeNavigation)

                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Organization)
                    .ThenInclude(o => o.PimsPersonOrganizations)
                    .ThenInclude(o => o.Person)
                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Organization)
                    .ThenInclude(o => o.PimsOrganizationAddresses)
                    .ThenInclude(o => o.AddressUsageTypeCodeNavigation)
                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Organization)
                    .ThenInclude(o => o.PimsOrganizationAddresses)
                    .ThenInclude(o => o.Address)
                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Organization)
                    .ThenInclude(o => o.PimsContactMethods)
                    .ThenInclude(cm => cm.ContactMethodTypeCodeNavigation)

                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.PrimaryContact)

                .Include(l => l.PimsLeaseTenants)
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

                .Include(l => l.PimsLeaseTerms)
                     .ThenInclude(t => t.LeasePmtFreqTypeCodeNavigation)
                .Include(t => t.PimsLeaseTerms)
                     .ThenInclude(t => t.LeaseTermStatusTypeCodeNavigation)
                .Include(t => t.PimsLeaseTerms)
                    .ThenInclude(t => t.PimsLeasePayments)
                    .ThenInclude(t => t.LeasePaymentMethodTypeCodeNavigation)
                .Include(t => t.PimsLeaseTerms)
                    .ThenInclude(t => t.PimsLeasePayments)
                    .ThenInclude(t => t.LeasePaymentStatusTypeCodeNavigation)
                .FirstOrDefault(l => l.LeaseId == id) ?? throw new KeyNotFoundException();

            lease.LeasePurposeTypeCodeNavigation = this.Context.PimsLeasePurposeTypes.Single(type => type.LeasePurposeTypeCode == lease.LeasePurposeTypeCode);
            lease.PimsPropertyImprovements = lease.PimsPropertyImprovements.OrderBy(i => i.PropertyImprovementTypeCode).ToArray();
            lease.PimsLeaseTerms = lease.PimsLeaseTerms.OrderBy(t => t.TermStartDate).ThenBy(t => t.LeaseTermId).Select(t =>
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
            var query = this.Context.GenerateLeaseQuery(filter, regions);
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

            this.User.ThrowIfNotAuthorized(Permissions.LeaseAdd);

            lease = this.Context.GenerateLFileNo(lease);

            this.Context.PimsLeases.Add(lease);
            this.Context.CommitTransaction();
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
            if (commitTransaction)
            {
                this.Context.CommitTransaction();
            }
            return existingLease;
        }

        /// <summary>
        /// Update the consultations of a lease.
        /// </summary>
        /// <param name="leaseId"></param>
        /// <param name="rowVersion"></param>
        /// <param name="pimsLeaseConsultations"></param>
        /// <returns></returns>
        public PimsLease UpdateLeaseConsultations(long leaseId, long? rowVersion, ICollection<PimsLeaseConsultation> pimsLeaseConsultations)
        {
            var existingLease = this.Context.PimsLeases.Include(l => l.PimsLeaseConsultations).AsNoTracking().FirstOrDefault(l => l.LeaseId == leaseId)
                 ?? throw new KeyNotFoundException();
            if (existingLease.ConcurrencyControlNumber != rowVersion)
            {
                throw new DbUpdateConcurrencyException("Unable to save. Please refresh your page and try again");
            }

            this.Context.UpdateChild<PimsLease, long, PimsLeaseConsultation, long>(l => l.PimsLeaseConsultations, leaseId, pimsLeaseConsultations.ToArray());

            return GetNoTracking(existingLease.LeaseId);
        }
        #endregion
    }
}
