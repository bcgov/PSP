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

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// PropertyRepository class, provides a service layer to interact with properties within the datasource.
    /// </summary>
    public class PropertyRepository : BaseRepository<PimsProperty>, IPropertyRepository
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a PropertyRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public PropertyRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<PropertyRepository> logger, IMapper mapper)
            : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods
        /// <summary>
        /// Returns the total number of properties in the database.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.Context.PimsProperties.Count();
        }

        /// <summary>
        /// Get an array of properties within the specified filters.
        /// Will not return sensitive properties unless the user has the `sensitive-view` claim and belongs to the owning organization.
        /// Note that the 'parcelFilter' will control the 'page' and 'quantity'.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<PimsProperty> Get(PropertyFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

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
        public Paged<PimsProperty> GetPage(PropertyFilter filter)
        {
            this.User.ThrowIfNotAuthorized(Permissions.PropertyView);
            filter.ThrowIfNull(nameof(filter));
            if (!filter.IsValid())
            {
                throw new ArgumentException("Argument must have a valid filter", nameof(filter));
            }

            var skip = (filter.Page - 1) * filter.Quantity;
            var query = this.Context.GeneratePropertyQuery(this.User, filter);
            var items = query
                .Skip(skip)
                .Take(filter.Quantity)
                .ToArray();

            return new Paged<PimsProperty>(items, filter.Page, filter.Quantity, query.Count());
        }

        /// <summary>
        /// Get the property for the specified primary key 'id' value.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsProperty Get(int id)
        {
            this.User.ThrowIfNotAllAuthorized(Permissions.PropertyView);

            var property = this.Context.PimsProperties
                .Include(p => p.DistrictCodeNavigation)
                .Include(p => p.RegionCodeNavigation)
                .Include(p => p.PropertyTypeCodeNavigation)
                .Include(p => p.PropertyStatusTypeCodeNavigation)
                .Include(p => p.PropertyDataSourceTypeCodeNavigation)
                .Include(p => p.PropertyClassificationTypeCodeNavigation)
                .Include(p => p.PimsPropPropTenureTypes)
                    .ThenInclude(t => t.PropertyTenureTypeCodeNavigation)
                .Include(p => p.PropertyAreaUnitTypeCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.RegionCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.DistrictCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.ProvinceState)
                .Include(p => p.Address)
                    .ThenInclude(a => a.Country)
                .Include(p => p.PimsPropertyLeases)
                    .ThenInclude(l => l.Lease)
                    .ThenInclude(l => l.PimsLeaseTenants)
                .Include(p => p.PimsPropertyLeases)
                    .ThenInclude(l => l.Lease)
                    .ThenInclude(l => l.PimsLeaseTenants)
                    .ThenInclude(l => l.Person)
                .Include(p => p.PimsPropertyLeases)
                    .ThenInclude(l => l.Lease)
                    .ThenInclude(l => l.PimsLeaseTenants)
                    .ThenInclude(l => l.Organization)
                .Where(p => p.PropertyId == id)
                .FirstOrDefault() ?? throw new KeyNotFoundException();
            return property;
        }

        /// <summary>
        /// Get the property for the specified PID string value.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public PimsProperty GetByPid(string pid)
        {
            this.User.ThrowIfNotAllAuthorized(Permissions.PropertyView);
            var parsedPid = pid.ConvertPID();

            return GetByPid(parsedPid);
        }

        /// <summary>
        /// Get the property for the specified PID value.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public PimsProperty GetByPid(int pid)
        {
            this.User.ThrowIfNotAllAuthorized(Permissions.PropertyView);

            var property = this.Context.PimsProperties
                .Include(p => p.DistrictCodeNavigation)
                .Include(p => p.RegionCodeNavigation)
                .Include(p => p.PropertyTypeCodeNavigation)
                .Include(p => p.PimsPropPropAnomalyTypes)
                .Include(p => p.PropertyStatusTypeCodeNavigation)
                .Include(p => p.PropertyDataSourceTypeCodeNavigation)
                .Include(p => p.PimsPropPropRoadTypes)
                .Include(p => p.PimsPropPropAdjacentLandTypes)
                .Include(p => p.PimsPropPropTenureTypes)
                    .ThenInclude(t => t.PropertyTenureTypeCodeNavigation)
                .Include(p => p.PropertyAreaUnitTypeCodeNavigation)
                .Include(p => p.VolumetricTypeCodeNavigation)
                .Include(p => p.VolumeUnitTypeCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.RegionCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.DistrictCodeNavigation)
                .Include(p => p.Address)
                    .ThenInclude(a => a.ProvinceState)
                .Include(p => p.Address)
                    .ThenInclude(a => a.Country)
                .FirstOrDefault(p => p.Pid == pid) ?? throw new KeyNotFoundException();
            return property;
        }
        #endregion
    }
}
