using Pims.Dal.Entities;
using System.Collections.Generic;

namespace Pims.Dal.Services
{
    /// <summary>
    /// ILookupService interface, provides a way to fetch lookup lists from the datasource.
    /// </summary>
    public interface ILookupService : IService
    {
        IEnumerable<Organization> GetOrganizations();
        IEnumerable<OrganizationType> GetOrganizationTypes();
        IEnumerable<Country> GetCountries();
        IEnumerable<Province> GetProvinces();
        IEnumerable<Region> GetRegions();
        IEnumerable<District> GetDistricts();
        IEnumerable<PropertyType> GetPropertyTypes();
        IEnumerable<PropertyClassificationType> GetPropertyClassificationTypes();
        IEnumerable<PropertyAreaUnitType> GetPropertyAreaUnitTypes();
        IEnumerable<PropertyTenureType> GetPropertyTenureTypes();
        IEnumerable<Role> GetRoles();
    }
}
