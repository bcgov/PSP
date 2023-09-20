using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// ILookupRepository interface, provides a way to fetch lookup lists from the datasource.
    /// </summary>
    public interface ILookupRepository : IRepository
    {
        IEnumerable<PimsOrganization> GetAllOrganizations();

        IEnumerable<PimsOrganizationType> GetAllOrganizationTypes();

        IEnumerable<PimsCountry> GetAllCountries();

        IEnumerable<PimsProvinceState> GetAllProvinces();

        IEnumerable<PimsRegion> GetAllRegions();

        IEnumerable<PimsDistrict> GetAllDistricts();

        IEnumerable<PimsPropertyType> GetAllPropertyTypes();

        IEnumerable<PimsPropertyClassificationType> GetAllPropertyClassificationTypes();

        IEnumerable<PimsAreaUnitType> GetAllPropertyAreaUnitTypes();

        IEnumerable<PimsVolumeUnitType> GetAllPropertyVolumeUnitTypes();

        IEnumerable<PimsPropertyTenureType> GetAllPropertyTenureTypes();

        IEnumerable<PimsRole> GetAllRoles();

        IEnumerable<PimsLeasePayRvblType> GetAllPaymentReceivableTypes();

        IEnumerable<PimsLeaseProgramType> GetAllLeaseProgramTypes();

        IEnumerable<PimsLeaseStatusType> GetAllLeaseStatusTypes();

        IEnumerable<PimsLeaseLicenseType> GetAllLeaseTypes();

        IEnumerable<PimsLeaseCategoryType> GetAllLeaseCategoryTypes();

        IEnumerable<PimsLeasePurposeType> GetAllLeasePurposeTypes();

        IEnumerable<PimsLeaseInitiatorType> GetAllLeaseInitiatorTypes();

        IEnumerable<PimsLeaseTermStatusType> GetAllLeaseTermStatusTypes();

        IEnumerable<PimsLeasePmtFreqType> GetAllLeasePmtFreqTypes();

        IEnumerable<PimsLeaseResponsibilityType> GetAllLeaseResponsibilityTypes();

        IEnumerable<PimsInsuranceType> GetAllInsuranceTypes();

        IEnumerable<PimsContactMethodType> GetAllContactMethodTypes();

        IEnumerable<PimsPropertyImprovementType> GetAllPropertyImprovementTypes();

        IEnumerable<PimsSecurityDepositType> GetAllSecurityDepositTypes();

        IEnumerable<PimsLeasePaymentStatusType> GetAllLeasePaymentStatusTypes();

        IEnumerable<PimsLeasePaymentMethodType> GetAllLeasePaymentMethodTypes();

        IEnumerable<PimsResearchFileStatusType> GetAllResearchFileStatusTypes();

        IEnumerable<PimsRequestSourceType> GetAllRequestSourceTypes();

        IEnumerable<PimsResearchPurposeType> GetAllResearchPurposeTypes();

        IEnumerable<PimsPropResearchPurposeType> GetAllPropertyResearchPurposeTypes();

        IEnumerable<PimsPropertyAnomalyType> GetAllPropertyAnomalyTypes();

        IEnumerable<PimsPropertyRoadType> GetAllPropertyRoadTypes();

        IEnumerable<PimsVolumetricType> GetAllPropertyVolumetricTypes();

        IEnumerable<PimsPphStatusType> GetAllPPHStatusType();

        IEnumerable<PimsDocumentStatusType> GetAllDocumentStatusTypes();

        IEnumerable<PimsDocumentTyp> GetAllDocumentTypes();

        IEnumerable<PimsAcquisitionFileStatusType> GetAllAcquisitionFileStatusTypes();

        IEnumerable<PimsAcqPhysFileStatusType> GetAllAcquisitionPhysFileStatusTypes();

        IEnumerable<PimsAcquisitionType> GetAllAcquisitionTypes();

        IEnumerable<PimsAcqFlPersonProfileType> GetAllAcqFilePersonProfileTypes();

        IEnumerable<PimsTenantType> GetAllTenantTypes();

        IEnumerable<PimsAcquisitionFundingType> GetAllAcquisitionFundingTypes();

        IEnumerable<PimsProjectStatusType> GetAllProjectStatusTypes();

        IEnumerable<PimsConsultationType> GetAllConsultationTypes();

        IEnumerable<PimsFormType> GetAllFormTypes();

        IEnumerable<PimsConsultationStatusType> GetAllConsultationStatusTypes();

        IEnumerable<PimsTakeType> GetAllTakeTypes();

        IEnumerable<PimsTakeStatusType> GetAllTakeStatusTypes();

        IEnumerable<PimsLandActType> GetAllLandActTypes();

        IEnumerable<PimsTakeSiteContamType> GetAllTakeSiteContamTypes();

        IEnumerable<PimsAcqChklstSectionType> GetAllAcquisitionChecklistSectionTypes();

        IEnumerable<PimsAcqChklstItemStatusType> GetAllAcquisitionChecklistItemStatusTypes();

        IEnumerable<PimsAgreementType> GetAllAgreementTypes();

        IEnumerable<PimsInterestHolderInterestType> GetAllInterestHolderInterestTypes();

        IEnumerable<PimsPaymentItemType> GetAllExpropriationPaymentItemTypes();
    }
}
