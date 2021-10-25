using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pims.Dal.Services
{
    /// <summary>
    /// PropertyService class, provides a service layer to interact with properties within the datasource.
    /// </summary>
    public class PropertyService : BaseService<Property>, IPropertyService
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a PropertyService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public PropertyService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<PropertyService> logger) : base(dbContext, user, service, logger) { }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the total number of properties in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Context.Properties.Count();
        }

        /// <summary>
        /// Get an array of properties within the specified filters.
        /// Will not return sensitive properties unless the user has the `sensitive-view` claim and belongs to the owning organization.
        /// Note that the 'parcelFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<Property> Get(PropertyFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid()) throw new ArgumentException("Argument must have a valid filter", nameof(filter));

            var query = this.Context.GeneratePropertyQuery(this.User, filter);
            var properties = query.ToArray();

            // TODO: Add optional paging ability to query.

            return properties;
        }

        /// <summary>
        /// Get a page with an array of properties within the specified filters.
        /// Will not return sensitive properties unless the user has the `sensitive-view` claim and belongs to the owning organization.
        /// Note that the 'parcelFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Paged<Property> GetPage(PropertyFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid()) throw new ArgumentException("Argument must have a valid filter", nameof(filter));

            var skip = (filter.Page - 1) * filter.Quantity;
            var query = this.Context.GeneratePropertyQuery(this.User, filter);
            var items = query
                .Skip(skip)
                .Take(filter.Quantity)
                .ToArray();

            return new Paged<Property>(items, filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Get the property for the specified primary key 'id' value.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Property Get(int id)
        {
            this.User.ThrowIfNotAllAuthorized(Permissions.PropertyView);

            var property = this.Context.Properties
                .Include(p => p.District)
                .Include(p => p.Region)
                .Include(p => p.PropertyType)
                .Include(p => p.Status)
                .Include(p => p.DataSource)
                .Include(p => p.Classification)
                .Include(p => p.Tenure)
                .Include(p => p.AreaUnit)
                .Include(p => p.Address)
                .ThenInclude(a => a.AddressType)
                .Include(p => p.Address)
                .ThenInclude(a => a.Region)
                .Include(p => p.Address)
                .ThenInclude(a => a.District)
                .Include(p => p.Address)
                .ThenInclude(a => a.Province)
                .Include(p => p.Address)
                .ThenInclude(a => a.Country)
                .Include(p => p.Leases)
                .ThenInclude(l => l.TenantsManyToMany)
                .Include(p => p.Leases)
                .ThenInclude(l => l.Persons)
                .Include(p => p.Leases)
                .ThenInclude(l => l.Organizations)
                .Where(p => p.Id == id)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
            return property;
        }

        /// <summary>
        /// Get the property for the specified PID value.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Property GetForPID(string pid)
        {
            this.User.ThrowIfNotAllAuthorized(Permissions.PropertyView);
            var search = pid.ConvertPID();

            var property = this.Context.Properties
                .Include(p => p.District)
                .Include(p => p.Region)
                .Include(p => p.PropertyType)
                .Include(p => p.Status)
                .Include(p => p.DataSource)
                .Include(p => p.Classification)
                .Include(p => p.Tenure)
                .Include(p => p.AreaUnit)
                .Include(p => p.Address)
                .ThenInclude(a => a.AddressType)
                .Include(p => p.Address)
                .ThenInclude(a => a.Region)
                .Include(p => p.Address)
                .ThenInclude(a => a.District)
                .Include(p => p.Address)
                .ThenInclude(a => a.Province)
                .Include(p => p.Address)
                .ThenInclude(a => a.Country)
                .Where(p => p.PID == search)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
            return property;
        }
        #endregion
    }
}
