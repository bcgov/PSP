using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ILookupService interface, provides a way to fetch lookup lists from the datasource.
    /// </summary>
    public interface ILookupService : IRepository
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

        IEnumerable<PimsVolumeUnitType> GetPropertyVolumeUnitTypes();

        IEnumerable<PimsPropertyTenureType> GetPropertyTenureTypes();

        IEnumerable<PimsRole> GetRoles();

        IEnumerable<PimsLeasePayRvblType> GetPaymentReceivableTypes();

        IEnumerable<PimsLeaseProgramType> GetLeaseProgramTypes();

        IEnumerable<PimsLeaseStatusType> GetLeaseStatusTypes();

        IEnumerable<PimsLeaseLicenseType> GetLeaseTypes();

        IEnumerable<PimsLeaseCategoryType> GetLeaseCategoryTypes();

        IEnumerable<PimsLeasePurposeType> GetLeasePurposeTypes();

        IEnumerable<PimsLeaseInitiatorType> GetLeaseInitiatorTypes();

        IEnumerable<PimsLeaseTermStatusType> GetLeaseTermStatusTypes();

        IEnumerable<PimsLeasePmtFreqType> GetLeasePmtFreqTypes();

        IEnumerable<PimsLeaseResponsibilityType> GetLeaseResponsibilityTypes();

        IEnumerable<PimsInsuranceType> GetInsuranceTypes();

        IEnumerable<PimsContactMethodType> GetContactMethodTypes();

        IEnumerable<PimsPropertyImprovementType> GetPropertyImprovementTypes();

        IEnumerable<PimsSecurityDepositType> GetSecurityDepositTypes();

        IEnumerable<PimsLeasePaymentStatusType> GetLeasePaymentStatusTypes();

        IEnumerable<PimsLeasePaymentMethodType> GetLeasePaymentMethodTypes();

        IEnumerable<PimsResearchFileStatusType> GetResearchFileStatusTypes();

        IEnumerable<PimsRequestSourceType> GeRequestSourceTypes();

        IEnumerable<PimsResearchPurposeType> GetResearchPurposeTypes();

        IEnumerable<PimsPropResearchPurposeType> GetPropertyResearchPurposeTypes();

        IEnumerable<PimsPropertyAnomalyType> GetPropertyAnomalyTypes();

        IEnumerable<PimsPropertyRoadType> GetPropertyRoadTypes();

        IEnumerable<PimsPropertyAdjacentLandType> GetPropertyAdjacentLandTypes();

        IEnumerable<PimsVolumetricType> GetPropertyVolumetricTypes();

        IEnumerable<PimsPphStatusType> GetPPHStatusType();

        IEnumerable<PimsDocumentStatusType> GetDocumentStatusTypes();

        IEnumerable<PimsDocumentTyp> GetDocumentTypes();

        IEnumerable<PimsAcquisitionFileStatusType> GetAcquisitionFileStatusTypes();

        IEnumerable<PimsAcqPhysFileStatusType> GetAcquisitionPhysFileStatusTypes();

        IEnumerable<PimsAcquisitionType> GetAcquisitionTypes();

        IEnumerable<PimsActivityTemplateType> GetActivityTemplateTypes();

        IEnumerable<PimsActivityInstanceStatusType> GetActivityStatusTypes();
    }
}
