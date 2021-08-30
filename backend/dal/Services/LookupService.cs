using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Dal.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pims.Dal.Services
{
    /// <summary>
    /// LookupService interface, provides a way to fetch lookup lists from the datasource.
    /// </summary>
    public class LookupService : BaseService, ILookupService
    {
        #region Constructors
        /// <summary>
        /// Creates a new instance of a LookService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public LookupService(PimsContext dbContext, ClaimsPrincipal user, IPimsService service, ILogger<LookupService> logger) : base(dbContext, user, service, logger) { }
        #endregion

        #region Methods
        /// <summary>
        /// Get all organizations sorted by DisplayOrder and Name
        /// </summary>
        public IEnumerable<Organization> GetOrganizations()
        {
            return this.Context.Organizations.AsNoTracking().OrderBy(a => a.Name).ToArray();
        }

        /// <summary>
        /// Get all organization types sorted by DisplayOrder and Id
        /// </summary>
        public IEnumerable<OrganizationType> GetOrganizationTypes()
        {
            return this.Context.OrganizationTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.Id).ToArray();
        }

        /// <summary>
        /// Get all countries sorted by DisplayOrder and Code
        /// </summary>
        public IEnumerable<Country> GetCountries()
        {
            return this.Context.Countries.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.Code).ToArray();
        }

        /// <summary>
        /// Get all provinces sorted by DisplayOrder and Code
        /// </summary>
        public IEnumerable<Province> GetProvinces()
        {
            return this.Context.Provinces.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.Code).ToArray();
        }

        /// <summary>
        /// Get all regions sorted by DisplayOrder and Name
        /// </summary>
        public IEnumerable<Region> GetRegions()
        {
            return this.Context.Regions.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.Name).ToArray();
        }

        /// <summary>
        /// Get all districts sorted by DisplayOrder and Name
        /// </summary>
        public IEnumerable<District> GetDistricts()
        {
            return this.Context.Districts.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.Name).ToArray();
        }

        /// <summary>
        /// Get all property classification types sorted by DisplayOrder and Id
        /// </summary>
        public IEnumerable<PropertyClassificationType> GetPropertyClassificationTypes()
        {
            return this.Context.PropertyClassificationTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.Id).ToArray();
        }

        /// <summary>
        /// Get all property types sorted by DisplayOrder and Id
        /// </summary>
        public IEnumerable<PropertyType> GetPropertyTypes()
        {
            return this.Context.PropertyTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).ThenBy(a => a.Id).ToArray();
        }

        /// <summary>
        /// Get all property tenure types sorted by DisplayOrder and Id
        /// </summary>
        public IEnumerable<PropertyTenureType> GetPropertyTenureTypes()
        {
            return this.Context.PropertyTenureTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).OrderBy(a => a.Id).ToArray();
        }

        /// <summary>
        /// Get all property area unit types sorted by DisplayOrder and Id
        /// </summary>
        public IEnumerable<PropertyAreaUnitType> GetPropertyAreaUnitTypes()
        {
            return this.Context.PropertyAreaUnitTypes.AsNoTracking().OrderBy(a => a.DisplayOrder).OrderBy(a => a.Id).ToArray();
        }

        /// <summary>
        /// Get all roles sorted by Name
        /// </summary>
        public IEnumerable<Role> GetRoles()
        {
            return this.Context.Roles.AsNoTracking().OrderBy(a => a.Name).ToArray();
        }
        #endregion
    }
}
