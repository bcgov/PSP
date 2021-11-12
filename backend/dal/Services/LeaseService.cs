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

namespace Pims.Dal.Services
{
    /// <summary>
    /// LeaseService class, provides a service layer to interact with leases within the datasource.
    /// </summary>
    public class LeaseService : BaseService<Lease>, ILeaseService
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a LeaseService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public LeaseService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<LeaseService> logger) : base(dbContext, user, service, logger) { }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the total number of leases in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Context.Leases.Count();
        }

        /// <summary>
        /// Get an array of leases within the specified filters.
        /// Note that the 'leaseFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<Lease> Get(LeaseFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid()) throw new ArgumentException("Argument must have a valid filter", nameof(filter));

            var query = this.Context.GenerateLeaseQuery(filter);
            var leases = query.ToArray();

            return leases;
        }

        public Lease Get(int id)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            return this.Context.Leases.Include(l => l.Properties)
                .ThenInclude(p => p.Address)
                .ThenInclude(p => p.Country)
                .Include(l => l.Properties)
                .ThenInclude(p => p.Address)
                .ThenInclude(p => p.Province)
                .Include(l => l.Properties)
                .ThenInclude(s => s.SurplusDeclarationType)
                .Include(l => l.Properties)
                .ThenInclude(p => p.AreaUnit)
                .Include(l => l.ProgramType)
                .Include(l => l.PaymentFrequencyType)
                .Include(l => l.MotiName)

                .Include(l => l.TenantsManyToMany)
                .ThenInclude(t => t.Person)
                .ThenInclude(o => o.OrganizationsManyToMany)
                .Include(l => l.TenantsManyToMany)
                .ThenInclude(t => t.Person)
                .ThenInclude(o => o.Address)
                .Include(l => l.TenantsManyToMany)
                .ThenInclude(t => t.Person)
                .ThenInclude(o => o.ContactMethods)

                .Include(l => l.TenantsManyToMany)
                .ThenInclude(t => t.Organization)
                .ThenInclude(o => o.PersonsManyToMany)
                .Include(l => l.TenantsManyToMany)
                .ThenInclude(t => t.Organization)
                .ThenInclude(o => o.Persons)
                .Include(l => l.TenantsManyToMany)
                .ThenInclude(t => t.Organization)
                .ThenInclude(o => o.Address)
                .Include(l => l.TenantsManyToMany)
                .ThenInclude(t => t.Organization)
                .ThenInclude(o => o.ContactMethods)

                .Include(l => l.Improvements)
                .ThenInclude(t => t.PropertyImprovementType)

                .Include(l => l.Insurances)
                .ThenInclude(i => i.InsurancePayeeType)
                .Include(l => l.Insurances)
                .ThenInclude(i => i.InsuranceType)
                .Include(l => l.Insurances)
                .ThenInclude(i => i.InsurerOrganization)
                .Include(l => l.Insurances)
                .ThenInclude(i => i.InsurerContact)

                .Where(l => l.Id == id)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Get a page with an array of leases within the specified filters.
        /// Note that the 'leaseFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<Lease> GetPage(LeaseFilter filter)
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

            return new Paged<Lease>(items, filter.Page, filter.Quantity, query.Count());
        }
        #endregion
    }
}
