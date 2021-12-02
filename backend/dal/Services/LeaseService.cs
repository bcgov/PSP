using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;

namespace Pims.Dal.Services
{
    /// <summary>
    /// LeaseService class, provides a service layer to interact with leases within the datasource.
    /// </summary>
    public class LeaseService : BaseService<PimsLease>, ILeaseService
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a LeaseService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public LeaseService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<LeaseService> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
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
        public IEnumerable<PimsLease> Get(LeaseFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid()) throw new ArgumentException("Argument must have a valid filter", nameof(filter));

            var query = this.Context.GenerateLeaseQuery(filter);
            var leases = query.ToArray();

            return leases;
        }

        public PimsLease Get(int id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            return this.Context.PimsLeases.Include(l => l.PimsPropertyLeases)
                .ThenInclude(p => p.Property)
                    .ThenInclude(p => p.Address)
                    .ThenInclude(p => p.Country)
                .Include(l => l.PimsPropertyLeases)
                    .ThenInclude(p => p.Property)
                    .ThenInclude(p => p.Address)
                    .ThenInclude(p => p.ProvinceState)
                .Include(l => l.PimsPropertyLeases)
                    .ThenInclude(p => p.Property)
                    .ThenInclude(s => s.SurplusDeclarationTypeCodeNavigation)
                .Include(l => l.PimsPropertyLeases)
                    .ThenInclude(p => p.Property)
                    .ThenInclude(p => p.PropertyAreaUnitTypeCodeNavigation)
                .Include(l => l.LeaseProgramTypeCodeNavigation)
                .Include(l => l.LeasePmtFreqTypeCodeNavigation)
                .Include(l => l.LeasePayRvblTypeCodeNavigation)
                .Include(l => l.LeaseLicenseTypeCodeNavigation)
                .Include(l => l.LeaseResponsibilityTypeCodeNavigation)
                .Include(l => l.LeaseInitiatorTypeCodeNavigation)
                .Include(l => l.LeasePurposeTypeCodeNavigation)
                .Include(l => l.LeaseCategoryTypeCodeNavigation)

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
                    .ThenInclude(o => o.PimsContactMethods)

                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Organization)
                    .ThenInclude(o => o.PimsPersonOrganizations)
                    .ThenInclude(o => o.Person)
                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Organization)
                    .ThenInclude(o => o.PimsOrganizationAddresses)
                    .ThenInclude(o => o.Address)
                .Include(l => l.PimsLeaseTenants)
                    .ThenInclude(t => t.Organization)
                    .ThenInclude(o => o.PimsContactMethods)

                .Include(t => t.PimsPropertyImprovements)

                .Include(l => l.PimsInsurances)
                    .ThenInclude(i => i.InsurancePayeeTypeCodeNavigation)
                .Include(l => l.PimsInsurances)
                    .ThenInclude(i => i.InsuranceTypeCodeNavigation)
                .Include(l => l.PimsInsurances)
                    .ThenInclude(i => i.InsurerOrg)
                    .ThenInclude(o => o.PimsPersonOrganizations)
                    .ThenInclude(o => o.Person)
                    .ThenInclude(p => p.PimsContactMethods)
                .Include(l => l.PimsInsurances)
                    .ThenInclude(i => i.InsurerOrg)
                    .ThenInclude(o => o.PimsContactMethods)
                .Include(l => l.PimsInsurances)
                    .ThenInclude(i => i.InsurerContact)
                    .ThenInclude(p => p.PimsContactMethods)
                .Include(l => l.PimsInsurances)
                    .ThenInclude(i => i.MotiRiskMgmtContact)
                    .ThenInclude(p => p.PimsContactMethods)
                .Include(l => l.PimsInsurances)
                    .ThenInclude(i => i.BctfaRiskMgmtContact)
                    .ThenInclude(p => p.PimsContactMethods)

                .Include(l => l.PimsSecurityDeposits)
                    .ThenInclude(s => s.SecDepHolderTypeCodeNavigation)
                .Include(l => l.PimsSecurityDeposits)
                    .ThenInclude(s => s.SecurityDepositTypeCodeNavigation)
                .Include(l => l.PimsSecurityDepositReturns)
                    .ThenInclude(s => s.SecurityDepositTypeCodeNavigation)

                .Include(l => l.PimsLeaseTerms)
                    .ThenInclude(t => t.LeaseTermStatusTypeCodeNavigation)

                .Where(l => l.LeaseId == id)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get a page with an array of leases within the specified filters.
        /// Note that the 'leaseFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<PimsLease> GetPage(LeaseFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid()) throw new ArgumentException("Argument must have a valid filter", nameof(filter));

            var skip = (filter.Page - 1) * filter.Quantity;
            var query = this.Context.GenerateLeaseQuery(filter);
            var items = query
                .Skip(skip)
                .Take(filter.Quantity)
                .ToArray();

            return new Paged<PimsLease>(items, filter.Page, filter.Quantity, query.Count());
        }
        #endregion
    }
}
