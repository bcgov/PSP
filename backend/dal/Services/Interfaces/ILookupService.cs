using Pims.Dal.Entities;
using System.Collections.Generic;

namespace Pims.Dal.Services
{
    /// <summary>
    /// ILookupService interface, provides a way to fetch lookup lists from the datasource.
    /// </summary>
    public interface ILookupService : IService
    {
        IEnumerable<PimsOrganization> GetOrganizations();
        IEnumerable<PimsOrganizationType> GetOrganizationTypes();
        IEnumerable<PimsCountry> GetCountries();
        IEnumerable<PimsProvinceState> GetProvinces();
        IEnumerable<PimsRegion> GetRegions();
        IEnumerable<PimsDistrict> GetDistricts();
        IEnumerable<PimsPropertyType> GetPropertyTypes();
        IEnumerable<PimsPropertyClassificationType> GetPropertyClassificationTypes();
        IEnumerable<PimsAreaUnitType> GetPropertyAreaUnitTypes();
        IEnumerable<PimsPropertyTenureType> GetPropertyTenureTypes();
        IEnumerable<PimsRole> GetRoles();
        IEnumerable<PimsLeasePayRvblType> GetPaymentReceivableTypes();
        IEnumerable<LeaseProgramType> GetLeaseProgramTypes();
    }
}
