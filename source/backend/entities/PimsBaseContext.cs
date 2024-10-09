using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pims.Dal.Entities;

namespace Pims.Dal;

public partial class PimsBaseContext : DbContext
{
    public PimsBaseContext(DbContextOptions<PimsBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PimsAccessRequest> PimsAccessRequests { get; set; }

    public virtual DbSet<PimsAccessRequestHist> PimsAccessRequestHists { get; set; }

    public virtual DbSet<PimsAccessRequestOrganization> PimsAccessRequestOrganizations { get; set; }

    public virtual DbSet<PimsAccessRequestOrganizationHist> PimsAccessRequestOrganizationHists { get; set; }

    public virtual DbSet<PimsAccessRequestStatusType> PimsAccessRequestStatusTypes { get; set; }

    public virtual DbSet<PimsAcqChklstItemType> PimsAcqChklstItemTypes { get; set; }

    public virtual DbSet<PimsAcqChklstSectionType> PimsAcqChklstSectionTypes { get; set; }

    public virtual DbSet<PimsAcqFlTeamProfileType> PimsAcqFlTeamProfileTypes { get; set; }

    public virtual DbSet<PimsAcqPhysFileStatusType> PimsAcqPhysFileStatusTypes { get; set; }

    public virtual DbSet<PimsAcquisitionChecklistItem> PimsAcquisitionChecklistItems { get; set; }

    public virtual DbSet<PimsAcquisitionChecklistItemHist> PimsAcquisitionChecklistItemHists { get; set; }

    public virtual DbSet<PimsAcquisitionFile> PimsAcquisitionFiles { get; set; }

    public virtual DbSet<PimsAcquisitionFileDocument> PimsAcquisitionFileDocuments { get; set; }

    public virtual DbSet<PimsAcquisitionFileDocumentHist> PimsAcquisitionFileDocumentHists { get; set; }

    public virtual DbSet<PimsAcquisitionFileForm> PimsAcquisitionFileForms { get; set; }

    public virtual DbSet<PimsAcquisitionFileFormHist> PimsAcquisitionFileFormHists { get; set; }

    public virtual DbSet<PimsAcquisitionFileHist> PimsAcquisitionFileHists { get; set; }

    public virtual DbSet<PimsAcquisitionFileNote> PimsAcquisitionFileNotes { get; set; }

    public virtual DbSet<PimsAcquisitionFileNoteHist> PimsAcquisitionFileNoteHists { get; set; }

    public virtual DbSet<PimsAcquisitionFileStatusType> PimsAcquisitionFileStatusTypes { get; set; }

    public virtual DbSet<PimsAcquisitionFileTeam> PimsAcquisitionFileTeams { get; set; }

    public virtual DbSet<PimsAcquisitionFileTeamHist> PimsAcquisitionFileTeamHists { get; set; }

    public virtual DbSet<PimsAcquisitionFundingType> PimsAcquisitionFundingTypes { get; set; }

    public virtual DbSet<PimsAcquisitionOwner> PimsAcquisitionOwners { get; set; }

    public virtual DbSet<PimsAcquisitionOwnerHist> PimsAcquisitionOwnerHists { get; set; }

    public virtual DbSet<PimsAcquisitionType> PimsAcquisitionTypes { get; set; }

    public virtual DbSet<PimsAddress> PimsAddresses { get; set; }

    public virtual DbSet<PimsAddressHist> PimsAddressHists { get; set; }

    public virtual DbSet<PimsAddressUsageType> PimsAddressUsageTypes { get; set; }

    public virtual DbSet<PimsAgreement> PimsAgreements { get; set; }

    public virtual DbSet<PimsAgreementHist> PimsAgreementHists { get; set; }

    public virtual DbSet<PimsAgreementStatusType> PimsAgreementStatusTypes { get; set; }

    public virtual DbSet<PimsAgreementType> PimsAgreementTypes { get; set; }

    public virtual DbSet<PimsAreaUnitType> PimsAreaUnitTypes { get; set; }

    public virtual DbSet<PimsBusinessFunctionCode> PimsBusinessFunctionCodes { get; set; }

    public virtual DbSet<PimsBusinessFunctionCodeHist> PimsBusinessFunctionCodeHists { get; set; }

    public virtual DbSet<PimsChartOfAccountsCode> PimsChartOfAccountsCodes { get; set; }

    public virtual DbSet<PimsChartOfAccountsCodeHist> PimsChartOfAccountsCodeHists { get; set; }

    public virtual DbSet<PimsChklstItemStatusType> PimsChklstItemStatusTypes { get; set; }

    public virtual DbSet<PimsClaim> PimsClaims { get; set; }

    public virtual DbSet<PimsClaimHist> PimsClaimHists { get; set; }

    public virtual DbSet<PimsCompReqFinancial> PimsCompReqFinancials { get; set; }

    public virtual DbSet<PimsCompReqFinancialHist> PimsCompReqFinancialHists { get; set; }

    public virtual DbSet<PimsCompensationRequisition> PimsCompensationRequisitions { get; set; }

    public virtual DbSet<PimsCompensationRequisitionHist> PimsCompensationRequisitionHists { get; set; }

    public virtual DbSet<PimsConsultationOutcomeType> PimsConsultationOutcomeTypes { get; set; }

    public virtual DbSet<PimsConsultationStatusType> PimsConsultationStatusTypes { get; set; }

    public virtual DbSet<PimsConsultationType> PimsConsultationTypes { get; set; }

    public virtual DbSet<PimsContactMethod> PimsContactMethods { get; set; }

    public virtual DbSet<PimsContactMethodHist> PimsContactMethodHists { get; set; }

    public virtual DbSet<PimsContactMethodType> PimsContactMethodTypes { get; set; }

    public virtual DbSet<PimsContactMgrVw> PimsContactMgrVws { get; set; }

    public virtual DbSet<PimsCostTypeCode> PimsCostTypeCodes { get; set; }

    public virtual DbSet<PimsCostTypeCodeHist> PimsCostTypeCodeHists { get; set; }

    public virtual DbSet<PimsCountry> PimsCountries { get; set; }

    public virtual DbSet<PimsDataSourceType> PimsDataSourceTypes { get; set; }

    public virtual DbSet<PimsDispositionAppraisal> PimsDispositionAppraisals { get; set; }

    public virtual DbSet<PimsDispositionAppraisalHist> PimsDispositionAppraisalHists { get; set; }

    public virtual DbSet<PimsDispositionChecklistItem> PimsDispositionChecklistItems { get; set; }

    public virtual DbSet<PimsDispositionChecklistItemHist> PimsDispositionChecklistItemHists { get; set; }

    public virtual DbSet<PimsDispositionFile> PimsDispositionFiles { get; set; }

    public virtual DbSet<PimsDispositionFileDocument> PimsDispositionFileDocuments { get; set; }

    public virtual DbSet<PimsDispositionFileDocumentHist> PimsDispositionFileDocumentHists { get; set; }

    public virtual DbSet<PimsDispositionFileHist> PimsDispositionFileHists { get; set; }

    public virtual DbSet<PimsDispositionFileNote> PimsDispositionFileNotes { get; set; }

    public virtual DbSet<PimsDispositionFileNoteHist> PimsDispositionFileNoteHists { get; set; }

    public virtual DbSet<PimsDispositionFileProperty> PimsDispositionFileProperties { get; set; }

    public virtual DbSet<PimsDispositionFilePropertyHist> PimsDispositionFilePropertyHists { get; set; }

    public virtual DbSet<PimsDispositionFileStatusType> PimsDispositionFileStatusTypes { get; set; }

    public virtual DbSet<PimsDispositionFileTeam> PimsDispositionFileTeams { get; set; }

    public virtual DbSet<PimsDispositionFileTeamHist> PimsDispositionFileTeamHists { get; set; }

    public virtual DbSet<PimsDispositionFundingType> PimsDispositionFundingTypes { get; set; }

    public virtual DbSet<PimsDispositionInitiatingDocType> PimsDispositionInitiatingDocTypes { get; set; }

    public virtual DbSet<PimsDispositionOffer> PimsDispositionOffers { get; set; }

    public virtual DbSet<PimsDispositionOfferHist> PimsDispositionOfferHists { get; set; }

    public virtual DbSet<PimsDispositionOfferStatusType> PimsDispositionOfferStatusTypes { get; set; }

    public virtual DbSet<PimsDispositionPurchaser> PimsDispositionPurchasers { get; set; }

    public virtual DbSet<PimsDispositionPurchaserHist> PimsDispositionPurchaserHists { get; set; }

    public virtual DbSet<PimsDispositionSale> PimsDispositionSales { get; set; }

    public virtual DbSet<PimsDispositionSaleHist> PimsDispositionSaleHists { get; set; }

    public virtual DbSet<PimsDispositionStatusType> PimsDispositionStatusTypes { get; set; }

    public virtual DbSet<PimsDispositionType> PimsDispositionTypes { get; set; }

    public virtual DbSet<PimsDistrict> PimsDistricts { get; set; }

    public virtual DbSet<PimsDocument> PimsDocuments { get; set; }

    public virtual DbSet<PimsDocumentCategorySubtype> PimsDocumentCategorySubtypes { get; set; }

    public virtual DbSet<PimsDocumentCategoryType> PimsDocumentCategoryTypes { get; set; }

    public virtual DbSet<PimsDocumentFormatType> PimsDocumentFormatTypes { get; set; }

    public virtual DbSet<PimsDocumentHist> PimsDocumentHists { get; set; }

    public virtual DbSet<PimsDocumentStatusType> PimsDocumentStatusTypes { get; set; }

    public virtual DbSet<PimsDocumentTyp> PimsDocumentTyps { get; set; }

    public virtual DbSet<PimsDocumentTypHist> PimsDocumentTypHists { get; set; }

    public virtual DbSet<PimsDspChklstItemType> PimsDspChklstItemTypes { get; set; }

    public virtual DbSet<PimsDspChklstSectionType> PimsDspChklstSectionTypes { get; set; }

    public virtual DbSet<PimsDspFlTeamProfileType> PimsDspFlTeamProfileTypes { get; set; }

    public virtual DbSet<PimsDspInitiatingBranchType> PimsDspInitiatingBranchTypes { get; set; }

    public virtual DbSet<PimsDspPhysFileStatusType> PimsDspPhysFileStatusTypes { get; set; }

    public virtual DbSet<PimsDspPurchAgent> PimsDspPurchAgents { get; set; }

    public virtual DbSet<PimsDspPurchAgentHist> PimsDspPurchAgentHists { get; set; }

    public virtual DbSet<PimsDspPurchSolicitor> PimsDspPurchSolicitors { get; set; }

    public virtual DbSet<PimsDspPurchSolicitorHist> PimsDspPurchSolicitorHists { get; set; }

    public virtual DbSet<PimsExpropPmtPmtItem> PimsExpropPmtPmtItems { get; set; }

    public virtual DbSet<PimsExpropPmtPmtItemHist> PimsExpropPmtPmtItemHists { get; set; }

    public virtual DbSet<PimsExpropriationPayment> PimsExpropriationPayments { get; set; }

    public virtual DbSet<PimsExpropriationPaymentHist> PimsExpropriationPaymentHists { get; set; }

    public virtual DbSet<PimsFenceType> PimsFenceTypes { get; set; }

    public virtual DbSet<PimsFinancialActivityCode> PimsFinancialActivityCodes { get; set; }

    public virtual DbSet<PimsFinancialActivityCodeHist> PimsFinancialActivityCodeHists { get; set; }

    public virtual DbSet<PimsFormType> PimsFormTypes { get; set; }

    public virtual DbSet<PimsH120Category> PimsH120Categories { get; set; }

    public virtual DbSet<PimsH120CategoryHist> PimsH120CategoryHists { get; set; }

    public virtual DbSet<PimsHistoricalFileNumber> PimsHistoricalFileNumbers { get; set; }

    public virtual DbSet<PimsHistoricalFileNumberHist> PimsHistoricalFileNumberHists { get; set; }

    public virtual DbSet<PimsHistoricalFileNumberType> PimsHistoricalFileNumberTypes { get; set; }

    public virtual DbSet<PimsHistoricalFileNumberVw> PimsHistoricalFileNumberVws { get; set; }

    public virtual DbSet<PimsInsurance> PimsInsurances { get; set; }

    public virtual DbSet<PimsInsuranceHist> PimsInsuranceHists { get; set; }

    public virtual DbSet<PimsInsuranceType> PimsInsuranceTypes { get; set; }

    public virtual DbSet<PimsInterestHolder> PimsInterestHolders { get; set; }

    public virtual DbSet<PimsInterestHolderHist> PimsInterestHolderHists { get; set; }

    public virtual DbSet<PimsInterestHolderInterestType> PimsInterestHolderInterestTypes { get; set; }

    public virtual DbSet<PimsInterestHolderType> PimsInterestHolderTypes { get; set; }

    public virtual DbSet<PimsInthldrPropInterest> PimsInthldrPropInterests { get; set; }

    public virtual DbSet<PimsInthldrPropInterestHist> PimsInthldrPropInterestHists { get; set; }

    public virtual DbSet<PimsLandActType> PimsLandActTypes { get; set; }

    public virtual DbSet<PimsLandSurveyorType> PimsLandSurveyorTypes { get; set; }

    public virtual DbSet<PimsLease> PimsLeases { get; set; }

    public virtual DbSet<PimsLeaseChecklistItem> PimsLeaseChecklistItems { get; set; }

    public virtual DbSet<PimsLeaseChecklistItemHist> PimsLeaseChecklistItemHists { get; set; }

    public virtual DbSet<PimsLeaseChklstItemType> PimsLeaseChklstItemTypes { get; set; }

    public virtual DbSet<PimsLeaseChklstSectionType> PimsLeaseChklstSectionTypes { get; set; }

    public virtual DbSet<PimsLeaseConsultation> PimsLeaseConsultations { get; set; }

    public virtual DbSet<PimsLeaseConsultationHist> PimsLeaseConsultationHists { get; set; }

    public virtual DbSet<PimsLeaseDocument> PimsLeaseDocuments { get; set; }

    public virtual DbSet<PimsLeaseDocumentHist> PimsLeaseDocumentHists { get; set; }

    public virtual DbSet<PimsLeaseHist> PimsLeaseHists { get; set; }

    public virtual DbSet<PimsLeaseInitiatorType> PimsLeaseInitiatorTypes { get; set; }

    public virtual DbSet<PimsLeaseLeasePurpose> PimsLeaseLeasePurposes { get; set; }

    public virtual DbSet<PimsLeaseLeasePurposeHist> PimsLeaseLeasePurposeHists { get; set; }

    public virtual DbSet<PimsLeaseLicenseType> PimsLeaseLicenseTypes { get; set; }

    public virtual DbSet<PimsLeaseNote> PimsLeaseNotes { get; set; }

    public virtual DbSet<PimsLeaseNoteHist> PimsLeaseNoteHists { get; set; }

    public virtual DbSet<PimsLeasePayRvblType> PimsLeasePayRvblTypes { get; set; }

    public virtual DbSet<PimsLeasePayment> PimsLeasePayments { get; set; }

    public virtual DbSet<PimsLeasePaymentCategoryType> PimsLeasePaymentCategoryTypes { get; set; }

    public virtual DbSet<PimsLeasePaymentHist> PimsLeasePaymentHists { get; set; }

    public virtual DbSet<PimsLeasePaymentMethodType> PimsLeasePaymentMethodTypes { get; set; }

    public virtual DbSet<PimsLeasePaymentStatusType> PimsLeasePaymentStatusTypes { get; set; }

    public virtual DbSet<PimsLeasePeriod> PimsLeasePeriods { get; set; }

    public virtual DbSet<PimsLeasePeriodHist> PimsLeasePeriodHists { get; set; }

    public virtual DbSet<PimsLeasePeriodStatusType> PimsLeasePeriodStatusTypes { get; set; }

    public virtual DbSet<PimsLeasePmtFreqType> PimsLeasePmtFreqTypes { get; set; }

    public virtual DbSet<PimsLeaseProgramType> PimsLeaseProgramTypes { get; set; }

    public virtual DbSet<PimsLeasePurposeType> PimsLeasePurposeTypes { get; set; }

    public virtual DbSet<PimsLeaseRenewal> PimsLeaseRenewals { get; set; }

    public virtual DbSet<PimsLeaseRenewalHist> PimsLeaseRenewalHists { get; set; }

    public virtual DbSet<PimsLeaseResponsibilityType> PimsLeaseResponsibilityTypes { get; set; }

    public virtual DbSet<PimsLeaseStakeholder> PimsLeaseStakeholders { get; set; }

    public virtual DbSet<PimsLeaseStakeholderCompReq> PimsLeaseStakeholderCompReqs { get; set; }

    public virtual DbSet<PimsLeaseStakeholderCompReqHist> PimsLeaseStakeholderCompReqHists { get; set; }

    public virtual DbSet<PimsLeaseStakeholderHist> PimsLeaseStakeholderHists { get; set; }

    public virtual DbSet<PimsLeaseStakeholderType> PimsLeaseStakeholderTypes { get; set; }

    public virtual DbSet<PimsLeaseStatusType> PimsLeaseStatusTypes { get; set; }

    public virtual DbSet<PimsLessorType> PimsLessorTypes { get; set; }

    public virtual DbSet<PimsLetterType> PimsLetterTypes { get; set; }

    public virtual DbSet<PimsNote> PimsNotes { get; set; }

    public virtual DbSet<PimsNoteHist> PimsNoteHists { get; set; }

    public virtual DbSet<PimsOrgIdentifierType> PimsOrgIdentifierTypes { get; set; }

    public virtual DbSet<PimsOrganization> PimsOrganizations { get; set; }

    public virtual DbSet<PimsOrganizationAddress> PimsOrganizationAddresses { get; set; }

    public virtual DbSet<PimsOrganizationAddressHist> PimsOrganizationAddressHists { get; set; }

    public virtual DbSet<PimsOrganizationHist> PimsOrganizationHists { get; set; }

    public virtual DbSet<PimsOrganizationType> PimsOrganizationTypes { get; set; }

    public virtual DbSet<PimsPaymentItemType> PimsPaymentItemTypes { get; set; }

    public virtual DbSet<PimsPerson> PimsPeople { get; set; }

    public virtual DbSet<PimsPersonAddress> PimsPersonAddresses { get; set; }

    public virtual DbSet<PimsPersonAddressHist> PimsPersonAddressHists { get; set; }

    public virtual DbSet<PimsPersonContactVw> PimsPersonContactVws { get; set; }

    public virtual DbSet<PimsPersonHist> PimsPersonHists { get; set; }

    public virtual DbSet<PimsPersonOrganization> PimsPersonOrganizations { get; set; }

    public virtual DbSet<PimsPersonOrganizationHist> PimsPersonOrganizationHists { get; set; }

    public virtual DbSet<PimsPphStatusType> PimsPphStatusTypes { get; set; }

    public virtual DbSet<PimsPrfPropResearchPurposeType> PimsPrfPropResearchPurposeTypes { get; set; }

    public virtual DbSet<PimsProduct> PimsProducts { get; set; }

    public virtual DbSet<PimsProductHist> PimsProductHists { get; set; }

    public virtual DbSet<PimsProject> PimsProjects { get; set; }

    public virtual DbSet<PimsProjectDocument> PimsProjectDocuments { get; set; }

    public virtual DbSet<PimsProjectDocumentHist> PimsProjectDocumentHists { get; set; }

    public virtual DbSet<PimsProjectHist> PimsProjectHists { get; set; }

    public virtual DbSet<PimsProjectNote> PimsProjectNotes { get; set; }

    public virtual DbSet<PimsProjectNoteHist> PimsProjectNoteHists { get; set; }

    public virtual DbSet<PimsProjectPerson> PimsProjectPeople { get; set; }

    public virtual DbSet<PimsProjectPersonHist> PimsProjectPersonHists { get; set; }

    public virtual DbSet<PimsProjectPersonRoleType> PimsProjectPersonRoleTypes { get; set; }

    public virtual DbSet<PimsProjectProduct> PimsProjectProducts { get; set; }

    public virtual DbSet<PimsProjectProductHist> PimsProjectProductHists { get; set; }

    public virtual DbSet<PimsProjectStatusType> PimsProjectStatusTypes { get; set; }

    public virtual DbSet<PimsPropAcqFlCompReq> PimsPropAcqFlCompReqs { get; set; }

    public virtual DbSet<PimsPropAcqFlCompReqHist> PimsPropAcqFlCompReqHists { get; set; }

    public virtual DbSet<PimsPropActInvolvedParty> PimsPropActInvolvedParties { get; set; }

    public virtual DbSet<PimsPropActInvolvedPartyHist> PimsPropActInvolvedPartyHists { get; set; }

    public virtual DbSet<PimsPropActMinContact> PimsPropActMinContacts { get; set; }

    public virtual DbSet<PimsPropActMinContactHist> PimsPropActMinContactHists { get; set; }

    public virtual DbSet<PimsPropInthldrInterestType> PimsPropInthldrInterestTypes { get; set; }

    public virtual DbSet<PimsPropLeaseCompReq> PimsPropLeaseCompReqs { get; set; }

    public virtual DbSet<PimsPropLeaseCompReqHist> PimsPropLeaseCompReqHists { get; set; }

    public virtual DbSet<PimsPropMgmtActivityStatusType> PimsPropMgmtActivityStatusTypes { get; set; }

    public virtual DbSet<PimsPropMgmtActivitySubtype> PimsPropMgmtActivitySubtypes { get; set; }

    public virtual DbSet<PimsPropMgmtActivityType> PimsPropMgmtActivityTypes { get; set; }

    public virtual DbSet<PimsPropPropActivity> PimsPropPropActivities { get; set; }

    public virtual DbSet<PimsPropPropActivityHist> PimsPropPropActivityHists { get; set; }

    public virtual DbSet<PimsPropPropAnomalyType> PimsPropPropAnomalyTypes { get; set; }

    public virtual DbSet<PimsPropPropPurpose> PimsPropPropPurposes { get; set; }

    public virtual DbSet<PimsPropPropPurposeHist> PimsPropPropPurposeHists { get; set; }

    public virtual DbSet<PimsPropPropRoadType> PimsPropPropRoadTypes { get; set; }

    public virtual DbSet<PimsPropPropTenureType> PimsPropPropTenureTypes { get; set; }

    public virtual DbSet<PimsPropResearchPurposeType> PimsPropResearchPurposeTypes { get; set; }

    public virtual DbSet<PimsProperty> PimsProperties { get; set; }

    public virtual DbSet<PimsPropertyAcquisitionFile> PimsPropertyAcquisitionFiles { get; set; }

    public virtual DbSet<PimsPropertyAcquisitionFileHist> PimsPropertyAcquisitionFileHists { get; set; }

    public virtual DbSet<PimsPropertyActivity> PimsPropertyActivities { get; set; }

    public virtual DbSet<PimsPropertyActivityDocument> PimsPropertyActivityDocuments { get; set; }

    public virtual DbSet<PimsPropertyActivityDocumentHist> PimsPropertyActivityDocumentHists { get; set; }

    public virtual DbSet<PimsPropertyActivityHist> PimsPropertyActivityHists { get; set; }

    public virtual DbSet<PimsPropertyActivityInvoice> PimsPropertyActivityInvoices { get; set; }

    public virtual DbSet<PimsPropertyActivityInvoiceHist> PimsPropertyActivityInvoiceHists { get; set; }

    public virtual DbSet<PimsPropertyAnomalyType> PimsPropertyAnomalyTypes { get; set; }

    public virtual DbSet<PimsPropertyBoundaryLiteVw> PimsPropertyBoundaryLiteVws { get; set; }

    public virtual DbSet<PimsPropertyBoundaryVw> PimsPropertyBoundaryVws { get; set; }

    public virtual DbSet<PimsPropertyContact> PimsPropertyContacts { get; set; }

    public virtual DbSet<PimsPropertyContactHist> PimsPropertyContactHists { get; set; }

    public virtual DbSet<PimsPropertyHist> PimsPropertyHists { get; set; }

    public virtual DbSet<PimsPropertyImprovement> PimsPropertyImprovements { get; set; }

    public virtual DbSet<PimsPropertyImprovementHist> PimsPropertyImprovementHists { get; set; }

    public virtual DbSet<PimsPropertyImprovementType> PimsPropertyImprovementTypes { get; set; }

    public virtual DbSet<PimsPropertyLease> PimsPropertyLeases { get; set; }

    public virtual DbSet<PimsPropertyLeaseHist> PimsPropertyLeaseHists { get; set; }

    public virtual DbSet<PimsPropertyLocationLiteVw> PimsPropertyLocationLiteVws { get; set; }

    public virtual DbSet<PimsPropertyLocationVw> PimsPropertyLocationVws { get; set; }

    public virtual DbSet<PimsPropertyOperation> PimsPropertyOperations { get; set; }

    public virtual DbSet<PimsPropertyOperationHist> PimsPropertyOperationHists { get; set; }

    public virtual DbSet<PimsPropertyOperationType> PimsPropertyOperationTypes { get; set; }

    public virtual DbSet<PimsPropertyOrganization> PimsPropertyOrganizations { get; set; }

    public virtual DbSet<PimsPropertyOrganizationHist> PimsPropertyOrganizationHists { get; set; }

    public virtual DbSet<PimsPropertyPurposeType> PimsPropertyPurposeTypes { get; set; }

    public virtual DbSet<PimsPropertyResearchFile> PimsPropertyResearchFiles { get; set; }

    public virtual DbSet<PimsPropertyResearchFileHist> PimsPropertyResearchFileHists { get; set; }

    public virtual DbSet<PimsPropertyRoadType> PimsPropertyRoadTypes { get; set; }

    public virtual DbSet<PimsPropertyStatusType> PimsPropertyStatusTypes { get; set; }

    public virtual DbSet<PimsPropertyTenureType> PimsPropertyTenureTypes { get; set; }

    public virtual DbSet<PimsPropertyType> PimsPropertyTypes { get; set; }

    public virtual DbSet<PimsPropertyVw> PimsPropertyVws { get; set; }

    public virtual DbSet<PimsProvinceState> PimsProvinceStates { get; set; }

    public virtual DbSet<PimsRegion> PimsRegions { get; set; }

    public virtual DbSet<PimsRegionUser> PimsRegionUsers { get; set; }

    public virtual DbSet<PimsRegionUserHist> PimsRegionUserHists { get; set; }

    public virtual DbSet<PimsRequestSourceType> PimsRequestSourceTypes { get; set; }

    public virtual DbSet<PimsResearchFile> PimsResearchFiles { get; set; }

    public virtual DbSet<PimsResearchFileDocument> PimsResearchFileDocuments { get; set; }

    public virtual DbSet<PimsResearchFileDocumentHist> PimsResearchFileDocumentHists { get; set; }

    public virtual DbSet<PimsResearchFileHist> PimsResearchFileHists { get; set; }

    public virtual DbSet<PimsResearchFileNote> PimsResearchFileNotes { get; set; }

    public virtual DbSet<PimsResearchFileNoteHist> PimsResearchFileNoteHists { get; set; }

    public virtual DbSet<PimsResearchFileProject> PimsResearchFileProjects { get; set; }

    public virtual DbSet<PimsResearchFileProjectHist> PimsResearchFileProjectHists { get; set; }

    public virtual DbSet<PimsResearchFilePurpose> PimsResearchFilePurposes { get; set; }

    public virtual DbSet<PimsResearchFilePurposeHist> PimsResearchFilePurposeHists { get; set; }

    public virtual DbSet<PimsResearchFileStatusType> PimsResearchFileStatusTypes { get; set; }

    public virtual DbSet<PimsResearchPurposeType> PimsResearchPurposeTypes { get; set; }

    public virtual DbSet<PimsResponsibilityCode> PimsResponsibilityCodes { get; set; }

    public virtual DbSet<PimsResponsibilityCodeHist> PimsResponsibilityCodeHists { get; set; }

    public virtual DbSet<PimsRole> PimsRoles { get; set; }

    public virtual DbSet<PimsRoleClaim> PimsRoleClaims { get; set; }

    public virtual DbSet<PimsRoleClaimHist> PimsRoleClaimHists { get; set; }

    public virtual DbSet<PimsRoleHist> PimsRoleHists { get; set; }

    public virtual DbSet<PimsSecurityDeposit> PimsSecurityDeposits { get; set; }

    public virtual DbSet<PimsSecurityDepositHist> PimsSecurityDepositHists { get; set; }

    public virtual DbSet<PimsSecurityDepositHolder> PimsSecurityDepositHolders { get; set; }

    public virtual DbSet<PimsSecurityDepositHolderHist> PimsSecurityDepositHolderHists { get; set; }

    public virtual DbSet<PimsSecurityDepositReturn> PimsSecurityDepositReturns { get; set; }

    public virtual DbSet<PimsSecurityDepositReturnHist> PimsSecurityDepositReturnHists { get; set; }

    public virtual DbSet<PimsSecurityDepositReturnHolder> PimsSecurityDepositReturnHolders { get; set; }

    public virtual DbSet<PimsSecurityDepositReturnHolderHist> PimsSecurityDepositReturnHolderHists { get; set; }

    public virtual DbSet<PimsSecurityDepositType> PimsSecurityDepositTypes { get; set; }

    public virtual DbSet<PimsStaticVariable> PimsStaticVariables { get; set; }

    public virtual DbSet<PimsStaticVariableHist> PimsStaticVariableHists { get; set; }

    public virtual DbSet<PimsSurplusDeclarationType> PimsSurplusDeclarationTypes { get; set; }

    public virtual DbSet<PimsSurveyPlanType> PimsSurveyPlanTypes { get; set; }

    public virtual DbSet<PimsTake> PimsTakes { get; set; }

    public virtual DbSet<PimsTakeHist> PimsTakeHists { get; set; }

    public virtual DbSet<PimsTakeSiteContamType> PimsTakeSiteContamTypes { get; set; }

    public virtual DbSet<PimsTakeStatusType> PimsTakeStatusTypes { get; set; }

    public virtual DbSet<PimsTakeType> PimsTakeTypes { get; set; }

    public virtual DbSet<PimsTenant> PimsTenants { get; set; }

    public virtual DbSet<PimsUser> PimsUsers { get; set; }

    public virtual DbSet<PimsUserHist> PimsUserHists { get; set; }

    public virtual DbSet<PimsUserOrganization> PimsUserOrganizations { get; set; }

    public virtual DbSet<PimsUserOrganizationHist> PimsUserOrganizationHists { get; set; }

    public virtual DbSet<PimsUserRole> PimsUserRoles { get; set; }

    public virtual DbSet<PimsUserRoleHist> PimsUserRoleHists { get; set; }

    public virtual DbSet<PimsUserType> PimsUserTypes { get; set; }

    public virtual DbSet<PimsVolumeUnitType> PimsVolumeUnitTypes { get; set; }

    public virtual DbSet<PimsVolumetricType> PimsVolumetricTypes { get; set; }

    public virtual DbSet<PimsWorkActivityCode> PimsWorkActivityCodes { get; set; }

    public virtual DbSet<PimsWorkActivityCodeHist> PimsWorkActivityCodeHists { get; set; }

    public virtual DbSet<PimsYearlyFinancialCode> PimsYearlyFinancialCodes { get; set; }

    public virtual DbSet<PimsYearlyFinancialCodeHist> PimsYearlyFinancialCodeHists { get; set; }

    public virtual DbSet<PimsxTableDefinition> PimsxTableDefinitions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PimsAccessRequest>(entity =>
        {
            entity.HasKey(e => e.AccessRequestId).HasName("ACRQST_PK");

            entity.ToTable("PIMS_ACCESS_REQUEST", tb =>
                {
                    tb.HasTrigger("PIMS_ACRQST_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ACRQST_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACRQST_I_S_U_TR");
                });

            entity.Property(e => e.AccessRequestId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACCESS_REQUEST_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Note).HasComment("Note associated with this access request.");
            entity.Property(e => e.RegionCode).HasDefaultValue((short)4);

            entity.HasOne(d => d.AccessRequestStatusTypeCodeNavigation).WithMany(p => p.PimsAccessRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ARQSTT_PIM_ACRQST_FK");

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.PimsAccessRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_REGION_PIM_ACRQST_FK");

            entity.HasOne(d => d.Role).WithMany(p => p.PimsAccessRequests).HasConstraintName("PIM_ROLE_PIM_ACRQST_FK");

            entity.HasOne(d => d.User).WithMany(p => p.PimsAccessRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_USER_PIM_ACRQST_FK");

            entity.HasOne(d => d.UserTypeCodeNavigation).WithMany(p => p.PimsAccessRequests).HasConstraintName("PIM_USERTY_PIM_ACRQST_FK");
        });

        modelBuilder.Entity<PimsAccessRequestHist>(entity =>
        {
            entity.HasKey(e => e.AccessRequestHistId).HasName("PIMS_ACRQST_H_PK");

            entity.Property(e => e.AccessRequestHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACCESS_REQUEST_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsAccessRequestOrganization>(entity =>
        {
            entity.HasKey(e => e.AccessRequestOrganizationId).HasName("ACRQOR_PK");

            entity.ToTable("PIMS_ACCESS_REQUEST_ORGANIZATION", tb =>
                {
                    tb.HasTrigger("PIMS_ACRQOR_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ACRQOR_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACRQOR_I_S_U_TR");
                });

            entity.Property(e => e.AccessRequestOrganizationId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACCESS_REQUEST_ORGANIZATION_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.IsDisabled).HasDefaultValue(false);

            entity.HasOne(d => d.AccessRequest).WithMany(p => p.PimsAccessRequestOrganizations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ACRQST_PIM_ACRQOR_FK");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsAccessRequestOrganizations).HasConstraintName("PIM_ORG_PIM_ACRQOR_FK");
        });

        modelBuilder.Entity<PimsAccessRequestOrganizationHist>(entity =>
        {
            entity.HasKey(e => e.AccessRequestOrganizationHistId).HasName("PIMS_ACRQOR_H_PK");

            entity.Property(e => e.AccessRequestOrganizationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACCESS_REQUEST_ORGANIZATION_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsAccessRequestStatusType>(entity =>
        {
            entity.HasKey(e => e.AccessRequestStatusTypeCode).HasName("ARQSTT_PK");

            entity.ToTable("PIMS_ACCESS_REQUEST_STATUS_TYPE", tb =>
                {
                    tb.HasTrigger("PIMS_ARQSTT_I_S_I_TR");
                    tb.HasTrigger("PIMS_ARQSTT_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsAcqChklstItemType>(entity =>
        {
            entity.HasKey(e => e.AcqChklstItemTypeCode).HasName("ACQCIT_PK");

            entity.ToTable("PIMS_ACQ_CHKLST_ITEM_TYPE", tb =>
                {
                    tb.HasComment("Table that contains the checklist items that are presented to the user through dynamically building the input form.");
                    tb.HasTrigger("PIMS_ACQCIT_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACQCIT_I_S_U_TR");
                });

            entity.Property(e => e.AcqChklstItemTypeCode).HasComment("Checklist item code value.");
            entity.Property(e => e.AcqChklstSectionTypeCode).HasComment("Section to which the item belongs.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Checklist item descriptive text presented to the user.");
            entity.Property(e => e.DisplayOrder).HasComment("Specifies the order that the checklist items are presented to the user.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the checklist item is able to be presented to the user via the input form.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the checklist item is removed from the input form.");
            entity.Property(e => e.Hint).HasComment("Checklist item descriptive tooltip presented to the user.");
            entity.Property(e => e.IsRequired)
                .HasDefaultValue(false)
                .HasComment("Indicates if the checklist item is a required field.");

            entity.HasOne(d => d.AcqChklstSectionTypeCodeNavigation).WithMany(p => p.PimsAcqChklstItemTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_AQCSCT_PIM_ACQCIT_FK");
        });

        modelBuilder.Entity<PimsAcqChklstSectionType>(entity =>
        {
            entity.HasKey(e => e.AcqChklstSectionTypeCode).HasName("AQCSCT_PK");

            entity.ToTable("PIMS_ACQ_CHKLST_SECTION_TYPE", tb =>
                {
                    tb.HasComment("Table that contains the checklist sctions that are presented to the user through dynamically building the input form.");
                    tb.HasTrigger("PIMS_AQCSCT_I_S_I_TR");
                    tb.HasTrigger("PIMS_AQCSCT_I_S_U_TR");
                });

            entity.Property(e => e.AcqChklstSectionTypeCode).HasComment("Checklist section code value.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Checklist section descriptive text presented to the user.");
            entity.Property(e => e.DisplayOrder).HasComment("Specifies the order that the checklist sections are presented to the user.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the checklist section is able to be presented to the user via the input form.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the checklist section is removed from the input form.");
        });

        modelBuilder.Entity<PimsAcqFlTeamProfileType>(entity =>
        {
            entity.HasKey(e => e.AcqFlTeamProfileTypeCode).HasName("AQTPPT_PK");

            entity.ToTable("PIMS_ACQ_FL_TEAM_PROFILE_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the acquistion file staff profile (role).");
                    tb.HasTrigger("PIMS_AQTPPT_I_S_I_TR");
                    tb.HasTrigger("PIMS_AQTPPT_I_S_U_TR");
                });

            entity.Property(e => e.AcqFlTeamProfileTypeCode).HasComment("Code value for the acquistion file staff/org profile (role).");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the acquistion file staff/org profile (role).");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsAcqPhysFileStatusType>(entity =>
        {
            entity.HasKey(e => e.AcqPhysFileStatusTypeCode).HasName("ACQPFS_PK");

            entity.ToTable("PIMS_ACQ_PHYS_FILE_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the acquistion physical file status type.");
                    tb.HasTrigger("PIMS_ACQPFS_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACQPFS_I_S_U_TR");
                });

            entity.Property(e => e.AcqPhysFileStatusTypeCode).HasComment("Code value for the acquistion physical file status type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the acquistion physical file status type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsAcquisitionChecklistItem>(entity =>
        {
            entity.HasKey(e => e.AcquisitionChecklistItemId).HasName("ACQCKI_PK");

            entity.ToTable("PIMS_ACQUISITION_CHECKLIST_ITEM", tb =>
                {
                    tb.HasTrigger("PIMS_ACQCKI_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ACQCKI_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACQCKI_I_S_U_TR");
                });

            entity.Property(e => e.AcquisitionChecklistItemId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_CHECKLIST_ITEM_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ChklstItemStatusTypeCode).HasComment("Foreign key to the PIMS_CHKLST_ITEM_STATUS_TYPE table.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.AcqChklstItemTypeCodeNavigation).WithMany(p => p.PimsAcquisitionChecklistItems).HasConstraintName("PIM_ACQCIT_PIM_ACQCKI_FK");

            entity.HasOne(d => d.AcquisitionFile).WithMany(p => p.PimsAcquisitionChecklistItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ACQNFL_PIM_ACQCKI_FK");

            entity.HasOne(d => d.ChklstItemStatusTypeCodeNavigation).WithMany(p => p.PimsAcquisitionChecklistItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_CHKLIS_PIM_ACQCKI_FK");
        });

        modelBuilder.Entity<PimsAcquisitionChecklistItemHist>(entity =>
        {
            entity.HasKey(e => e.AcquisitionChecklistItemHistId).HasName("PIMS_ACQCKI_H_PK");

            entity.Property(e => e.AcquisitionChecklistItemHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_CHECKLIST_ITEM_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsAcquisitionFile>(entity =>
        {
            entity.HasKey(e => e.AcquisitionFileId).HasName("ACQNFL_PK");

            entity.ToTable("PIMS_ACQUISITION_FILE", tb =>
                {
                    tb.HasComment("Entity containing information regarding an acquisition file.");
                    tb.HasTrigger("PIMS_ACQNFL_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ACQNFL_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACQNFL_I_S_U_TR");
                });

            entity.Property(e => e.AcquisitionFileId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AssignedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of file assignment.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DeliveryDate).HasComment("Date of file delivery.");
            entity.Property(e => e.FileName).HasComment("Descriptive name given to the acquisition file.");
            entity.Property(e => e.FileNo)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_NO_SEQ])")
                .HasComment("File number assigned to the acquisition file.");
            entity.Property(e => e.FileNumber)
                .HasDefaultValue("<Empty>")
                .HasComment("Formatted file number assigned to the acquisition file.  Format follows YY-XXXXXX-ZZ where YY = MoTI region number, XXXXXX = generated integer sequence number,  and ZZ = file suffix number (defaulting to '01')");
            entity.Property(e => e.FundingOther).HasComment("Description of other funding type.");
            entity.Property(e => e.LegacyFileNumber).HasComment("Legacy formatted file number assigned to the acquisition file.  Format follows YY-XXXXXX-ZZ where YY = MoTI region number, XXXXXX = generated integer sequence number,  and ZZ = file suffix number (defaulting to '01').   Required due to some files having t");
            entity.Property(e => e.LegacyStakeholder).HasComment("Legacy stakeholders imported from PAIMS.");
            entity.Property(e => e.PaimsAcquisitionFileId).HasComment("Legacy Acquisition File ID from the PAIMS system.");
            entity.Property(e => e.RegionCode)
                .HasDefaultValue((short)-1)
                .HasComment("Region responsible for oversight of the acquisition.");
            entity.Property(e => e.TotalAllowableCompensation).HasComment("The maximum allowable compensation for the acquisition file.  This amount should not be exceeded by the total of all assiciated H120's.");

            entity.HasOne(d => d.AcqPhysFileStatusTypeCodeNavigation).WithMany(p => p.PimsAcquisitionFiles).HasConstraintName("PIM_ACQPFS_PIM_ACQNFL_FK");

            entity.HasOne(d => d.AcquisitionFileStatusTypeCodeNavigation).WithMany(p => p.PimsAcquisitionFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ACQFST_PIM_ACQNFL_FK");

            entity.HasOne(d => d.AcquisitionFundingTypeCodeNavigation).WithMany(p => p.PimsAcquisitionFiles).HasConstraintName("PIM_ACQFTY_PIM_ACQNFL_FK");

            entity.HasOne(d => d.AcquisitionTypeCodeNavigation).WithMany(p => p.PimsAcquisitionFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ACQTYP_PIM_ACQNFL_FK");

            entity.HasOne(d => d.Product).WithMany(p => p.PimsAcquisitionFiles).HasConstraintName("PIM_PRODCT_PIM_ACQNFL_FK");

            entity.HasOne(d => d.Project).WithMany(p => p.PimsAcquisitionFiles).HasConstraintName("PIM_PROJCT_PIM_ACQNFL_FK");

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.PimsAcquisitionFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_REGION_PIM_ACQNFL_FK");
        });

        modelBuilder.Entity<PimsAcquisitionFileDocument>(entity =>
        {
            entity.HasKey(e => e.AcquisitionFileDocumentId).HasName("ACQDOC_PK");

            entity.ToTable("PIMS_ACQUISITION_FILE_DOCUMENT", tb =>
                {
                    tb.HasComment("Defines the relationship between an acquisition file and a document.");
                    tb.HasTrigger("PIMS_ACQDOC_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ACQDOC_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACQDOC_I_S_U_TR");
                });

            entity.Property(e => e.AcquisitionFileDocumentId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_DOCUMENT_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.AcquisitionFile).WithMany(p => p.PimsAcquisitionFileDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ACQNFL_PIM_ACQDOC_FK");

            entity.HasOne(d => d.Document).WithMany(p => p.PimsAcquisitionFileDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DOCMNT_PIM_ACQDOC_FK");
        });

        modelBuilder.Entity<PimsAcquisitionFileDocumentHist>(entity =>
        {
            entity.HasKey(e => e.AcquisitionFileDocumentHistId).HasName("PIMS_ACQDOC_H_PK");

            entity.Property(e => e.AcquisitionFileDocumentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_DOCUMENT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsAcquisitionFileForm>(entity =>
        {
            entity.HasKey(e => e.AcquisitionFileFormId).HasName("ACQFRM_PK");

            entity.ToTable("PIMS_ACQUISITION_FILE_FORM", tb =>
                {
                    tb.HasComment("Entity associating a form to an acquisition file.  The acquisition file can have multiple forms.");
                    tb.HasTrigger("PIMS_ACQFRM_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ACQFRM_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACQFRM_I_S_U_TR");
                });

            entity.Property(e => e.AcquisitionFileFormId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_FORM_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.AcquisitionFile).WithMany(p => p.PimsAcquisitionFileForms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ACQNFL_PIM_ACQFRM_FK");

            entity.HasOne(d => d.FormTypeCodeNavigation).WithMany(p => p.PimsAcquisitionFileForms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_FRMTYP_PIM_ACQFRM_FK");
        });

        modelBuilder.Entity<PimsAcquisitionFileFormHist>(entity =>
        {
            entity.HasKey(e => e.AcquisitionFileFormHistId).HasName("PIMS_ACQFRM_H_PK");

            entity.Property(e => e.AcquisitionFileFormHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_FORM_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsAcquisitionFileHist>(entity =>
        {
            entity.HasKey(e => e.AcquisitionFileHistId).HasName("PIMS_ACQNFL_H_PK");

            entity.Property(e => e.AcquisitionFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsAcquisitionFileNote>(entity =>
        {
            entity.HasKey(e => e.AcquisitionFileNoteId).HasName("ACQNOT_PK");

            entity.ToTable("PIMS_ACQUISITION_FILE_NOTE", tb =>
                {
                    tb.HasComment("Defines the relationship between an acquisition file and a note.");
                    tb.HasTrigger("PIMS_ACQNOT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ACQNOT_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACQNOT_I_S_U_TR");
                });

            entity.Property(e => e.AcquisitionFileNoteId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_NOTE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.AcquisitionFile).WithMany(p => p.PimsAcquisitionFileNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ACQNFL_PIM_ACQNOT_FK");

            entity.HasOne(d => d.Note).WithOne(p => p.PimsAcquisitionFileNote)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_NOTE_PIM_ACQNOT_FK");
        });

        modelBuilder.Entity<PimsAcquisitionFileNoteHist>(entity =>
        {
            entity.HasKey(e => e.AcquisitionFileNoteHistId).HasName("PIMS_ACQNOT_H_PK");

            entity.Property(e => e.AcquisitionFileNoteHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_NOTE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsAcquisitionFileStatusType>(entity =>
        {
            entity.HasKey(e => e.AcquisitionFileStatusTypeCode).HasName("ACQFST_PK");

            entity.ToTable("PIMS_ACQUISITION_FILE_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the acquistion file status.");
                    tb.HasTrigger("PIMS_ACQFST_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACQFST_I_S_U_TR");
                });

            entity.Property(e => e.AcquisitionFileStatusTypeCode).HasComment("Code value for the acquistion file status.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the acquistion file status.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsAcquisitionFileTeam>(entity =>
        {
            entity.HasKey(e => e.AcquisitionFileTeamId).HasName("ACQNTM_PK");

            entity.ToTable("PIMS_ACQUISITION_FILE_TEAM", tb =>
                {
                    tb.HasComment("Table to associate an acquisition file to a person.");
                    tb.HasTrigger("PIMS_ACQNTM_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ACQNTM_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACQNTM_I_S_U_TR");
                });

            entity.Property(e => e.AcquisitionFileTeamId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_TEAM_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.OrganizationId).HasComment("Foreign key to the team member's organization");
            entity.Property(e => e.PersonId).HasComment("Foreign key to the team member");
            entity.Property(e => e.PrimaryContactId).HasComment("Primary contact for the organization");

            entity.HasOne(d => d.AcqFlTeamProfileTypeCodeNavigation).WithMany(p => p.PimsAcquisitionFileTeams).HasConstraintName("PIM_AQFPPT_PIM_ACQPER_FK");

            entity.HasOne(d => d.AcquisitionFile).WithMany(p => p.PimsAcquisitionFileTeams).HasConstraintName("PIM_ACQNFL_PIM_ACQPER_FK");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsAcquisitionFileTeams).HasConstraintName("PIM_ORG_PIM_ACQNTM_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsAcquisitionFileTeamPeople).HasConstraintName("PIM_PERSON_PIM_ACQPER_FK");

            entity.HasOne(d => d.PrimaryContact).WithMany(p => p.PimsAcquisitionFileTeamPrimaryContacts).HasConstraintName("PIM_PERSON_PIM_ACQNTM_FK");
        });

        modelBuilder.Entity<PimsAcquisitionFileTeamHist>(entity =>
        {
            entity.HasKey(e => e.AcquisitionFileTeamHistId).HasName("PIMS_ACQNTM_H_PK");

            entity.Property(e => e.AcquisitionFileTeamHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_TEAM_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsAcquisitionFundingType>(entity =>
        {
            entity.HasKey(e => e.AcquisitionFundingTypeCode).HasName("ACQFTY_PK");

            entity.ToTable("PIMS_ACQUISITION_FUNDING_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the acquistion funding type.");
                    tb.HasTrigger("PIMS_ACQFTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACQFTY_I_S_U_TR");
                });

            entity.Property(e => e.AcquisitionFundingTypeCode).HasComment("Code value for the acquistion funding type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the acquistion funding type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsAcquisitionOwner>(entity =>
        {
            entity.HasKey(e => e.AcquisitionOwnerId).HasName("ACQOWN_PK");

            entity.ToTable("PIMS_ACQUISITION_OWNER", tb =>
                {
                    tb.HasComment("Entity containing information regarding the owner of an acquisition file.");
                    tb.HasTrigger("PIMS_ACQOWN_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ACQOWN_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACQOWN_I_S_U_TR");
                });

            entity.Property(e => e.AcquisitionOwnerId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_OWNER_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.ContactEmailAddr).HasComment("Email address to be used for contacting the owner.");
            entity.Property(e => e.ContactPhoneNum).HasComment("Phone number to be used for contacting the owner.");
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the owner record became effective. Defaults to current date/time.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the owner record expired.");
            entity.Property(e => e.GivenName).HasComment("Given name of the owner (person).");
            entity.Property(e => e.IncorporationNumber).HasComment("Incorporation number of the organization.");
            entity.Property(e => e.IsOrganization).HasComment("Indicates if the owner is an organization.  Default value is FALSE, indicating that the owner is a person.");
            entity.Property(e => e.IsPrimaryOwner).HasComment("Indicates that this is the file's primary owner.");
            entity.Property(e => e.LastNameAndCorpName).HasComment("Name of the owner (person or organization).  If person, surname.");
            entity.Property(e => e.OtherName).HasComment("Optional name field if required.");
            entity.Property(e => e.RegistrationNumber).HasComment("Registration number of the organization.");

            entity.HasOne(d => d.AcquisitionFile).WithMany(p => p.PimsAcquisitionOwners)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("PIM_ACQNFL_PIM_ACQOWN_FK");

            entity.HasOne(d => d.Address).WithMany(p => p.PimsAcquisitionOwners).HasConstraintName("PIM_ADDRSS_PIM_ACQOWN_FK");
        });

        modelBuilder.Entity<PimsAcquisitionOwnerHist>(entity =>
        {
            entity.HasKey(e => e.AcquisitionOwnerHistId).HasName("PIMS_ACQOWN_H_PK");

            entity.Property(e => e.AcquisitionOwnerHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_OWNER_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsAcquisitionType>(entity =>
        {
            entity.HasKey(e => e.AcquisitionTypeCode).HasName("ACQTYP_PK");

            entity.ToTable("PIMS_ACQUISITION_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the acquistion type.");
                    tb.HasTrigger("PIMS_ACQTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_ACQTYP_I_S_U_TR");
                });

            entity.Property(e => e.AcquisitionTypeCode).HasComment("Code value for the acquistion type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the acquistion type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("ADDRSS_PK");

            entity.ToTable("PIMS_ADDRESS", tb =>
                {
                    tb.HasTrigger("PIMS_ADDRSS_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ADDRSS_I_S_I_TR");
                    tb.HasTrigger("PIMS_ADDRSS_I_S_U_TR");
                });

            entity.Property(e => e.AddressId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ADDRESS_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.OtherCountry).HasComment("Other country not listed in drop-down list");

            entity.HasOne(d => d.Country).WithMany(p => p.PimsAddresses).HasConstraintName("PIM_CNTRY_PIM_ADDRSS_FK");

            entity.HasOne(d => d.DistrictCodeNavigation).WithMany(p => p.PimsAddresses).HasConstraintName("PIM_DSTRCT_PIM_ADDRSS_FK");

            entity.HasOne(d => d.ProvinceState).WithMany(p => p.PimsAddresses).HasConstraintName("PIM_PROVNC_PIM_ADDRSS_FK");

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.PimsAddresses).HasConstraintName("PIM_REGION_PIM_ADDRSS_FK");
        });

        modelBuilder.Entity<PimsAddressHist>(entity =>
        {
            entity.HasKey(e => e.AddressHistId).HasName("PIMS_ADDRSS_H_PK");

            entity.Property(e => e.AddressHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ADDRESS_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsAddressUsageType>(entity =>
        {
            entity.HasKey(e => e.AddressUsageTypeCode).HasName("ADUSGT_PK");

            entity.ToTable("PIMS_ADDRESS_USAGE_TYPE", tb =>
                {
                    tb.HasTrigger("PIMS_ADUSGT_I_S_I_TR");
                    tb.HasTrigger("PIMS_ADUSGT_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsAgreement>(entity =>
        {
            entity.HasKey(e => e.AgreementId).HasName("AGRMNT_PK");

            entity.ToTable("PIMS_AGREEMENT", tb =>
                {
                    tb.HasComment("Table containing the details of the acquisition agreement.");
                    tb.HasTrigger("PIMS_AGRMNT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_AGRMNT_I_S_I_TR");
                    tb.HasTrigger("PIMS_AGRMNT_I_S_U_TR");
                });

            entity.Property(e => e.AgreementId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_AGREEMENT_ID_SEQ])");
            entity.Property(e => e.AgreementDate).HasComment("Date of the agreement.");
            entity.Property(e => e.AgreementStatusTypeCode).HasDefaultValue("DRAFT");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.CancellationNote).HasComment("Note pertaining to the cancellation of the agreement.");
            entity.Property(e => e.CommencementDate).HasComment("Date of commencement of the agreement.");
            entity.Property(e => e.CompletionDate).HasComment("Date of completion of the agreement.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DepositAmount).HasComment("Amount of the deposit on the agreement.");
            entity.Property(e => e.ExpiryTs).HasComment("Expiry date and time of acquisition offer.");
            entity.Property(e => e.ExpropriationDate).HasComment("Date of expropriation of the property.");
            entity.Property(e => e.InspectionDate).HasComment("Date of inspection.");
            entity.Property(e => e.LegalSurveyPlanNum).HasComment("Legal survey plan number,");
            entity.Property(e => e.NoLaterThanDays).HasComment("Deposit due date");
            entity.Property(e => e.OfferDate).HasComment("Date of acquisition offer.");
            entity.Property(e => e.PossessionDate).HasComment("Date of possession of the property.");
            entity.Property(e => e.PurchasePrice).HasComment("Amount of the purchase price of the agreement.");
            entity.Property(e => e.SignedDate).HasComment("Signed date of acquisition offer.");
            entity.Property(e => e.TerminationDate).HasComment("Date of termination of the agreement.");

            entity.HasOne(d => d.AcquisitionFile).WithMany(p => p.PimsAgreements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ACQNFL_PIM_AGRMNT_FK");

            entity.HasOne(d => d.AgreementStatusTypeCodeNavigation).WithMany(p => p.PimsAgreements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_AGRSTY_PIM_AGRMNT_FK");

            entity.HasOne(d => d.AgreementTypeCodeNavigation).WithMany(p => p.PimsAgreements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_AGRTYP_PIM_AGRMNT_FK");
        });

        modelBuilder.Entity<PimsAgreementHist>(entity =>
        {
            entity.HasKey(e => e.AgreementHistId).HasName("PIMS_AGRMNT_H_PK");

            entity.Property(e => e.AgreementHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_AGREEMENT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsAgreementStatusType>(entity =>
        {
            entity.HasKey(e => e.AgreementStatusTypeCode).HasName("AGRSTY_PK");

            entity.ToTable("PIMS_AGREEMENT_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Table that contains the codes and associated descriptions of the agreement types.");
                    tb.HasTrigger("PIMS_AGRSTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_AGRSTY_I_S_U_TR");
                });

            entity.Property(e => e.AgreementStatusTypeCode).HasComment("Codified version of the agreement status.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the agreement status type.");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the codes.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsAgreementType>(entity =>
        {
            entity.HasKey(e => e.AgreementTypeCode).HasName("AGRTYP_PK");

            entity.ToTable("PIMS_AGREEMENT_TYPE", tb =>
                {
                    tb.HasComment("Table that contains the codes and associated descriptions of the agreement types.");
                    tb.HasTrigger("PIMS_AGRTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_AGRTYP_I_S_U_TR");
                });

            entity.Property(e => e.AgreementTypeCode).HasComment("Codified version of the agreement type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the agreement type.");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the codes.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsAreaUnitType>(entity =>
        {
            entity.HasKey(e => e.AreaUnitTypeCode).HasName("ARUNIT_PK");

            entity.ToTable("PIMS_AREA_UNIT_TYPE", tb =>
                {
                    tb.HasComment("The area unit used for measuring Properties.  The units must be in metric: square metres or hectares.");
                    tb.HasTrigger("PIMS_ARUNIT_I_S_I_TR");
                    tb.HasTrigger("PIMS_ARUNIT_I_S_U_TR");
                });

            entity.Property(e => e.AreaUnitTypeCode).HasComment("The area unit used for measuring Properties.  The units must be in metric: square metres or hectares.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Translation of the code value into a description that can be displayed to the user.");
            entity.Property(e => e.DisplayOrder).HasComment("Order in which to display the code values, if required.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is still active or is now disabled.");
        });

        modelBuilder.Entity<PimsBusinessFunctionCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("BIZFCN_PK");

            entity.ToTable("PIMS_BUSINESS_FUNCTION_CODE", tb =>
                {
                    tb.HasComment("Code and description of the business function codes.");
                    tb.HasTrigger("PIMS_BIZFCN_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_BIZFCN_I_S_I_TR");
                    tb.HasTrigger("PIMS_BIZFCN_I_S_U_TR");
                });

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_BUSINESS_FUNCTION_ID_SEQ])")
                .HasComment("System-generated primary key.");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Code)
                .HasDefaultValue("<Empty>")
                .HasComment("Name of the code.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Descriptive value  of a code within the set.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the code became effective.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the code ceased to be in effect.");
        });

        modelBuilder.Entity<PimsBusinessFunctionCodeHist>(entity =>
        {
            entity.HasKey(e => e.BusinessFunctionCodeHistId).HasName("PIMS_BIZFCN_H_PK");

            entity.Property(e => e.BusinessFunctionCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_BUSINESS_FUNCTION_CODE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsChartOfAccountsCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CHRTAC_PK");

            entity.ToTable("PIMS_CHART_OF_ACCOUNTS_CODE", tb =>
                {
                    tb.HasComment("Code and description of the chart of accounts codes.");
                    tb.HasTrigger("PIMS_CHRTAC_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_CHRTAC_I_S_I_TR");
                    tb.HasTrigger("PIMS_CHRTAC_I_S_U_TR");
                });

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CHART_OF_ACCOUNTS_ID_SEQ])")
                .HasComment("System-generated primary key.");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Code)
                .HasDefaultValue("<Empty>")
                .HasComment("Name of the code.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Descriptive value  of a code within the set.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the code became effective.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the code ceased to be in effect.");
        });

        modelBuilder.Entity<PimsChartOfAccountsCodeHist>(entity =>
        {
            entity.HasKey(e => e.ChartOfAccountsCodeHistId).HasName("PIMS_CHRTAC_H_PK");

            entity.Property(e => e.ChartOfAccountsCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CHART_OF_ACCOUNTS_CODE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsChklstItemStatusType>(entity =>
        {
            entity.HasKey(e => e.ChklstItemStatusTypeCode).HasName("CHKLIS_PK");

            entity.ToTable("PIMS_CHKLST_ITEM_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Common table that contains the codes and associated descriptions of the various checklist item status types.");
                    tb.HasTrigger("PIMS_CHKLIS_I_S_I_TR");
                    tb.HasTrigger("PIMS_CHKLIS_I_S_U_TR");
                });

            entity.Property(e => e.ChklstItemStatusTypeCode).HasComment("Codified version of the various checklist item status types.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the various checklist item status type.");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsClaim>(entity =>
        {
            entity.HasKey(e => e.ClaimId).HasName("CLMTYP_PK");

            entity.ToTable("PIMS_CLAIM", tb =>
                {
                    tb.HasTrigger("PIMS_CLMTYP_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_CLMTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_CLMTYP_I_S_U_TR");
                });

            entity.Property(e => e.ClaimId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CLAIM_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsClaimHist>(entity =>
        {
            entity.HasKey(e => e.ClaimHistId).HasName("PIMS_CLMTYP_H_PK");

            entity.Property(e => e.ClaimHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CLAIM_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsCompReqFinancial>(entity =>
        {
            entity.HasKey(e => e.CompReqFinancialId).HasName("CRQFIN_PK");

            entity.ToTable("PIMS_COMP_REQ_FINANCIAL", tb =>
                {
                    tb.HasComment("Table associating compensation requisitions related to work activities.");
                    tb.HasTrigger("PIMS_CRQFIN_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_CRQFIN_I_S_I_TR");
                    tb.HasTrigger("PIMS_CRQFIN_I_S_U_TR");
                });

            entity.Property(e => e.CompReqFinancialId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_COMP_REQ_FINANCIAL_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the requisition is inactive.");
            entity.Property(e => e.IsGstRequired)
                .HasDefaultValue(false)
                .HasComment("Indicates if GST is required for this transaction.");
            entity.Property(e => e.PretaxAmt).HasComment("Subtotal of the requisition's work activity.");
            entity.Property(e => e.TaxAmt).HasComment("Taxes on the requisition's work activity.");
            entity.Property(e => e.TotalAmt).HasComment("Total value of the requisition's work activity.");

            entity.HasOne(d => d.CompensationRequisition).WithMany(p => p.PimsCompReqFinancials)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_CMPREQ_PIM_CRH120_FK");

            entity.HasOne(d => d.FinancialActivityCode).WithMany(p => p.PimsCompReqFinancials)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_FINACT_PIM_CRH120_FK");
        });

        modelBuilder.Entity<PimsCompReqFinancialHist>(entity =>
        {
            entity.HasKey(e => e.CompReqFinancialHistId).HasName("PIMS_CRQFIN_H_PK");

            entity.Property(e => e.CompReqFinancialHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_COMP_REQ_FINANCIAL_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsCompensationRequisition>(entity =>
        {
            entity.HasKey(e => e.CompensationRequisitionId).HasName("CMPREQ_PK");

            entity.ToTable("PIMS_COMPENSATION_REQUISITION", tb =>
                {
                    tb.HasComment("Table containing the compensation requisition data for the acquisition file.");
                    tb.HasTrigger("PIMS_CMPREQ_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_CMPREQ_I_S_I_TR");
                    tb.HasTrigger("PIMS_CMPREQ_I_S_U_TR");
                });

            entity.Property(e => e.CompensationRequisitionId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_COMPENSATION_REQUISITION_ID_SEQ])");
            entity.Property(e => e.AcquisitionFileId).HasComment("Foreign key to the PIMS_ACQUISITION_FILE table.");
            entity.Property(e => e.AdvPmtServedDt).HasComment("Date that the advanced payment was made.");
            entity.Property(e => e.AgreementDt).HasComment("Agreement date.");
            entity.Property(e => e.AlternateProjectId).HasComment("Link a file to an \"Alternate Project\", so the user can make alternate payments that may be due after the original file's project closes.");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DetailedRemarks).HasComment("Detailed remarks for the compensation requisition.");
            entity.Property(e => e.ExpropNoticeServedDt).HasComment("Expropriation notice served date.");
            entity.Property(e => e.ExpropVestingDt).HasComment("Expropriation vesting date.");
            entity.Property(e => e.FinalizedDate).HasComment("Date that the draft Compensation Req changed from Draft to Final status.");
            entity.Property(e => e.FiscalYear).HasComment("Fiscal year of the compensation requisition.");
            entity.Property(e => e.GenerationDt).HasComment("Document generation date.");
            entity.Property(e => e.GstNumber).HasComment("GST number of the organization receiving the payment.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the requisition is inactive.");
            entity.Property(e => e.IsDraft)
                .HasDefaultValue(false)
                .HasComment("Indicates if the agreement is in draft format.");
            entity.Property(e => e.IsPaymentInTrust)
                .HasDefaultValue(false)
                .HasComment("Indicates if the payment was made in trust.");
            entity.Property(e => e.LeaseId).HasComment("Foreign key to the PIMS_LEASE table.");
            entity.Property(e => e.LegacyPayee).HasComment("Payee where only the name is known from the PAIMS system,");
            entity.Property(e => e.SpecialInstruction).HasComment("Special instructions for the compensation requisition.");

            entity.HasOne(d => d.AcquisitionFile).WithMany(p => p.PimsCompensationRequisitions).HasConstraintName("PIM_ACQNFL_PIM_CMPREQ_FK");

            entity.HasOne(d => d.AcquisitionFileTeam).WithMany(p => p.PimsCompensationRequisitions).HasConstraintName("PIM_ACQPER_PIM_CMPREQ_FK");

            entity.HasOne(d => d.AcquisitionOwner).WithMany(p => p.PimsCompensationRequisitions).HasConstraintName("PIM_ACQOWN_PIM_CMPREQ_FK");

            entity.HasOne(d => d.AlternateProject).WithMany(p => p.PimsCompensationRequisitions).HasConstraintName("PIM_PROJCT_PIM_CMPREQ_FK");

            entity.HasOne(d => d.ChartOfAccounts).WithMany(p => p.PimsCompensationRequisitions).HasConstraintName("PIM_CHRTAC_PIM_CMPREQ_FK");

            entity.HasOne(d => d.InterestHolder).WithMany(p => p.PimsCompensationRequisitions).HasConstraintName("PIM_INTHLD_PIM_CMPREQ_FK");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsCompensationRequisitions).HasConstraintName("PIM_LEASE_PIM_CMPREQ_FK");

            entity.HasOne(d => d.Responsibility).WithMany(p => p.PimsCompensationRequisitions).HasConstraintName("PIM_RESPCD_PIM_CMPREQ_FK");

            entity.HasOne(d => d.YearlyFinancial).WithMany(p => p.PimsCompensationRequisitions).HasConstraintName("PIM_YRFINC_PIM_CMPREQ_FK");
        });

        modelBuilder.Entity<PimsCompensationRequisitionHist>(entity =>
        {
            entity.HasKey(e => e.CompensationRequisitionHistId).HasName("PIMS_CMPREQ_H_PK");

            entity.Property(e => e.CompensationRequisitionHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_COMPENSATION_REQUISITION_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsConsultationOutcomeType>(entity =>
        {
            entity.HasKey(e => e.ConsultationOutcomeTypeCode).HasName("OUTCMT_PK");

            entity.ToTable("PIMS_CONSULTATION_OUTCOME_TYPE", tb =>
                {
                    tb.HasComment("Description of the consultation outcome type for a lease or license.");
                    tb.HasTrigger("PIMS_OUTCMT_I_S_I_TR");
                    tb.HasTrigger("PIMS_OUTCMT_I_S_U_TR");
                });

            entity.Property(e => e.ConsultationOutcomeTypeCode).HasComment("Code value of the consultation outcome type.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the consultation outcome type.");
            entity.Property(e => e.DisplayOrder).HasComment("Onscreen display order of the code types.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code type is active.");
        });

        modelBuilder.Entity<PimsConsultationStatusType>(entity =>
        {
            entity.HasKey(e => e.ConsultationStatusTypeCode).HasName("CONSTY_PK");

            entity.ToTable("PIMS_CONSULTATION_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Description of the consultation status type for (currently) a lease or license.");
                    tb.HasTrigger("PIMS_CONSTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_CONSTY_I_S_U_TR");
                });

            entity.Property(e => e.ConsultationStatusTypeCode)
                .HasDefaultValue("OTHER")
                .HasComment("Code value of the consultation status type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description)
                .HasDefaultValue("<Empty>")
                .HasComment("Description of the consultation status type.");
            entity.Property(e => e.DisplayOrder).HasComment("Onscreen display order of the consultation types.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the consultation status  type is active.");
        });

        modelBuilder.Entity<PimsConsultationType>(entity =>
        {
            entity.HasKey(e => e.ConsultationTypeCode).HasName("CONTYP_PK");

            entity.ToTable("PIMS_CONSULTATION_TYPE", tb =>
                {
                    tb.HasComment("Description of the consultation type required for (currently) a lease or license.");
                    tb.HasTrigger("PIMS_CONTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_CONTYP_I_S_U_TR");
                });

            entity.Property(e => e.ConsultationTypeCode)
                .HasDefaultValue("OTHER")
                .HasComment("Code value of the consultation type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description)
                .HasDefaultValue("<Empty>")
                .HasComment("Description of the consultation type.");
            entity.Property(e => e.DisplayOrder).HasComment("Onscreen display order of the consultation types.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the consultation type is active.");
            entity.Property(e => e.OtherDescription).HasComment("Additional descriptive text of the consultation type.");
        });

        modelBuilder.Entity<PimsContactMethod>(entity =>
        {
            entity.HasKey(e => e.ContactMethodId).HasName("CNTMTH_PK");

            entity.ToTable("PIMS_CONTACT_METHOD", tb =>
                {
                    tb.HasTrigger("PIMS_CNTMTH_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_CNTMTH_I_S_I_TR");
                    tb.HasTrigger("PIMS_CNTMTH_I_S_U_TR");
                });

            entity.Property(e => e.ContactMethodId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CONTACT_METHOD_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.IsPreferredMethod).HasDefaultValue(false);

            entity.HasOne(d => d.ContactMethodTypeCodeNavigation).WithMany(p => p.PimsContactMethods)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_CNTMTT_PIM_CNTMTH_FK");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsContactMethods).HasConstraintName("PIM_ORG_PIM_CNTMTH_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsContactMethods).HasConstraintName("PIM_PERSON_PIM_CNTMTH_FK");
        });

        modelBuilder.Entity<PimsContactMethodHist>(entity =>
        {
            entity.HasKey(e => e.ContactMethodHistId).HasName("PIMS_CNTMTH_H_PK");

            entity.Property(e => e.ContactMethodHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CONTACT_METHOD_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsContactMethodType>(entity =>
        {
            entity.HasKey(e => e.ContactMethodTypeCode).HasName("CNTMTT_PK");

            entity.ToTable("PIMS_CONTACT_METHOD_TYPE", tb =>
                {
                    tb.HasTrigger("PIMS_CNTMTT_I_S_I_TR");
                    tb.HasTrigger("PIMS_CNTMTT_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsContactMgrVw>(entity =>
        {
            entity.ToView("PIMS_CONTACT_MGR_VW");
        });

        modelBuilder.Entity<PimsCostTypeCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("COSTYP_PK");

            entity.ToTable("PIMS_COST_TYPE_CODE", tb =>
                {
                    tb.HasComment("Code and description of the cost type codes.");
                    tb.HasTrigger("PIMS_COSTYP_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_COSTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_COSTYP_I_S_U_TR");
                });

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_COST_TYPE_ID_SEQ])")
                .HasComment("System-generated primary key.");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Code)
                .HasDefaultValue("<Empty>")
                .HasComment("Name of the code.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Descriptive value  of a code within the set.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the code became effective.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the code ceased to be in effect.");
        });

        modelBuilder.Entity<PimsCostTypeCodeHist>(entity =>
        {
            entity.HasKey(e => e.CostTypeCodeHistId).HasName("PIMS_COSTYP_H_PK");

            entity.Property(e => e.CostTypeCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_COST_TYPE_CODE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsCountry>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("CNTRY_PK");

            entity.ToTable("PIMS_COUNTRY", tb =>
                {
                    tb.HasComment("Table containing the countries defined to the system.");
                    tb.HasTrigger("PIMS_CNTRY_I_S_I_TR");
                    tb.HasTrigger("PIMS_CNTRY_I_S_U_TR");
                });

            entity.Property(e => e.CountryId).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.CountryCode).HasComment("Abbreviated country code.");
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Country name/description.");
            entity.Property(e => e.DisplayOrder).HasComment("Defines the display order of the codes.");
        });

        modelBuilder.Entity<PimsDataSourceType>(entity =>
        {
            entity.HasKey(e => e.DataSourceTypeCode).HasName("PIDSRT_PK");

            entity.ToTable("PIMS_DATA_SOURCE_TYPE", tb =>
                {
                    tb.HasComment("Describes the source system of the data (PAIMS, LIS, etc.)");
                    tb.HasTrigger("PIMS_PIDSRT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PIDSRT_I_S_U_TR");
                });

            entity.Property(e => e.DataSourceTypeCode).HasComment("Code value of the source system of the data (PAIMS, LIS, etc.)");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the source system of the data (PAIMS, LIS, etc.)");
            entity.Property(e => e.DisplayOrder).HasComment("Defines the default display order of the descriptions");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is still in use");
        });

        modelBuilder.Entity<PimsDispositionAppraisal>(entity =>
        {
            entity.HasKey(e => e.DispositionAppraisalId).HasName("DSPAPP_PK");

            entity.ToTable("PIMS_DISPOSITION_APPRAISAL", tb =>
                {
                    tb.HasComment("Entity containing the appraisal and assessment information about the disposition.");
                    tb.HasTrigger("PIMS_DSPAPP_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DSPAPP_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPAPP_I_S_U_TR");
                });

            entity.Property(e => e.DispositionAppraisalId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_APPRAISAL_ID_SEQ])")
                .HasComment("Unique auto-generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.AppraisalDt).HasComment("Date of the disposition file appraisal.");
            entity.Property(e => e.AppraisedAmt).HasComment("Appraised value of the disposition file.");
            entity.Property(e => e.BcaRollYear).HasComment("BC Assessment roll year for the disposition file appraisal.");
            entity.Property(e => e.BcaValueAmt).HasComment("BC Assessment value of the disposition file.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DispositionFileId).HasComment("Foreign key to the disposition file.");
            entity.Property(e => e.ListPriceAmt).HasComment("Listed disposition file selling price.");

            entity.HasOne(d => d.DispositionFile).WithMany(p => p.PimsDispositionAppraisals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DISPFL_PIM_DSPAPP_FK");
        });

        modelBuilder.Entity<PimsDispositionAppraisalHist>(entity =>
        {
            entity.HasKey(e => e.DispositionAppraisalHistId).HasName("PIMS_DSPAPP_H_PK");

            entity.Property(e => e.DispositionAppraisalHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_APPRAISAL_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDispositionChecklistItem>(entity =>
        {
            entity.HasKey(e => e.DispositionChecklistItemId).HasName("DSPCKI_PK");

            entity.ToTable("PIMS_DISPOSITION_CHECKLIST_ITEM", tb =>
                {
                    tb.HasTrigger("PIMS_DSPCKI_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DSPCKI_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPCKI_I_S_U_TR");
                });

            entity.Property(e => e.DispositionChecklistItemId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_CHECKLIST_ITEM_ID_SEQ])")
                .HasComment("Unique auto-generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.ChklstItemStatusTypeCode).HasComment("Foreign key to the PIMS_CHKLST_ITEM_STATUS_TYPE table.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DispositionFileId).HasComment("Foreign key of the disposition file.");
            entity.Property(e => e.DspChklstItemTypeCode).HasComment("Code value for the checklist item.");

            entity.HasOne(d => d.ChklstItemStatusTypeCodeNavigation).WithMany(p => p.PimsDispositionChecklistItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_CHKLIS_PIM_DSPCKI_FK");

            entity.HasOne(d => d.DispositionFile).WithMany(p => p.PimsDispositionChecklistItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DISPFL_PIM_DSPCKI_FK");

            entity.HasOne(d => d.DspChklstItemTypeCodeNavigation).WithMany(p => p.PimsDispositionChecklistItems).HasConstraintName("PIM_DSPCIT_PIM_DSPCKI_FK");
        });

        modelBuilder.Entity<PimsDispositionChecklistItemHist>(entity =>
        {
            entity.HasKey(e => e.DispositionChecklistItemHistId).HasName("PIMS_DSPCKI_H_PK");

            entity.Property(e => e.DispositionChecklistItemHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_CHECKLIST_ITEM_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDispositionFile>(entity =>
        {
            entity.HasKey(e => e.DispositionFileId).HasName("DISPFL_PK");

            entity.ToTable("PIMS_DISPOSITION_FILE", tb =>
                {
                    tb.HasComment("Entity containing information regarding an disposition file.");
                    tb.HasTrigger("PIMS_DISPFL_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DISPFL_I_S_I_TR");
                    tb.HasTrigger("PIMS_DISPFL_I_S_U_TR");
                });

            entity.Property(e => e.DispositionFileId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_FILE_ID_SEQ])")
                .HasComment("Unique auto-generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.AssignedDt).HasComment("Date the disposition file was assigned.");
            entity.Property(e => e.CompletedDt).HasComment("Date the disposition file was completed.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DispositionFileStatusTypeCode)
                .HasDefaultValue("ACTIVE")
                .HasComment("Code value for the dispostion file status.");
            entity.Property(e => e.DispositionFundingTypeCode).HasComment("Code value for the disposition funding type.");
            entity.Property(e => e.DispositionInitiatingDocTypeCode).HasComment("Code value for the dispostion initiating document type.");
            entity.Property(e => e.DispositionStatusTypeCode)
                .HasDefaultValue("UNKNOWN")
                .HasComment("Code value for the dispostion status.");
            entity.Property(e => e.DispositionTypeCode).HasComment("Code value for the disposition type.");
            entity.Property(e => e.DspInitiatingBranchTypeCode).HasComment("Code value for the dispostion initiating branch.");
            entity.Property(e => e.DspPhysFileStatusTypeCode).HasComment("Code value for the dispostion physical file status.");
            entity.Property(e => e.FileName).HasComment("Name of the disposition file.");
            entity.Property(e => e.FileNumber).HasComment("The formatted disposition file number, seeded from the PIMS_DISPOSITION_FILE_NO_SEQ sequence.  Sample formats are D-1, D-2, D-3, etc.");
            entity.Property(e => e.FileReference).HasComment("Provide available reference number for historic program or file number (e.g.? RAEG, Acquisition File, etc.).");
            entity.Property(e => e.InitiatingDocumentDt).HasComment("Signoff date of the initiating document.");
            entity.Property(e => e.OtherDispositionType).HasComment("Required if \"Other\" disposition type selected.");
            entity.Property(e => e.OtherInitiatingDocType).HasComment("Required if \"Other\" disposition initiating document type selected.");
            entity.Property(e => e.ProductId).HasComment("Foreign key reference to the product table.");
            entity.Property(e => e.ProjectId).HasComment("Foreign key reference to the project table.");
            entity.Property(e => e.RegionCode).HasComment("Code value for the Ministry region code.");

            entity.HasOne(d => d.DispositionFileStatusTypeCodeNavigation).WithMany(p => p.PimsDispositionFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DSPFST_PIM_DISPFL_FK");

            entity.HasOne(d => d.DispositionFundingTypeCodeNavigation).WithMany(p => p.PimsDispositionFiles).HasConstraintName("PIM_DSPFTY_PIM_DISPFL_FK");

            entity.HasOne(d => d.DispositionInitiatingDocTypeCodeNavigation).WithMany(p => p.PimsDispositionFiles).HasConstraintName("PIM_DSPIDT_PIM_DISPFL_FK");

            entity.HasOne(d => d.DispositionStatusTypeCodeNavigation).WithMany(p => p.PimsDispositionFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DSPSTY_PIM_DISPFL_FK");

            entity.HasOne(d => d.DispositionTypeCodeNavigation).WithMany(p => p.PimsDispositionFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DSPTYP_PIM_DISPFL_FK");

            entity.HasOne(d => d.DspInitiatingBranchTypeCodeNavigation).WithMany(p => p.PimsDispositionFiles).HasConstraintName("PIM_DSPIBT_PIM_DISPFL_FK");

            entity.HasOne(d => d.DspPhysFileStatusTypeCodeNavigation).WithMany(p => p.PimsDispositionFiles).HasConstraintName("PIM_DSPPFS_PIM_DISPFL_FK");

            entity.HasOne(d => d.Product).WithMany(p => p.PimsDispositionFiles).HasConstraintName("PIM_PRODCT_PIM_DISPFL_FK");

            entity.HasOne(d => d.Project).WithMany(p => p.PimsDispositionFiles).HasConstraintName("PIM_PROJCT_PIM_DISPFL_FK");

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.PimsDispositionFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_REGION_PIM_DISPFL_FK");
        });

        modelBuilder.Entity<PimsDispositionFileDocument>(entity =>
        {
            entity.HasKey(e => e.DispositionFileDocumentId).HasName("DSPDOC_PK");

            entity.ToTable("PIMS_DISPOSITION_FILE_DOCUMENT", tb =>
                {
                    tb.HasTrigger("PIMS_DSPDOC_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DSPDOC_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPDOC_I_S_U_TR");
                });

            entity.Property(e => e.DispositionFileDocumentId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_FILE_DOCUMENT_ID_SEQ])")
                .HasComment("Unique auto-generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");

            entity.HasOne(d => d.DispositionFile).WithMany(p => p.PimsDispositionFileDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DISPFL_PIM_DSPDOC_FK");

            entity.HasOne(d => d.Document).WithMany(p => p.PimsDispositionFileDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DOCMNT_PIM_DSPDOC_FK");
        });

        modelBuilder.Entity<PimsDispositionFileDocumentHist>(entity =>
        {
            entity.HasKey(e => e.DispositionFileDocumentHistId).HasName("PIMS_DSPDOC_H_PK");

            entity.Property(e => e.DispositionFileDocumentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_FILE_DOCUMENT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDispositionFileHist>(entity =>
        {
            entity.HasKey(e => e.DispositionFileHistId).HasName("PIMS_DISPFL_H_PK");

            entity.Property(e => e.DispositionFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_FILE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDispositionFileNote>(entity =>
        {
            entity.HasKey(e => e.DispositionFileNoteId).HasName("DSPNOT_PK");

            entity.ToTable("PIMS_DISPOSITION_FILE_NOTE", tb =>
                {
                    tb.HasTrigger("PIMS_DSPNOT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DSPNOT_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPNOT_I_S_U_TR");
                });

            entity.Property(e => e.DispositionFileNoteId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_FILE_NOTE_ID_SEQ])")
                .HasComment("Unique auto-generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DispositionFileId).HasComment("Foreign key value for the associated disposition file.");
            entity.Property(e => e.NoteId).HasComment("Foreign key value for the associated note.");

            entity.HasOne(d => d.DispositionFile).WithMany(p => p.PimsDispositionFileNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DISPFL_PIM_DSPNOT_FK");

            entity.HasOne(d => d.Note).WithMany(p => p.PimsDispositionFileNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_NOTE_PIM_DSPNOT_FK");
        });

        modelBuilder.Entity<PimsDispositionFileNoteHist>(entity =>
        {
            entity.HasKey(e => e.DispositionFileNoteHistId).HasName("PIMS_DSPNOT_H_PK");

            entity.Property(e => e.DispositionFileNoteHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_FILE_NOTE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDispositionFileProperty>(entity =>
        {
            entity.HasKey(e => e.DispositionFilePropertyId).HasName("DSPPRP_PK");

            entity.ToTable("PIMS_DISPOSITION_FILE_PROPERTY", tb =>
                {
                    tb.HasComment("Entity to associate the properties involved with the disposition file.");
                    tb.HasTrigger("PIMS_DSPPRP_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DSPPRP_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPPRP_I_S_U_TR");
                });

            entity.Property(e => e.DispositionFilePropertyId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_FILE_PROPERTY_ID_SEQ])")
                .HasComment("Unique auto-generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DispositionFileId).HasComment("Primary key of the associated disposition file.");
            entity.Property(e => e.Location).HasComment("Geospatial location (pin) of property");
            entity.Property(e => e.PropertyId).HasComment("Primary key of the associated property.");
            entity.Property(e => e.PropertyName).HasComment("Descriptive reference for the property associated with the disposition file.");

            entity.HasOne(d => d.DispositionFile).WithMany(p => p.PimsDispositionFileProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DISPFL_PIM_DSPPRP_FK");

            entity.HasOne(d => d.Property).WithMany(p => p.PimsDispositionFileProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPRTY_PIM_DSPPRP_FK");
        });

        modelBuilder.Entity<PimsDispositionFilePropertyHist>(entity =>
        {
            entity.HasKey(e => e.DispositionFilePropertyHistId).HasName("PIMS_DSPPRP_H_PK");

            entity.Property(e => e.DispositionFilePropertyHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_FILE_PROPERTY_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDispositionFileStatusType>(entity =>
        {
            entity.HasKey(e => e.DispositionFileStatusTypeCode).HasName("DSPFST_PK");

            entity.ToTable("PIMS_DISPOSITION_FILE_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the dispostion file status.");
                    tb.HasTrigger("PIMS_DSPFST_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPFST_I_S_U_TR");
                });

            entity.Property(e => e.DispositionFileStatusTypeCode).HasComment("Code value for the dispostion file status.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the dispostion file status.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsDispositionFileTeam>(entity =>
        {
            entity.HasKey(e => e.DispositionFileTeamId).HasName("DSPFTM_PK");

            entity.ToTable("PIMS_DISPOSITION_FILE_TEAM", tb =>
                {
                    tb.HasComment("Table to associate an acquisition file to a person.");
                    tb.HasTrigger("PIMS_DSPFTM_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DSPFTM_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPFTM_I_S_U_TR");
                });

            entity.Property(e => e.DispositionFileTeamId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_FILE_TEAM_ID_SEQ])")
                .HasComment("Unique auto-generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DispositionFileId).HasComment("Foreign key value for the dispostion file");
            entity.Property(e => e.DspFlTeamProfileTypeCode).HasComment("Code value for the disposition file profile type.");
            entity.Property(e => e.OrganizationId).HasComment("Foreign key value for the organization.");
            entity.Property(e => e.PersonId).HasComment("Foreign key value for the person.");
            entity.Property(e => e.PrimaryContactId).HasComment("Foreign key value for the primary contact person.");

            entity.HasOne(d => d.DispositionFile).WithMany(p => p.PimsDispositionFileTeams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DISPFL_PIM_DSPFTM_FK");

            entity.HasOne(d => d.DspFlTeamProfileTypeCodeNavigation).WithMany(p => p.PimsDispositionFileTeams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DSPFTP_PIM_DSPFTM_FK");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsDispositionFileTeams).HasConstraintName("PIM_ORG_PIM_DSPFTM_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsDispositionFileTeamPeople).HasConstraintName("PIM_PERSON_PIM_DSPFTM_FK");

            entity.HasOne(d => d.PrimaryContact).WithMany(p => p.PimsDispositionFileTeamPrimaryContacts).HasConstraintName("PIM_PERSON_PIM_DSPFTM_CONTACT_FK");
        });

        modelBuilder.Entity<PimsDispositionFileTeamHist>(entity =>
        {
            entity.HasKey(e => e.DispositionFileTeamHistId).HasName("PIMS_DSPFTM_H_PK");

            entity.Property(e => e.DispositionFileTeamHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_FILE_TEAM_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDispositionFundingType>(entity =>
        {
            entity.HasKey(e => e.DispositionFundingTypeCode).HasName("DSPFTY_PK");

            entity.ToTable("PIMS_DISPOSITION_FUNDING_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the dispostion funding type.");
                    tb.HasTrigger("PIMS_DSPFTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPFTY_I_S_U_TR");
                });

            entity.Property(e => e.DispositionFundingTypeCode).HasComment("Code value for the disposition funding type.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the dispostion funding type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsDispositionInitiatingDocType>(entity =>
        {
            entity.HasKey(e => e.DispositionInitiatingDocTypeCode).HasName("DSPIDT_PK");

            entity.ToTable("PIMS_DISPOSITION_INITIATING_DOC_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the dispostion initiating document type.");
                    tb.HasTrigger("PIMS_DSPIDT_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPIDT_I_S_U_TR");
                });

            entity.Property(e => e.DispositionInitiatingDocTypeCode).HasComment("Code value for the dispostion initiating document type.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the dispostion initiating document type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsDispositionOffer>(entity =>
        {
            entity.HasKey(e => e.DispositionOfferId).HasName("DSPOFR_PK");

            entity.ToTable("PIMS_DISPOSITION_OFFER", tb =>
                {
                    tb.HasComment("Entity containing information regarding an disposition offer.");
                    tb.HasTrigger("PIMS_DSPOFR_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DSPOFR_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPOFR_I_S_U_TR");
                });

            entity.Property(e => e.DispositionOfferId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_OFFER_ID_SEQ])")
                .HasComment("Unique auto-generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DispositionFileId).HasComment("Foreign key value for the dispostion file");
            entity.Property(e => e.DispositionOfferStatusTypeCode).HasComment("Code value for the dispostion offer status.");
            entity.Property(e => e.OfferAmt).HasComment("The monetary value of the disposition offer.");
            entity.Property(e => e.OfferDt).HasComment("The date the disposition offer was made.");
            entity.Property(e => e.OfferExpiryDt).HasComment("The date the disposition offer expires.");
            entity.Property(e => e.OfferName).HasComment("The name(s) associated with this disposition offer.");
            entity.Property(e => e.OfferNote).HasComment("Provide any additional details such as offer terms or conditions, and any commentary on why the offer was accepted/countered/rejected.");

            entity.HasOne(d => d.DispositionFile).WithMany(p => p.PimsDispositionOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DISPFL_PIM_DSPOFR_FK");

            entity.HasOne(d => d.DispositionOfferStatusTypeCodeNavigation).WithMany(p => p.PimsDispositionOffers).HasConstraintName("PIM_DSPOFT_PIM_DSPOFR_FK");
        });

        modelBuilder.Entity<PimsDispositionOfferHist>(entity =>
        {
            entity.HasKey(e => e.DispositionOfferHistId).HasName("PIMS_DSPOFR_H_PK");

            entity.Property(e => e.DispositionOfferHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_OFFER_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDispositionOfferStatusType>(entity =>
        {
            entity.HasKey(e => e.DispositionOfferStatusTypeCode).HasName("DSPOFT_PK");

            entity.ToTable("PIMS_DISPOSITION_OFFER_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the dispostion offer status.");
                    tb.HasTrigger("PIMS_DSPOFT_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPOFT_I_S_U_TR");
                });

            entity.Property(e => e.DispositionOfferStatusTypeCode).HasComment("Code value for the dispostion offer status.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the dispostion offer status.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsDispositionPurchaser>(entity =>
        {
            entity.HasKey(e => e.DispositionPurchaserId).HasName("DSPPUR_PK");

            entity.ToTable("PIMS_DISPOSITION_PURCHASER", tb =>
                {
                    tb.HasComment("Describes the purchaser of the disposition.  There may be multiple purchasers and the purchasers include organizations and individuals.  If an organization is a purchaser, a primary contact person must be provided.");
                    tb.HasTrigger("PIMS_DSPPUR_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DSPPUR_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPPUR_I_S_U_TR");
                });

            entity.Property(e => e.DispositionPurchaserId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_PURCHASER_ID_SEQ])")
                .HasComment("Unique auto-generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DispositionSaleId).HasComment("Foreign key value for the dispostion sale.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code value is inactive.");
            entity.Property(e => e.OrganizationId).HasComment("Foreign key of the organization purchasing the disposition file.");
            entity.Property(e => e.PersonId).HasComment("Foreign key of the individual purchasing the disposition file.");
            entity.Property(e => e.PrimaryContactId).HasComment("Primary contact person for the organization");

            entity.HasOne(d => d.DispositionSale).WithMany(p => p.PimsDispositionPurchasers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DSPSAL_PIM_DSPPUR_FK");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsDispositionPurchasers).HasConstraintName("PIM_ORG_PIM_DSPPUR_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsDispositionPurchaserPeople).HasConstraintName("PIM_PERSON_PIM_DSPPUR_FK");

            entity.HasOne(d => d.PrimaryContact).WithMany(p => p.PimsDispositionPurchaserPrimaryContacts).HasConstraintName("PIM_PERSON_PIM_DSPPUR_CONTACT_FK");
        });

        modelBuilder.Entity<PimsDispositionPurchaserHist>(entity =>
        {
            entity.HasKey(e => e.DispositionPurchaserHistId).HasName("PIMS_DSPPUR_H_PK");

            entity.Property(e => e.DispositionPurchaserHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_PURCHASER_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDispositionSale>(entity =>
        {
            entity.HasKey(e => e.DispositionSaleId).HasName("DSPSAL_PK");

            entity.ToTable("PIMS_DISPOSITION_SALE", tb =>
                {
                    tb.HasComment("Entity containing information regarding an disposition sale.");
                    tb.HasTrigger("PIMS_DSPSAL_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DSPSAL_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPSAL_I_S_U_TR");
                });

            entity.Property(e => e.DispositionSaleId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_SALE_ID_SEQ])")
                .HasComment("Unique auto-generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DispositionFileId).HasComment("Foreign key value for the dispostion file");
            entity.Property(e => e.DspPurchAgentId).HasComment("Foreign key to the agent associated with the sale of the disposition.");
            entity.Property(e => e.DspPurchSolicitorId).HasComment("Foreign key to the solicitor associated with the sale of the disposition.");
            entity.Property(e => e.FinalConditionRemovalDt).HasComment("For general sales, provide the date when the last condition(s) are to be removed. For road closures enter the condition precedent date.");
            entity.Property(e => e.GstCollectedAmt).HasComment("GST collected is calculated based upon Final Sales Price.");
            entity.Property(e => e.IsGstRequired).HasComment("Is GST required for this sale?");
            entity.Property(e => e.NetBookAmt).HasComment("The net book value of the disposition sale.");
            entity.Property(e => e.RealtorCommissionAmt).HasComment("Amount paid to the realtor managing the sale.");
            entity.Property(e => e.RemediationAmt).HasComment("Cost of propery remediation.");
            entity.Property(e => e.SaleCompletionDt).HasComment("The date the disposition was completed.");
            entity.Property(e => e.SaleFinalAmt).HasComment("Value of the final sale.");
            entity.Property(e => e.SaleFiscalYear).HasComment("The fiscal year in which the sale was completed.");
            entity.Property(e => e.SppAmt).HasComment("Surplus Property Program (SPP) fee to be paid to CITZ.");
            entity.Property(e => e.TotalCostAmt).HasComment("The sum of all costs incurred to prepare property for sale (e.g., appraisal, environmental and other consultants, legal fees, First Nations accommodation, etc.).");

            entity.HasOne(d => d.DispositionFile).WithMany(p => p.PimsDispositionSales)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DISPFL_PIM_DSPSAL_FK");

            entity.HasOne(d => d.DspPurchAgent).WithMany(p => p.PimsDispositionSales).HasConstraintName("PIM_DSPPAG_PIM_DSPSAL_FK");

            entity.HasOne(d => d.DspPurchSolicitor).WithMany(p => p.PimsDispositionSales).HasConstraintName("PIM_DSPPSL_PIM_DSPSAL_FK");
        });

        modelBuilder.Entity<PimsDispositionSaleHist>(entity =>
        {
            entity.HasKey(e => e.DispositionSaleHistId).HasName("PIMS_DSPSAL_H_PK");

            entity.Property(e => e.DispositionSaleHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DISPOSITION_SALE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDispositionStatusType>(entity =>
        {
            entity.HasKey(e => e.DispositionStatusTypeCode).HasName("DSPSTY_PK");

            entity.ToTable("PIMS_DISPOSITION_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the dispostion status.");
                    tb.HasTrigger("PIMS_DSPSTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPSTY_I_S_U_TR");
                });

            entity.Property(e => e.DispositionStatusTypeCode).HasComment("Code value for the dispostion status.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the dispostion status.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsDispositionType>(entity =>
        {
            entity.HasKey(e => e.DispositionTypeCode).HasName("DSPTYP_PK");

            entity.ToTable("PIMS_DISPOSITION_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the dispostion type.");
                    tb.HasTrigger("PIMS_DSPTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPTYP_I_S_U_TR");
                });

            entity.Property(e => e.DispositionTypeCode).HasComment("Code value for the disposition type.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the dispostion type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsDistrict>(entity =>
        {
            entity.HasKey(e => e.DistrictCode).HasName("DSTRCT_PK");

            entity.ToTable("PIMS_DISTRICT", tb =>
                {
                    tb.HasTrigger("PIMS_DSTRCT_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSTRCT_I_S_U_TR");
                });

            entity.Property(e => e.DistrictCode).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.PimsDistricts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_REGION_PIM_DSTRCT_FK");
        });

        modelBuilder.Entity<PimsDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("DOCMNT_PK");

            entity.ToTable("PIMS_DOCUMENT", tb =>
                {
                    tb.HasComment("Table describing the available document types.");
                    tb.HasTrigger("PIMS_DOCMNT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DOCMNT_I_S_I_TR");
                    tb.HasTrigger("PIMS_DOCMNT_I_S_U_TR");
                });

            entity.Property(e => e.DocumentId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DOCUMENT_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DocumentStatusTypeCode).HasDefaultValue("NONE");
            entity.Property(e => e.FileName)
                .HasDefaultValue("<Empty>")
                .HasComment("Name of the file stored on Mayan EDMS.");
            entity.Property(e => e.MayanId)
                .HasDefaultValue(-1L)
                .HasComment("Mayan-generated document number used for retrieval from Mayan EDMS.");

            entity.HasOne(d => d.DocumentStatusTypeCodeNavigation).WithMany(p => p.PimsDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DOCSTY_PIM_DOCMNT_FK");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.PimsDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DOCTYP_PIM_DOCMNT_FK");
        });

        modelBuilder.Entity<PimsDocumentCategorySubtype>(entity =>
        {
            entity.HasKey(e => e.DocumentCategorySubtypeId).HasName("DCCTSB_PK");

            entity.ToTable("PIMS_DOCUMENT_CATEGORY_SUBTYPE", tb =>
                {
                    tb.HasTrigger("PIMS_DCCTSB_I_S_I_TR");
                    tb.HasTrigger("PIMS_DCCTSB_I_S_U_TR");
                });

            entity.Property(e => e.DocumentCategorySubtypeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DOCUMENT_CATEGORY_SUBTYPE_ID_SEQ])");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DisplayOrder).HasComment("Order in which to display the code values, if required.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is still active or is now disabled.");

            entity.HasOne(d => d.DocumentCategoryTypeCodeNavigation).WithMany(p => p.PimsDocumentCategorySubtypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DOCCAT_PIM_DCCTSB_FK");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.PimsDocumentCategorySubtypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DOCTYP_PIM_DCCTSB_FK");
        });

        modelBuilder.Entity<PimsDocumentCategoryType>(entity =>
        {
            entity.HasKey(e => e.DocumentCategoryTypeCode).HasName("DOCCAT_PK");

            entity.ToTable("PIMS_DOCUMENT_CATEGORY_TYPE", tb =>
                {
                    tb.HasComment("The volume unit used for measuring Properties.");
                    tb.HasTrigger("PIMS_DOCCAT_I_S_I_TR");
                    tb.HasTrigger("PIMS_DOCCAT_I_S_U_TR");
                });

            entity.Property(e => e.DocumentCategoryTypeCode).HasComment("The code value category of the document.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Translation of the code value into a description that can be displayed to the user.");
            entity.Property(e => e.DisplayOrder).HasComment("Order in which to display the code values, if required.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is still active or is now disabled.");
        });

        modelBuilder.Entity<PimsDocumentFormatType>(entity =>
        {
            entity.HasKey(e => e.DocumentFormatTypeCode).HasName("DOCFMT_PK");

            entity.ToTable("PIMS_DOCUMENT_FORMAT_TYPE", tb =>
                {
                    tb.HasComment("Table to contain the acceptable document formats that can be uploaded to the system.");
                    tb.HasTrigger("PIMS_DOCFMT_I_S_I_TR");
                    tb.HasTrigger("PIMS_DOCFMT_I_S_U_TR");
                });

            entity.Property(e => e.DocumentFormatTypeCode).HasComment("Code value of the acceptable document type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Decription of the acceptable document type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code values or descriptions.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date that the document format became acceptable to the system.");
            entity.Property(e => e.ExpiryDate).HasComment("Date that the document format became unsupported in the system.");
        });

        modelBuilder.Entity<PimsDocumentHist>(entity =>
        {
            entity.HasKey(e => e.DocumentHistId).HasName("PIMS_DOCMNT_H_PK");

            entity.Property(e => e.DocumentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DOCUMENT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDocumentStatusType>(entity =>
        {
            entity.HasKey(e => e.DocumentStatusTypeCode).HasName("DOCSTY_PK");

            entity.ToTable("PIMS_DOCUMENT_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Table describing the available document types.");
                    tb.HasTrigger("PIMS_DOCSTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_DOCSTY_I_S_U_TR");
                });

            entity.Property(e => e.DocumentStatusTypeCode)
                .HasDefaultValue("NEXT VALUE FOR [PIMS_DOCUMENT_ID_SEQ]")
                .HasComment("Code value of the document status type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Descriptive translation of the document status type code.");
            entity.Property(e => e.DisplayOrder).HasComment("Determines the default display order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is still in use.");
        });

        modelBuilder.Entity<PimsDocumentTyp>(entity =>
        {
            entity.HasKey(e => e.DocumentTypeId).HasName("DOCTYP_PK");

            entity.ToTable("PIMS_DOCUMENT_TYP", tb =>
                {
                    tb.HasComment("Table describing the available document types.");
                    tb.HasTrigger("PIMS_DOCTYP_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DOCTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_DOCTYP_I_S_U_TR");
                });

            entity.Property(e => e.DocumentTypeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DOCUMENT_TYPE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DisplayOrder).HasComment("Determines the default display order of the code descriptions.");
            entity.Property(e => e.DocumentType)
                .HasDefaultValue("<Empty>")
                .HasComment("Code value of the available document types.");
            entity.Property(e => e.DocumentTypeDescription)
                .HasDefaultValue("<Empty>")
                .HasComment("Description of the available document types.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is still in use.");
            entity.Property(e => e.MayanId)
                .HasDefaultValue(-1L)
                .HasComment("Mayan-generated document type number used for retrieval from Mayan EDMS.");
        });

        modelBuilder.Entity<PimsDocumentTypHist>(entity =>
        {
            entity.HasKey(e => e.DocumentTypHistId).HasName("PIMS_DOCTYP_H_PK");

            entity.Property(e => e.DocumentTypHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DOCUMENT_TYP_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDspChklstItemType>(entity =>
        {
            entity.HasKey(e => e.DspChklstItemTypeCode).HasName("DSPCIT_PK");

            entity.ToTable("PIMS_DSP_CHKLST_ITEM_TYPE", tb =>
                {
                    tb.HasComment("Table that contains the disposition checklist items that are presented to the user through dynamically building the input form.");
                    tb.HasTrigger("PIMS_DSPCIT_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPCIT_I_S_U_TR");
                });

            entity.Property(e => e.DspChklstItemTypeCode).HasComment("Disposition checklist item code value.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Disposition Checklist item descriptive text presented to the user.");
            entity.Property(e => e.DisplayOrder).HasComment("Specifies the order that the disposition checklist items are presented to the user.");
            entity.Property(e => e.DspChklstSectionTypeCode).HasComment("Disposition Section to which the item belongs.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the disposition checklist item is able to be presented to the user via the input form.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the disposition checklist item is removed from the input form.");
            entity.Property(e => e.Hint).HasComment("Disposition Checklist item descriptive tooltip presented to the user.");
            entity.Property(e => e.IsRequired)
                .HasDefaultValue(false)
                .HasComment("Indicates if the disposition checklist item is a required field.");

            entity.HasOne(d => d.DspChklstSectionTypeCodeNavigation).WithMany(p => p.PimsDspChklstItemTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DSPSCT_PIM_DSPCIT_FK");
        });

        modelBuilder.Entity<PimsDspChklstSectionType>(entity =>
        {
            entity.HasKey(e => e.DspChklstSectionTypeCode).HasName("DSPSCT_PK");

            entity.ToTable("PIMS_DSP_CHKLST_SECTION_TYPE", tb =>
                {
                    tb.HasComment("Table that contains the disposition checklist sctions that are presented to the user through dynamically building the input form.");
                    tb.HasTrigger("PIMS_DSPSCT_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPSCT_I_S_U_TR");
                });

            entity.Property(e => e.DspChklstSectionTypeCode).HasComment("Disposition checklist section code value.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Disposition checklist section descriptive text presented to the user.");
            entity.Property(e => e.DisplayOrder).HasComment("Specifies the order that the disposition checklist sections are presented to the user.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the disposition checklist section is able to be presented to the user via the input form.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the disposition checklist section is removed from the input form.");
        });

        modelBuilder.Entity<PimsDspFlTeamProfileType>(entity =>
        {
            entity.HasKey(e => e.DspFlTeamProfileTypeCode).HasName("DSPFTP_PK");

            entity.ToTable("PIMS_DSP_FL_TEAM_PROFILE_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the dispostion type.");
                    tb.HasTrigger("PIMS_DSPFTP_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPFTP_I_S_U_TR");
                });

            entity.Property(e => e.DspFlTeamProfileTypeCode).HasComment("Code value for the disposition file profile type.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the dispostion file profile type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsDspInitiatingBranchType>(entity =>
        {
            entity.HasKey(e => e.DspInitiatingBranchTypeCode).HasName("DSPIBT_PK");

            entity.ToTable("PIMS_DSP_INITIATING_BRANCH_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the dispostion inititating branch.");
                    tb.HasTrigger("PIMS_DSPIBT_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPIBT_I_S_U_TR");
                });

            entity.Property(e => e.DspInitiatingBranchTypeCode).HasComment("Code value for the dispostion initiating branch.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the dispostion initiating branch.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsDspPhysFileStatusType>(entity =>
        {
            entity.HasKey(e => e.DspPhysFileStatusTypeCode).HasName("DSPPFS_PK");

            entity.ToTable("PIMS_DSP_PHYS_FILE_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the dispostion physical file status.");
                    tb.HasTrigger("PIMS_DSPPFS_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPPFS_I_S_U_TR");
                });

            entity.Property(e => e.DspPhysFileStatusTypeCode).HasComment("Code value for the dispostion physical file status.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the dispostion physical file status.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsDspPurchAgent>(entity =>
        {
            entity.HasKey(e => e.DspPurchAgentId).HasName("DSPPAG_PK");

            entity.ToTable("PIMS_DSP_PURCH_AGENT", tb =>
                {
                    tb.HasComment("Describes the agent associated with the sale of the disposition.  The agent may be an organizations or an individual.  If an organization is the agent, a primary contact person must be provided.");
                    tb.HasTrigger("PIMS_DSPPAG_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DSPPAG_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPPAG_I_S_U_TR");
                });

            entity.Property(e => e.DspPurchAgentId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DSP_PURCH_AGENT_ID_SEQ])")
                .HasComment("Unique auto-generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code value is inactive.");
            entity.Property(e => e.OrganizationId).HasComment("Foreign key of the organization agent for the disposition file.");
            entity.Property(e => e.PersonId).HasComment("Foreign key of the individual agent for the disposition file.");
            entity.Property(e => e.PrimaryContactId).HasComment("Primary contact person for the organization.");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsDspPurchAgents).HasConstraintName("PIM_ORG_PIM_DSPPAG_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsDspPurchAgentPeople).HasConstraintName("PIM_PERSON_PIM_DSPPAG_FK");

            entity.HasOne(d => d.PrimaryContact).WithMany(p => p.PimsDspPurchAgentPrimaryContacts).HasConstraintName("PIM_PERSON_PIM_DSPPAG_CONTACT_FK");
        });

        modelBuilder.Entity<PimsDspPurchAgentHist>(entity =>
        {
            entity.HasKey(e => e.DspPurchAgentHistId).HasName("PIMS_DSPPAG_H_PK");

            entity.Property(e => e.DspPurchAgentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DSP_PURCH_AGENT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsDspPurchSolicitor>(entity =>
        {
            entity.HasKey(e => e.DspPurchSolicitorId).HasName("DSPPSL_PK");

            entity.ToTable("PIMS_DSP_PURCH_SOLICITOR", tb =>
                {
                    tb.HasComment("Describes the solicitor associated with the sale of the disposition.  The solicitor may be an organizations or an individual.  If an organization is the solicitor, a primary contact person must be provided.");
                    tb.HasTrigger("PIMS_DSPPSL_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_DSPPSL_I_S_I_TR");
                    tb.HasTrigger("PIMS_DSPPSL_I_S_U_TR");
                });

            entity.Property(e => e.DspPurchSolicitorId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DSP_PURCH_SOLICITOR_ID_SEQ])")
                .HasComment("Unique auto-generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code value is inactive.");
            entity.Property(e => e.OrganizationId).HasComment("Foreign key of the organization solicitor for the disposition file.");
            entity.Property(e => e.PersonId).HasComment("Foreign key of the individual solicitor for the disposition file.");
            entity.Property(e => e.PrimaryContactId).HasComment("Primary contact person for the organization.");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsDspPurchSolicitors).HasConstraintName("PIM_ORG_PIM_DSPPSL_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsDspPurchSolicitorPeople).HasConstraintName("PIM_PERSON_PIM_DSPPSL_FK");

            entity.HasOne(d => d.PrimaryContact).WithMany(p => p.PimsDspPurchSolicitorPrimaryContacts).HasConstraintName("PIM_PERSON_PIM_DSPPSL_CONTACT_FK");
        });

        modelBuilder.Entity<PimsDspPurchSolicitorHist>(entity =>
        {
            entity.HasKey(e => e.DspPurchSolicitorHistId).HasName("PIMS_DSPPSL_H_PK");

            entity.Property(e => e.DspPurchSolicitorHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DSP_PURCH_SOLICITOR_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsExpropPmtPmtItem>(entity =>
        {
            entity.HasKey(e => e.ExpropPmtPmtItemId).HasName("XPMTITY_PK");

            entity.ToTable("PIMS_EXPROP_PMT_PMT_ITEM", tb =>
                {
                    tb.HasComment("Associative entity to connect expropriation forms (Form 8) to payment item types.  The supports the ability to associate multiple payment item types to a single expropriation form (Form 8).");
                    tb.HasTrigger("PIMS_XPMTITY_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_XPMTITY_I_S_I_TR");
                    tb.HasTrigger("PIMS_XPMTITY_I_S_U_TR");
                });

            entity.Property(e => e.ExpropPmtPmtItemId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_EXPROP_PMT_PMT_ITEM_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the relationship is active.");
            entity.Property(e => e.IsGstRequired)
                .HasDefaultValue(false)
                .HasComment("Indicates if GST is required for this transaction.");
            entity.Property(e => e.PretaxAmt).HasComment("Subtotal of the Form 8 payment.");
            entity.Property(e => e.TaxAmt).HasComment("GST on the Form 8 oayment.");
            entity.Property(e => e.TotalAmt).HasComment("Total amount of the Form 8 payment.");

            entity.HasOne(d => d.ExpropriationPayment).WithMany(p => p.PimsExpropPmtPmtItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_EXPPMT_PIM_XPMTITY_FK");

            entity.HasOne(d => d.PaymentItemTypeCodeNavigation).WithMany(p => p.PimsExpropPmtPmtItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PMTITM_PIM_XPMTITY_FK");
        });

        modelBuilder.Entity<PimsExpropPmtPmtItemHist>(entity =>
        {
            entity.HasKey(e => e.ExpropPmtPmtItemHistId).HasName("PIMS_XPMTIT_H_PK");

            entity.Property(e => e.ExpropPmtPmtItemHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_EXPROP_PMT_PMT_ITEM_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsExpropriationPayment>(entity =>
        {
            entity.HasKey(e => e.ExpropriationPaymentId).HasName("EXPPMT_PK");

            entity.ToTable("PIMS_EXPROPRIATION_PAYMENT", tb =>
                {
                    tb.HasComment("Entity continaing the details regarding a Form 8 (Notice of Advance Payment).");
                    tb.HasTrigger("PIMS_EXPPMT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_EXPPMT_I_S_I_TR");
                    tb.HasTrigger("PIMS_EXPPMT_I_S_U_TR");
                });

            entity.Property(e => e.ExpropriationPaymentId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_EXPROPRIATION_PAYMENT_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Form 8 description field.  There are lawyer remarks pending.  This field could be used for: - providing remarks particular to an expropriation form, and /or - for any ETL descriptive fields as well as - a place-holder forfields that do not have a mapping");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the Form 8 payment is inactive.");

            entity.HasOne(d => d.AcquisitionFile).WithMany(p => p.PimsExpropriationPayments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ACQNFL_PIM_FORM8_FK");

            entity.HasOne(d => d.AcquisitionOwner).WithMany(p => p.PimsExpropriationPayments).HasConstraintName("PIM_ACQOWN_PIM_FORM8_FK");

            entity.HasOne(d => d.ExpropriatingAuthorityNavigation).WithMany(p => p.PimsExpropriationPayments).HasConstraintName("PIM_ORG_PIM_FORM8_FK");

            entity.HasOne(d => d.InterestHolder).WithMany(p => p.PimsExpropriationPayments).HasConstraintName("PIM_INTHLD_PIM_FORM8_FK");
        });

        modelBuilder.Entity<PimsExpropriationPaymentHist>(entity =>
        {
            entity.HasKey(e => e.ExpropriationPaymentHistId).HasName("PIMS_EXPPMT_H_PK");

            entity.Property(e => e.ExpropriationPaymentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_EXPROPRIATION_PAYMENT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsFenceType>(entity =>
        {
            entity.HasKey(e => e.FenceTypeCode).HasName("FNCTYP_PK");

            entity.ToTable("PIMS_FENCE_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the fence type.  This is an unassociated table that is used in the UI to populate JSON attributes.");
                    tb.HasTrigger("PIMS_FNCTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_FNCTYP_I_S_U_TR");
                });

            entity.Property(e => e.FenceTypeCode).HasComment("Code value for the fence type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the fence type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsFinancialActivityCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FINACT_PK");

            entity.ToTable("PIMS_FINANCIAL_ACTIVITY_CODE", tb =>
                {
                    tb.HasComment("Code and description of the financial activity codes.");
                    tb.HasTrigger("PIMS_FINACT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_FINACT_I_S_I_TR");
                    tb.HasTrigger("PIMS_FINACT_I_S_U_TR");
                });

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_FINANCIAL_ACTIVITY_ID_SEQ])")
                .HasComment("System-generated primary key.");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Code)
                .HasDefaultValue("<Empty>")
                .HasComment("Value of the code.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Descriptive value  of a code within the set.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the code became effective.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the code ceased to be in effect.");
        });

        modelBuilder.Entity<PimsFinancialActivityCodeHist>(entity =>
        {
            entity.HasKey(e => e.FinancialActivityCodeHistId).HasName("PIMS_FINACT_H_PK");

            entity.Property(e => e.FinancialActivityCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_FINANCIAL_ACTIVITY_CODE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsFormType>(entity =>
        {
            entity.HasKey(e => e.FormTypeCode).HasName("FRMTYP_PK");

            entity.ToTable("PIMS_FORM_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the form types.");
                    tb.HasTrigger("PIMS_FRMTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_FRMTYP_I_S_U_TR");
                });

            entity.Property(e => e.FormTypeCode).HasComment("Code value of the form type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the form type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");

            entity.HasOne(d => d.Document).WithMany(p => p.PimsFormTypes).HasConstraintName("PIM_DOCMNT_PIM_FRMTYP_FK");
        });

        modelBuilder.Entity<PimsH120Category>(entity =>
        {
            entity.HasKey(e => e.H120CategoryId).HasName("H120CT_PK");

            entity.ToTable("PIMS_H120_CATEGORY", tb =>
                {
                    tb.HasComment("Table containing the compensation requisition data for the acquisition file.");
                    tb.HasTrigger("PIMS_H120CT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_H120CT_I_S_I_TR");
                    tb.HasTrigger("PIMS_H120CT_I_S_U_TR");
                });

            entity.Property(e => e.H120CategoryId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_H120_CATEGORY_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the H120 category.");
            entity.Property(e => e.ExpiryDate).HasComment("Expiry date of the H120 category.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the requisition is inactive.");

            entity.HasOne(d => d.CostType).WithMany(p => p.PimsH120Categories).HasConstraintName("PIM_COSTYP_PIM_H120CT_FK");

            entity.HasOne(d => d.FinancialActivity).WithMany(p => p.PimsH120Categories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_FINACT_PIM_H120CT_FK");

            entity.HasOne(d => d.WorkActivity).WithMany(p => p.PimsH120Categories).HasConstraintName("PIM_WRKACT_PIM_H120CT_FK");
        });

        modelBuilder.Entity<PimsH120CategoryHist>(entity =>
        {
            entity.HasKey(e => e.H120CategoryHistId).HasName("PIMS_H120CT_H_PK");

            entity.Property(e => e.H120CategoryHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_H120_CATEGORY_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsHistoricalFileNumber>(entity =>
        {
            entity.HasKey(e => e.HistoricalFileNumberId).HasName("HFLNUM_PK");

            entity.ToTable("PIMS_HISTORICAL_FILE_NUMBER", tb =>
                {
                    tb.HasComment("Table containing the historical file numbers associated with a property.");
                    tb.HasTrigger("PIMS_HFLNUM_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_HFLNUM_I_S_I_TR");
                    tb.HasTrigger("PIMS_HFLNUM_I_S_U_TR");
                });

            entity.Property(e => e.HistoricalFileNumberId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_HISTORICAL_FILE_NUMBER_ID_SEQ])")
                .HasComment("Generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DataSourceTypeCode).HasComment("Foreign key indicating the source of the data.");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.HistoricalFileNumber).HasComment("The historical file number value.");
            entity.Property(e => e.HistoricalFileNumberTypeCode).HasComment("Foreign key describing the historical file number type.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the record is disabled.");
            entity.Property(e => e.OtherHistFileNumberTypeCode).HasComment("Description of the historical file number type that's not currently listed.");
            entity.Property(e => e.PropertyId).HasComment("Foreign key to the PIMS_PROPERTY table.");

            entity.HasOne(d => d.DataSourceTypeCodeNavigation).WithMany(p => p.PimsHistoricalFileNumbers).HasConstraintName("PIM_PIDSRT_PIM_HFLNUM_FK");

            entity.HasOne(d => d.HistoricalFileNumberTypeCodeNavigation).WithMany(p => p.PimsHistoricalFileNumbers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_HFLNMT_PIM_HFLNUM_FK");

            entity.HasOne(d => d.Property).WithMany(p => p.PimsHistoricalFileNumbers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPRTY_PIM_HFLNUM_FK");
        });

        modelBuilder.Entity<PimsHistoricalFileNumberHist>(entity =>
        {
            entity.HasKey(e => e.HistoricalFileNumberHistId).HasName("PIMS_HFLNUM_H_PK");

            entity.Property(e => e.HistoricalFileNumberHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_HISTORICAL_FILE_NUMBER_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsHistoricalFileNumberType>(entity =>
        {
            entity.HasKey(e => e.HistoricalFileNumberTypeCode).HasName("HFLNMT_PK");

            entity.ToTable("PIMS_HISTORICAL_FILE_NUMBER_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe the type of historical property file number.");
                    tb.HasTrigger("PIMS_HFLNMT_I_S_I_TR");
                    tb.HasTrigger("PIMS_HFLNMT_I_S_U_TR");
                });

            entity.Property(e => e.HistoricalFileNumberTypeCode).HasComment("Code representing the type of historical file number.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the type of historical file number.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsHistoricalFileNumberVw>(entity =>
        {
            entity.ToView("PIMS_HISTORICAL_FILE_NUMBER_VW");
        });

        modelBuilder.Entity<PimsInsurance>(entity =>
        {
            entity.HasKey(e => e.InsuranceId).HasName("INSRNC_PK");

            entity.ToTable("PIMS_INSURANCE", tb =>
                {
                    tb.HasTrigger("PIMS_INSRNC_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_INSRNC_I_S_I_TR");
                    tb.HasTrigger("PIMS_INSRNC_I_S_U_TR");
                });

            entity.Property(e => e.InsuranceId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_INSURANCE_ID_SEQ])")
                .HasComment("Generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.CoverageDescription).HasComment("Description of the insurance coverage");
            entity.Property(e => e.CoverageLimit)
                .HasDefaultValue(0m)
                .HasComment("Monetary limit of the insurance coverage");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that updated the record.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the insurance expires");
            entity.Property(e => e.InsuranceTypeCode).HasComment("Foreign key indicating the type of insurance on the lease.");
            entity.Property(e => e.IsInsuranceInPlace).HasComment("Indicator that insurance is in place.  TRUE if insurance is in place, FALSE if insurance is not in place, and NULL if it is unknown if insurance is in place.");
            entity.Property(e => e.LeaseId).HasComment("Foreign key to the PIMS_LEASE table.");
            entity.Property(e => e.OtherInsuranceType).HasComment("Description of the non-standard insurance coverage type");

            entity.HasOne(d => d.InsuranceTypeCodeNavigation).WithMany(p => p.PimsInsurances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_INSPYT_PIM_INSRNC_FK");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsInsurances)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LEASE_PIM_INSRNC_FK");
        });

        modelBuilder.Entity<PimsInsuranceHist>(entity =>
        {
            entity.HasKey(e => e.InsuranceHistId).HasName("PIMS_INSRNC_H_PK");

            entity.Property(e => e.InsuranceHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_INSURANCE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsInsuranceType>(entity =>
        {
            entity.HasKey(e => e.InsuranceTypeCode).HasName("INSPYT_PK");

            entity.ToTable("PIMS_INSURANCE_TYPE", tb =>
                {
                    tb.HasTrigger("PIMS_INSPYT_I_S_I_TR");
                    tb.HasTrigger("PIMS_INSPYT_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsInterestHolder>(entity =>
        {
            entity.HasKey(e => e.InterestHolderId).HasName("INTHLD_PK");

            entity.ToTable("PIMS_INTEREST_HOLDER", tb =>
                {
                    tb.HasComment("Documents the interest holders that have an stake in the acquisition.");
                    tb.HasTrigger("PIMS_INTHLD_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_INTHLD_I_S_I_TR");
                    tb.HasTrigger("PIMS_INTHLD_I_S_U_TR");
                });

            entity.Property(e => e.InterestHolderId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_INTEREST_HOLDER_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Comment).HasComment("Additional comment concerning the owener representative.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.InterestHolderTypeCode).HasDefaultValue("INTHLDR");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code value is inactive.");
            entity.Property(e => e.PrimaryContactId).HasComment("Primary contact for the organization");

            entity.HasOne(d => d.AcquisitionFile).WithMany(p => p.PimsInterestHolders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ACQNFL_PIM_INTHLD_FK");

            entity.HasOne(d => d.InterestHolderTypeCodeNavigation).WithMany(p => p.PimsInterestHolders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_INHLDT_PIM_INTHLD_FK");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsInterestHolders).HasConstraintName("PIM_ORG_PIM_INTHLD_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsInterestHolderPeople).HasConstraintName("PIM_PERSON_PIM_INTHLD_FK");

            entity.HasOne(d => d.PrimaryContact).WithMany(p => p.PimsInterestHolderPrimaryContacts).HasConstraintName("PIM_PERSON_PIM_INTHLD_PRIMARY_FK");
        });

        modelBuilder.Entity<PimsInterestHolderHist>(entity =>
        {
            entity.HasKey(e => e.InterestHolderHistId).HasName("PIMS_INTHLD_H_PK");

            entity.Property(e => e.InterestHolderHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_INTEREST_HOLDER_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsInterestHolderInterestType>(entity =>
        {
            entity.HasKey(e => e.InterestHolderInterestTypeCode).HasName("IHINTT_PK");

            entity.ToTable("PIMS_INTEREST_HOLDER_INTEREST_TYPE", tb =>
                {
                    tb.HasComment("Tables that contains the codes and associated descriptions of the interest holder interest types.");
                    tb.HasTrigger("PIMS_IHINTT_I_S_I_TR");
                    tb.HasTrigger("PIMS_IHINTT_I_S_U_TR");
                });

            entity.Property(e => e.InterestHolderInterestTypeCode).HasComment("Codified version of the interest holder interest type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the interest holder interest type.");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the codes.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsInterestHolderType>(entity =>
        {
            entity.HasKey(e => e.InterestHolderTypeCode).HasName("INHLDT_PK");

            entity.ToTable("PIMS_INTEREST_HOLDER_TYPE", tb =>
                {
                    tb.HasComment("Tables that contains the codes and associated descriptions of the interest holder types, such as solicitors, representatives, and interest holders.");
                    tb.HasTrigger("PIMS_INHLDT_I_S_I_TR");
                    tb.HasTrigger("PIMS_INHLDT_I_S_U_TR");
                });

            entity.Property(e => e.InterestHolderTypeCode).HasComment("Codified version of the interest holder types, such as solicitors, representatives, and interest holders.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the interest holder types, such as solicitors, representatives, and interest holders.");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsInthldrPropInterest>(entity =>
        {
            entity.HasKey(e => e.PimsInthldrPropInterestId).HasName("IHPRIN_PK");

            entity.ToTable("PIMS_INTHLDR_PROP_INTEREST", tb =>
                {
                    tb.HasTrigger("PIMS_IHPRIN_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_IHPRIN_I_S_I_TR");
                    tb.HasTrigger("PIMS_IHPRIN_I_S_U_TR");
                });

            entity.Property(e => e.PimsInthldrPropInterestId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_INTHLDR_PROP_INTEREST_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.InterestHolder).WithMany(p => p.PimsInthldrPropInterests).HasConstraintName("PIM_INTHLD_PIM_IHPRIN_FK");

            entity.HasOne(d => d.PropertyAcquisitionFile).WithMany(p => p.PimsInthldrPropInterests).HasConstraintName("PIM_PRACQF_PIM_IHPRIN_FK");
        });

        modelBuilder.Entity<PimsInthldrPropInterestHist>(entity =>
        {
            entity.HasKey(e => e.InthldrPropInterestHistId).HasName("PIMS_IHPRIN_H_PK");

            entity.Property(e => e.InthldrPropInterestHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_INTHLDR_PROP_INTEREST_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsLandActType>(entity =>
        {
            entity.HasKey(e => e.LandActTypeCode).HasName("LNDATY_PK");

            entity.ToTable("PIMS_LAND_ACT_TYPE", tb =>
                {
                    tb.HasComment("Tables that contains the codes and associated descriptions of the site contamination types.");
                    tb.HasTrigger("PIMS_LNDATY_I_S_I_TR");
                    tb.HasTrigger("PIMS_LNDATY_I_S_U_TR");
                });

            entity.Property(e => e.LandActTypeCode).HasComment("Codified version of the Land Act type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the Land Act type.");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsLandSurveyorType>(entity =>
        {
            entity.HasKey(e => e.LandSurveyorTypeCode).HasName("LNSRVT_PK");

            entity.ToTable("PIMS_LAND_SURVEYOR_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the land surveyor type.  This is an unassociated table that is used in the UI to populate JSON attributes.");
                    tb.HasTrigger("PIMS_LNSRVT_I_S_I_TR");
                    tb.HasTrigger("PIMS_LNSRVT_I_S_U_TR");
                });

            entity.Property(e => e.LandSurveyorTypeCode).HasComment("Code value for the land surveyor type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the land surveyor type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsLease>(entity =>
        {
            entity.HasKey(e => e.LeaseId).HasName("LEASE_PK");

            entity.ToTable("PIMS_LEASE", tb =>
                {
                    tb.HasComment("Details of a lease that is inventoried in PIMS system.");
                    tb.HasTrigger("PIMS_LEASE_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_LEASE_I_S_I_TR");
                    tb.HasTrigger("PIMS_LEASE_I_S_U_TR");
                });

            entity.Property(e => e.LeaseId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_ID_SEQ])")
                .HasComment("Generated surrogate primary key.");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that updated the record.");
            entity.Property(e => e.CancellationReason).HasComment("Reason for the cancellation of the lease.  For example, \"The request for leasing the space was withdrawn.\"");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DocumentationReference).HasComment("Location of documents pertianing to the lease/license");
            entity.Property(e => e.FeeDeterminationNote).HasComment("Note associated with fee determination.");
            entity.Property(e => e.HasDigitalFile).HasComment("Indicator that digital file exists");
            entity.Property(e => e.HasDigitalLicense).HasComment("Indicator that digital license exists");
            entity.Property(e => e.HasPhysicalFile).HasComment("Indicator that phyical file exists");
            entity.Property(e => e.HasPhysicialLicense).HasComment("Indicator that physical license exists");
            entity.Property(e => e.InspectionDate).HasComment("Inspection date");
            entity.Property(e => e.InspectionNotes).HasComment("Notes accompanying inspection");
            entity.Property(e => e.IsCommBldg)
                .HasDefaultValue(false)
                .HasComment("Is a commercial building");
            entity.Property(e => e.IsExpired).HasComment("Incidcator that lease/license has expired");
            entity.Property(e => e.IsFinancialGain).HasComment("Is there an associated financial gain with this lease?  TRUE = Yes, FALSE = No, and NULL = Unknown.  The default is NULL (Unknown).");
            entity.Property(e => e.IsOtherImprovement)
                .HasDefaultValue(false)
                .HasComment("Is improvement of another description");
            entity.Property(e => e.IsPublicBenefit).HasComment("Is there an associated public benefit with this lease?  TRUE = Yes, FALSE = No, and NULL = Unknown.  The default is NULL (Unknown).");
            entity.Property(e => e.IsSubjectToRta)
                .HasDefaultValue(false)
                .HasComment("Is subject the Residential Tenancy Act");
            entity.Property(e => e.LFileNo).HasComment("Generated identifying lease/licence number");
            entity.Property(e => e.LeaseAmount).HasComment("Lease/licence amount");
            entity.Property(e => e.LeaseDescription).HasComment("Manually etered lease description, not the legal description");
            entity.Property(e => e.LeaseInitiatorTypeCode).HasComment("Foreign key to the PIMS_LEASE_INITIATOR_TYPE table.");
            entity.Property(e => e.LeaseLicenseTypeCode).HasComment("Foreign key to the PIMS_LEASE_LICENSE_TYPE table.");
            entity.Property(e => e.LeaseNotes).HasComment("Notes accompanying lease");
            entity.Property(e => e.LeasePayRvblTypeCode).HasComment("Foreign key to the PIMS_LEASE_PAY_RVBL_TYPE table.");
            entity.Property(e => e.LeaseProgramTypeCode).HasComment("Foreign key to the PIMS_LEASE_PROGRAM_TYPE table.");
            entity.Property(e => e.LeaseResponsibilityTypeCode).HasComment("Foreign key to the PIMS_LEASE_RESPONSIBILITY_TYPE table.");
            entity.Property(e => e.LeaseStatusTypeCode).HasComment("Foreign key to the PIMS_LEASE_STATUS_TYPE table.");
            entity.Property(e => e.MotiContact).HasComment("Contact of the MoTI person associated with the lease");
            entity.Property(e => e.OrigExpiryDate).HasComment("Original expiry date of the lease/license");
            entity.Property(e => e.OrigStartDate).HasComment("Original start date of the lease/license");
            entity.Property(e => e.OtherLeaseLicenseType).HasComment("Description of a non-standard lease/license type");
            entity.Property(e => e.OtherLeaseProgramType).HasComment("Description of a non-standard lease program type");
            entity.Property(e => e.PrimaryArbitrationCity).HasComment("The location in which primary arbtration of the lease occurred.");
            entity.Property(e => e.ProductId).HasComment("Foreign key to the PIMS_PRODUCT table.");
            entity.Property(e => e.ProjectId).HasComment("Foreign key to the PIMS_PROJECT table.");
            entity.Property(e => e.PsFileNo).HasComment("Sourced from t_fileSubOverrideData.PSFile_No");
            entity.Property(e => e.RegionCode).HasComment("Foreign key to the PIMS_REGION table.");
            entity.Property(e => e.ResponsibilityEffectiveDate).HasComment("Date current responsibility came into effect for this lease");
            entity.Property(e => e.ReturnNotes).HasComment("Notes accompanying lease");
            entity.Property(e => e.TerminationDate).HasComment("Date that the lease was terminated.");
            entity.Property(e => e.TerminationReason).HasComment("Reason for the termination of the lease.  For example, \"The tenant is in violation of the terms of the agreement.\"");
            entity.Property(e => e.TfaFileNo).HasComment("Sourced from t_fileMain.TFA_File_Number");
            entity.Property(e => e.TfaFileNumber).HasComment("Sourced from t_fileMain.TFA_File_Number || - || t_fileSub.Subfile_Sequence_Code");
            entity.Property(e => e.TotalAllowableCompensation).HasComment("The maximum allowable compensation for the lease.  This amount should not be exceeded by the total of all assiciated H120's.");

            entity.HasOne(d => d.LeaseInitiatorTypeCodeNavigation).WithMany(p => p.PimsLeases).HasConstraintName("PIM_LINITT_PIM_LEASE_FK");

            entity.HasOne(d => d.LeaseLicenseTypeCodeNavigation).WithMany(p => p.PimsLeases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LELIST_PIM_LEASE_FK");

            entity.HasOne(d => d.LeasePayRvblTypeCodeNavigation).WithMany(p => p.PimsLeases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LSPRTY_PIM_LEASE_FK");

            entity.HasOne(d => d.LeaseProgramTypeCodeNavigation).WithMany(p => p.PimsLeases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LSPRGT_PIM_LEASE_FK");

            entity.HasOne(d => d.LeaseResponsibilityTypeCodeNavigation).WithMany(p => p.PimsLeases).HasConstraintName("PIM_LRESPT_PIM_LEASE_FK");

            entity.HasOne(d => d.LeaseStatusTypeCodeNavigation).WithMany(p => p.PimsLeases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LSSTYP_PIM_LEASE_FK");

            entity.HasOne(d => d.Product).WithMany(p => p.PimsLeases).HasConstraintName("PIM_PRODCT_PIM_LEASE_FK");

            entity.HasOne(d => d.Project).WithMany(p => p.PimsLeases).HasConstraintName("PIM_PROJCT_PIM_LEASE_FK");

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.PimsLeases).HasConstraintName("PIM_REGION_PIM_LEASE_FK");
        });

        modelBuilder.Entity<PimsLeaseChecklistItem>(entity =>
        {
            entity.HasKey(e => e.LeaseChecklistItemId).HasName("LCHKLI_PK");

            entity.ToTable("PIMS_LEASE_CHECKLIST_ITEM", tb =>
                {
                    tb.HasComment("Table that contains the lease & license checklist items.");
                    tb.HasTrigger("PIMS_LCHKLI_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_LCHKLI_I_S_I_TR");
                    tb.HasTrigger("PIMS_LCHKLI_I_S_U_TR");
                });

            entity.Property(e => e.LeaseChecklistItemId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_CHECKLIST_ITEM_ID_SEQ])")
                .HasComment("Generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that updated the record.");
            entity.Property(e => e.ChklstItemStatusTypeCode)
                .HasDefaultValue("INCOMP")
                .HasComment("Foreign key to the PIMS_CHKLST_ITEM_STATUS_TYPE table.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.LeaseChklstItemTypeCode).HasComment("Foreign key to the PIMS_LEASE_CHKLST_ITEM_TYPE table.");
            entity.Property(e => e.LeaseId).HasComment("Foreign key to the PIMS_LEASE table.");

            entity.HasOne(d => d.ChklstItemStatusTypeCodeNavigation).WithMany(p => p.PimsLeaseChecklistItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LCISTY_PIM_LCHKLI_FK");

            entity.HasOne(d => d.LeaseChklstItemTypeCodeNavigation).WithMany(p => p.PimsLeaseChecklistItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LCKITY_PIM_LCHKLI_FK");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsLeaseChecklistItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LEASE_PIM_LCHKLI_FK");
        });

        modelBuilder.Entity<PimsLeaseChecklistItemHist>(entity =>
        {
            entity.HasKey(e => e.LeaseChecklistItemHistId).HasName("PIMS_LCHKLI_H_PK");

            entity.Property(e => e.LeaseChecklistItemHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_CHECKLIST_ITEM_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsLeaseChklstItemType>(entity =>
        {
            entity.HasKey(e => e.LeaseChklstItemTypeCode).HasName("LCKITY_PK");

            entity.ToTable("PIMS_LEASE_CHKLST_ITEM_TYPE", tb =>
                {
                    tb.HasComment("Table that contains the lease & license checklist items that are presented to the user through dynamically building the input form.");
                    tb.HasTrigger("PIMS_LCKITY_I_S_I_TR");
                    tb.HasTrigger("PIMS_LCKITY_I_S_U_TR");
                });

            entity.Property(e => e.LeaseChklstItemTypeCode).HasComment("Lease & license checklist item code value.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Lease & license checklist item descriptive text presented to the user.");
            entity.Property(e => e.DisplayOrder).HasComment("Specifies the order that the lease & license checklist items are presented to the user.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the lease & license checklist item is able to be presented to the user via the input form.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the lease & license checklist item is removed from the input form.");
            entity.Property(e => e.Hint).HasComment("Lease & license checklist item descriptive tooltip presented to the user.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is currently active.");
            entity.Property(e => e.IsRequired)
                .HasDefaultValue(false)
                .HasComment("Indicates if the lease & license checklist item is a required field.");
            entity.Property(e => e.LeaseChklstSectionTypeCode).HasComment("Foreign key to the PIMS_LEASE_CHKLST_SECTION_TYPE table.");

            entity.HasOne(d => d.LeaseChklstSectionTypeCodeNavigation).WithMany(p => p.PimsLeaseChklstItemTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LCKSTY_PIM_LCKITY_FK");
        });

        modelBuilder.Entity<PimsLeaseChklstSectionType>(entity =>
        {
            entity.HasKey(e => e.LeaseChklstSectionTypeCode).HasName("LCKSTY_PK");

            entity.ToTable("PIMS_LEASE_CHKLST_SECTION_TYPE", tb =>
                {
                    tb.HasComment("Table that contains the lease & license checklist sctions that are presented to the user through dynamically building the input form.");
                    tb.HasTrigger("PIMS_LCKSTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_LCKSTY_I_S_U_TR");
                });

            entity.Property(e => e.LeaseChklstSectionTypeCode).HasComment("Lease & license checklist section code value.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Lease & license checklist section descriptive text presented to the user.");
            entity.Property(e => e.DisplayOrder).HasComment("Specifies the order that the lease & license checklist sections are presented to the user.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the lease & license checklist section is able to be presented to the user via the input form.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the lease & license checklist section is removed from the input form.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is currently active.");
        });

        modelBuilder.Entity<PimsLeaseConsultation>(entity =>
        {
            entity.HasKey(e => e.LeaseConsultationId).HasName("LESCON_PK");

            entity.ToTable("PIMS_LEASE_CONSULTATION", tb =>
                {
                    tb.HasTrigger("PIMS_LESCON_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_LESCON_I_S_I_TR");
                    tb.HasTrigger("PIMS_LESCON_I_S_U_TR");
                });

            entity.Property(e => e.LeaseConsultationId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_CONSULTATION_ID_SEQ])")
                .HasComment("Generated surrogate primary key.");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that updated the record.");
            entity.Property(e => e.Comment).HasComment("Remarks / summary on the process or its results.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.ConsultationOutcomeTypeCode)
                .HasDefaultValue("INPROGRESS")
                .HasComment("Foreign key to the PIMS_CONSULTATION_OUTCOME_TYPE table.");
            entity.Property(e => e.ConsultationStatusTypeCode)
                .HasDefaultValue("UNKNOWN")
                .HasComment("Foreign key to the PIMS_CONSULTATION_STATUS_TYPE table.");
            entity.Property(e => e.ConsultationTypeCode).HasComment("Foreign key to the PIMS_CONSULTATION_TYPE table.");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("if the record is disabled.");
            entity.Property(e => e.IsResponseReceived).HasComment("Has the consultation request response been received?");
            entity.Property(e => e.LeaseId).HasComment("Foreign key to the PIMS_LEASE table.");
            entity.Property(e => e.OrganizationId).HasComment("Consultation contact organization.");
            entity.Property(e => e.OtherDescription).HasComment("Description for the approval / consultation when \"Other\" consultation type is selected.");
            entity.Property(e => e.PersonId).HasComment("Consultation contact person exclusive of an organization.");
            entity.Property(e => e.PrimaryContactId).HasComment("Consultation contact person within the organization.");
            entity.Property(e => e.RequestedOn).HasComment("Date that the approval / consultation request was sent.");
            entity.Property(e => e.ResponseReceivedDate).HasComment("Date that the consultation request response was received.");

            entity.HasOne(d => d.ConsultationOutcomeTypeCodeNavigation).WithMany(p => p.PimsLeaseConsultations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_OUTCMT_PIM_LESCON_FK");

            entity.HasOne(d => d.ConsultationStatusTypeCodeNavigation).WithMany(p => p.PimsLeaseConsultations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_CONSTY_PIM_LESCON_FK");

            entity.HasOne(d => d.ConsultationTypeCodeNavigation).WithMany(p => p.PimsLeaseConsultations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_CONTYP_PIM_LESCON_FK");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsLeaseConsultations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LEASE_PIM_LESCON_FK");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsLeaseConsultations).HasConstraintName("PIM_ORG_PIM_LESCON_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsLeaseConsultationPeople).HasConstraintName("PIM_PERSON_PIM_LESCON_FK");

            entity.HasOne(d => d.PrimaryContact).WithMany(p => p.PimsLeaseConsultationPrimaryContacts).HasConstraintName("PIM_PERSON_PIM_LESCON_ORGCON_FK");
        });

        modelBuilder.Entity<PimsLeaseConsultationHist>(entity =>
        {
            entity.HasKey(e => e.LeaseConsultationHistId).HasName("PIMS_LESCON_H_PK");

            entity.Property(e => e.LeaseConsultationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_CONSULTATION_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsLeaseDocument>(entity =>
        {
            entity.HasKey(e => e.LeaseDocumentId).HasName("LESDOC_PK");

            entity.ToTable("PIMS_LEASE_DOCUMENT", tb =>
                {
                    tb.HasComment("Defines the relationship betwwen a lease and a document.");
                    tb.HasTrigger("PIMS_LESDOC_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_LESDOC_I_S_I_TR");
                    tb.HasTrigger("PIMS_LESDOC_I_S_U_TR");
                });

            entity.Property(e => e.LeaseDocumentId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_DOCUMENT_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Document).WithMany(p => p.PimsLeaseDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DOCMNT_PIM_LESDOC_FK");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsLeaseDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LEASE_PIM_LESDOC_FK");
        });

        modelBuilder.Entity<PimsLeaseDocumentHist>(entity =>
        {
            entity.HasKey(e => e.LeaseDocumentHistId).HasName("PIMS_LESDOC_H_PK");

            entity.Property(e => e.LeaseDocumentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_DOCUMENT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsLeaseHist>(entity =>
        {
            entity.HasKey(e => e.LeaseHistId).HasName("PIMS_LEASE_H_PK");

            entity.Property(e => e.LeaseHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsLeaseInitiatorType>(entity =>
        {
            entity.HasKey(e => e.LeaseInitiatorTypeCode).HasName("LINITT_PK");

            entity.ToTable("PIMS_LEASE_INITIATOR_TYPE", tb =>
                {
                    tb.HasComment("Describes the initiator of the lease");
                    tb.HasTrigger("PIMS_LINITT_I_S_I_TR");
                    tb.HasTrigger("PIMS_LINITT_I_S_U_TR");
                });

            entity.Property(e => e.LeaseInitiatorTypeCode).HasComment("Code value of the initiator of the lease");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the initiator of the lease");
        });

        modelBuilder.Entity<PimsLeaseLeasePurpose>(entity =>
        {
            entity.HasKey(e => e.LeaseLeasePurposeId).HasName("LLPURP_PK");

            entity.ToTable("PIMS_LEASE_LEASE_PURPOSE", tb =>
                {
                    tb.HasTrigger("PIMS_LLPURP_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_LLPURP_I_S_I_TR");
                    tb.HasTrigger("PIMS_LLPURP_I_S_U_TR");
                });

            entity.Property(e => e.LeaseLeasePurposeId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_LEASE_PURPOSE_ID_SEQ])")
                .HasComment("PK Generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.LeaseId).HasComment("FK Foreign key to the PIMS_LEASE table.");
            entity.Property(e => e.LeasePurposeOtherDesc).HasComment("User-specified lease purpose description not included in standard set of lease purposes");
            entity.Property(e => e.LeasePurposeTypeCode).HasComment("FK Foreign key to the PIMS_LEASE_PURPOSE_TYPE table.");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsLeaseLeasePurposes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LEASE_PIM_LLPURP_FK");

            entity.HasOne(d => d.LeasePurposeTypeCodeNavigation).WithMany(p => p.PimsLeaseLeasePurposes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LPRPTY_PIM_LLPURP_FK");
        });

        modelBuilder.Entity<PimsLeaseLeasePurposeHist>(entity =>
        {
            entity.HasKey(e => e.LeaseLeasePurposeHistId).HasName("PIMS_LLPURP_H_PK");

            entity.Property(e => e.LeaseLeasePurposeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_LEASE_PURPOSE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsLeaseLicenseType>(entity =>
        {
            entity.HasKey(e => e.LeaseLicenseTypeCode).HasName("LELIST_PK");

            entity.ToTable("PIMS_LEASE_LICENSE_TYPE", tb =>
                {
                    tb.HasTrigger("PIMS_LELIST_I_S_I_TR");
                    tb.HasTrigger("PIMS_LELIST_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsLeaseNote>(entity =>
        {
            entity.HasKey(e => e.LeaseNoteId).HasName("LESNOT_PK");

            entity.ToTable("PIMS_LEASE_NOTE", tb =>
                {
                    tb.HasComment("Defines the relationship betwwen a lease and a note.");
                    tb.HasTrigger("PIMS_LESNOT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_LESNOT_I_S_I_TR");
                    tb.HasTrigger("PIMS_LESNOT_I_S_U_TR");
                });

            entity.Property(e => e.LeaseNoteId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_NOTE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsLeaseNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LEASE_PIM_LESNOT_FK");

            entity.HasOne(d => d.Note).WithMany(p => p.PimsLeaseNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_NOTE_PIM_LESNOT_FK");
        });

        modelBuilder.Entity<PimsLeaseNoteHist>(entity =>
        {
            entity.HasKey(e => e.LeaseNoteHistId).HasName("PIMS_LESNOT_H_PK");

            entity.Property(e => e.LeaseNoteHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_NOTE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsLeasePayRvblType>(entity =>
        {
            entity.HasKey(e => e.LeasePayRvblTypeCode).HasName("LSPRTY_PK");

            entity.ToTable("PIMS_LEASE_PAY_RVBL_TYPE", tb =>
                {
                    tb.HasTrigger("PIMS_LSPRTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_LSPRTY_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsLeasePayment>(entity =>
        {
            entity.HasKey(e => e.LeasePaymentId).HasName("LSPYMT_PK");

            entity.ToTable("PIMS_LEASE_PAYMENT", tb =>
                {
                    tb.HasComment("Describes a payment associated with a lease term.");
                    tb.HasTrigger("PIMS_LSPYMT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_LSPYMT_I_S_I_TR");
                    tb.HasTrigger("PIMS_LSPYMT_I_S_U_TR");
                });

            entity.Property(e => e.LeasePaymentId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_PAYMENT_ID_SEQ])")
                .HasComment("Generated surrogate primary key.");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.LeasePaymentCategoryTypeCode).HasComment("Foreign key reference to the PIMS_LEASE_PAYMENT_CATEGORY_TYPE_CODE table.");
            entity.Property(e => e.LeasePaymentMethodTypeCode).HasComment("Foreign key reference to the PIMS_LEASE_PAYMENT_METHOD_TYPE_CODE table.");
            entity.Property(e => e.LeasePaymentStatusTypeCode).HasComment("Foreign key reference to the PIMS_LEASE_PAYMENT_STATUS_TYPE_CODE table.");
            entity.Property(e => e.LeasePeriodId).HasComment("Foreign key reference to the PIMS_LEASE_PERIOD table.");
            entity.Property(e => e.LeasePmtFreqTypeCode).HasComment("Foreign key reference to the PIMS_LEASE_PMT_FREQ_TYPE_CODE table.");
            entity.Property(e => e.Note).HasComment("Notes regarding this payment");
            entity.Property(e => e.PaymentAmountGst).HasComment("GST owing on payment if applicable");
            entity.Property(e => e.PaymentAmountPreTax).HasComment("Principal amount of the payment before applicable taxes");
            entity.Property(e => e.PaymentAmountPst).HasComment("PST owing on payment if applicable");
            entity.Property(e => e.PaymentAmountTotal).HasComment("Total amount of payment including principal plus all applicable taxes");
            entity.Property(e => e.PaymentReceivedDate).HasComment("Date the payment was received or sent");

            entity.HasOne(d => d.LeasePaymentCategoryTypeCodeNavigation).WithMany(p => p.PimsLeasePayments).HasConstraintName("PIM_LPCATT_PIM_LSPYMT_FK");

            entity.HasOne(d => d.LeasePaymentMethodTypeCodeNavigation).WithMany(p => p.PimsLeasePayments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LSPMMT_PIM_LSPYMT_FK");

            entity.HasOne(d => d.LeasePaymentStatusTypeCodeNavigation).WithMany(p => p.PimsLeasePayments).HasConstraintName("PIM_LPSTST_PIM_LSPYMT_FK");

            entity.HasOne(d => d.LeasePeriod).WithMany(p => p.PimsLeasePayments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LSTERM_PIM_LSPYMT_FK");

            entity.HasOne(d => d.LeasePmtFreqTypeCodeNavigation).WithMany(p => p.PimsLeasePayments).HasConstraintName("PIM_LSPMTF_PIM_LSPYMT_FK");
        });

        modelBuilder.Entity<PimsLeasePaymentCategoryType>(entity =>
        {
            entity.HasKey(e => e.LeasePaymentCategoryTypeCode).HasName("LPCATT_PK");

            entity.ToTable("PIMS_LEASE_PAYMENT_CATEGORY_TYPE", tb =>
                {
                    tb.HasComment("Describes the category of the lease payment (currently Base, Additional, or Variable).");
                    tb.HasTrigger("PIMS_LPCATT_I_S_I_TR");
                    tb.HasTrigger("PIMS_LPCATT_I_S_U_TR");
                });

            entity.Property(e => e.LeasePaymentCategoryTypeCode).HasComment("Payment category type code.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Payment category type description.");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the descriptions");
            entity.Property(e => e.IsDisabled).HasComment("Indicates that the record is disabled.");
        });

        modelBuilder.Entity<PimsLeasePaymentHist>(entity =>
        {
            entity.HasKey(e => e.LeasePaymentHistId).HasName("PIMS_LSPYMT_H_PK");

            entity.Property(e => e.LeasePaymentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_PAYMENT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsLeasePaymentMethodType>(entity =>
        {
            entity.HasKey(e => e.LeasePaymentMethodTypeCode).HasName("LSPMMT_PK");

            entity.ToTable("PIMS_LEASE_PAYMENT_METHOD_TYPE", tb =>
                {
                    tb.HasComment("Describes the type of payment method for a lease.");
                    tb.HasTrigger("PIMS_LSPMMT_I_S_I_TR");
                    tb.HasTrigger("PIMS_LSPMMT_I_S_U_TR");
                });

            entity.Property(e => e.LeasePaymentMethodTypeCode).HasComment("Payment method type code");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Payment method type description");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the descriptions");
            entity.Property(e => e.IsDisabled).HasComment("Is this code disabled?");
        });

        modelBuilder.Entity<PimsLeasePaymentStatusType>(entity =>
        {
            entity.HasKey(e => e.LeasePaymentStatusTypeCode).HasName("LPSTST_PK");

            entity.ToTable("PIMS_LEASE_PAYMENT_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Describes the status of forecast payments");
                    tb.HasTrigger("PIMS_LPSTST_I_S_I_TR");
                    tb.HasTrigger("PIMS_LPSTST_I_S_U_TR");
                });

            entity.Property(e => e.LeasePaymentStatusTypeCode).HasComment("Payment status type code");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Payment status type description");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the descriptions");
            entity.Property(e => e.IsDisabled).HasComment("Is this code disabled?");
        });

        modelBuilder.Entity<PimsLeasePeriod>(entity =>
        {
            entity.HasKey(e => e.LeasePeriodId).HasName("LSPERD_PK");

            entity.ToTable("PIMS_LEASE_PERIOD", tb =>
                {
                    tb.HasComment("Describes a duration period for the associated lease.");
                    tb.HasTrigger("PIMS_LSPERD_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_LSPERD_I_S_I_TR");
                    tb.HasTrigger("PIMS_LSPERD_I_S_U_TR");
                });

            entity.Property(e => e.LeasePeriodId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_PERIOD_ID_SEQ])")
                .HasComment("Generated surrogate primary key.");
            entity.Property(e => e.AddlRentAgreedPmt).HasComment("Indicates the agreed-to variable additional rent payment amount.");
            entity.Property(e => e.AddlRentFreq).HasComment("Foreign key reference to the PIMS_LEASE_PMT_FREQ_TYPE table.");
            entity.Property(e => e.AddlRentGstAmount).HasComment("GST dollar amount for the additional rent.");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.GstAmount).HasComment("Calculated/entered GST portion of the payment.  Can be overridden by the user.");
            entity.Property(e => e.IsAddlRentSubjectToGst).HasComment("Is the variable additional rent payment subject to GST?");
            entity.Property(e => e.IsFlexibleDuration).HasComment("Indicates whether the period duration is fixed (FALSE) or flexible (TRUE).  Fixed (FALSE) is the default value.");
            entity.Property(e => e.IsGstEligible).HasComment("Is the lease subject to GST?");
            entity.Property(e => e.IsPeriodExercised).HasComment("Has the lease period been exercised?");
            entity.Property(e => e.IsVariablePayment).HasComment("Indicates whether the payment type is predetermined (FALSE) or variable (TRUE).  Predetermined (FALSE) is the default value.");
            entity.Property(e => e.IsVblRentSubjectToGst).HasComment("Is the variable rent payment subject to GST?");
            entity.Property(e => e.LeaseId).HasComment("Foreign key reference to the PIMS_LEASE table.");
            entity.Property(e => e.LeasePeriodStatusTypeCode).HasComment("Foreign key reference to the PIMS_LEASE_PERIOD_STATUS_TYPE table.");
            entity.Property(e => e.LeasePmtFreqTypeCode).HasComment("Foreign key reference to the PIMS_LEASE_PMT_FREQ_TYPE table.");
            entity.Property(e => e.PaymentAmount).HasComment("Agreed-to payment amount (exclusive of GST)");
            entity.Property(e => e.PaymentDueDate).HasComment("Anecdotal description of payment due date (e.g. 1st of month, end of month)");
            entity.Property(e => e.PaymentNote).HasComment("Notes regarding payment status for the lease period");
            entity.Property(e => e.PeriodExpiryDate).HasComment("Expiry date of the current period of the lease/licence");
            entity.Property(e => e.PeriodRenewalDate).HasComment("Renewal date of the current period of the lease/licence");
            entity.Property(e => e.PeriodStartDate).HasComment("Start date of the current period of the lease/licence");
            entity.Property(e => e.VblRentAgreedPmt).HasComment("Indicates the agreed-to variable rent payment amount.");
            entity.Property(e => e.VblRentFreq).HasComment("Foreign key reference to the PIMS_LEASE_PMT_FREQ_TYPE table.");
            entity.Property(e => e.VblRentGstAmount).HasComment("GST dollar amount for the variable rent.");

            entity.HasOne(d => d.AddlRentFreqNavigation).WithMany(p => p.PimsLeasePeriodAddlRentFreqNavigations).HasConstraintName("PIM_LSPMTF_ADDL_RENT_FREQ_FK");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsLeasePeriods)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LEASE_PIM_LSTERM_FK");

            entity.HasOne(d => d.LeasePeriodStatusTypeCodeNavigation).WithMany(p => p.PimsLeasePeriods).HasConstraintName("PIM_LTRMST_PIM_LSTERM_FK");

            entity.HasOne(d => d.LeasePmtFreqTypeCodeNavigation).WithMany(p => p.PimsLeasePeriodLeasePmtFreqTypeCodeNavigations).HasConstraintName("PIM_LSPMTF_PIM_LSTERM_FK");

            entity.HasOne(d => d.VblRentFreqNavigation).WithMany(p => p.PimsLeasePeriodVblRentFreqNavigations).HasConstraintName("PIM_LSPMTF_VBL_RENT_FREQ_FK");
        });

        modelBuilder.Entity<PimsLeasePeriodHist>(entity =>
        {
            entity.HasKey(e => e.LeasePeriodHistId).HasName("PIMS_LSPERD_H_PK");

            entity.Property(e => e.LeasePeriodHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_PERIOD_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsLeasePeriodStatusType>(entity =>
        {
            entity.HasKey(e => e.LeasePeriodStatusTypeCode).HasName("LSPRST_PK");

            entity.ToTable("PIMS_LEASE_PERIOD_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Describes the status of the lease period.");
                    tb.HasTrigger("PIMS_LSPRST_I_S_I_TR");
                    tb.HasTrigger("PIMS_LSPRST_I_S_U_TR");
                });

            entity.Property(e => e.LeasePeriodStatusTypeCode).HasComment("Code value of the status of the lease period.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the status of the lease period.");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates that the record is disabled.");
        });

        modelBuilder.Entity<PimsLeasePmtFreqType>(entity =>
        {
            entity.HasKey(e => e.LeasePmtFreqTypeCode).HasName("LSPMTF_PK");

            entity.ToTable("PIMS_LEASE_PMT_FREQ_TYPE", tb =>
                {
                    tb.HasComment("Describes the frequency of payments for a lease.");
                    tb.HasTrigger("PIMS_LSPMTF_I_S_I_TR");
                    tb.HasTrigger("PIMS_LSPMTF_I_S_U_TR");
                });

            entity.Property(e => e.LeasePmtFreqTypeCode).HasComment("Payment frequency type code");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Payment frequency type code description");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates that the record is disabled.");
        });

        modelBuilder.Entity<PimsLeaseProgramType>(entity =>
        {
            entity.HasKey(e => e.LeaseProgramTypeCode).HasName("LSPRGT_PK");

            entity.ToTable("PIMS_LEASE_PROGRAM_TYPE", tb =>
                {
                    tb.HasComment("Describes the program type associated with a lease.");
                    tb.HasTrigger("PIMS_LSPRGT_I_S_I_TR");
                    tb.HasTrigger("PIMS_LSPRGT_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsLeasePurposeType>(entity =>
        {
            entity.HasKey(e => e.LeasePurposeTypeCode).HasName("LPRPTY_PK");

            entity.ToTable("PIMS_LEASE_PURPOSE_TYPE", tb =>
                {
                    tb.HasComment("Describes the purpose type associated with a lease.");
                    tb.HasTrigger("PIMS_LPRPTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_LPRPTY_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsLeaseRenewal>(entity =>
        {
            entity.HasKey(e => e.LeaseRenewalId).HasName("LSRNWL_PK");

            entity.ToTable("PIMS_LEASE_RENEWAL", tb =>
                {
                    tb.HasComment("Table containing lease renewal options.");
                    tb.HasTrigger("PIMS_LSRNWL_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_LSRNWL_I_S_I_TR");
                    tb.HasTrigger("PIMS_LSRNWL_I_S_U_TR");
                });

            entity.Property(e => e.LeaseRenewalId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_RENEWAL_ID_SEQ])")
                .HasComment("Generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that updated the record.");
            entity.Property(e => e.CommencementDt).HasComment("Date that the lease lease begins.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.ExpiryDt).HasComment("Date that the lease lease ends.");
            entity.Property(e => e.IsExercised).HasComment("Indicates if the lease renewal was exercised.");
            entity.Property(e => e.LeaseId).HasComment("Foreign key to the PIMS_LEASE table.");
            entity.Property(e => e.RenewalNote).HasComment("Notes pertaining to the lease reewal.");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsLeaseRenewals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LEASE_PIM_LSRNWL_FK");
        });

        modelBuilder.Entity<PimsLeaseRenewalHist>(entity =>
        {
            entity.HasKey(e => e.LeaseRenewalHistId).HasName("PIMS_LSRNWL_H_PK");

            entity.Property(e => e.LeaseRenewalHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_RENEWAL_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsLeaseResponsibilityType>(entity =>
        {
            entity.HasKey(e => e.LeaseResponsibilityTypeCode).HasName("LRESPT_PK");

            entity.ToTable("PIMS_LEASE_RESPONSIBILITY_TYPE", tb =>
                {
                    tb.HasComment("Describes which organization is responsible for this lease");
                    tb.HasTrigger("PIMS_LRESPT_I_S_I_TR");
                    tb.HasTrigger("PIMS_LRESPT_I_S_U_TR");
                });

            entity.Property(e => e.LeaseResponsibilityTypeCode).HasComment("Code value of the organization responsible for this lease");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the organization responsible for this lease");
        });

        modelBuilder.Entity<PimsLeaseStakeholder>(entity =>
        {
            entity.HasKey(e => e.LeaseStakeholderId).HasName("LSTKHL_PK");

            entity.ToTable("PIMS_LEASE_STAKEHOLDER", tb =>
                {
                    tb.HasComment("Associates a tenant with a lease");
                    tb.HasTrigger("PIMS_LSTKHL_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_LSTKHL_I_S_I_TR");
                    tb.HasTrigger("PIMS_LSTKHL_I_S_U_TR");
                });

            entity.Property(e => e.LeaseStakeholderId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_STAKEHOLDER_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.LeaseStakeholderTypeCode).HasDefaultValue("UNK");
            entity.Property(e => e.LessorTypeCode).HasDefaultValue("UNK");
            entity.Property(e => e.Note).HasComment("Notes associated with the lease/tenant relationship.");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsLeaseStakeholders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LEASE_PIM_TENANT_FK");

            entity.HasOne(d => d.LeaseStakeholderTypeCodeNavigation).WithMany(p => p.PimsLeaseStakeholders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_TENTYP_PIM_TENANT_FK");

            entity.HasOne(d => d.LessorTypeCodeNavigation).WithMany(p => p.PimsLeaseStakeholders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LSSRTY_PIM_TENANT_FK");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsLeaseStakeholders).HasConstraintName("PIM_ORG_PIM_TENANT_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsLeaseStakeholderPeople).HasConstraintName("PIM_PERSON_PIM_TENANT_FK");

            entity.HasOne(d => d.PrimaryContact).WithMany(p => p.PimsLeaseStakeholderPrimaryContacts).HasConstraintName("PIM_PERSON_PIM_PRIMARY_CONTACT_FK");
        });

        modelBuilder.Entity<PimsLeaseStakeholderCompReq>(entity =>
        {
            entity.HasKey(e => e.LeaseStakeholderCompReqId).HasName("LSKCRQ_PK");

            entity.ToTable("PIMS_LEASE_STAKEHOLDER_COMP_REQ", tb =>
                {
                    tb.HasComment("Desribes the relationship between a lease stakeholder and a compensation requisition.");
                    tb.HasTrigger("PIMS_LSKCRQ_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_LSKCRQ_I_S_I_TR");
                    tb.HasTrigger("PIMS_LSKCRQ_I_S_U_TR");
                });

            entity.Property(e => e.LeaseStakeholderCompReqId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_STAKEHOLDER_COMP_REQ_ID_SEQ])")
                .HasComment("Generated surrogate primary key.");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.CompensationRequisitionId).HasComment("Foreign key to the PIMS_COMPENSATION_REQUISITION table.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.LeaseStakeholderId).HasComment("Foreign key to the LEASE_STAKEHOLDER table.");

            entity.HasOne(d => d.CompensationRequisition).WithMany(p => p.PimsLeaseStakeholderCompReqs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_CMPREQ_PIM_LSKCRQ_FK");

            entity.HasOne(d => d.LeaseStakeholder).WithMany(p => p.PimsLeaseStakeholderCompReqs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LSTKHL_PIM_LSKCRQ_FK");
        });

        modelBuilder.Entity<PimsLeaseStakeholderCompReqHist>(entity =>
        {
            entity.HasKey(e => e.LeaseStakeholderCompReqHistId).HasName("PIMS_LSKCRQ_H_PK");

            entity.Property(e => e.LeaseStakeholderCompReqHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_STAKEHOLDER_COMP_REQ_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsLeaseStakeholderHist>(entity =>
        {
            entity.HasKey(e => e.LeaseStakeholderHistId).HasName("PIMS_LSTKHL_H_PK");

            entity.Property(e => e.LeaseStakeholderHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_STAKEHOLDER_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsLeaseStakeholderType>(entity =>
        {
            entity.HasKey(e => e.LeaseStakeholderTypeCode).HasName("STKHLT_PK");

            entity.ToTable("PIMS_LEASE_STAKEHOLDER_TYPE", tb =>
                {
                    tb.HasComment("Code table describing the type of tenant on a lease.");
                    tb.HasTrigger("PIMS_STKHLT_I_S_I_TR");
                    tb.HasTrigger("PIMS_STKHLT_I_S_U_TR");
                });

            entity.Property(e => e.LeaseStakeholderTypeCode)
                .HasDefaultValue("UNK")
                .HasComment("Code representing the types of stakeholders on a lease.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description)
                .HasDefaultValue("Unknown")
                .HasComment("Description of the types of stakeholders on a lease.");
            entity.Property(e => e.DisplayOrder).HasComment("Specifies a specific order to visually present the code.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is currently active.");
            entity.Property(e => e.IsPayableRelated)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code is related to payable leases.");
        });

        modelBuilder.Entity<PimsLeaseStatusType>(entity =>
        {
            entity.HasKey(e => e.LeaseStatusTypeCode).HasName("LSSTYP_PK");

            entity.ToTable("PIMS_LEASE_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Describes the status of the lease");
                    tb.HasTrigger("PIMS_LSSTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_LSSTYP_I_S_U_TR");
                });

            entity.Property(e => e.LeaseStatusTypeCode).HasComment("Code value of the status of the lease");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the status of the lease");
        });

        modelBuilder.Entity<PimsLessorType>(entity =>
        {
            entity.HasKey(e => e.LessorTypeCode).HasName("LSSRTY_PK");

            entity.ToTable("PIMS_LESSOR_TYPE", tb =>
                {
                    tb.HasComment("Code table describing the type of lessor on a lease.");
                    tb.HasTrigger("PIMS_LSSRTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_LSSRTY_I_S_U_TR");
                });

            entity.Property(e => e.LessorTypeCode)
                .HasDefaultValue("UNK")
                .HasComment("Code representing the types of lessors on a lease.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description)
                .HasDefaultValue("Unknown")
                .HasComment("Description of the types of lessors on a lease.");
            entity.Property(e => e.DisplayOrder).HasComment("Specifies a specific order to visually present the code.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is currently active.");
        });

        modelBuilder.Entity<PimsLetterType>(entity =>
        {
            entity.HasKey(e => e.LetterTypeCode).HasName("LTRTYP_PK");

            entity.ToTable("PIMS_LETTER_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the letter type.  This is an unassociated table that is used in the UI to populate JSON attributes.");
                    tb.HasTrigger("PIMS_LTRTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_LTRTYP_I_S_U_TR");
                });

            entity.Property(e => e.LetterTypeCode).HasComment("Code value for the letter type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the letter type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsNote>(entity =>
        {
            entity.HasKey(e => e.NoteId).HasName("NOTE_PK");

            entity.ToTable("PIMS_NOTE", tb =>
                {
                    tb.HasComment("Table to contain all applicable notes for the PIMS PSP system.");
                    tb.HasTrigger("PIMS_NOTE_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_NOTE_I_S_I_TR");
                    tb.HasTrigger("PIMS_NOTE_I_S_U_TR");
                });

            entity.Property(e => e.NoteId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_NOTE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.IsSystemGenerated).HasComment("Indicatesd if this note is system-generated.");
            entity.Property(e => e.NoteTxt)
                .HasDefaultValue("<Empty>")
                .HasComment("Contents of the note.");
        });

        modelBuilder.Entity<PimsNoteHist>(entity =>
        {
            entity.HasKey(e => e.NoteHistId).HasName("PIMS_NOTE_H_PK");

            entity.Property(e => e.NoteHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_NOTE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsOrgIdentifierType>(entity =>
        {
            entity.HasKey(e => e.OrgIdentifierTypeCode).HasName("ORGIDT_PK");

            entity.ToTable("PIMS_ORG_IDENTIFIER_TYPE", tb =>
                {
                    tb.HasTrigger("PIMS_ORGIDT_I_S_I_TR");
                    tb.HasTrigger("PIMS_ORGIDT_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsOrganization>(entity =>
        {
            entity.HasKey(e => e.OrganizationId).HasName("ORG_PK");

            entity.ToTable("PIMS_ORGANIZATION", tb =>
                {
                    tb.HasComment("Information related to an organization identified in the PSP system.");
                    tb.HasTrigger("PIMS_ORG_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ORG_I_S_I_TR");
                    tb.HasTrigger("PIMS_ORG_I_S_U_TR");
                });

            entity.Property(e => e.OrganizationId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ORGANIZATION_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.IncorporationNumber).HasComment("Incorporation number of the orgnization");

            entity.HasOne(d => d.DistrictCodeNavigation).WithMany(p => p.PimsOrganizations).HasConstraintName("PIM_DSTRCT_PIM_ORG_FK");

            entity.HasOne(d => d.OrgIdentifierTypeCodeNavigation).WithMany(p => p.PimsOrganizations).HasConstraintName("PIM_ORGIDT_PIM_ORG_FK");

            entity.HasOne(d => d.OrganizationTypeCodeNavigation).WithMany(p => p.PimsOrganizations).HasConstraintName("PIM_ORGTYP_PIM_ORG_FK");

            entity.HasOne(d => d.PrntOrganization).WithMany(p => p.InversePrntOrganization).HasConstraintName("PIM_ORG_PIM_PRNT_ORG_FK");

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.PimsOrganizations).HasConstraintName("PIM_REGION_PIM_ORG_FK");
        });

        modelBuilder.Entity<PimsOrganizationAddress>(entity =>
        {
            entity.HasKey(e => e.OrganizationAddressId).HasName("ORGADD_PK");

            entity.ToTable("PIMS_ORGANIZATION_ADDRESS", tb =>
                {
                    tb.HasComment("An associative entity to define multiple addresses for a person.");
                    tb.HasTrigger("PIMS_ORGADD_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ORGADD_I_S_I_TR");
                    tb.HasTrigger("PIMS_ORGADD_I_S_U_TR");
                });

            entity.Property(e => e.OrganizationAddressId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ORGANIZATION_ADDRESS_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Address).WithMany(p => p.PimsOrganizationAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ADDRSS_PIM_ORGADD_FK");

            entity.HasOne(d => d.AddressUsageTypeCodeNavigation).WithMany(p => p.PimsOrganizationAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ADUSGT_PIM_ORGADD_FK");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsOrganizationAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ORG_PIM_ORGADD_FK");
        });

        modelBuilder.Entity<PimsOrganizationAddressHist>(entity =>
        {
            entity.HasKey(e => e.OrganizationAddressHistId).HasName("PIMS_ORGADD_H_PK");

            entity.Property(e => e.OrganizationAddressHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ORGANIZATION_ADDRESS_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsOrganizationHist>(entity =>
        {
            entity.HasKey(e => e.OrganizationHistId).HasName("PIMS_ORG_H_PK");

            entity.Property(e => e.OrganizationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ORGANIZATION_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsOrganizationType>(entity =>
        {
            entity.HasKey(e => e.OrganizationTypeCode).HasName("ORGTYP_PK");

            entity.ToTable("PIMS_ORGANIZATION_TYPE", tb =>
                {
                    tb.HasTrigger("PIMS_ORGTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_ORGTYP_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsPaymentItemType>(entity =>
        {
            entity.HasKey(e => e.PaymentItemTypeCode).HasName("PMTITM_PK");

            entity.ToTable("PIMS_PAYMENT_ITEM_TYPE", tb =>
                {
                    tb.HasComment("Table that contains the codes and associated descriptions of the payment item types, such as market value, temporary SRW, and disturbance damages.");
                    tb.HasTrigger("PIMS_PMTITM_I_S_I_TR");
                    tb.HasTrigger("PIMS_PMTITM_I_S_U_TR");
                });

            entity.Property(e => e.PaymentItemTypeCode).HasComment("Codified version of the payment item types, such as market value, temporary SRW, and disturbance damages.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the payment item types, such as market value, temporary SRW, and disturbance damages.");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsPerson>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PERSON_PK");

            entity.ToTable("PIMS_PERSON", tb =>
                {
                    tb.HasTrigger("PIMS_PERSON_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PERSON_I_S_I_TR");
                    tb.HasTrigger("PIMS_PERSON_I_S_U_TR");
                });

            entity.Property(e => e.PersonId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.UseOrganizationAddress).HasDefaultValue(false);
        });

        modelBuilder.Entity<PimsPersonAddress>(entity =>
        {
            entity.HasKey(e => e.PersonAddressId).HasName("PERADD_PK");

            entity.ToTable("PIMS_PERSON_ADDRESS", tb =>
                {
                    tb.HasComment("An associative entity to define multiple addresses for a person.");
                    tb.HasTrigger("PIMS_PERADD_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PERADD_I_S_I_TR");
                    tb.HasTrigger("PIMS_PERADD_I_S_U_TR");
                });

            entity.Property(e => e.PersonAddressId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ADDRESS_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Address).WithMany(p => p.PimsPersonAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ADDRSS_PIM_PERADD_FK");

            entity.HasOne(d => d.AddressUsageTypeCodeNavigation).WithMany(p => p.PimsPersonAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ADUSGT_PIM_PERADD_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsPersonAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PERSON_PIM_PERADD_FK");
        });

        modelBuilder.Entity<PimsPersonAddressHist>(entity =>
        {
            entity.HasKey(e => e.PersonAddressHistId).HasName("PIMS_PERADD_H_PK");

            entity.Property(e => e.PersonAddressHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ADDRESS_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPersonContactVw>(entity =>
        {
            entity.ToView("PIMS_PERSON_CONTACT_VW");
        });

        modelBuilder.Entity<PimsPersonHist>(entity =>
        {
            entity.HasKey(e => e.PersonHistId).HasName("PIMS_PERSON_H_PK");

            entity.Property(e => e.PersonHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPersonOrganization>(entity =>
        {
            entity.HasKey(e => e.PersonOrganizationId).HasName("PERORG_PK");

            entity.ToTable("PIMS_PERSON_ORGANIZATION", tb =>
                {
                    tb.HasTrigger("PIMS_PERORG_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PERORG_I_S_I_TR");
                    tb.HasTrigger("PIMS_PERORG_I_S_U_TR");
                });

            entity.Property(e => e.PersonOrganizationId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ORGANIZATION_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsPersonOrganizations).HasConstraintName("PIM_ORG_PIM_PERORG_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsPersonOrganizations).HasConstraintName("PIM_PERSON_PIM_PERORG_FK");
        });

        modelBuilder.Entity<PimsPersonOrganizationHist>(entity =>
        {
            entity.HasKey(e => e.PersonOrganizationHistId).HasName("PIMS_PERORG_H_PK");

            entity.Property(e => e.PersonOrganizationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ORGANIZATION_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPphStatusType>(entity =>
        {
            entity.HasKey(e => e.PphStatusTypeCode).HasName("PPHSTT_PK");

            entity.ToTable("PIMS_PPH_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe the Provincial Public Highway status.");
                    tb.HasTrigger("PIMS_PPHSTT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PPHSTT_I_S_U_TR");
                });

            entity.Property(e => e.PphStatusTypeCode).HasComment("Code indicating the Provincial Public Highway status");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the code indicating the purpose of the property research");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsPrfPropResearchPurposeType>(entity =>
        {
            entity.HasKey(e => e.PrfPropResearchPurposeId).HasName("PRSPRP_PK");

            entity.ToTable("PIMS_PRF_PROP_RESEARCH_PURPOSE_TYPE", tb =>
                {
                    tb.HasTrigger("PIMS_PRSPRP_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRSPRP_I_S_U_TR");
                });

            entity.Property(e => e.PrfPropResearchPurposeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PRF_PROP_RESEARCH_PURPOSE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.PropResearchPurposeTypeCodeNavigation).WithMany(p => p.PimsPrfPropResearchPurposeTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_RRESPT_PIM_PRSPRP_FK");

            entity.HasOne(d => d.PropertyResearchFile).WithMany(p => p.PimsPrfPropResearchPurposeTypes).HasConstraintName("PIM_PRSCRC_PIM_PRSPRP_FK");
        });

        modelBuilder.Entity<PimsProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRODCT_PK");

            entity.ToTable("PIMS_PRODUCT", tb =>
                {
                    tb.HasComment("Code and description of a project.");
                    tb.HasTrigger("PIMS_PRODCT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRODCT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRODCT_I_S_U_TR");
                });

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PRODUCT_ID_SEQ])")
                .HasComment("System-generated primary key.");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Code).HasComment("Product number.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.CostEstimate).HasComment("Estimate cost of the product.");
            entity.Property(e => e.CostEstimateDate).HasComment("Date the product cost was estimated.");
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Product description.");
            entity.Property(e => e.Objective).HasComment("Product objective(s).");
            entity.Property(e => e.Scope).HasComment("Product scope.");
            entity.Property(e => e.StartDate).HasComment("Product start date.");
        });

        modelBuilder.Entity<PimsProductHist>(entity =>
        {
            entity.HasKey(e => e.ProductHistId).HasName("PIMS_PRODCT_H_PK");

            entity.Property(e => e.ProductHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PRODUCT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsProject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PROJCT_PK");

            entity.ToTable("PIMS_PROJECT", tb =>
                {
                    tb.HasComment("Code and description of a project.");
                    tb.HasTrigger("PIMS_PROJCT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PROJCT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PROJCT_I_S_U_TR");
                });

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_ID_SEQ])")
                .HasComment("System-generated primary key.");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Code).HasComment("Project number.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description)
                .HasDefaultValue("<Empty>")
                .HasComment("Project description.");
            entity.Property(e => e.Note).HasComment("Descriptive note relevant to the project.");
            entity.Property(e => e.ProjectStatusTypeCode).HasDefaultValue("AC");
            entity.Property(e => e.RegionCode).HasDefaultValue((short)4);

            entity.HasOne(d => d.BusinessFunctionCode).WithMany(p => p.PimsProjects).HasConstraintName("PIM_BIZFCN_PIM_PROJCT_FK");

            entity.HasOne(d => d.CostTypeCode).WithMany(p => p.PimsProjects).HasConstraintName("PIM_COSTYP_PIM_PROJCT_FK");

            entity.HasOne(d => d.ProjectStatusTypeCodeNavigation).WithMany(p => p.PimsProjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRJSTS_PIM_PROJCT_FK");

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.PimsProjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_REGION_PIM_PROJCT_FK");

            entity.HasOne(d => d.WorkActivityCode).WithMany(p => p.PimsProjects).HasConstraintName("PIM_WRKACT_PIM_PROJCT_FK");
        });

        modelBuilder.Entity<PimsProjectDocument>(entity =>
        {
            entity.HasKey(e => e.ProjectDocumentId).HasName("PRJDOC_PK");

            entity.ToTable("PIMS_PROJECT_DOCUMENT", tb =>
                {
                    tb.HasTrigger("PIMS_PRJDOC_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRJDOC_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRJDOC_I_S_U_TR");
                });

            entity.Property(e => e.ProjectDocumentId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_DOCUMENT_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Document).WithMany(p => p.PimsProjectDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DOCMNT_PIM_PRJDOC_FK");

            entity.HasOne(d => d.Project).WithMany(p => p.PimsProjectDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PROJCT_PIM_PRJDOC_FK");
        });

        modelBuilder.Entity<PimsProjectDocumentHist>(entity =>
        {
            entity.HasKey(e => e.ProjectDocumentHistId).HasName("PIMS_PRJDOC_H_PK");

            entity.Property(e => e.ProjectDocumentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_DOCUMENT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsProjectHist>(entity =>
        {
            entity.HasKey(e => e.ProjectHistId).HasName("PIMS_PROJCT_H_PK");

            entity.Property(e => e.ProjectHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsProjectNote>(entity =>
        {
            entity.HasKey(e => e.ProjectNoteId).HasName("PRJNOT_PK");

            entity.ToTable("PIMS_PROJECT_NOTE", tb =>
                {
                    tb.HasTrigger("PIMS_PRJNOT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRJNOT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRJNOT_I_S_U_TR");
                });

            entity.Property(e => e.ProjectNoteId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_NOTE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Note).WithMany(p => p.PimsProjectNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_NOTE_PIM_PRJNOT_FK");

            entity.HasOne(d => d.Project).WithMany(p => p.PimsProjectNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PROJCT_PIM_PRJNOT_FK");
        });

        modelBuilder.Entity<PimsProjectNoteHist>(entity =>
        {
            entity.HasKey(e => e.ProjectNoteHistId).HasName("PIMS_PRJNOT_H_PK");

            entity.Property(e => e.ProjectNoteHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_NOTE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsProjectPerson>(entity =>
        {
            entity.HasKey(e => e.ProjectPersonId).HasName("PRJPER_PK");

            entity.ToTable("PIMS_PROJECT_PERSON", tb =>
                {
                    tb.HasComment("Entity associating a paerson to a project.");
                    tb.HasTrigger("PIMS_PRJPER_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRJPER_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRJPER_I_S_U_TR");
                });

            entity.Property(e => e.ProjectPersonId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_PERSON_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the relationship is active.");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsProjectPeople)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PERSON_PIM_PRJPER_FK");

            entity.HasOne(d => d.Project).WithMany(p => p.PimsProjectPeople)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PROJCT_PIM_PRJPER_FK");

            entity.HasOne(d => d.ProjectPersonRoleTypeCodeNavigation).WithMany(p => p.PimsProjectPeople)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPRLT_PIM_PRJPER_FK");
        });

        modelBuilder.Entity<PimsProjectPersonHist>(entity =>
        {
            entity.HasKey(e => e.ProjectPersonHistId).HasName("PIMS_PRJPER_H_PK");

            entity.Property(e => e.ProjectPersonHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_PERSON_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsProjectPersonRoleType>(entity =>
        {
            entity.HasKey(e => e.ProjectPersonRoleTypeCode).HasName("PRPRLT_PK");

            entity.ToTable("PIMS_PROJECT_PERSON_ROLE_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the project/person role.  A given person is able to have multiple roles in the project.");
                    tb.HasTrigger("PIMS_PRPRLT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPRLT_I_S_U_TR");
                });

            entity.Property(e => e.ProjectPersonRoleTypeCode).HasComment("Code value of the project/person role.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the project/person role.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsProjectProduct>(entity =>
        {
            entity.HasKey(e => e.ProjectProductId).HasName("PRJPRD_PK");

            entity.ToTable("PIMS_PROJECT_PRODUCT", tb =>
                {
                    tb.HasTrigger("PIMS_PRJPRD_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRJPRD_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRJPRD_I_S_U_TR");
                });

            entity.Property(e => e.ProjectProductId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_PRODUCT_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Product).WithMany(p => p.PimsProjectProducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRODCT_PIM_PRJPRD_FK");

            entity.HasOne(d => d.Project).WithMany(p => p.PimsProjectProducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PROJCT_PIM_PRJPRD_FK");
        });

        modelBuilder.Entity<PimsProjectProductHist>(entity =>
        {
            entity.HasKey(e => e.ProjectProductHistId).HasName("PIMS_PRJPRD_H_PK");

            entity.Property(e => e.ProjectProductHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_PRODUCT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsProjectStatusType>(entity =>
        {
            entity.HasKey(e => e.ProjectStatusTypeCode).HasName("PRJSTY_PK");

            entity.ToTable("PIMS_PROJECT_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the project status.");
                    tb.HasTrigger("PIMS_PRJSTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRJSTY_I_S_U_TR");
                });

            entity.Property(e => e.ProjectStatusTypeCode).HasComment("Code value for the project status.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the project status.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsPropAcqFlCompReq>(entity =>
        {
            entity.HasKey(e => e.PropAcqFlCompReqId).HasName("PACMRQ_PK");

            entity.ToTable("PIMS_PROP_ACQ_FL_COMP_REQ", tb =>
                {
                    tb.HasTrigger("PIMS_PACMRQ_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PACMRQ_I_S_I_TR");
                    tb.HasTrigger("PIMS_PACMRQ_I_S_U_TR");
                });

            entity.Property(e => e.PropAcqFlCompReqId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_ACQ_FL_COMP_REQ_ID_SEQ])")
                .HasComment("Generated surrogate primary key.");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that updated the record.");
            entity.Property(e => e.CompensationRequisitionId).HasComment("Foreign key reference to the COMPENSATION_REQUISITION table.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.PropertyAcquisitionFileId).HasComment("Foreign key reference to the PROPERTY_ACQUISITION_FILE table.");

            entity.HasOne(d => d.CompensationRequisition).WithMany(p => p.PimsPropAcqFlCompReqs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_CMPREQ_PIM_PACMRQ_FK");

            entity.HasOne(d => d.PropertyAcquisitionFile).WithMany(p => p.PimsPropAcqFlCompReqs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRACQF_PIM_PACMRQ_FK");
        });

        modelBuilder.Entity<PimsPropAcqFlCompReqHist>(entity =>
        {
            entity.HasKey(e => e.PropAcqFlCompReqHistId).HasName("PIMS_PACMRQ_H_PK");

            entity.Property(e => e.PropAcqFlCompReqHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_ACQ_FL_COMP_REQ_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropActInvolvedParty>(entity =>
        {
            entity.HasKey(e => e.PropActInvolvedPartyId).HasName("PAINVP_PK");

            entity.ToTable("PIMS_PROP_ACT_INVOLVED_PARTY", tb =>
                {
                    tb.HasComment("Associates a property management activity to a vendor (many-to-many).");
                    tb.HasTrigger("PIMS_PAINVP_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PAINVP_I_S_I_TR");
                    tb.HasTrigger("PIMS_PAINVP_I_S_U_TR");
                });

            entity.Property(e => e.PropActInvolvedPartyId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_ACT_INVOLVED_PARTY_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsPropActInvolvedParties).HasConstraintName("PIM_ORG_PIM_PAINVP_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsPropActInvolvedParties).HasConstraintName("PIM_PERSON_PIM_PAINVP_FK");

            entity.HasOne(d => d.PimsPropertyActivity).WithMany(p => p.PimsPropActInvolvedParties).HasConstraintName("PIM_PRPACT_PIM_PAINVP_FK");
        });

        modelBuilder.Entity<PimsPropActInvolvedPartyHist>(entity =>
        {
            entity.HasKey(e => e.PropActInvolvedPartyHistId).HasName("PIMS_PAINVP_H_PK");

            entity.Property(e => e.PropActInvolvedPartyHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_ACT_INVOLVED_PARTY_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropActMinContact>(entity =>
        {
            entity.HasKey(e => e.PropActMinContactId).HasName("PRACMC_PK");

            entity.ToTable("PIMS_PROP_ACT_MIN_CONTACT", tb =>
                {
                    tb.HasComment("Associates a property management activity to a Ministry contact (many-to-many).");
                    tb.HasTrigger("PIMS_PRACMC_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRACMC_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRACMC_I_S_U_TR");
                });

            entity.Property(e => e.PropActMinContactId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_ACT_MIN_CONTACT_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsPropActMinContacts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PERSON_PIM_PRACMC_FK");

            entity.HasOne(d => d.PimsPropertyActivity).WithMany(p => p.PimsPropActMinContacts).HasConstraintName("PIM_PRPACT_PIM_PRACMC_FK");
        });

        modelBuilder.Entity<PimsPropActMinContactHist>(entity =>
        {
            entity.HasKey(e => e.PropActMinContactHistId).HasName("PIMS_PRACMC_H_PK");

            entity.Property(e => e.PropActMinContactHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_ACT_MIN_CONTACT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropInthldrInterestType>(entity =>
        {
            entity.HasKey(e => e.PropInthldrInterestTypeId).HasName("PIHITY_PK");

            entity.ToTable("PIMS_PROP_INTHLDR_INTEREST_TYPE", tb =>
                {
                    tb.HasTrigger("PIMS_PIHITY_I_S_I_TR");
                    tb.HasTrigger("PIMS_PIHITY_I_S_U_TR");
                });

            entity.Property(e => e.PropInthldrInterestTypeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_INTHLDR_INTEREST_TYPE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.InterestHolderInterestTypeCodeNavigation).WithMany(p => p.PimsPropInthldrInterestTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_IHINTT_PIM_PIHITY_FK");

            entity.HasOne(d => d.PimsInthldrPropInterest).WithMany(p => p.PimsPropInthldrInterestTypes).HasConstraintName("PIM_IHPRIN_PIM_PIHITY_FK");
        });

        modelBuilder.Entity<PimsPropLeaseCompReq>(entity =>
        {
            entity.HasKey(e => e.PropLeaseCompReqId).HasName("PLCMRQ_PK");

            entity.ToTable("PIMS_PROP_LEASE_COMP_REQ", tb =>
                {
                    tb.HasComment("Desribes the relationship between a leased property and a compensation requisition.");
                    tb.HasTrigger("PIMS_PLCMRQ_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PLCMRQ_I_S_I_TR");
                    tb.HasTrigger("PIMS_PLCMRQ_I_S_U_TR");
                });

            entity.Property(e => e.PropLeaseCompReqId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_LEASE_COMP_REQ_ID_SEQ])")
                .HasComment("Generated surrogate primary key");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created by the user.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("GUID of the user that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was updated by the user.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("User directory of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("GUID of the user that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user that updated the record.");
            entity.Property(e => e.CompensationRequisitionId).HasComment("Foreign key to the PIMS_COMPENSATION_REQUISITION table.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.PropertyLeaseId).HasComment("Foreign key to the PIMS_LEASE table.");

            entity.HasOne(d => d.CompensationRequisition).WithMany(p => p.PimsPropLeaseCompReqs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_CMPREQ_PIM_PLCMRQ_FK");

            entity.HasOne(d => d.PropertyLease).WithMany(p => p.PimsPropLeaseCompReqs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PROPLS_PIM_PLCMRQ_FK");
        });

        modelBuilder.Entity<PimsPropLeaseCompReqHist>(entity =>
        {
            entity.HasKey(e => e.PropLeaseCompReqHistId).HasName("PIMS_PLCMRQ_H_PK");

            entity.Property(e => e.PropLeaseCompReqHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_LEASE_COMP_REQ_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropMgmtActivityStatusType>(entity =>
        {
            entity.HasKey(e => e.PropMgmtActivityStatusTypeCode).HasName("PACSTY_PK");

            entity.ToTable("PIMS_PROP_MGMT_ACTIVITY_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe the status of the property management activity.");
                    tb.HasTrigger("PIMS_PACSTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_PACSTY_I_S_U_TR");
                });

            entity.Property(e => e.PropMgmtActivityStatusTypeCode).HasComment("Code representing the status of the property management activity.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the status of the property management status.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsPropMgmtActivitySubtype>(entity =>
        {
            entity.HasKey(e => e.PropMgmtActivitySubtypeCode).HasName("PRACST_PK");

            entity.ToTable("PIMS_PROP_MGMT_ACTIVITY_SUBTYPE", tb =>
                {
                    tb.HasComment("Code table to describe the subtype of property management.");
                    tb.HasTrigger("PIMS_PRACST_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRACST_I_S_U_TR");
                });

            entity.Property(e => e.PropMgmtActivitySubtypeCode).HasComment("Code representing the subtype of property management.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the subtype of property management.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is disabled.");
            entity.Property(e => e.PropMgmtActivityTypeCode).HasComment("Code representing the type of property management.");

            entity.HasOne(d => d.PropMgmtActivityTypeCodeNavigation).WithMany(p => p.PimsPropMgmtActivitySubtypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRACTY_PIM_PRACST_FK");
        });

        modelBuilder.Entity<PimsPropMgmtActivityType>(entity =>
        {
            entity.HasKey(e => e.PropMgmtActivityTypeCode).HasName("PRACTY_PK");

            entity.ToTable("PIMS_PROP_MGMT_ACTIVITY_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe the type of property management.");
                    tb.HasTrigger("PIMS_PRACTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRACTY_I_S_U_TR");
                });

            entity.Property(e => e.PropMgmtActivityTypeCode).HasComment("Code representing the type of property management.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the type of property management.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsPropPropActivity>(entity =>
        {
            entity.HasKey(e => e.PropPropActivityId).HasName("PRPRAC_PK");

            entity.ToTable("PIMS_PROP_PROP_ACTIVITY", tb =>
                {
                    tb.HasComment("Associates a property to a property management actity (many-to-many).");
                    tb.HasTrigger("PIMS_PRPRAC_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRPRAC_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPRAC_I_S_U_TR");
                });

            entity.Property(e => e.PropPropActivityId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_PROP_ACTIVITY_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.PimsPropertyActivity).WithMany(p => p.PimsPropPropActivities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPACT_PIM_PRPRAC_FK");

            entity.HasOne(d => d.Property).WithMany(p => p.PimsPropPropActivities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPRTY_PIM_PRPRAC_FK");
        });

        modelBuilder.Entity<PimsPropPropActivityHist>(entity =>
        {
            entity.HasKey(e => e.PropPropActivityHistId).HasName("PIMS_PRPRAC_H_PK");

            entity.Property(e => e.PropPropActivityHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_PROP_ACTIVITY_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropPropAnomalyType>(entity =>
        {
            entity.HasKey(e => e.PropPropAnomalyTypeId).HasName("PRPRAT_PK");

            entity.ToTable("PIMS_PROP_PROP_ANOMALY_TYPE", tb =>
                {
                    tb.HasComment("Resolves many-to-many relationship between PIMS_PROPERTY and PIMS_PROPERTY_ANOMALY_TYPE");
                    tb.HasTrigger("PIMS_PRPRAT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPRAT_I_S_U_TR");
                });

            entity.Property(e => e.PropPropAnomalyTypeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_PROP_ANOMALY_TYPE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.PropertyAnomalyTypeCodeNavigation).WithMany(p => p.PimsPropPropAnomalyTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRANOM_PIM_PRPRAT_FK");

            entity.HasOne(d => d.Property).WithMany(p => p.PimsPropPropAnomalyTypes).HasConstraintName("PIM_PRPRTY_PIM_PRPRAT_FK");
        });

        modelBuilder.Entity<PimsPropPropPurpose>(entity =>
        {
            entity.HasKey(e => e.PropPropPurposeId).HasName("PRPRPU_PK");

            entity.ToTable("PIMS_PROP_PROP_PURPOSE", tb =>
                {
                    tb.HasComment("Defines the contacts that are associated with this property.");
                    tb.HasTrigger("PIMS_PRPRPU_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRPRPU_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPRPU_I_S_U_TR");
                });

            entity.Property(e => e.PropPropPurposeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_PROP_PURPOSE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.PropertyId).HasComment("Primary key of the associated property.");
            entity.Property(e => e.PropertyPurposeTypeCode).HasComment("Primary key of the associated property purpose.");

            entity.HasOne(d => d.Property).WithMany(p => p.PimsPropPropPurposes).HasConstraintName("PIM_PRPRTY_PIM_PRPRPU_FK");

            entity.HasOne(d => d.PropertyPurposeTypeCodeNavigation).WithMany(p => p.PimsPropPropPurposes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPPUR_PIM_PRPRPU_FK");
        });

        modelBuilder.Entity<PimsPropPropPurposeHist>(entity =>
        {
            entity.HasKey(e => e.PropPropPurposeHistId).HasName("PIMS_PRPRPU_H_PK");

            entity.Property(e => e.PropPropPurposeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_PROP_PURPOSE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropPropRoadType>(entity =>
        {
            entity.HasKey(e => e.PropPropRoadTypeId).HasName("PRPRRT_PK");

            entity.ToTable("PIMS_PROP_PROP_ROAD_TYPE", tb =>
                {
                    tb.HasComment("Resolves many-to-many relationship between PIMS_PROPERTY and PIMS_PROPERTY_ROAD_TYPE");
                    tb.HasTrigger("PIMS_PRPRRT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPRRT_I_S_U_TR");
                });

            entity.Property(e => e.PropPropRoadTypeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_PROP_ROAD_TYPE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Property).WithMany(p => p.PimsPropPropRoadTypes).HasConstraintName("PIM_PRPRTY_PIM_PRPRRT_FK");

            entity.HasOne(d => d.PropertyRoadTypeCodeNavigation).WithMany(p => p.PimsPropPropRoadTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRROAD_PIM_PRPRRT_FK");
        });

        modelBuilder.Entity<PimsPropPropTenureType>(entity =>
        {
            entity.HasKey(e => e.PropPropTenureTypeId).HasName("PRPRTT_PK");

            entity.ToTable("PIMS_PROP_PROP_TENURE_TYPE", tb =>
                {
                    tb.HasComment("Resolves many-to-many relationship between PIMS_PROPERTY and PIMS_PROPERTY_TENURE_TYPE");
                    tb.HasTrigger("PIMS_PRPRTT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPRTT_I_S_U_TR");
                });

            entity.Property(e => e.PropPropTenureTypeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_PROP_TENURE_TYPE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.PropertyTenureTypeCode).HasDefaultValue("UNKNOWN");

            entity.HasOne(d => d.Property).WithMany(p => p.PimsPropPropTenureTypes).HasConstraintName("PIM_PRPRTY_PIM_PRPRTT_FK");

            entity.HasOne(d => d.PropertyTenureTypeCodeNavigation).WithMany(p => p.PimsPropPropTenureTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPTNR_PIM_PRPRTT_FK");
        });

        modelBuilder.Entity<PimsPropResearchPurposeType>(entity =>
        {
            entity.HasKey(e => e.PropResearchPurposeTypeCode).HasName("RRESPT_PK");

            entity.ToTable("PIMS_PROP_RESEARCH_PURPOSE_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe the purpose ot the property research");
                    tb.HasTrigger("PIMS_RRESPT_I_S_I_TR");
                    tb.HasTrigger("PIMS_RRESPT_I_S_U_TR");
                });

            entity.Property(e => e.PropResearchPurposeTypeCode).HasComment("Code indicating the purpose of the property research");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the code indicating the purpose of the property research");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsProperty>(entity =>
        {
            entity.HasKey(e => e.PropertyId).HasName("PRPRTY_PK");

            entity.ToTable("PIMS_PROPERTY", tb =>
                {
                    tb.HasComment("Describes the attributes of a property.");
                    tb.HasTrigger("PIMS_PRPRTY_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRPRTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPRTY_I_S_U_TR");
                });

            entity.Property(e => e.PropertyId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ID_SEQ])")
                .HasComment("Generated surrogate primary key");
            entity.Property(e => e.AdditionalDetails).HasComment("Additional details about the property.");
            entity.Property(e => e.AddressId).HasComment("Foreign key to the address table.");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory).HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid).HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid).HasComment("The user account that updated the record.");
            entity.Property(e => e.BandName).HasComment("Name of the Indian band.");
            entity.Property(e => e.Boundary).HasComment("Spatial bundary of land");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DistrictCode).HasComment("Foreign key to the district table.");
            entity.Property(e => e.FileNumber).HasComment("The (ARCS/ORCS) number identifying the Property File.");
            entity.Property(e => e.FileNumberSuffix).HasComment("A suffix to distinguish between Property Files with the same number.");
            entity.Property(e => e.GeneralLocation).HasComment("Descriptive location of the property, primarily for H120 activities.");
            entity.Property(e => e.IsOwned)
                .HasDefaultValue(true)
                .HasComment("Is the property currently owned?");
            entity.Property(e => e.IsRetired).HasComment("If the property was the source of a subdivision operation or the target of a consolidation operation, the property is marked as retired.");
            entity.Property(e => e.IsRwyBeltDomPatent)
                .HasDefaultValue(false)
                .HasComment("Indicates if this property is original federal vs. provincial ownership.");
            entity.Property(e => e.IsTaxesPayable).HasComment("Indicates if the property taxes are being paid.");
            entity.Property(e => e.IsUtilitiesPayable).HasComment("Indicates if the utilities are being paid.");
            entity.Property(e => e.IsVolumetricParcel)
                .HasDefaultValue(false)
                .HasComment("Is there a volumetric measurement for this parcel?");
            entity.Property(e => e.LandArea).HasComment("Area occupied by property");
            entity.Property(e => e.LandLegalDescription).HasComment("Legal description of property");
            entity.Property(e => e.Location).HasComment("Geospatial location (pin) of property");
            entity.Property(e => e.MunicipalZoning).HasComment("Municipal zoning that applies this property.");
            entity.Property(e => e.Notes).HasComment("Notes about the property");
            entity.Property(e => e.Pid).HasComment("Property ID");
            entity.Property(e => e.Pin).HasComment("Property number");
            entity.Property(e => e.PphStatusTypeCode).HasComment("Foreign key to the provincial public highway status type table.");
            entity.Property(e => e.PphStatusUpdateTimestamp).HasComment("Date / time that the Provincial Public Highway status was updated.");
            entity.Property(e => e.PphStatusUpdateUserGuid).HasComment("GUID of the user that updated the PPH status.");
            entity.Property(e => e.PphStatusUpdateUserid).HasComment("Userid that updated the Provincial Public Highway status.");
            entity.Property(e => e.PropertyAreaUnitTypeCode).HasComment("Foreign key to the property area unit type table.");
            entity.Property(e => e.PropertyDataSourceEffectiveDate).HasComment("Date the property was officially registered");
            entity.Property(e => e.PropertyDataSourceTypeCode).HasComment("Foreign key to the property data source type table.");
            entity.Property(e => e.PropertyStatusTypeCode).HasComment("Foreign key to the property status type table.");
            entity.Property(e => e.PropertyTypeCode).HasComment("Foreign key to the proprty type table.");
            entity.Property(e => e.RegionCode).HasComment("Foreign key to the region table.");
            entity.Property(e => e.ReserveName).HasComment("Name of the Indian reserve.");
            entity.Property(e => e.SurplusDeclarationComment).HasComment("Comment regarding the surplus declaration");
            entity.Property(e => e.SurplusDeclarationDate).HasComment("Date the property was declared surplus");
            entity.Property(e => e.SurplusDeclarationTypeCode).HasComment("Foreign key to the surplus declaration type table.");
            entity.Property(e => e.SurveyPlanNumber).HasComment("Property/Land Parcel survey plan number");
            entity.Property(e => e.VolumeUnitTypeCode).HasComment("Foreign key to the volume unit type table.");
            entity.Property(e => e.VolumetricMeasurement).HasComment("Volumetric measurement of the parcel.");
            entity.Property(e => e.VolumetricTypeCode).HasComment("Foreign key to the volumetric type table.");

            entity.HasOne(d => d.Address).WithMany(p => p.PimsProperties).HasConstraintName("PIM_ADDRSS_PIM_PRPRTY_FK");

            entity.HasOne(d => d.DistrictCodeNavigation).WithMany(p => p.PimsProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DSTRCT_PIM_PRPRTY_FK");

            entity.HasOne(d => d.PphStatusTypeCodeNavigation).WithMany(p => p.PimsProperties).HasConstraintName("PIM_PPHSTT_PIM_PRPRTY_FK");

            entity.HasOne(d => d.PropertyAreaUnitTypeCodeNavigation).WithMany(p => p.PimsProperties).HasConstraintName("PIM_ARUNIT_PIM_PRPRTY_FK");

            entity.HasOne(d => d.PropertyDataSourceTypeCodeNavigation).WithMany(p => p.PimsProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PIDSRT_PIM_PRPRTY_FK");

            entity.HasOne(d => d.PropertyStatusTypeCodeNavigation).WithMany(p => p.PimsProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPSTS_PIM_PRPRTY_FK");

            entity.HasOne(d => d.PropertyTypeCodeNavigation).WithMany(p => p.PimsProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPTYP_PIM_PRPRTY_FK");

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.PimsProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_REGION_PIM_PRPRTY_FK");

            entity.HasOne(d => d.SurplusDeclarationTypeCodeNavigation).WithMany(p => p.PimsProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_SPDCLT_PIM_PRPRTY_FK");

            entity.HasOne(d => d.VolumeUnitTypeCodeNavigation).WithMany(p => p.PimsProperties).HasConstraintName("PIM_VOLUTY_PIM_PRPRTY_FK");

            entity.HasOne(d => d.VolumetricTypeCodeNavigation).WithMany(p => p.PimsProperties).HasConstraintName("PIM_PRVOLT_PIM_PRPRTY_FK");
        });

        modelBuilder.Entity<PimsPropertyAcquisitionFile>(entity =>
        {
            entity.HasKey(e => e.PropertyAcquisitionFileId).HasName("PRACQF_PK");

            entity.ToTable("PIMS_PROPERTY_ACQUISITION_FILE", tb =>
                {
                    tb.HasComment("Associates a property with an acquisition file.");
                    tb.HasTrigger("PIMS_PRACQF_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRACQF_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRACQF_I_S_U_TR");
                });

            entity.Property(e => e.PropertyAcquisitionFileId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACQUISITION_FILE_ID_SEQ])")
                .HasComment("Generated surrogate primary key.");
            entity.Property(e => e.AcquisitionFileId).HasComment("Foreign key to the ACQUISTION_FILE table.");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.Location).HasComment("Geospatial location (pin) of property");
            entity.Property(e => e.PropertyId).HasComment("Foreign key to the PROPERTY table.");
            entity.Property(e => e.PropertyName).HasComment("Descriptive reference for the property associated with the acquisition file.");

            entity.HasOne(d => d.AcquisitionFile).WithMany(p => p.PimsPropertyAcquisitionFiles).HasConstraintName("PIM_ACQNFL_PIM_PRACQF_FK");

            entity.HasOne(d => d.Property).WithMany(p => p.PimsPropertyAcquisitionFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPRTY_PIM_PRACQF_FK");
        });

        modelBuilder.Entity<PimsPropertyAcquisitionFileHist>(entity =>
        {
            entity.HasKey(e => e.PropertyAcquisitionFileHistId).HasName("PIMS_PRACQF_H_PK");

            entity.Property(e => e.PropertyAcquisitionFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACQUISITION_FILE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropertyActivity>(entity =>
        {
            entity.HasKey(e => e.PimsPropertyActivityId).HasName("PRPACT_PK");

            entity.ToTable("PIMS_PROPERTY_ACTIVITY", tb =>
                {
                    tb.HasComment("Defines the activities that are associated with this property.");
                    tb.HasTrigger("PIMS_PRPACT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRPACT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPACT_I_S_U_TR");
                });

            entity.Property(e => e.PimsPropertyActivityId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACTIVITY_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.CompletionDt).HasComment("Date the property management activity was completed.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the property management activity.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code is disabled.");
            entity.Property(e => e.PropMgmtActivityStatusTypeCode)
                .HasDefaultValue("NOTSTARTED")
                .HasComment("Status of the property management activity.");
            entity.Property(e => e.PropMgmtActivitySubtypeCode)
                .HasDefaultValue("UNKNOWN")
                .HasComment("Subtype of property management activity.");
            entity.Property(e => e.PropMgmtActivityTypeCode)
                .HasDefaultValue("UNKNOWN")
                .HasComment("Type of property management activity.");
            entity.Property(e => e.RequestAddedDt).HasComment("Date the request for a property management activity was added");
            entity.Property(e => e.RequestSource).HasComment("Source of the management activity request.");
            entity.Property(e => e.ServiceProviderOrgId).HasComment("Foreign key of the organization as a service provider.");
            entity.Property(e => e.ServiceProviderPersonId).HasComment("Foreign key of the person as a service provider.");

            entity.HasOne(d => d.PropMgmtActivityStatusTypeCodeNavigation).WithMany(p => p.PimsPropertyActivities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PACSTY_PIM_PRPACT_FK");

            entity.HasOne(d => d.PropMgmtActivitySubtypeCodeNavigation).WithMany(p => p.PimsPropertyActivities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRACST_PIM_PRPACT_FK");

            entity.HasOne(d => d.PropMgmtActivityTypeCodeNavigation).WithMany(p => p.PimsPropertyActivities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRACTY_PIM_PRPACT_FK");

            entity.HasOne(d => d.ServiceProviderOrg).WithMany(p => p.PimsPropertyActivities).HasConstraintName("PIM_ORG_PIM_PRPACT_FK");

            entity.HasOne(d => d.ServiceProviderPerson).WithMany(p => p.PimsPropertyActivities).HasConstraintName("PIM_PERSON_PIM_PRPACT_FK");
        });

        modelBuilder.Entity<PimsPropertyActivityDocument>(entity =>
        {
            entity.HasKey(e => e.PropertyActivityDocumentId).HasName("PRACDO_PK");

            entity.ToTable("PIMS_PROPERTY_ACTIVITY_DOCUMENT", tb =>
                {
                    tb.HasTrigger("PIMS_PRACDO_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRACDO_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRACDO_I_S_U_TR");
                });

            entity.Property(e => e.PropertyActivityDocumentId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACTIVITY_DOCUMENT_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Document).WithMany(p => p.PimsPropertyActivityDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DOCMNT_PIM_PRACDO_FK");

            entity.HasOne(d => d.PimsPropertyActivity).WithMany(p => p.PimsPropertyActivityDocuments).HasConstraintName("PIM_PRPACT_PIM_PRACDO_FK");
        });

        modelBuilder.Entity<PimsPropertyActivityDocumentHist>(entity =>
        {
            entity.HasKey(e => e.PropertyActivityDocumentHistId).HasName("PIMS_PRACDO_H_PK");

            entity.Property(e => e.PropertyActivityDocumentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACTIVITY_DOCUMENT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropertyActivityHist>(entity =>
        {
            entity.HasKey(e => e.PropertyActivityHistId).HasName("PIMS_PRPACT_H_PK");

            entity.Property(e => e.PropertyActivityHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACTIVITY_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropertyActivityInvoice>(entity =>
        {
            entity.HasKey(e => e.PropertyActivityInvoiceId).HasName("PRACIN_PK");

            entity.ToTable("PIMS_PROPERTY_ACTIVITY_INVOICE", tb =>
                {
                    tb.HasComment("Defines the activities that are associated with this property.");
                    tb.HasTrigger("PIMS_PRACIN_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRACIN_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRACIN_I_S_U_TR");
                });

            entity.Property(e => e.PropertyActivityInvoiceId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACTIVITY_INVOICE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description)
                .HasDefaultValue("Unknown")
                .HasComment("Description of the invoice.");
            entity.Property(e => e.GstAmt).HasComment("GST on the invoice.");
            entity.Property(e => e.InvoiceDt)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date of the invoice");
            entity.Property(e => e.InvoiceNum).HasComment("Number assigned to the invoice.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the invoice is disabled.");
            entity.Property(e => e.IsPstRequired).HasComment("Indicates if the invoice requires PST.");
            entity.Property(e => e.PretaxAmt).HasComment("Subtotal of the invoice,");
            entity.Property(e => e.PstAmt).HasComment("PST on the invoice.");
            entity.Property(e => e.TotalAmt).HasComment("Total cost of the invoice.");

            entity.HasOne(d => d.PimsPropertyActivity).WithMany(p => p.PimsPropertyActivityInvoices).HasConstraintName("PIM_PRPACT_PIM_PRACIN_FK");
        });

        modelBuilder.Entity<PimsPropertyActivityInvoiceHist>(entity =>
        {
            entity.HasKey(e => e.PropertyActivityInvoiceHistId).HasName("PIMS_PRACIN_H_PK");

            entity.Property(e => e.PropertyActivityInvoiceHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACTIVITY_INVOICE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropertyAnomalyType>(entity =>
        {
            entity.HasKey(e => e.PropertyAnomalyTypeCode).HasName("PRANOM_PK");

            entity.ToTable("PIMS_PROPERTY_ANOMALY_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe property anomalies.");
                    tb.HasTrigger("PIMS_PRANOM_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRANOM_I_S_U_TR");
                });

            entity.Property(e => e.PropertyAnomalyTypeCode).HasComment("Property anomaly code.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Property anomaly code description.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsPropertyBoundaryLiteVw>(entity =>
        {
            entity.ToView("PIMS_PROPERTY_BOUNDARY_LITE_VW");
        });

        modelBuilder.Entity<PimsPropertyBoundaryVw>(entity =>
        {
            entity.ToView("PIMS_PROPERTY_BOUNDARY_VW");
        });

        modelBuilder.Entity<PimsPropertyContact>(entity =>
        {
            entity.HasKey(e => e.PropertyContactId).HasName("PRPCNT_PK");

            entity.ToTable("PIMS_PROPERTY_CONTACT", tb =>
                {
                    tb.HasComment("Defines the contacts that are associated with this property.");
                    tb.HasTrigger("PIMS_PRPCNT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRPCNT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPCNT_I_S_U_TR");
                });

            entity.Property(e => e.PropertyContactId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_CONTACT_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.OrganizationId).HasComment("Organization ID of the property contact.");
            entity.Property(e => e.PersonId).HasComment("Person ID of the property contact.");
            entity.Property(e => e.PrimaryContactId).HasComment("Primary contact for the organization");
            entity.Property(e => e.PropertyId).HasComment("Primary key of the associated property.");
            entity.Property(e => e.Purpose)
                .HasDefaultValue("Unknown")
                .HasComment("Purpose of property contact");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsPropertyContacts).HasConstraintName("PIM_ORG_PIM_PRPCNT_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsPropertyContactPeople).HasConstraintName("PIM_PERSON_PIM_PRPCNT_FK");

            entity.HasOne(d => d.PrimaryContact).WithMany(p => p.PimsPropertyContactPrimaryContacts).HasConstraintName("PIM_PERSON_PIM_PRM_PRPCNT_FK");

            entity.HasOne(d => d.Property).WithMany(p => p.PimsPropertyContacts).HasConstraintName("PIM_PRPRTY_PIM_PRPCNT_FK");
        });

        modelBuilder.Entity<PimsPropertyContactHist>(entity =>
        {
            entity.HasKey(e => e.PropertyContactHistId).HasName("PIMS_PRPCNT_H_PK");

            entity.Property(e => e.PropertyContactHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_CONTACT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropertyHist>(entity =>
        {
            entity.HasKey(e => e.PropertyHistId).HasName("PIMS_PRPRTY_H_PK");

            entity.Property(e => e.PropertyHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropertyImprovement>(entity =>
        {
            entity.HasKey(e => e.PropertyImprovementId).HasName("PIMPRV_PK");

            entity.ToTable("PIMS_PROPERTY_IMPROVEMENT", tb =>
                {
                    tb.HasComment("Description of property improvements associated with the lease.");
                    tb.HasTrigger("PIMS_PIMPRV_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PIMPRV_I_S_I_TR");
                    tb.HasTrigger("PIMS_PIMPRV_I_S_U_TR");
                });

            entity.Property(e => e.PropertyImprovementId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_IMPROVEMENT_ID_SEQ])");
            entity.Property(e => e.Address).HasComment("Addresses affected");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ImprovementDescription).HasComment("Description of the improvements");
            entity.Property(e => e.StructureSize).HasComment("Size of the structure (house, building, bridge, etc,)");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsPropertyImprovements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LEASE_PIM_PIMPRV_FK");

            entity.HasOne(d => d.PropertyImprovementTypeCodeNavigation).WithMany(p => p.PimsPropertyImprovements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PIMPRT_PIM_PIMPRV_FK");
        });

        modelBuilder.Entity<PimsPropertyImprovementHist>(entity =>
        {
            entity.HasKey(e => e.PropertyImprovementHistId).HasName("PIMS_PIMPRV_H_PK");

            entity.Property(e => e.PropertyImprovementHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_IMPROVEMENT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropertyImprovementType>(entity =>
        {
            entity.HasKey(e => e.PropertyImprovementTypeCode).HasName("PIMPRT_PK");

            entity.ToTable("PIMS_PROPERTY_IMPROVEMENT_TYPE", tb =>
                {
                    tb.HasComment("Description of the types of improvements made to a property during the lease.");
                    tb.HasTrigger("PIMS_PIMPRT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PIMPRT_I_S_U_TR");
                });

            entity.Property(e => e.PropertyImprovementTypeCode).HasComment("Code value of the types of improvements made to a property during the lease.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Code description of the types of improvements made to a property during the lease.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is disabled");
        });

        modelBuilder.Entity<PimsPropertyLease>(entity =>
        {
            entity.HasKey(e => e.PropertyLeaseId).HasName("PROPLS_PK");

            entity.ToTable("PIMS_PROPERTY_LEASE", tb =>
                {
                    tb.HasTrigger("PIMS_PROPLS_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PROPLS_I_S_I_TR");
                    tb.HasTrigger("PIMS_PROPLS_I_S_U_TR");
                });

            entity.Property(e => e.PropertyLeaseId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_LEASE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.LeaseArea).HasComment("Leased area measurement");
            entity.Property(e => e.Location).HasComment("Geospatial location (pin) of property");
            entity.Property(e => e.Name).HasComment("Property/lease name");

            entity.HasOne(d => d.AreaUnitTypeCodeNavigation).WithMany(p => p.PimsPropertyLeases).HasConstraintName("PIM_ARUNIT_PIM_PROPLS_FK");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsPropertyLeases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LEASE_PIM_PROPLS_FK");

            entity.HasOne(d => d.Property).WithMany(p => p.PimsPropertyLeases)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPRTY_PIM_PROPLS_FK");
        });

        modelBuilder.Entity<PimsPropertyLeaseHist>(entity =>
        {
            entity.HasKey(e => e.PropertyLeaseHistId).HasName("PIMS_PROPLS_H_PK");

            entity.Property(e => e.PropertyLeaseHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_LEASE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropertyLocationLiteVw>(entity =>
        {
            entity.ToView("PIMS_PROPERTY_LOCATION_LITE_VW");
        });

        modelBuilder.Entity<PimsPropertyLocationVw>(entity =>
        {
            entity.ToView("PIMS_PROPERTY_LOCATION_VW");
        });

        modelBuilder.Entity<PimsPropertyOperation>(entity =>
        {
            entity.HasKey(e => e.PropertyOperationId).HasName("PROPOP_PK");

            entity.ToTable("PIMS_PROPERTY_OPERATION", tb =>
                {
                    tb.HasComment("Defines the operations that are associated with properties.  These operations conccern property consolidations and suvdivisions.");
                    tb.HasTrigger("PIMS_PROPOP_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PROPOP_I_S_I_TR");
                    tb.HasTrigger("PIMS_PROPOP_I_S_U_TR");
                });

            entity.Property(e => e.PropertyOperationId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_OPERATION_ID_SEQ])")
                .HasComment("Surrogate sequence-based generated primary key for the table.  This is used internally to enforce data uniqueness.");
            entity.Property(e => e.AppCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user created the record.");
            entity.Property(e => e.AppCreateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that created the record.");
            entity.Property(e => e.AppCreateUserGuid).HasComment("The GUID of the user account that created the record.");
            entity.Property(e => e.AppCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that created the record.");
            entity.Property(e => e.AppLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the user updated the record.");
            entity.Property(e => e.AppLastUpdateUserDirectory)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The directory of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserGuid).HasComment("The GUID of the user account that updated the record.");
            entity.Property(e => e.AppLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user account that updated the record.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.DestinationPropertyId).HasComment("Foreign key to the destination property of the property operation.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the record is disabled.");
            entity.Property(e => e.OperationDt).HasComment("Business date of the property operation.");
            entity.Property(e => e.PropertyOperationNo).HasComment("Sequence-based operation identifying business key.  This is used to help identify when multiple properties were involved in a discrete operation.  The sequence number referenced is PIMS_PROPERTY_OPERATION_NO_SEQ.");
            entity.Property(e => e.PropertyOperationTypeCode).HasComment("Foriegn key to the descriptive operation  type code.");
            entity.Property(e => e.SourcePropertyId).HasComment("Foreign key to the source property of the property operation.");

            entity.HasOne(d => d.DestinationProperty).WithMany(p => p.PimsPropertyOperationDestinationProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPRTY_PIM_PROPOP_DST_FK");

            entity.HasOne(d => d.PropertyOperationTypeCodeNavigation).WithMany(p => p.PimsPropertyOperations).HasConstraintName("PIM_PRPOTY_PIM_PROPOP_FK");

            entity.HasOne(d => d.SourceProperty).WithMany(p => p.PimsPropertyOperationSourceProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPRTY_PIM_PROPOP_SRC_FK");
        });

        modelBuilder.Entity<PimsPropertyOperationHist>(entity =>
        {
            entity.HasKey(e => e.PropertyOperationHistId).HasName("PIMS_PROPOP_H_PK");

            entity.Property(e => e.PropertyOperationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_OPERATION_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropertyOperationType>(entity =>
        {
            entity.HasKey(e => e.PropertyOperationTypeCode).HasName("PRPOTY_PK");

            entity.ToTable("PIMS_PROPERTY_OPERATION_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe the type of property operation.  Currently, property operations are consolidations and subdivisions.");
                    tb.HasTrigger("PIMS_PRPOTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPOTY_I_S_U_TR");
                });

            entity.Property(e => e.PropertyOperationTypeCode).HasComment("Code representing the type of property operation.");
            entity.Property(e => e.ConcurrencyControlNumber)
                .HasDefaultValue(1L)
                .HasComment("Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o");
            entity.Property(e => e.DbCreateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created.");
            entity.Property(e => e.DbCreateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created the record.");
            entity.Property(e => e.DbLastUpdateTimestamp)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("The date and time the record was created or last updated.");
            entity.Property(e => e.DbLastUpdateUserid)
                .HasDefaultValueSql("(user_name())")
                .HasComment("The user or proxy account that created or last updated the record.");
            entity.Property(e => e.Description).HasComment("Description of the type of property operation.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsPropertyOrganization>(entity =>
        {
            entity.HasKey(e => e.PropertyOrganizationId).HasName("PRPORG_PK");

            entity.ToTable("PIMS_PROPERTY_ORGANIZATION", tb =>
                {
                    tb.HasTrigger("PIMS_PRPORG_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRPORG_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPORG_I_S_U_TR");
                });

            entity.Property(e => e.PropertyOrganizationId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ORGANIZATION_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsPropertyOrganizations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ORG_PIM_PRPORG_FK");

            entity.HasOne(d => d.Property).WithMany(p => p.PimsPropertyOrganizations).HasConstraintName("PIM_PRPRTY_PIM_PRPORG_FK");
        });

        modelBuilder.Entity<PimsPropertyOrganizationHist>(entity =>
        {
            entity.HasKey(e => e.PropertyOrganizationHistId).HasName("PIMS_PRPORG_H_PK");

            entity.Property(e => e.PropertyOrganizationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ORGANIZATION_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropertyPurposeType>(entity =>
        {
            entity.HasKey(e => e.PropertyPurposeTypeCode).HasName("PRPPUR_PK");

            entity.ToTable("PIMS_PROPERTY_PURPOSE_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe the property purpose type.");
                    tb.HasTrigger("PIMS_PRPPUR_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPPUR_I_S_U_TR");
                });

            entity.Property(e => e.PropertyPurposeTypeCode).HasComment("Code representing the purpose of the property.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the purpose of the property.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsPropertyResearchFile>(entity =>
        {
            entity.HasKey(e => e.PropertyResearchFileId).HasName("PRSCRC_PK");

            entity.ToTable("PIMS_PROPERTY_RESEARCH_FILE", tb =>
                {
                    tb.HasComment("Associates a property with a research file.");
                    tb.HasTrigger("PIMS_PRSCRC_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_PRSCRC_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRSCRC_I_S_U_TR");
                });

            entity.Property(e => e.PropertyResearchFileId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_RESEARCH_FILE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.DocumentReference).HasComment("URL / reference to a LAN Drive");
            entity.Property(e => e.IsLegalOpinionObtained).HasComment("Indicates whether a legal opinion was obtained (0 = No, 1 = Yes, null = Unknown)");
            entity.Property(e => e.IsLegalOpinionRequired).HasComment("Indicates whether a legal opinion is required (0 = No, 1 = Yes, null = Unknown)");
            entity.Property(e => e.Location).HasComment("Geospatial location (pin) of property");
            entity.Property(e => e.PropertyName).HasComment("Descriptive reference for the property being researched.");
            entity.Property(e => e.ResearchSummary).HasComment("Summary of the property research.");

            entity.HasOne(d => d.Property).WithMany(p => p.PimsPropertyResearchFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRPRTY_PIM_PRSCRC_FK");

            entity.HasOne(d => d.ResearchFile).WithMany(p => p.PimsPropertyResearchFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_RESRCH_PIM_PRSCRC_FK");
        });

        modelBuilder.Entity<PimsPropertyResearchFileHist>(entity =>
        {
            entity.HasKey(e => e.PropertyResearchFileHistId).HasName("PIMS_PRSCRC_H_PK");

            entity.Property(e => e.PropertyResearchFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_RESEARCH_FILE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsPropertyRoadType>(entity =>
        {
            entity.HasKey(e => e.PropertyRoadTypeCode).HasName("PRROAD_PK");

            entity.ToTable("PIMS_PROPERTY_ROAD_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe property highway/road type.");
                    tb.HasTrigger("PIMS_PRROAD_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRROAD_I_S_U_TR");
                });

            entity.Property(e => e.PropertyRoadTypeCode).HasComment("Property highway/road code.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Property highway/road code description.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsPropertyStatusType>(entity =>
        {
            entity.HasKey(e => e.PropertyStatusTypeCode).HasName("PRPSTS_PK");

            entity.ToTable("PIMS_PROPERTY_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe property status.");
                    tb.HasTrigger("PIMS_PRPSTS_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPSTS_I_S_U_TR");
                });

            entity.Property(e => e.PropertyStatusTypeCode).HasComment("Property status code.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Property status code description.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsPropertyTenureType>(entity =>
        {
            entity.HasKey(e => e.PropertyTenureTypeCode).HasName("PRPTNR_PK");

            entity.ToTable("PIMS_PROPERTY_TENURE_TYPE", tb =>
                {
                    tb.HasComment("A code table to store property tenure codes. Tenure is defined as : \"The act, right, manner or term of holding something(as a landed property)\" In this case, tenure is required on Properties to indicate MoTI's legal tenure on the property. The land parcel");
                    tb.HasTrigger("PIMS_PRPTNR_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPTNR_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsPropertyType>(entity =>
        {
            entity.HasKey(e => e.PropertyTypeCode).HasName("PRPTYP_PK");

            entity.ToTable("PIMS_PROPERTY_TYPE", tb =>
                {
                    tb.HasTrigger("PIMS_PRPTYP_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRPTYP_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsPropertyVw>(entity =>
        {
            entity.ToView("PIMS_PROPERTY_VW");
        });

        modelBuilder.Entity<PimsProvinceState>(entity =>
        {
            entity.HasKey(e => e.ProvinceStateId).HasName("PROVNC_PK");

            entity.ToTable("PIMS_PROVINCE_STATE", tb =>
                {
                    tb.HasComment("Table containing the provinces and states that are defined for the system.");
                    tb.HasTrigger("PIMS_PROVNC_I_S_I_TR");
                    tb.HasTrigger("PIMS_PROVNC_I_S_U_TR");
                });

            entity.Property(e => e.ProvinceStateId).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Full name/description of the provbince/state.");
            entity.Property(e => e.DisplayOrder).HasComment("Defines the display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if this code is disabled or enabled.");
            entity.Property(e => e.ProvinceStateCode).HasComment("Abbreviated province.state code.");

            entity.HasOne(d => d.Country).WithMany(p => p.PimsProvinceStates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_CNTRY_PIM_PROVNC_FK");
        });

        modelBuilder.Entity<PimsRegion>(entity =>
        {
            entity.HasKey(e => e.RegionCode).HasName("REGION_PK");

            entity.ToTable("PIMS_REGION", tb =>
                {
                    tb.HasTrigger("PIMS_REGION_I_S_I_TR");
                    tb.HasTrigger("PIMS_REGION_I_S_U_TR");
                });

            entity.Property(e => e.RegionCode).ValueGeneratedNever();
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsRegionUser>(entity =>
        {
            entity.HasKey(e => e.RegionUserId).HasName("RGNUSR_PK");

            entity.ToTable("PIMS_REGION_USER", tb =>
                {
                    tb.HasTrigger("PIMS_RGNUSR_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_RGNUSR_I_S_I_TR");
                    tb.HasTrigger("PIMS_RGNUSR_I_S_U_TR");
                });

            entity.Property(e => e.RegionUserId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_REGION_USER_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.RegionCode).HasDefaultValue((short)4);

            entity.HasOne(d => d.RegionCodeNavigation).WithMany(p => p.PimsRegionUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_REGION_PIM_RGNUSR_FK");

            entity.HasOne(d => d.User).WithMany(p => p.PimsRegionUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_USER_PIM_RGNUSR_FK");
        });

        modelBuilder.Entity<PimsRegionUserHist>(entity =>
        {
            entity.HasKey(e => e.RegionUserHistId).HasName("PIMS_RGNUSR_H_PK");

            entity.Property(e => e.RegionUserHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_REGION_USER_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsRequestSourceType>(entity =>
        {
            entity.HasKey(e => e.RequestSourceTypeCode).HasName("RQSRCT_PK");

            entity.ToTable("PIMS_REQUEST_SOURCE_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe source ot the research request");
                    tb.HasTrigger("PIMS_RQSRCT_I_S_I_TR");
                    tb.HasTrigger("PIMS_RQSRCT_I_S_U_TR");
                });

            entity.Property(e => e.RequestSourceTypeCode).HasComment("Code indicating the source of the research request.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the code indicating the source of the research request.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsResearchFile>(entity =>
        {
            entity.HasKey(e => e.ResearchFileId).HasName("RESRCH_PK");

            entity.ToTable("PIMS_RESEARCH_FILE", tb =>
                {
                    tb.HasComment("Property research file");
                    tb.HasTrigger("PIMS_RESRCH_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_RESRCH_I_S_I_TR");
                    tb.HasTrigger("PIMS_RESRCH_I_S_U_TR");
                });

            entity.Property(e => e.ResearchFileId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ExpropriationNotes).HasComment("Notes associated with an expropriation.");
            entity.Property(e => e.IsExpropriation).HasComment("Is this an expropriation?");
            entity.Property(e => e.Name).HasComment("Name given to the research file.");
            entity.Property(e => e.RequestDate).HasComment("Date of the research request.");
            entity.Property(e => e.RequestDescription).HasComment("Description of the research request.");
            entity.Property(e => e.RequestorName).HasComment("Name of the research requestor.");
            entity.Property(e => e.RequestorOrganization).HasComment("Organization associated with the research requestor.");
            entity.Property(e => e.ResearchCompletionDate).HasComment("Date the research request was completed.");
            entity.Property(e => e.ResearchFileStatusTypeCode).HasDefaultValue("ACTIVE");
            entity.Property(e => e.ResearchResult).HasComment("Result of the research request.");
            entity.Property(e => e.RfileNumber)
                .HasDefaultValue("RFILE-UNKNOWN")
                .HasComment("R-File number assigned to the research file, formatted value from PIMS_RFILE_NUMBER_SEQ sequence generator");
            entity.Property(e => e.RoadAlias).HasComment("Alias(es) of roads associated with this research request.");
            entity.Property(e => e.RoadName).HasComment("Name(s) of roads associated with this research request.");

            entity.HasOne(d => d.RequestSourceTypeCodeNavigation).WithMany(p => p.PimsResearchFiles).HasConstraintName("PIM_RQSRCT_PIM_RESRCH_FK");

            entity.HasOne(d => d.RequestorNameNavigation).WithMany(p => p.PimsResearchFiles).HasConstraintName("PIM_PERSON_PIM_RESRCH_FK");

            entity.HasOne(d => d.RequestorOrganizationNavigation).WithMany(p => p.PimsResearchFiles).HasConstraintName("PIM_ORG_PIM_RESRCH_FK");

            entity.HasOne(d => d.ResearchFileStatusTypeCodeNavigation).WithMany(p => p.PimsResearchFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_RSRCHS_PIM_RESRCH_FK");
        });

        modelBuilder.Entity<PimsResearchFileDocument>(entity =>
        {
            entity.HasKey(e => e.ResearchFileDocumentId).HasName("RFLDOC_PK");

            entity.ToTable("PIMS_RESEARCH_FILE_DOCUMENT", tb =>
                {
                    tb.HasComment("Defines the relationship betwwen a research file and a document.");
                    tb.HasTrigger("PIMS_RFLDOC_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_RFLDOC_I_S_I_TR");
                    tb.HasTrigger("PIMS_RFLDOC_I_S_U_TR");
                });

            entity.Property(e => e.ResearchFileDocumentId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_DOCUMENT_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Document).WithMany(p => p.PimsResearchFileDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_DOCMNT_PIM_RFLDOC_FK");

            entity.HasOne(d => d.ResearchFile).WithMany(p => p.PimsResearchFileDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_RESRCH_PIM_RFLDOC_FK");
        });

        modelBuilder.Entity<PimsResearchFileDocumentHist>(entity =>
        {
            entity.HasKey(e => e.ResearchFileDocumentHistId).HasName("PIMS_RFLDOC_H_PK");

            entity.Property(e => e.ResearchFileDocumentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_DOCUMENT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsResearchFileHist>(entity =>
        {
            entity.HasKey(e => e.ResearchFileHistId).HasName("PIMS_RESRCH_H_PK");

            entity.Property(e => e.ResearchFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsResearchFileNote>(entity =>
        {
            entity.HasKey(e => e.ResearchFileNoteId).HasName("RFLNOT_PK");

            entity.ToTable("PIMS_RESEARCH_FILE_NOTE", tb =>
                {
                    tb.HasComment("Defines the relationship betwwen a research file and a note.");
                    tb.HasTrigger("PIMS_RFLNOT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_RFLNOT_I_S_I_TR");
                    tb.HasTrigger("PIMS_RFLNOT_I_S_U_TR");
                });

            entity.Property(e => e.ResearchFileNoteId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_NOTE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Note).WithMany(p => p.PimsResearchFileNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_NOTE_PIM_RFLNOT_FK");

            entity.HasOne(d => d.ResearchFile).WithMany(p => p.PimsResearchFileNotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_RESRCH_PIM_RFLNOT_FK");
        });

        modelBuilder.Entity<PimsResearchFileNoteHist>(entity =>
        {
            entity.HasKey(e => e.ResearchFileNoteHistId).HasName("PIMS_RFLNOT_H_PK");

            entity.Property(e => e.ResearchFileNoteHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_NOTE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsResearchFileProject>(entity =>
        {
            entity.HasKey(e => e.ResearchFileProjectId).HasName("RFLPRJ_PK");

            entity.ToTable("PIMS_RESEARCH_FILE_PROJECT", tb =>
                {
                    tb.HasComment("Defines the relationship betwwen a research file and a project.");
                    tb.HasTrigger("PIMS_RFLPRJ_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_RFLPRJ_I_S_I_TR");
                    tb.HasTrigger("PIMS_RFLPRJ_I_S_U_TR");
                });

            entity.Property(e => e.ResearchFileProjectId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_PROJECT_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Project).WithMany(p => p.PimsResearchFileProjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PROJCT_PIM_RFLPRJ_FK");

            entity.HasOne(d => d.ResearchFile).WithMany(p => p.PimsResearchFileProjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_RESRCH_PIM_RFLPRJ_FK");
        });

        modelBuilder.Entity<PimsResearchFileProjectHist>(entity =>
        {
            entity.HasKey(e => e.ResearchFileProjectHistId).HasName("PIMS_RFLPRJ_H_PK");

            entity.Property(e => e.ResearchFileProjectHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_PROJECT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsResearchFilePurpose>(entity =>
        {
            entity.HasKey(e => e.ResearchFilePurposeId).HasName("RSFLPR_PK");

            entity.ToTable("PIMS_RESEARCH_FILE_PURPOSE", tb =>
                {
                    tb.HasTrigger("PIMS_RSFLPR_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_RSFLPR_I_S_I_TR");
                    tb.HasTrigger("PIMS_RSFLPR_I_S_U_TR");
                });

            entity.Property(e => e.ResearchFilePurposeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_PURPOSE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ResearchPurposeTypeCode).HasDefaultValue("GENENQ");

            entity.HasOne(d => d.ResearchFile).WithMany(p => p.PimsResearchFilePurposes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_RESRCH_PIM_RSFLPR_FK");

            entity.HasOne(d => d.ResearchPurposeTypeCodeNavigation).WithMany(p => p.PimsResearchFilePurposes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_RSHPRT_PIM_RSFLPR_FK");
        });

        modelBuilder.Entity<PimsResearchFilePurposeHist>(entity =>
        {
            entity.HasKey(e => e.ResearchFilePurposeHistId).HasName("PIMS_RSFLPR_H_PK");

            entity.Property(e => e.ResearchFilePurposeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_PURPOSE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsResearchFileStatusType>(entity =>
        {
            entity.HasKey(e => e.ResearchFileStatusTypeCode).HasName("RSRCHS_PK");

            entity.ToTable("PIMS_RESEARCH_FILE_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe property adjacent land type.");
                    tb.HasTrigger("PIMS_RSRCHS_I_S_I_TR");
                    tb.HasTrigger("PIMS_RSRCHS_I_S_U_TR");
                });

            entity.Property(e => e.ResearchFileStatusTypeCode).HasComment("Code indicating the status of the research file.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the code indicating the status of the research file.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsResearchPurposeType>(entity =>
        {
            entity.HasKey(e => e.ResearchPurposeTypeCode).HasName("RSHPRT_PK");

            entity.ToTable("PIMS_RESEARCH_PURPOSE_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe the purpose ot the research request");
                    tb.HasTrigger("PIMS_RSHPRT_I_S_I_TR");
                    tb.HasTrigger("PIMS_RSHPRT_I_S_U_TR");
                });

            entity.Property(e => e.ResearchPurposeTypeCode).HasComment("Code indicating the purpose of the research request.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the code indicating the purpose of the research request.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsResponsibilityCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RESPCD_PK");

            entity.ToTable("PIMS_RESPONSIBILITY_CODE", tb =>
                {
                    tb.HasComment("Code and description of the responsibility codes.");
                    tb.HasTrigger("PIMS_RESPCD_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_RESPCD_I_S_I_TR");
                    tb.HasTrigger("PIMS_RESPCD_I_S_U_TR");
                });

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESPONSIBILITY_ID_SEQ])")
                .HasComment("System-generated primary key.");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Code)
                .HasDefaultValue("<Empty>")
                .HasComment("Name of the code.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Descriptive value  of a code within the set.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the code became effective.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the code ceased to be in effect.");
        });

        modelBuilder.Entity<PimsResponsibilityCodeHist>(entity =>
        {
            entity.HasKey(e => e.ResponsibilityCodeHistId).HasName("PIMS_RESPCD_H_PK");

            entity.Property(e => e.ResponsibilityCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESPONSIBILITY_CODE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("ROLE_PK");

            entity.ToTable("PIMS_ROLE", tb =>
                {
                    tb.HasTrigger("PIMS_ROLE_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ROLE_I_S_I_TR");
                    tb.HasTrigger("PIMS_ROLE_I_S_U_TR");
                });

            entity.Property(e => e.RoleId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ROLE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsRoleClaim>(entity =>
        {
            entity.HasKey(e => e.RoleClaimId).HasName("ROLCLM_PK");

            entity.ToTable("PIMS_ROLE_CLAIM", tb =>
                {
                    tb.HasTrigger("PIMS_ROLCLM_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_ROLCLM_I_S_I_TR");
                    tb.HasTrigger("PIMS_ROLCLM_I_S_U_TR");
                });

            entity.Property(e => e.RoleClaimId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ROLE_CLAIM_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.IsDisabled).HasDefaultValue(false);

            entity.HasOne(d => d.Claim).WithMany(p => p.PimsRoleClaims)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_CLMTYP_PIM_ROLCLM_FK");

            entity.HasOne(d => d.Role).WithMany(p => p.PimsRoleClaims)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ROLE_PIM_ROLCLM_FK");
        });

        modelBuilder.Entity<PimsRoleClaimHist>(entity =>
        {
            entity.HasKey(e => e.RoleClaimHistId).HasName("PIMS_ROLCLM_H_PK");

            entity.Property(e => e.RoleClaimHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ROLE_CLAIM_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsRoleHist>(entity =>
        {
            entity.HasKey(e => e.RoleHistId).HasName("PIMS_ROLE_H_PK");

            entity.Property(e => e.RoleHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ROLE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsSecurityDeposit>(entity =>
        {
            entity.HasKey(e => e.SecurityDepositId).HasName("SECDEP_PK");

            entity.ToTable("PIMS_SECURITY_DEPOSIT", tb =>
                {
                    tb.HasComment("Description of a security deposit associated with a lease.");
                    tb.HasTrigger("PIMS_SECDEP_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_SECDEP_I_S_I_TR");
                    tb.HasTrigger("PIMS_SECDEP_I_S_U_TR");
                });

            entity.Property(e => e.SecurityDepositId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_ID_SEQ])");
            entity.Property(e => e.AmountPaid).HasComment("Amount paid of this security deposit");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DepositDate).HasComment("Date of this security deposit");
            entity.Property(e => e.Description).HasComment("Descirption of this security deposit");
            entity.Property(e => e.OtherDepositTypeDesc).HasComment("Description of the deposit type If the SECURITY_DEPOSIT_TYPE_CODE has been chosen for this scurity deposit.");

            entity.HasOne(d => d.Lease).WithMany(p => p.PimsSecurityDeposits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_LEASE_PIM_SECDEP_FK");

            entity.HasOne(d => d.SecurityDepositTypeCodeNavigation).WithMany(p => p.PimsSecurityDeposits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_SECDPT_PIM_SECDEP_FK");
        });

        modelBuilder.Entity<PimsSecurityDepositHist>(entity =>
        {
            entity.HasKey(e => e.SecurityDepositHistId).HasName("PIMS_SECDEP_H_PK");

            entity.Property(e => e.SecurityDepositHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsSecurityDepositHolder>(entity =>
        {
            entity.HasKey(e => e.SecurityDepositHolderId).HasName("SCDPHL_PK");

            entity.ToTable("PIMS_SECURITY_DEPOSIT_HOLDER", tb =>
                {
                    tb.HasTrigger("PIMS_SCDPHL_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_SCDPHL_I_S_I_TR");
                    tb.HasTrigger("PIMS_SCDPHL_I_S_U_TR");
                });

            entity.Property(e => e.SecurityDepositHolderId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_HOLDER_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsSecurityDepositHolders).HasConstraintName("PIM_ORG_PIM_SCDPHL_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsSecurityDepositHolders).HasConstraintName("PIM_PERSON_PIM_SCDPHL_FK");

            entity.HasOne(d => d.SecurityDeposit).WithOne(p => p.PimsSecurityDepositHolder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_SECDEP_PIM_SCDPHL_FK");
        });

        modelBuilder.Entity<PimsSecurityDepositHolderHist>(entity =>
        {
            entity.HasKey(e => e.SecurityDepositHolderHistId).HasName("PIMS_SCDPHL_H_PK");

            entity.Property(e => e.SecurityDepositHolderHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_HOLDER_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsSecurityDepositReturn>(entity =>
        {
            entity.HasKey(e => e.SecurityDepositReturnId).HasName("SDRTRN_PK");

            entity.ToTable("PIMS_SECURITY_DEPOSIT_RETURN", tb =>
                {
                    tb.HasComment("Describes the details of the return of a security deposit.");
                    tb.HasTrigger("PIMS_SDRTRN_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_SDRTRN_I_S_I_TR");
                    tb.HasTrigger("PIMS_SDRTRN_I_S_U_TR");
                });

            entity.Property(e => e.SecurityDepositReturnId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_RETURN_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ClaimsAgainst).HasComment("Amount of claims against the deposit");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.InterestPaid).HasComment("Interest paid on the deposit to the deposit holder");
            entity.Property(e => e.ReturnAmount).HasComment("Amount returned minus claims");
            entity.Property(e => e.ReturnDate).HasComment("Date of deposit return");
            entity.Property(e => e.TerminationDate).HasComment("Date the lease/license was terminated or surrendered");

            entity.HasOne(d => d.SecurityDeposit).WithMany(p => p.PimsSecurityDepositReturns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_SECDEP_PIM_SDRTRN_FK");
        });

        modelBuilder.Entity<PimsSecurityDepositReturnHist>(entity =>
        {
            entity.HasKey(e => e.SecurityDepositReturnHistId).HasName("PIMS_SDRTRN_H_PK");

            entity.Property(e => e.SecurityDepositReturnHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_RETURN_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsSecurityDepositReturnHolder>(entity =>
        {
            entity.HasKey(e => e.SecurityDepositReturnHolderId).HasName("SCDPRH_PK");

            entity.ToTable("PIMS_SECURITY_DEPOSIT_RETURN_HOLDER", tb =>
                {
                    tb.HasTrigger("PIMS_SCDPRH_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_SCDPRH_I_S_I_TR");
                    tb.HasTrigger("PIMS_SCDPRH_I_S_U_TR");
                });

            entity.Property(e => e.SecurityDepositReturnHolderId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_RETURN_HOLDER_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsSecurityDepositReturnHolders).HasConstraintName("PIM_ORG_PIM_SCDPRH_FK");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsSecurityDepositReturnHolders).HasConstraintName("PIM_PERSON_PIM_SCDPRH_FK");

            entity.HasOne(d => d.SecurityDepositReturn).WithOne(p => p.PimsSecurityDepositReturnHolder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_SDRTRN_PIM_SCDPRH_FK");
        });

        modelBuilder.Entity<PimsSecurityDepositReturnHolderHist>(entity =>
        {
            entity.HasKey(e => e.SecurityDepositReturnHolderHistId).HasName("PIMS_SCDPRH_H_PK");

            entity.Property(e => e.SecurityDepositReturnHolderHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_RETURN_HOLDER_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsSecurityDepositType>(entity =>
        {
            entity.HasKey(e => e.SecurityDepositTypeCode).HasName("SECDPT_PK");

            entity.ToTable("PIMS_SECURITY_DEPOSIT_TYPE", tb =>
                {
                    tb.HasTrigger("PIMS_SECDPT_I_S_I_TR");
                    tb.HasTrigger("PIMS_SECDPT_I_S_U_TR");
                });

            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsStaticVariable>(entity =>
        {
            entity.HasKey(e => e.StaticVariableName).HasName("STAVBL_PK");

            entity.ToTable("PIMS_STATIC_VARIABLE", tb =>
                {
                    tb.HasTrigger("PIMS_STAVBL_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_STAVBL_I_S_I_TR");
                    tb.HasTrigger("PIMS_STAVBL_I_S_U_TR");
                });

            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
        });

        modelBuilder.Entity<PimsStaticVariableHist>(entity =>
        {
            entity.HasKey(e => e.StaticVariableHistId).HasName("PIMS_STAVBL_H_PK");

            entity.Property(e => e.StaticVariableHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_STATIC_VARIABLE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsSurplusDeclarationType>(entity =>
        {
            entity.HasKey(e => e.SurplusDeclarationTypeCode).HasName("SPDCLT_PK");

            entity.ToTable("PIMS_SURPLUS_DECLARATION_TYPE", tb =>
                {
                    tb.HasComment("Description of the surplus property type.");
                    tb.HasTrigger("PIMS_SPDCLT_I_S_I_TR");
                    tb.HasTrigger("PIMS_SPDCLT_I_S_U_TR");
                });

            entity.Property(e => e.SurplusDeclarationTypeCode).HasComment("Code value of the surplus property type");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Code description of the surplus property type");
            entity.Property(e => e.IsDisabled).HasComment("Indicates that the code value is disabled");
        });

        modelBuilder.Entity<PimsSurveyPlanType>(entity =>
        {
            entity.HasKey(e => e.SurveyPlanTypeCode).HasName("SRVPLT_PK");

            entity.ToTable("PIMS_SURVEY_PLAN_TYPE", tb =>
                {
                    tb.HasComment("Codified values for the survey plan type.  This is an unassociated table that is used in the UI to populate JSON attributes.");
                    tb.HasTrigger("PIMS_SRVPLT_I_S_I_TR");
                    tb.HasTrigger("PIMS_SRVPLT_I_S_U_TR");
                });

            entity.Property(e => e.SurveyPlanTypeCode).HasComment("Code value for the survey plan type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the survey plan type.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsTake>(entity =>
        {
            entity.HasKey(e => e.TakeId).HasName("TAKE_PK");

            entity.ToTable("PIMS_TAKE", tb =>
                {
                    tb.HasComment("Table defining the take related to a specific acquisition file and property.");
                    tb.HasTrigger("PIMS_TAKE_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_TAKE_I_S_I_TR");
                    tb.HasTrigger("PIMS_TAKE_I_S_U_TR");
                });

            entity.Property(e => e.TakeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TAKE_ID_SEQ])");
            entity.Property(e => e.ActiveLeaseArea).HasComment("Area of the active lease.");
            entity.Property(e => e.ActiveLeaseEndDt).HasComment("End date of the active lease.");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.CompletionDt).HasComment("Date the take was completed.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the property take.");
            entity.Property(e => e.IsAcquiredForInventory).HasComment("Is this being acquired for inventory? (default = TRUE)");
            entity.Property(e => e.IsActiveLease).HasComment("Is there an active lease associated with the take?");
            entity.Property(e => e.IsNewHighwayDedication).HasComment("Is there a new right of way? (default = FALSE)");
            entity.Property(e => e.IsNewInterestInSrw).HasComment("Is there a statutory right of way? (default = FALSE)");
            entity.Property(e => e.IsNewLandAct).HasComment("Is there a Section 16? (default = FALSE)");
            entity.Property(e => e.IsNewLicenseToConstruct).HasComment("Is there a license to construct? (default = FALSE)");
            entity.Property(e => e.IsThereSurplus).HasComment("Is there a surplus or severance? (default = FALSE)");
            entity.Property(e => e.LandActArea).HasComment("Area of the Section 16 activity.");
            entity.Property(e => e.LandActEndDt).HasComment("End date of the Section 16 activity.");
            entity.Property(e => e.LicenseToConstructArea).HasComment("Area of the license to construct.");
            entity.Property(e => e.LtcEndDt).HasComment("End date of the license to construct.");
            entity.Property(e => e.NewHighwayDedicationArea).HasComment("Area of the new right-of-way.");
            entity.Property(e => e.SrwEndDt).HasComment("End date of the statutory right-of-way.");
            entity.Property(e => e.StatutoryRightOfWayArea).HasComment("Area of the statutory right-of-way.");
            entity.Property(e => e.SurplusArea).HasComment("Surplus/severance area.");
            entity.Property(e => e.TakeSiteContamTypeCode).HasDefaultValue("UNK");
            entity.Property(e => e.TakeStatusTypeCode).HasDefaultValue("INPROGRESS");

            entity.HasOne(d => d.AreaUnitTypeCodeNavigation).WithMany(p => p.PimsTakes).HasConstraintName("PIM_ARUNIT_PIM_TAKE_FK");

            entity.HasOne(d => d.LandActTypeCodeNavigation).WithMany(p => p.PimsTakes).HasConstraintName("PIM_LNDATY_PIM_TAKE_FK");

            entity.HasOne(d => d.PropertyAcquisitionFile).WithMany(p => p.PimsTakes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PRACQF_PIM_TAKE_FK");

            entity.HasOne(d => d.TakeSiteContamTypeCodeNavigation).WithMany(p => p.PimsTakes).HasConstraintName("PIM_TKCONT_PIM_TAKE_FK");

            entity.HasOne(d => d.TakeStatusTypeCodeNavigation).WithMany(p => p.PimsTakes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_TKSTST_PIM_TAKE_FK");

            entity.HasOne(d => d.TakeTypeCodeNavigation).WithMany(p => p.PimsTakes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_TKTYPE_PIM_TAKE_FK");
        });

        modelBuilder.Entity<PimsTakeHist>(entity =>
        {
            entity.HasKey(e => e.TakeHistId).HasName("PIMS_TAKE_H_PK");

            entity.Property(e => e.TakeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TAKE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsTakeSiteContamType>(entity =>
        {
            entity.HasKey(e => e.TakeSiteContamTypeCode).HasName("TKCONT_PK");

            entity.ToTable("PIMS_TAKE_SITE_CONTAM_TYPE", tb =>
                {
                    tb.HasComment("Tables that contains the codes and associated descriptions of the site contamination types.");
                    tb.HasTrigger("PIMS_TKCONT_I_S_I_TR");
                    tb.HasTrigger("PIMS_TKCONT_I_S_U_TR");
                });

            entity.Property(e => e.TakeSiteContamTypeCode).HasComment("Codified version of the site contamination type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the site contamination type.");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsTakeStatusType>(entity =>
        {
            entity.HasKey(e => e.TakeStatusTypeCode).HasName("TKSTST_PK");

            entity.ToTable("PIMS_TAKE_STATUS_TYPE", tb =>
                {
                    tb.HasComment("Tables that contains the codes and associated descriptions of the property take status types.");
                    tb.HasTrigger("PIMS_TKSTST_I_S_I_TR");
                    tb.HasTrigger("PIMS_TKSTST_I_S_U_TR");
                });

            entity.Property(e => e.TakeStatusTypeCode).HasComment("Codified version of the take status type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the take status type.");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsTakeType>(entity =>
        {
            entity.HasKey(e => e.TakeTypeCode).HasName("TKTYPE_PK");

            entity.ToTable("PIMS_TAKE_TYPE", tb =>
                {
                    tb.HasComment("Tables that contains the codes and associated descriptions of the property take types.");
                    tb.HasTrigger("PIMS_TKTYPE_I_S_I_TR");
                    tb.HasTrigger("PIMS_TKTYPE_I_S_U_TR");
                });

            entity.Property(e => e.TakeTypeCode).HasComment("Codified version of the take type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the take type.");
            entity.Property(e => e.DisplayOrder).HasComment("Display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is inactive.");
        });

        modelBuilder.Entity<PimsTenant>(entity =>
        {
            entity.HasKey(e => e.TenantId).HasName("TENNTX_PK");

            entity.ToTable("PIMS_TENANT", tb =>
                {
                    tb.HasComment("Deprecated table to support legacy CITZ-PIMS application code.  This table will be removed once the code dependency is removed from the system.");
                    tb.HasTrigger("PIMS_TENNTX_I_S_I_TR");
                    tb.HasTrigger("PIMS_TENNTX_I_S_U_TR");
                });

            entity.Property(e => e.TenantId)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TENANT_ID_SEQ])")
                .HasComment("Auto-sequenced unique key value");
            entity.Property(e => e.Code).HasComment("Code value for entry");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Description of the entry for display purposes");
            entity.Property(e => e.Name).HasComment("Name of the entry for display purposes");
            entity.Property(e => e.Settings).HasComment("Serialized JSON value for the configuration");
        });

        modelBuilder.Entity<PimsUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("USER_PK");

            entity.ToTable("PIMS_USER", tb =>
                {
                    tb.HasComment("Information associated with an identified PIMS system user.");
                    tb.HasTrigger("PIMS_USER_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_USER_I_S_I_TR");
                    tb.HasTrigger("PIMS_USER_I_S_U_TR");
                });

            entity.Property(e => e.UserId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ApprovedById).HasComment("Identifier of the person that approved the creation of this PIMS user.");
            entity.Property(e => e.BusinessIdentifierValue).HasComment("Accepted identifier of a user (e.g. IDIR)");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.ExpiryDate).HasComment("Expiry date/time of this user account.");
            entity.Property(e => e.GuidIdentifierValue).HasComment("Unique GUID associated with the user.");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if this user account is disabled.");
            entity.Property(e => e.IssueDate).HasComment("Date/time that this user was identified as a PIMS user,");
            entity.Property(e => e.LastLogin).HasComment("Last date/time the user was logged into PIMS.");
            entity.Property(e => e.Note).HasComment("Notes associated with this user.");
            entity.Property(e => e.Position).HasComment("Role/position assigned to the user.");

            entity.HasOne(d => d.Person).WithMany(p => p.PimsUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_PERSON_PIM_USER_FK");

            entity.HasOne(d => d.UserTypeCodeNavigation).WithMany(p => p.PimsUsers).HasConstraintName("PIM_USERTY_PIM_USER_FK");
        });

        modelBuilder.Entity<PimsUserHist>(entity =>
        {
            entity.HasKey(e => e.UserHistId).HasName("PIMS_USER_H_PK");

            entity.Property(e => e.UserHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsUserOrganization>(entity =>
        {
            entity.HasKey(e => e.UserOrganizationId).HasName("USRORG_PK");

            entity.ToTable("PIMS_USER_ORGANIZATION", tb =>
                {
                    tb.HasComment("Associates a user with an organization.");
                    tb.HasTrigger("PIMS_USRORG_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_USRORG_I_S_I_TR");
                    tb.HasTrigger("PIMS_USRORG_I_S_U_TR");
                });

            entity.Property(e => e.UserOrganizationId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ORGANIZATION_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

            entity.HasOne(d => d.Organization).WithMany(p => p.PimsUserOrganizations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ORG_PIM_USRORG_FK");

            entity.HasOne(d => d.Role).WithMany(p => p.PimsUserOrganizations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ROLE_PIM_USRORG_FK");

            entity.HasOne(d => d.User).WithMany(p => p.PimsUserOrganizations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_USER_PIM_USRORG_FK");
        });

        modelBuilder.Entity<PimsUserOrganizationHist>(entity =>
        {
            entity.HasKey(e => e.UserOrganizationHistId).HasName("PIMS_USRORG_H_PK");

            entity.Property(e => e.UserOrganizationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ORGANIZATION_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsUserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("USERRL_PK");

            entity.ToTable("PIMS_USER_ROLE", tb =>
                {
                    tb.HasComment("Associates a user with an role.");
                    tb.HasTrigger("PIMS_USERRL_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_USERRL_I_S_I_TR");
                    tb.HasTrigger("PIMS_USERRL_I_S_U_TR");
                });

            entity.Property(e => e.UserRoleId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ROLE_ID_SEQ])");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Indicates if this association is disabled.");

            entity.HasOne(d => d.Role).WithMany(p => p.PimsUserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_ROLE_PIM_USERRL_FK");

            entity.HasOne(d => d.User).WithMany(p => p.PimsUserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PIM_USER_PIM_USERRL_FK");
        });

        modelBuilder.Entity<PimsUserRoleHist>(entity =>
        {
            entity.HasKey(e => e.UserRoleHistId).HasName("PIMS_USERRL_H_PK");

            entity.Property(e => e.UserRoleHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ROLE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsUserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeCode).HasName("USERTY_PK");

            entity.ToTable("PIMS_USER_TYPE", tb =>
                {
                    tb.HasComment("Table describing the type of user.  Currently the user types are Ministry Staff and Contractor.");
                    tb.HasTrigger("PIMS_USERTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_USERTY_I_S_U_TR");
                });

            entity.Property(e => e.UserTypeCode)
                .HasDefaultValue("CONTRACT")
                .HasComment("Code value of the user type.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description)
                .HasDefaultValue("Contractor")
                .HasComment("Code description of the user type.");
            entity.Property(e => e.DisplayOrder).HasComment("Specified display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the user type is active.");
        });

        modelBuilder.Entity<PimsVolumeUnitType>(entity =>
        {
            entity.HasKey(e => e.VolumeUnitTypeCode).HasName("VOLUTY_PK");

            entity.ToTable("PIMS_VOLUME_UNIT_TYPE", tb =>
                {
                    tb.HasComment("The volume unit used for measuring Properties.");
                    tb.HasTrigger("PIMS_VOLUTY_I_S_I_TR");
                    tb.HasTrigger("PIMS_VOLUTY_I_S_U_TR");
                });

            entity.Property(e => e.VolumeUnitTypeCode).HasComment("The volume unit used for measuring Properties.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Translation of the code value into a description that can be displayed to the user.");
            entity.Property(e => e.DisplayOrder).HasComment("Order in which to display the code values, if required.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code value is still active or is now disabled.");
        });

        modelBuilder.Entity<PimsVolumetricType>(entity =>
        {
            entity.HasKey(e => e.VolumetricTypeCode).HasName("PRVOLT_PK");

            entity.ToTable("PIMS_VOLUMETRIC_TYPE", tb =>
                {
                    tb.HasComment("Code table to describe parcel/property volumetric type.");
                    tb.HasTrigger("PIMS_PRVOLT_I_S_I_TR");
                    tb.HasTrigger("PIMS_PRVOLT_I_S_U_TR");
                });

            entity.Property(e => e.VolumetricTypeCode).HasComment("Property parcel/property volumetric code.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Property parcel/property volumetric code description.");
            entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");
            entity.Property(e => e.IsDisabled).HasComment("Indicates if the code is disabled.");
        });

        modelBuilder.Entity<PimsWorkActivityCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("WRKACT_PK");

            entity.ToTable("PIMS_WORK_ACTIVITY_CODE", tb =>
                {
                    tb.HasComment("Code and description of the work activity codes.");
                    tb.HasTrigger("PIMS_WRKACT_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_WRKACT_I_S_I_TR");
                    tb.HasTrigger("PIMS_WRKACT_I_S_U_TR");
                });

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_WORK_ACTIVITY_ID_SEQ])")
                .HasComment("System-generated primary key.");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Code)
                .HasDefaultValue("<Empty>")
                .HasComment("Name of the code.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Descriptive value  of a code within the set.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the code became effective.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the code ceased to be in effect.");
        });

        modelBuilder.Entity<PimsWorkActivityCodeHist>(entity =>
        {
            entity.HasKey(e => e.WorkActivityCodeHistId).HasName("PIMS_WRKACT_H_PK");

            entity.Property(e => e.WorkActivityCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_WORK_ACTIVITY_CODE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<PimsYearlyFinancialCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("YRFINC_PK");

            entity.ToTable("PIMS_YEARLY_FINANCIAL_CODE", tb =>
                {
                    tb.HasComment("Code and description of the chart of accounts codes.");
                    tb.HasTrigger("PIMS_YRFINC_A_S_IUD_TR");
                    tb.HasTrigger("PIMS_YRFINC_I_S_I_TR");
                    tb.HasTrigger("PIMS_YRFINC_I_S_U_TR");
                });

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_YEARLY_FINANCIAL_CODE_ID_SEQ])")
                .HasComment("System-generated primary key.");
            entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Code)
                .HasDefaultValue("<Empty>")
                .HasComment("Standard Object of Expenditure (STOB) code.");
            entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValue(1L);
            entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            entity.Property(e => e.Description).HasComment("Descriptive value  of the STOB code.");
            entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");
            entity.Property(e => e.EffectiveDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasComment("Date the code became effective.");
            entity.Property(e => e.ExpiryDate).HasComment("Date the code ceased to be in effect.");
        });

        modelBuilder.Entity<PimsYearlyFinancialCodeHist>(entity =>
        {
            entity.HasKey(e => e.YearlyFinancialCodeHistId).HasName("PIMS_YRFINC_H_PK");

            entity.Property(e => e.YearlyFinancialCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_YEARLY_FINANCIAL_CODE_H_ID_SEQ])");
            entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
        });
        modelBuilder.HasSequence("PIMS_ACCESS_REQUEST_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACCESS_REQUEST_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACCESS_REQUEST_ORGANIZATION_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACCESS_REQUEST_ORGANIZATION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_ACTIVITY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_ACTIVITY_INSTANCE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_CHECKLIST_ITEM_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_CHECKLIST_ITEM_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_DOCUMENT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_DOCUMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_FORM_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_FORM_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence<int>("PIMS_ACQUISITION_FILE_NO_SEQ").HasMin(1L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_NOTE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_NOTE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_PERSON_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_TEAM_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_TEAM_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_OWNER_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_OWNER_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACQUISITION_PAYEE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACT_INST_PROP_ACQ_FILE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACT_INST_PROP_RSRCH_FILE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACTIVITY_INSTANCE_DOCUMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACTIVITY_INSTANCE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACTIVITY_INSTANCE_NOTE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACTIVITY_MODEL_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACTIVITY_MODEL_TASK_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACTIVITY_SERVICE_FILE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACTIVITY_TASK_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACTIVITY_TEMPLATE_DOCUMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ACTIVITY_TEMPLATE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ADDRESS_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ADDRESS_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_AGREEMENT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_AGREEMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ASSET_EVALUATION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_BUILDING_CONSTRUCTION_TYPE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_BUILDING_EVALUATION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_BUILDING_FISCAL_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_BUILDING_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_BUILDING_OCCUPANT_TYPE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_BUILDING_PREDOMINATE_USE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_BUSINESS_FUNCTION_CODE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_BUSINESS_FUNCTION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_CHART_OF_ACCOUNTS_CODE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_CHART_OF_ACCOUNTS_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_CLAIM_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_CLAIM_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_COMP_REQ_FINANCIAL_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_COMP_REQ_FINANCIAL_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_COMPENSATION_REQUISITION_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_COMPENSATION_REQUISITION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_CONTACT_METHOD_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_CONTACT_METHOD_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_COST_TYPE_CODE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_COST_TYPE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_APPRAISAL_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_APPRAISAL_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_CHECKLIST_ITEM_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_CHECKLIST_ITEM_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_FILE_DOCUMENT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_FILE_DOCUMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_FILE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_FILE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_FILE_NO_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_FILE_NOTE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_FILE_NOTE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_FILE_PROPERTY_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_FILE_PROPERTY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_FILE_TEAM_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_FILE_TEAM_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_OFFER_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_OFFER_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_PURCHASER_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_PURCHASER_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_SALE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DISPOSITION_SALE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DOCUMENT_CATEGORY_SUBTYPE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DOCUMENT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DOCUMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DOCUMENT_TYP_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DOCUMENT_TYPE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DSP_PURCH_AGENT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DSP_PURCH_AGENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DSP_PURCH_SOLICITOR_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_DSP_PURCH_SOLICITOR_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_EXPROP_PMT_PMT_ITEM_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_EXPROP_PMT_PMT_ITEM_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_EXPROPRIATION_PAYMENT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_EXPROPRIATION_PAYMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_FILE_ENTITY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_FILE_ENTITY_PERMISSIONS_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_FINANCIAL_ACTIVITY_CODE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_FINANCIAL_ACTIVITY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_FORM_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_GL_ACCOUNT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_H120_CATEGORY_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_H120_CATEGORY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_HISTORICAL_FILE_NUMBER_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_HISTORICAL_FILE_NUMBER_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_INSURANCE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_INSURANCE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_INTEREST_HOLDER_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_INTEREST_HOLDER_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_INTEREST_HOLDER_PROPERTY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_INTHLDR_PROP_INTEREST_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_INTHLDR_PROP_INTEREST_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_L_FILE_NO_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_ACTIVITY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_ACTIVITY_INSTANCE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_ACTIVITY_PERIOD_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_CHECKLIST_ITEM_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_CHECKLIST_ITEM_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_CONSULTATION_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_CONSULTATION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_DOCUMENT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_DOCUMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_LEASE_PURPOSE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_LEASE_PURPOSE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_NOTE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_NOTE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_PAYMENT_FORECAST_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_PAYMENT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_PAYMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_PAYMENT_PERIOD_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_PERIOD_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_PERIOD_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_RENEWAL_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_RENEWAL_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_STAKEHOLDER_COMP_REQ_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_STAKEHOLDER_COMP_REQ_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_STAKEHOLDER_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_STAKEHOLDER_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_LEASE_TERM_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_NOTE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_NOTE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ORGANIZATION_ADDRESS_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ORGANIZATION_ADDRESS_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ORGANIZATION_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ORGANIZATION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_OWNER_REPRESENTATIVE_ID_SEQ")
            .HasMin(1L)
            .HasMax(20147483647L);
        modelBuilder.HasSequence("PIMS_OWNER_SOLICITOR_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PERSON_ADDRESS_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PERSON_ADDRESS_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PERSON_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PERSON_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PERSON_ORGANIZATION_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PERSON_ORGANIZATION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PRF_PROP_RESEARCH_PURPOSE_ID_SEQ")
            .HasMin(1L)
            .HasMax(21474483647L);
        modelBuilder.HasSequence("PIMS_PRODUCT_BUSINESS_FUNCTION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PRODUCT_COST_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PRODUCT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PRODUCT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PRODUCT_WORK_ACTIVITY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_DOCUMENT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_DOCUMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_NOTE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_NOTE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_NUMBER_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_ORGANIZATION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_PERSON_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_PERSON_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_PRODUCT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_PRODUCT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_PROPERTY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROJECT_WORKFLOW_MODEL_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_ACQ_FL_COMP_REQ_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_ACQ_FL_COMP_REQ_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_ACT_INVOLVED_PARTY_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_ACT_INVOLVED_PARTY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_ACT_MIN_CONTACT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_ACT_MIN_CONTACT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_INTHLDR_INTEREST_TYPE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_LEASE_COMP_REQ_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_LEASE_COMP_REQ_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_PROP_ACTIVITY_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_PROP_ACTIVITY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_PROP_ADJACENT_LAND_TYPE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_PROP_ANOMALY_TYPE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_PROP_PURPOSE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_PROP_PURPOSE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_PROP_ROAD_TYPE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROP_PROP_TENURE_TYPE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_ACQUISITION_FILE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_ACQUISITION_FILE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_ACTIVITY_DOCUMENT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_ACTIVITY_DOCUMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_ACTIVITY_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_ACTIVITY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_ACTIVITY_INVOICE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_ACTIVITY_INVOICE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_CONTACT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_CONTACT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_EVALUATION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_IMPROVEMENT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_IMPROVEMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_LEASE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_LEASE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_OPERATION_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_OPERATION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_OPERATION_NO_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_ORGANIZATION_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_ORGANIZATION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_PROPERTY_SERVICE_FILE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_RESEARCH_FILE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_RESEARCH_FILE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_SERVICE_FILE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_STRUCTURE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_PROPERTY_TAX_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_REGION_USER_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_REGION_USER_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESEARCH_ACTIVITY_INSTANCE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESEARCH_FILE_DOCUMENT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESEARCH_FILE_DOCUMENT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESEARCH_FILE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESEARCH_FILE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESEARCH_FILE_NOTE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESEARCH_FILE_NOTE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESEARCH_FILE_PROJECT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESEARCH_FILE_PROJECT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESEARCH_FILE_PURPOSE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESEARCH_FILE_PURPOSE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESPONSIBILITY_CENTRE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESPONSIBILITY_CODE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESPONSIBILITY_CODE_SEQ")
            .StartsAt(991L)
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RESPONSIBILITY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_RFILE_NUMBER_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ROLE_CLAIM_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ROLE_CLAIM_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ROLE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_ROLE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_HOLDER_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_HOLDER_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_RETURN_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_RETURN_HOLDER_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_RETURN_HOLDER_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_RETURN_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_STATIC_VARIABLE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_STRUCTURE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_TAKE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_TAKE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_TASK_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_TASK_TEMPLATE_ACTIVITY_MODEL_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_TASK_TEMPLATE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_TENANT_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_USER_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_USER_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_USER_ORGANIZATION_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_USER_ORGANIZATION_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_USER_ROLE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_USER_ROLE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_USER_TASK_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_WORK_ACTIVITY_CODE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_WORK_ACTIVITY_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_WORKFLOW_MODEL_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_YEARLY_FINANCIAL_CODE_H_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);
        modelBuilder.HasSequence("PIMS_YEARLY_FINANCIAL_CODE_ID_SEQ")
            .HasMin(1L)
            .HasMax(2147483647L);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
