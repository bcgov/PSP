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
