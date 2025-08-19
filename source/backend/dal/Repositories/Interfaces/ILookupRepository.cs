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

        IEnumerable<PimsAreaUnitType> GetAllPropertyAreaUnitTypes();

        IEnumerable<PimsVolumeUnitType> GetAllPropertyVolumeUnitTypes();

        IEnumerable<PimsPropertyTenureType> GetAllPropertyTenureTypes();

        IEnumerable<PimsPropertyPurposeType> GetAllPropertyManagementPurposeTypes();

        IEnumerable<PimsRole> GetAllRoles();

        IEnumerable<PimsLeasePayRvblType> GetAllPaymentReceivableTypes();

        IEnumerable<PimsLeaseProgramType> GetAllLeaseProgramTypes();

        IEnumerable<PimsLeaseStatusType> GetAllLeaseStatusTypes();

        IEnumerable<PimsLeaseLicenseType> GetAllLeaseTypes();

        IEnumerable<PimsLeasePurposeType> GetAllLeasePurposeTypes();

        IEnumerable<PimsLeaseInitiatorType> GetAllLeaseInitiatorTypes();

        IEnumerable<PimsLeasePeriodStatusType> GetAllLeasePeriodStatusTypes();

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

        IEnumerable<PimsAcqFlTeamProfileType> GetAllAcqFileTeamProfileTypes();

        IEnumerable<PimsLeaseStakeholderType> GetAllLeaseStakeholderTypes();

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

        IEnumerable<PimsAgreementType> GetAllAgreementTypes();

        IEnumerable<PimsInterestHolderInterestType> GetAllInterestHolderInterestTypes();

        IEnumerable<PimsPaymentItemType> GetAllExpropriationPaymentItemTypes();

        IEnumerable<PimsMgmtActivityStatusType> GetAllMgmtActivityStatusTypes();

        IEnumerable<PimsMgmtActivitySubtype> GetAllMgmtActivitySubtypes();

        IEnumerable<PimsMgmtActivityType> GetAllMgmtActivityTypes();

        IEnumerable<PimsAgreementStatusType> GetAllAgreementStatusTypes();

        IEnumerable<PimsDispositionFileStatusType> GetAllDispositionFileStatusTypes();

        IEnumerable<PimsDispositionFundingType> GetAllDispositionFileFundingTypes();

        IEnumerable<PimsDispositionInitiatingDocType> GetAllDispositionInitiatingDocTypes();

        IEnumerable<PimsDispositionType> GetAllDispositionTypes();

        IEnumerable<PimsDispositionStatusType> GetAllDispositionStatusTypes();

        IEnumerable<PimsDspPhysFileStatusType> GetAllDispositionPhysFileStatusTypes();

        IEnumerable<PimsDspInitiatingBranchType> GetAllDispositionInitiatingBranchTypes();

        IEnumerable<PimsDspFlTeamProfileType> GetAllDispositionFlTeamProfileTypes();

        IEnumerable<PimsDispositionOfferStatusType> GetAllDispositionOfferStatusTypes();

        IEnumerable<PimsChklstItemStatusType> GetAllChecklistItemStatusTypes();

        IEnumerable<PimsDspChklstSectionType> GetAllDispositionChecklistSectionTypes();

        IEnumerable<PimsHistoricalFileNumberType> GetAllHistoricalNumberTypes();

        IEnumerable<PimsLeaseChklstSectionType> GetAllLeaseChecklistSectionTypes();

        IEnumerable<PimsLeasePaymentCategoryType> GetAllLeasePaymentCategoryTypes();

        IEnumerable<PimsConsultationOutcomeType> GetAllConsultationOutcomeTypes();

        IEnumerable<PimsSubfileInterestType> GetAllSubfileInterestTypes();

        IEnumerable<PimsAcqFileProgessType> GetAllAcquisitionFileProgressStatusTypes();

        IEnumerable<PimsAcqFileAppraisalType> GetAllAcquisitionFileAppraisalStatusTypes();

        IEnumerable<PimsAcqFileLglSrvyType> GetAllAcquisitionFileLegalSurveyStatusTypes();

        IEnumerable<PimsAcqFileTakeType> GetAllAcquisitionFileTakeStatusTypes();

        IEnumerable<PimsAcqFileExpropRiskType> GetAllAcquisitionFileExpropiationRiskStatusTypes();

        IEnumerable<PimsLlTeamProfileType> GetAllLlTeamProfileTypes();

        IEnumerable<PimsExpropOwnerHistoryType> GetAllExpropriationEventTypes();

        IEnumerable<PimsManagementFileStatusType> GetAllManagementFileStatusTypes();

        IEnumerable<PimsManagementFilePurposeType> GetAllManagementFilePurposeTypes();

        IEnumerable<PimsManagementFileProfileType> GetAllManagementFileProfileTypes();
    }
}
