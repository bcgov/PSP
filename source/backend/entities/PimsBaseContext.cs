﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pims.Dal.Entities;

#nullable disable

namespace Pims.Dal
{
    public partial class PimsBaseContext : DbContext
    {
        public PimsBaseContext()
        {
        }

        public PimsBaseContext(DbContextOptions<PimsBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BcaAreaAmendment> BcaAreaAmendments { get; set; }
        public virtual DbSet<BcaAreaBctransitValue> BcaAreaBctransitValues { get; set; }
        public virtual DbSet<BcaAreaDelete> BcaAreaDeletes { get; set; }
        public virtual DbSet<BcaAreaGeneralValue> BcaAreaGeneralValues { get; set; }
        public virtual DbSet<BcaAreaSchoolValue> BcaAreaSchoolValues { get; set; }
        public virtual DbSet<BcaAssessmentArea> BcaAssessmentAreas { get; set; }
        public virtual DbSet<BcaDataAdvice> BcaDataAdvices { get; set; }
        public virtual DbSet<BcaDataAdviceAmendment> BcaDataAdviceAmendments { get; set; }
        public virtual DbSet<BcaDataAdviceBctransitValue> BcaDataAdviceBctransitValues { get; set; }
        public virtual DbSet<BcaDataAdviceDelete> BcaDataAdviceDeletes { get; set; }
        public virtual DbSet<BcaDataAdviceGeneralValue> BcaDataAdviceGeneralValues { get; set; }
        public virtual DbSet<BcaDataAdviceSchoolValue> BcaDataAdviceSchoolValues { get; set; }
        public virtual DbSet<BcaDefined> BcaDefineds { get; set; }
        public virtual DbSet<BcaElectoralArea> BcaElectoralAreas { get; set; }
        public virtual DbSet<BcaFolioAddress> BcaFolioAddresses { get; set; }
        public virtual DbSet<BcaFolioAmendment> BcaFolioAmendments { get; set; }
        public virtual DbSet<BcaFolioBctransitValue> BcaFolioBctransitValues { get; set; }
        public virtual DbSet<BcaFolioDescription> BcaFolioDescriptions { get; set; }
        public virtual DbSet<BcaFolioFarm> BcaFolioFarms { get; set; }
        public virtual DbSet<BcaFolioGeneralValue> BcaFolioGeneralValues { get; set; }
        public virtual DbSet<BcaFolioLandCharacteristic> BcaFolioLandCharacteristics { get; set; }
        public virtual DbSet<BcaFolioLegalDescription> BcaFolioLegalDescriptions { get; set; }
        public virtual DbSet<BcaFolioManagedForest> BcaFolioManagedForests { get; set; }
        public virtual DbSet<BcaFolioManufacturedHome> BcaFolioManufacturedHomes { get; set; }
        public virtual DbSet<BcaFolioOilAndGa> BcaFolioOilAndGas { get; set; }
        public virtual DbSet<BcaFolioRecord> BcaFolioRecords { get; set; }
        public virtual DbSet<BcaFolioSale> BcaFolioSales { get; set; }
        public virtual DbSet<BcaFolioSchoolValue> BcaFolioSchoolValues { get; set; }
        public virtual DbSet<BcaFolioValuation> BcaFolioValuations { get; set; }
        public virtual DbSet<BcaGeneralService> BcaGeneralServices { get; set; }
        public virtual DbSet<BcaImprovementDistrict> BcaImprovementDistricts { get; set; }
        public virtual DbSet<BcaIslandsTrust> BcaIslandsTrusts { get; set; }
        public virtual DbSet<BcaJurisdiction> BcaJurisdictions { get; set; }
        public virtual DbSet<BcaJurisdictionAmendment> BcaJurisdictionAmendments { get; set; }
        public virtual DbSet<BcaJurisdictionBctransitValue> BcaJurisdictionBctransitValues { get; set; }
        public virtual DbSet<BcaJurisdictionDelete> BcaJurisdictionDeletes { get; set; }
        public virtual DbSet<BcaJurisdictionGeneralValue> BcaJurisdictionGeneralValues { get; set; }
        public virtual DbSet<BcaJurisdictionSchoolValue> BcaJurisdictionSchoolValues { get; set; }
        public virtual DbSet<BcaLocalArea> BcaLocalAreas { get; set; }
        public virtual DbSet<BcaMinorTaxing> BcaMinorTaxings { get; set; }
        public virtual DbSet<BcaOwner> BcaOwners { get; set; }
        public virtual DbSet<BcaOwnershipGroup> BcaOwnershipGroups { get; set; }
        public virtual DbSet<BcaServiceArea> BcaServiceAreas { get; set; }
        public virtual DbSet<BcaSpecifiedMunicipal> BcaSpecifiedMunicipals { get; set; }
        public virtual DbSet<BcaSpecifiedRegional> BcaSpecifiedRegionals { get; set; }
        public virtual DbSet<PimsAccessRequest> PimsAccessRequests { get; set; }
        public virtual DbSet<PimsAccessRequestHist> PimsAccessRequestHists { get; set; }
        public virtual DbSet<PimsAccessRequestOrganization> PimsAccessRequestOrganizations { get; set; }
        public virtual DbSet<PimsAccessRequestOrganizationHist> PimsAccessRequestOrganizationHists { get; set; }
        public virtual DbSet<PimsAccessRequestStatusType> PimsAccessRequestStatusTypes { get; set; }
        public virtual DbSet<PimsAcqFlPersonProfileType> PimsAcqFlPersonProfileTypes { get; set; }
        public virtual DbSet<PimsAcqPhysFileStatusType> PimsAcqPhysFileStatusTypes { get; set; }
        public virtual DbSet<PimsAcquisitionActivityInstance> PimsAcquisitionActivityInstances { get; set; }
        public virtual DbSet<PimsAcquisitionActivityInstanceHist> PimsAcquisitionActivityInstanceHists { get; set; }
        public virtual DbSet<PimsAcquisitionFile> PimsAcquisitionFiles { get; set; }
        public virtual DbSet<PimsAcquisitionFileHist> PimsAcquisitionFileHists { get; set; }
        public virtual DbSet<PimsAcquisitionFileNote> PimsAcquisitionFileNotes { get; set; }
        public virtual DbSet<PimsAcquisitionFileNoteHist> PimsAcquisitionFileNoteHists { get; set; }
        public virtual DbSet<PimsAcquisitionFilePerson> PimsAcquisitionFilePeople { get; set; }
        public virtual DbSet<PimsAcquisitionFilePersonHist> PimsAcquisitionFilePersonHists { get; set; }
        public virtual DbSet<PimsAcquisitionFileStatusType> PimsAcquisitionFileStatusTypes { get; set; }
        public virtual DbSet<PimsAcquisitionFundingType> PimsAcquisitionFundingTypes { get; set; }
        public virtual DbSet<PimsAcquisitionOwner> PimsAcquisitionOwners { get; set; }
        public virtual DbSet<PimsAcquisitionOwnerHist> PimsAcquisitionOwnerHists { get; set; }
        public virtual DbSet<PimsAcquisitionType> PimsAcquisitionTypes { get; set; }
        public virtual DbSet<PimsActInstPropAcqFile> PimsActInstPropAcqFiles { get; set; }
        public virtual DbSet<PimsActInstPropAcqFileHist> PimsActInstPropAcqFileHists { get; set; }
        public virtual DbSet<PimsActInstPropRsrchFile> PimsActInstPropRsrchFiles { get; set; }
        public virtual DbSet<PimsActInstPropRsrchFileHist> PimsActInstPropRsrchFileHists { get; set; }
        public virtual DbSet<PimsActivityInstance> PimsActivityInstances { get; set; }
        public virtual DbSet<PimsActivityInstanceDocument> PimsActivityInstanceDocuments { get; set; }
        public virtual DbSet<PimsActivityInstanceDocumentHist> PimsActivityInstanceDocumentHists { get; set; }
        public virtual DbSet<PimsActivityInstanceHist> PimsActivityInstanceHists { get; set; }
        public virtual DbSet<PimsActivityInstanceNote> PimsActivityInstanceNotes { get; set; }
        public virtual DbSet<PimsActivityInstanceNoteHist> PimsActivityInstanceNoteHists { get; set; }
        public virtual DbSet<PimsActivityInstanceStatusType> PimsActivityInstanceStatusTypes { get; set; }
        public virtual DbSet<PimsActivityTemplate> PimsActivityTemplates { get; set; }
        public virtual DbSet<PimsActivityTemplateDocument> PimsActivityTemplateDocuments { get; set; }
        public virtual DbSet<PimsActivityTemplateDocumentHist> PimsActivityTemplateDocumentHists { get; set; }
        public virtual DbSet<PimsActivityTemplateHist> PimsActivityTemplateHists { get; set; }
        public virtual DbSet<PimsActivityTemplateType> PimsActivityTemplateTypes { get; set; }
        public virtual DbSet<PimsAddress> PimsAddresses { get; set; }
        public virtual DbSet<PimsAddressHist> PimsAddressHists { get; set; }
        public virtual DbSet<PimsAddressUsageType> PimsAddressUsageTypes { get; set; }
        public virtual DbSet<PimsAreaUnitType> PimsAreaUnitTypes { get; set; }
        public virtual DbSet<PimsBusinessFunctionCode> PimsBusinessFunctionCodes { get; set; }
        public virtual DbSet<PimsBusinessFunctionCodeHist> PimsBusinessFunctionCodeHists { get; set; }
        public virtual DbSet<PimsChartOfAccountsCode> PimsChartOfAccountsCodes { get; set; }
        public virtual DbSet<PimsChartOfAccountsCodeHist> PimsChartOfAccountsCodeHists { get; set; }
        public virtual DbSet<PimsClaim> PimsClaims { get; set; }
        public virtual DbSet<PimsClaimHist> PimsClaimHists { get; set; }
        public virtual DbSet<PimsContactMethod> PimsContactMethods { get; set; }
        public virtual DbSet<PimsContactMethodHist> PimsContactMethodHists { get; set; }
        public virtual DbSet<PimsContactMethodType> PimsContactMethodTypes { get; set; }
        public virtual DbSet<PimsContactMgrVw> PimsContactMgrVws { get; set; }
        public virtual DbSet<PimsCostTypeCode> PimsCostTypeCodes { get; set; }
        public virtual DbSet<PimsCostTypeCodeHist> PimsCostTypeCodeHists { get; set; }
        public virtual DbSet<PimsCountry> PimsCountries { get; set; }
        public virtual DbSet<PimsDataSourceType> PimsDataSourceTypes { get; set; }
        public virtual DbSet<PimsDistrict> PimsDistricts { get; set; }
        public virtual DbSet<PimsDocument> PimsDocuments { get; set; }
        public virtual DbSet<PimsDocumentHist> PimsDocumentHists { get; set; }
        public virtual DbSet<PimsDocumentStatusType> PimsDocumentStatusTypes { get; set; }
        public virtual DbSet<PimsDocumentTyp> PimsDocumentTyps { get; set; }
        public virtual DbSet<PimsDocumentTypHist> PimsDocumentTypHists { get; set; }
        public virtual DbSet<PimsFenceType> PimsFenceTypes { get; set; }
        public virtual DbSet<PimsFinancialActivityCode> PimsFinancialActivityCodes { get; set; }
        public virtual DbSet<PimsFinancialActivityCodeHist> PimsFinancialActivityCodeHists { get; set; }
        public virtual DbSet<PimsInsurance> PimsInsurances { get; set; }
        public virtual DbSet<PimsInsuranceHist> PimsInsuranceHists { get; set; }
        public virtual DbSet<PimsInsuranceType> PimsInsuranceTypes { get; set; }
        public virtual DbSet<PimsLandSurveyorType> PimsLandSurveyorTypes { get; set; }
        public virtual DbSet<PimsLease> PimsLeases { get; set; }
        public virtual DbSet<PimsLeaseActivityInstance> PimsLeaseActivityInstances { get; set; }
        public virtual DbSet<PimsLeaseActivityInstanceHist> PimsLeaseActivityInstanceHists { get; set; }
        public virtual DbSet<PimsLeaseCategoryType> PimsLeaseCategoryTypes { get; set; }
        public virtual DbSet<PimsLeaseHist> PimsLeaseHists { get; set; }
        public virtual DbSet<PimsLeaseInitiatorType> PimsLeaseInitiatorTypes { get; set; }
        public virtual DbSet<PimsLeaseLicenseType> PimsLeaseLicenseTypes { get; set; }
        public virtual DbSet<PimsLeasePayRvblType> PimsLeasePayRvblTypes { get; set; }
        public virtual DbSet<PimsLeasePayment> PimsLeasePayments { get; set; }
        public virtual DbSet<PimsLeasePaymentHist> PimsLeasePaymentHists { get; set; }
        public virtual DbSet<PimsLeasePaymentMethodType> PimsLeasePaymentMethodTypes { get; set; }
        public virtual DbSet<PimsLeasePaymentStatusType> PimsLeasePaymentStatusTypes { get; set; }
        public virtual DbSet<PimsLeasePmtFreqType> PimsLeasePmtFreqTypes { get; set; }
        public virtual DbSet<PimsLeaseProgramType> PimsLeaseProgramTypes { get; set; }
        public virtual DbSet<PimsLeasePurposeType> PimsLeasePurposeTypes { get; set; }
        public virtual DbSet<PimsLeaseResponsibilityType> PimsLeaseResponsibilityTypes { get; set; }
        public virtual DbSet<PimsLeaseStatusType> PimsLeaseStatusTypes { get; set; }
        public virtual DbSet<PimsLeaseTenant> PimsLeaseTenants { get; set; }
        public virtual DbSet<PimsLeaseTenantHist> PimsLeaseTenantHists { get; set; }
        public virtual DbSet<PimsLeaseTerm> PimsLeaseTerms { get; set; }
        public virtual DbSet<PimsLeaseTermHist> PimsLeaseTermHists { get; set; }
        public virtual DbSet<PimsLeaseTermStatusType> PimsLeaseTermStatusTypes { get; set; }
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
        public virtual DbSet<PimsProjectHist> PimsProjectHists { get; set; }
        public virtual DbSet<PimsProjectStatusType> PimsProjectStatusTypes { get; set; }
        public virtual DbSet<PimsPropPropAdjacentLandType> PimsPropPropAdjacentLandTypes { get; set; }
        public virtual DbSet<PimsPropPropAnomalyType> PimsPropPropAnomalyTypes { get; set; }
        public virtual DbSet<PimsPropPropRoadType> PimsPropPropRoadTypes { get; set; }
        public virtual DbSet<PimsPropPropTenureType> PimsPropPropTenureTypes { get; set; }
        public virtual DbSet<PimsPropResearchPurposeType> PimsPropResearchPurposeTypes { get; set; }
        public virtual DbSet<PimsProperty> PimsProperties { get; set; }
        public virtual DbSet<PimsPropertyAcquisitionFile> PimsPropertyAcquisitionFiles { get; set; }
        public virtual DbSet<PimsPropertyAcquisitionFileHist> PimsPropertyAcquisitionFileHists { get; set; }
        public virtual DbSet<PimsPropertyAdjacentLandType> PimsPropertyAdjacentLandTypes { get; set; }
        public virtual DbSet<PimsPropertyAnomalyType> PimsPropertyAnomalyTypes { get; set; }
        public virtual DbSet<PimsPropertyBoundaryVw> PimsPropertyBoundaryVws { get; set; }
        public virtual DbSet<PimsPropertyClassificationType> PimsPropertyClassificationTypes { get; set; }
        public virtual DbSet<PimsPropertyEvaluation> PimsPropertyEvaluations { get; set; }
        public virtual DbSet<PimsPropertyEvaluationHist> PimsPropertyEvaluationHists { get; set; }
        public virtual DbSet<PimsPropertyHist> PimsPropertyHists { get; set; }
        public virtual DbSet<PimsPropertyImprovement> PimsPropertyImprovements { get; set; }
        public virtual DbSet<PimsPropertyImprovementHist> PimsPropertyImprovementHists { get; set; }
        public virtual DbSet<PimsPropertyImprovementType> PimsPropertyImprovementTypes { get; set; }
        public virtual DbSet<PimsPropertyLease> PimsPropertyLeases { get; set; }
        public virtual DbSet<PimsPropertyLeaseHist> PimsPropertyLeaseHists { get; set; }
        public virtual DbSet<PimsPropertyLocationVw> PimsPropertyLocationVws { get; set; }
        public virtual DbSet<PimsPropertyOrganization> PimsPropertyOrganizations { get; set; }
        public virtual DbSet<PimsPropertyOrganizationHist> PimsPropertyOrganizationHists { get; set; }
        public virtual DbSet<PimsPropertyPropertyServiceFile> PimsPropertyPropertyServiceFiles { get; set; }
        public virtual DbSet<PimsPropertyPropertyServiceFileHist> PimsPropertyPropertyServiceFileHists { get; set; }
        public virtual DbSet<PimsPropertyResearchFile> PimsPropertyResearchFiles { get; set; }
        public virtual DbSet<PimsPropertyResearchFileHist> PimsPropertyResearchFileHists { get; set; }
        public virtual DbSet<PimsPropertyRoadType> PimsPropertyRoadTypes { get; set; }
        public virtual DbSet<PimsPropertyServiceFile> PimsPropertyServiceFiles { get; set; }
        public virtual DbSet<PimsPropertyServiceFileHist> PimsPropertyServiceFileHists { get; set; }
        public virtual DbSet<PimsPropertyServiceFileType> PimsPropertyServiceFileTypes { get; set; }
        public virtual DbSet<PimsPropertyStatusType> PimsPropertyStatusTypes { get; set; }
        public virtual DbSet<PimsPropertyTax> PimsPropertyTaxes { get; set; }
        public virtual DbSet<PimsPropertyTaxHist> PimsPropertyTaxHists { get; set; }
        public virtual DbSet<PimsPropertyTaxRemitType> PimsPropertyTaxRemitTypes { get; set; }
        public virtual DbSet<PimsPropertyTenureType> PimsPropertyTenureTypes { get; set; }
        public virtual DbSet<PimsPropertyType> PimsPropertyTypes { get; set; }
        public virtual DbSet<PimsProvinceState> PimsProvinceStates { get; set; }
        public virtual DbSet<PimsRegion> PimsRegions { get; set; }
        public virtual DbSet<PimsRegionUser> PimsRegionUsers { get; set; }
        public virtual DbSet<PimsRegionUserHist> PimsRegionUserHists { get; set; }
        public virtual DbSet<PimsRequestSourceType> PimsRequestSourceTypes { get; set; }
        public virtual DbSet<PimsResearchActivityInstance> PimsResearchActivityInstances { get; set; }
        public virtual DbSet<PimsResearchActivityInstanceHist> PimsResearchActivityInstanceHists { get; set; }
        public virtual DbSet<PimsResearchFile> PimsResearchFiles { get; set; }
        public virtual DbSet<PimsResearchFileHist> PimsResearchFileHists { get; set; }
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
        public virtual DbSet<PimsTenant> PimsTenants { get; set; }
        public virtual DbSet<PimsTenantType> PimsTenantTypes { get; set; }
        public virtual DbSet<PimsUser> PimsUsers { get; set; }
        public virtual DbSet<PimsUserHist> PimsUserHists { get; set; }
        public virtual DbSet<PimsUserOrganization> PimsUserOrganizations { get; set; }
        public virtual DbSet<PimsUserOrganizationHist> PimsUserOrganizationHists { get; set; }
        public virtual DbSet<PimsUserRole> PimsUserRoles { get; set; }
        public virtual DbSet<PimsUserRoleHist> PimsUserRoleHists { get; set; }
        public virtual DbSet<PimsVolumeUnitType> PimsVolumeUnitTypes { get; set; }
        public virtual DbSet<PimsVolumetricType> PimsVolumetricTypes { get; set; }
        public virtual DbSet<PimsWorkActivityCode> PimsWorkActivityCodes { get; set; }
        public virtual DbSet<PimsWorkActivityCodeHist> PimsWorkActivityCodeHists { get; set; }
        public virtual DbSet<PimsYearlyFinancialCode> PimsYearlyFinancialCodes { get; set; }
        public virtual DbSet<PimsYearlyFinancialCodeHist> PimsYearlyFinancialCodeHists { get; set; }
        public virtual DbSet<PimsxTableDefinition> PimsxTableDefinitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<BcaAreaAmendment>(entity =>
            {
                entity.Property(e => e.AmendmentReasonCode).HasComment("A code indicating the amendment type.");

                entity.Property(e => e.AmendmentReasonDescription).HasComment("A short description of the amendment reason.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.FolioCount).HasComment("The folio count for the amendment type.");

                entity.HasOne(d => d.AreaCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.AreaCode)
                    .HasConstraintName("PIM_BCASAR_PIM_BCAAMD_FK");
            });

            modelBuilder.Entity<BcaAreaBctransitValue>(entity =>
            {
                entity.HasComment("Values summarized by property class and sub-class for BC Transit purposes.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GrossImprovementValue).HasComment("The gross improvement value.");

                entity.Property(e => e.GrossLandValue).HasComment("The gross land value.");

                entity.Property(e => e.NetImprovementValue).HasComment("The net improvement value.");

                entity.Property(e => e.NetLandValue).HasComment("The net land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the area class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The area class description.");

                entity.Property(e => e.PropertySubclassCode).HasComment("A code indicating the area sub-class.");

                entity.Property(e => e.PropertySubclassDescription).HasComment("The area sub-class description.");

                entity.Property(e => e.TaxExemptImprovementValue).HasComment("The tax exempt improvement value.");

                entity.Property(e => e.TaxExemptLandValue).HasComment("The tax exempt land value.");

                entity.HasOne(d => d.AreaCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.AreaCode)
                    .HasConstraintName("PIM_BCASAR_PIM_BCATRV_FK");
            });

            modelBuilder.Entity<BcaAreaDelete>(entity =>
            {
                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DeleteReasonCode)
                    .HasDefaultValueSql("('UNKNOWN')")
                    .HasComment("A code indicating the delete reason.");

                entity.Property(e => e.DeleteReasonDescription).HasComment("A short description of the delete reason.");

                entity.Property(e => e.FolioCount).HasComment("The folio count for the delete reason.");

                entity.HasOne(d => d.AreaCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.AreaCode)
                    .HasConstraintName("PIM_BCASAR_PIM_BCADEL_FK");
            });

            modelBuilder.Entity<BcaAreaGeneralValue>(entity =>
            {
                entity.HasComment("Values summarized by property class and sub-class for general purposes.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GrossImprovementValue).HasComment("The gross improvement value.");

                entity.Property(e => e.GrossLandValue).HasComment("The gross land value.");

                entity.Property(e => e.NetImprovementValue).HasComment("The net improvement value.");

                entity.Property(e => e.NetLandValue).HasComment("The net land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the area class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The area class description.");

                entity.Property(e => e.PropertySubclassCode).HasComment("A code indicating the area sub-class.");

                entity.Property(e => e.PropertySubclassDescription).HasComment("The area sub-class description.");

                entity.Property(e => e.TaxExemptImprovementValue).HasComment("The tax exempt improvement value.");

                entity.Property(e => e.TaxExemptLandValue).HasComment("The tax exempt land value.");

                entity.HasOne(d => d.AreaCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.AreaCode)
                    .HasConstraintName("PIM_BCASAR_PIM_BCAGNV_FK");
            });

            modelBuilder.Entity<BcaAreaSchoolValue>(entity =>
            {
                entity.HasComment("Values summarized by property class and sub-class for school purposes.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GrossImprovementValue).HasComment("The gross improvement value.");

                entity.Property(e => e.GrossLandValue).HasComment("The gross land value.");

                entity.Property(e => e.NetImprovementValue).HasComment("The net improvement value.");

                entity.Property(e => e.NetLandValue).HasComment("The net land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the area class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The area class description.");

                entity.Property(e => e.PropertySubclassCode).HasComment("A code indicating the area sub-class.");

                entity.Property(e => e.PropertySubclassDescription).HasComment("The area sub-class description.");

                entity.Property(e => e.TaxExemptImprovementValue).HasComment("The tax exempt improvement value.");

                entity.Property(e => e.TaxExemptLandValue).HasComment("The tax exempt land value.");

                entity.HasOne(d => d.AreaCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.AreaCode)
                    .HasConstraintName("PIM_BCASAR_PIM_BCASCV_FK");
            });

            modelBuilder.Entity<BcaAssessmentArea>(entity =>
            {
                entity.HasKey(e => e.AreaCode)
                    .HasName("BCASAR_PK");

                entity.HasComment("Represents a folio group for a single BCA assessment area.");

                entity.Property(e => e.AreaCode).HasComment("The BCA code that identifies the assessment area.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("The full name/description of the BCA assessment area.");

                entity.HasOne(d => d.DataAdvice)
                    .WithMany(p => p.BcaAssessmentAreas)
                    .HasForeignKey(d => d.DataAdviceId)
                    .HasConstraintName("PIM_BCDADV_PIM_BCASAR_FK");
            });

            modelBuilder.Entity<BcaDataAdvice>(entity =>
            {
                entity.HasKey(e => e.DataAdviceId)
                    .HasName("BCDADV_PK");

                entity.HasComment("Represents an entire Data Advice XML delivery for a single order.");

                entity.Property(e => e.DataAdviceId)
                    .HasDefaultValueSql("(NEXT VALUE FOR [BCA_DATA_ADVICE_ID_SEQ])")
                    .HasComment("Primary key");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.EndDate).HasComment("The end date of the reporting period for the Data Advice XML document.");

                entity.Property(e => e.OrderId).HasComment("A unique identifier for the order that generated the Data Advice XML document.  The value is intended for BCA internal purposes only.");

                entity.Property(e => e.OwnershipYear).HasComment("The ownership year parameter used to generate the Data Advice XML document.");

                entity.Property(e => e.RequestId).HasComment("A unique identifier for the request that specified the order parameters.  The value is intended for BCA internal purposes only.");

                entity.Property(e => e.RollYear).HasComment("The roll year parameter used to generate the Data Advice XML document.");

                entity.Property(e => e.RunDate).HasComment("The date when the order was run to generate the Data Advice XML document.");

                entity.Property(e => e.RunType).HasComment("Represents a code indicating the run type that generated the Data Advice XML document.");

                entity.Property(e => e.StartDate).HasComment("The start date of the reporting period for the Data Advice XML document.");

                entity.Property(e => e.TaxExemptFolioCount).HasComment("The number of exempt folios included in the Data Advice XML document.");

                entity.Property(e => e.TaxableFolioCount).HasComment("The number of taxable folios included in the Data Advice XML document.");

                entity.Property(e => e.TotalFolioCount).HasComment("The total number of folios included in the Data Advice XML document.");

                entity.Property(e => e.TotalGrossImprovementValue).HasComment("The gross sum of all improvement(s) values reported for the folio group.");

                entity.Property(e => e.TotalGrossLandValue).HasComment("The gross sum of all land values reported for the folio group.");

                entity.Property(e => e.TotalNetImprovementValue).HasComment("The net sum of all improvement(s) values reported for the folio group.");

                entity.Property(e => e.TotalNetLandValue).HasComment("The net sum of all land values reported for the folio group.");

                entity.Property(e => e.TotalTaxExemptImprovementValue).HasComment("The tax exempt sum of all improvement(s) values reported for the folio group.");

                entity.Property(e => e.TotalTaxExemptLandValue).HasComment("The tax exempt sum of all land values reported for the folio group.");

                entity.Property(e => e.Version).HasComment("Represents a version number of one to four integers separated by periods (e.g. 1.0).  Version numbers are assumed to be ordered according to standard conventions.  If \"x\" and \"y\" are version numbers where \"x\" precedes \"y\" in sort order, then \"x\" shall ide");
            });

            modelBuilder.Entity<BcaDataAdviceAmendment>(entity =>
            {
                entity.Property(e => e.AmendmentReasonCode).HasComment("A code indicating the amendment type.");

                entity.Property(e => e.AmendmentReasonDescription).HasComment("A short description of the amendment reason.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.FolioCount).HasComment("The folio count for the amendment type.");

                entity.HasOne(d => d.DataAdvice)
                    .WithMany()
                    .HasForeignKey(d => d.DataAdviceId)
                    .HasConstraintName("PIM_BCDADV_PIM_BCDAMD_FK");
            });

            modelBuilder.Entity<BcaDataAdviceBctransitValue>(entity =>
            {
                entity.HasComment("Values summarized by property class and sub-class for BC Transit purposes.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GrossImprovementValue).HasComment("The gross improvement value.");

                entity.Property(e => e.GrossLandValue).HasComment("The gross land value.");

                entity.Property(e => e.NetImprovementValue).HasComment("The net improvement value.");

                entity.Property(e => e.NetLandValue).HasComment("The net land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the property class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The property class description.");

                entity.Property(e => e.PropertySubclassCode).HasComment("A code indicating the property sub-class.");

                entity.Property(e => e.PropertySubclassDescription).HasComment("The property sub-class description.");

                entity.Property(e => e.TaxExemptImprovementValue).HasComment("The tax exempt improvement value.");

                entity.Property(e => e.TaxExemptLandValue).HasComment("The tax exempt land value.");

                entity.HasOne(d => d.DataAdvice)
                    .WithMany()
                    .HasForeignKey(d => d.DataAdviceId)
                    .HasConstraintName("PIM_BCDADV_PIM_BCDTRV_FK");
            });

            modelBuilder.Entity<BcaDataAdviceDelete>(entity =>
            {
                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DeleteReasonCode)
                    .HasDefaultValueSql("('UNKNOWN')")
                    .HasComment("A code indicating the delete reason.");

                entity.Property(e => e.DeleteReasonDescription).HasComment("A short description of the delete reason.");

                entity.Property(e => e.FolioCount).HasComment("The folio count for the delete reason.");

                entity.HasOne(d => d.DataAdvice)
                    .WithMany()
                    .HasForeignKey(d => d.DataAdviceId)
                    .HasConstraintName("PIM_BCDADV_PIM_BCDDEL_FK");
            });

            modelBuilder.Entity<BcaDataAdviceGeneralValue>(entity =>
            {
                entity.HasComment("Values summarized by property class and sub-class for general purposes.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GrossImprovementValue).HasComment("The gross improvement value.");

                entity.Property(e => e.GrossLandValue).HasComment("The gross land value.");

                entity.Property(e => e.NetImprovementValue).HasComment("The net improvement value.");

                entity.Property(e => e.NetLandValue).HasComment("The net land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the property class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The property class description.");

                entity.Property(e => e.PropertySubclassCode).HasComment("A code indicating the property sub-class.");

                entity.Property(e => e.PropertySubclassDescription).HasComment("The property sub-class description.");

                entity.Property(e => e.TaxExemptImprovementValue).HasComment("The tax exempt improvement value.");

                entity.Property(e => e.TaxExemptLandValue).HasComment("The tax exempt land value.");

                entity.HasOne(d => d.DataAdvice)
                    .WithMany()
                    .HasForeignKey(d => d.DataAdviceId)
                    .HasConstraintName("PIM_BCDADV_PIM_BCDGNV_FK");
            });

            modelBuilder.Entity<BcaDataAdviceSchoolValue>(entity =>
            {
                entity.HasComment("Values summarized by property class and sub-class for school purposes.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GrossImprovementValue).HasComment("The gross improvement value.");

                entity.Property(e => e.GrossLandValue).HasComment("The gross land value.");

                entity.Property(e => e.NetImprovementValue).HasComment("The net improvement value.");

                entity.Property(e => e.NetLandValue).HasComment("The net land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the property class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The property class description.");

                entity.Property(e => e.PropertySubclassCode).HasComment("A code indicating the property sub-class.");

                entity.Property(e => e.PropertySubclassDescription).HasComment("The property sub-class description.");

                entity.Property(e => e.TaxExemptImprovementValue).HasComment("The tax exempt improvement value.");

                entity.Property(e => e.TaxExemptLandValue).HasComment("The tax exempt land value.");

                entity.HasOne(d => d.DataAdvice)
                    .WithMany()
                    .HasForeignKey(d => d.DataAdviceId)
                    .HasConstraintName("PIM_BCDADV_PIM_BCDSCV_FK");
            });

            modelBuilder.Entity<BcaDefined>(entity =>
            {
                entity.HasComment("The Defined minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.MinorTaxingCode).HasComment("A code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingCodeShort).HasComment("A one-character code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingDescription).HasComment("The full name/description of the minor taxing jurisdiction (MTXJ).");

                entity.HasOne(d => d.MinorTaxing)
                    .WithMany()
                    .HasForeignKey(d => d.MinorTaxingId)
                    .HasConstraintName("PIM_BCMNTX_PIM_BCADFN_FK");
            });

            modelBuilder.Entity<BcaElectoralArea>(entity =>
            {
                entity.HasComment("The Electoral Areas minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.MinorTaxingCode).HasComment("A code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingCodeShort).HasComment("A one-character code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingDescription).HasComment("The full name/description of the minor taxing jurisdiction (MTXJ).");

                entity.HasOne(d => d.MinorTaxing)
                    .WithMany()
                    .HasForeignKey(d => d.MinorTaxingId)
                    .HasConstraintName("PIM_BCMNTX_PIM_BCELCT_FK");
            });

            modelBuilder.Entity<BcaFolioAddress>(entity =>
            {
                entity.Property(e => e.AddressId).HasComment("Unique address identifier provided by BC Assessment.");

                entity.Property(e => e.City).HasComment("City name.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.MapReferenceNumber).HasComment("Formerly National Topographic System (NTS) number. A geographic reference.");

                entity.Property(e => e.PostalZip).HasComment("Postal or zip code.");

                entity.Property(e => e.PrimaryFlag)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates whether the address is the primary address associated with the folio.");

                entity.Property(e => e.ProvinceState).HasComment("Province or state.");

                entity.Property(e => e.StreetDirectionPrefix).HasComment("Further refinement of the street name to facilitate location of the folio (pre-directional).");

                entity.Property(e => e.StreetDirectionSuffix).HasComment("Further refinement of the street name to facilitate location of the folio (post-directional).");

                entity.Property(e => e.StreetName).HasComment("Street identifier assigned by a local government (municipality).");

                entity.Property(e => e.StreetNumber).HasComment("Street number assigned by a local government (municipality).");

                entity.Property(e => e.StreetType).HasComment("Road, Street, Place, etc.");

                entity.Property(e => e.UnitNumber).HasComment("Apartment or suite or unit number.");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCFADR_FK");
            });

            modelBuilder.Entity<BcaFolioAmendment>(entity =>
            {
                entity.Property(e => e.AmendmentReasonCode).HasComment("Describes the reason for the amendment.");

                entity.Property(e => e.AmendmentReasonDescription).HasComment("A short description about the Amendment Reason.");

                entity.Property(e => e.AmendmentType).HasComment("A code identifying the process that resulted in the amendment.");

                entity.Property(e => e.AmendmentTypeDescription).HasComment("A short description about the Amendment Type.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.SuppOccupancyCode).HasComment("Specifies whether the SUPP occupancy date indicates a begin (B) or end (E) date.");

                entity.Property(e => e.SuppOccupancyDate).HasComment("Specifies the date when a SUPP occupancy began or ended.");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCAFAM_FK");
            });

            modelBuilder.Entity<BcaFolioBctransitValue>(entity =>
            {
                entity.HasComment("Values summarized by property class and sub-class for BC Transit purposes.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GrossImprovementValue).HasComment("The gross improvement value.");

                entity.Property(e => e.GrossLandValue).HasComment("The gross land value.");

                entity.Property(e => e.NetImprovementValue).HasComment("The net improvement value.");

                entity.Property(e => e.NetLandValue).HasComment("The net land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the jurisdiction class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The jurisdiction class description.");

                entity.Property(e => e.PropertySubclassCode).HasComment("A code indicating the jurisdiction sub-class.");

                entity.Property(e => e.PropertySubclassDescription).HasComment("The jurisdiction sub-class description.");

                entity.Property(e => e.TaxExemptImprovementValue).HasComment("The tax exempt improvement value.");

                entity.Property(e => e.TaxExemptLandValue).HasComment("The tax exempt land value.");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCFTRV_FK");
            });

            modelBuilder.Entity<BcaFolioDescription>(entity =>
            {
                entity.HasComment("Describes general characteristics of the folio.");

                entity.Property(e => e.ActualUseCode).HasComment("Represents the code for an entry in a lookup table.");

                entity.Property(e => e.ActualUseDescription).HasComment("The full name/description of the Actual Use.");

                entity.Property(e => e.AddSchoolTax3mTo4mFlag).HasComment("Indicates whether the folio is a subject to additional school tax (residential portion assessed between $3 and $4 million).");

                entity.Property(e => e.AddSchoolTaxGreater4mFlag).HasComment("Indicates whether the folio is a subject to additional school tax (residential portion assessed over $4 million).");

                entity.Property(e => e.AlrCode).HasComment("The Agricultural Land Reserve identifier for the folio.");

                entity.Property(e => e.AlrDescription).HasComment("A short description about the ALR code.");

                entity.Property(e => e.BctransitFlag).HasComment("Indicates whether the folio is subject to BC Transit taxation.");

                entity.Property(e => e.CandidateForSpecTaxFlag).HasComment("Indicates whether the folio is a candidate for speculation tax.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.LandDepth).HasComment("Land depth measurement.");

                entity.Property(e => e.LandDimension).HasComment("Freeform land measurement.");

                entity.Property(e => e.LandDimensionType).HasComment("A code indicating the type of land measurements being used.");

                entity.Property(e => e.LandDimensionTypeDescription).HasComment("A short description about the land dimension type.");

                entity.Property(e => e.LandWidth).HasComment("Land width measurement.");

                entity.Property(e => e.NeighbourhoodCode).HasComment("A code identifying the neighbourhood.");

                entity.Property(e => e.NeighbourhoodDescription).HasComment("The full name/description of the neighbourhood.");

                entity.Property(e => e.ParkingArea).HasComment("Identifies the total value of the parking area (TransLink values only for Roll Years 2005 to 2007).");

                entity.Property(e => e.PoliceTaxFlag).HasComment("Indicates whether the folio is subject to exemption.");

                entity.Property(e => e.PredominantManualClassCode).HasComment("A code identifying the state and condition of improvements and structural components.");

                entity.Property(e => e.PredominantManualClassDescription).HasComment("A short description about the manual class code.");

                entity.Property(e => e.PredominantPercentDeviation).HasComment("A percentage deviation from the manual class code.");

                entity.Property(e => e.RegionalDistrictCode).HasComment("A code identifying the special district.");

                entity.Property(e => e.RegionalDistrictDescription).HasComment("The full name/description of the special district.");

                entity.Property(e => e.RegionalHospitalDistrictCode).HasComment("A code identifying the special district.");

                entity.Property(e => e.RegionalHospitalDistrictDescription).HasComment("The full name/description of the special district.");

                entity.Property(e => e.SchoolDistrictCode).HasComment("A code identifying the special district.");

                entity.Property(e => e.SchoolDistrictDescription).HasComment("The full name/description of the special district.");

                entity.Property(e => e.TenureCode).HasComment("Identifies the type of ownership or occupation on land.");

                entity.Property(e => e.TenureDescription).HasComment("The short name/description of the tenure code.");

                entity.Property(e => e.VacantFlag).HasComment("Indicates whether the folio is vacant or occupied.");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCAFDE_FK");
            });

            modelBuilder.Entity<BcaFolioFarm>(entity =>
            {
                entity.HasComment("Farm associated with this folio.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.FarmNumber).HasComment("A BCA farm identification number");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCFARM_FK");
            });

            modelBuilder.Entity<BcaFolioGeneralValue>(entity =>
            {
                entity.HasComment("Values summarized by property class and sub-class for general purposes.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GrossImprovementValue).HasComment("The gross improvement value.");

                entity.Property(e => e.GrossLandValue).HasComment("The gross land value.");

                entity.Property(e => e.NetImprovementValue).HasComment("The net improvement value.");

                entity.Property(e => e.NetLandValue).HasComment("The net land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the jurisdiction class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The jurisdiction class description.");

                entity.Property(e => e.PropertySubclassCode).HasComment("A code indicating the jurisdiction sub-class.");

                entity.Property(e => e.PropertySubclassDescription).HasComment("The jurisdiction sub-class description.");

                entity.Property(e => e.TaxExemptImprovementValue).HasComment("The tax exempt improvement value.");

                entity.Property(e => e.TaxExemptLandValue).HasComment("The tax exempt land value.");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCFGNV_FK");
            });

            modelBuilder.Entity<BcaFolioLandCharacteristic>(entity =>
            {
                entity.HasComment("Represents a land characteristic.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.LandCharacteristicCode).HasComment("A code indicating a characteristic of the land.");

                entity.Property(e => e.LandCharacteristicDescription).HasComment("A description of the LandCharacteristicCode.");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCLCHR_FK");
            });

            modelBuilder.Entity<BcaFolioLegalDescription>(entity =>
            {
                entity.Property(e => e.AirSpaceParcelNumber).HasComment("A volumetric parcel identifier.");

                entity.Property(e => e.BcaGroup).HasComment("The legal description group as defined by BCA.");

                entity.Property(e => e.Block).HasComment("Block, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DistrictLot).HasComment("District lot, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.ExceptPlan).HasComment("Except plan, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.FirstNationReserveDescription).HasComment("The short name/description of the FirstNationReserveNumber.");

                entity.Property(e => e.FirstNationReserveNumber).HasComment("Identifier assigned to FN reserve properties by BCA.");

                entity.Property(e => e.FormattedLegalDescription).HasComment("A formatted string of all the legal description attributes with appropriate labels.");

                entity.Property(e => e.LandBranchFileNumber).HasComment("Lands Branch File Number as defined by the Ministry of Forest, Lands, and Natural Resource Operations.");

                entity.Property(e => e.LandDistrict).HasComment("Land district, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.LandDistrictDescription).HasComment("The short name/description of the LandDistrict.");

                entity.Property(e => e.LeaseLicenseNumber).HasComment("Lease licence number as defined by the Ministry of Forest, Lands, and Natural Resource Operations and other valid sources of leases added to the assessment roll.");

                entity.Property(e => e.LegalSubdivision).HasComment("Legal subdivision, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.LegalText).HasComment("Freeform legal text, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.Lot).HasComment("Lot, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.Meridian).HasComment("Meridian Code, identifies a line of longitude, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.MeridianShort).HasComment("Meridian Code, identifies a line of longitude, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.NtsLocation).HasComment("National Topographic System Reference Number.");

                entity.Property(e => e.Parcel).HasComment("Parcel, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.Part1).HasComment("Part 1, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.Part2).HasComment("Part 2, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.Part3).HasComment("Part 3, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.Part4).HasComment("Part 4, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.Pid).HasComment("The PID that uniquely identifies the legal description.");

                entity.Property(e => e.PlanNumber).HasComment("Plan number, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.Portion).HasComment("Portion, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.Range).HasComment("Range, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.Section).HasComment("Section, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.StrataLot).HasComment("Strata Lot, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.SubBlock).HasComment("Sub block, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.SubLot).HasComment("Suburban lot, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.Property(e => e.Township).HasComment("Township, part of the legal description of a parcel of land as provided by the Land Title and Survey Authority.");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCAFLD_FK");
            });

            modelBuilder.Entity<BcaFolioManagedForest>(entity =>
            {
                entity.HasComment("Managed forest information associated with this folio.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.FarmNumber).HasComment("A BCA farm identification number");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCMFOR_FK");
            });

            modelBuilder.Entity<BcaFolioManufacturedHome>(entity =>
            {
                entity.HasComment("Provides information for a single manufactured home.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.MhBayNumber).HasComment("Bay number associated with the manufactured home.");

                entity.Property(e => e.MhPark).HasComment("The name or other identification of the park associated with the manufactured home.");

                entity.Property(e => e.MhParkRollNumber).HasComment("Park roll number associated with the manufactured home.");

                entity.Property(e => e.MhRegistryNumber).HasComment("Registration number for a manufactured home (MH) by the Manufactured Home Registry.");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCMANH_FK");
            });

            modelBuilder.Entity<BcaFolioOilAndGa>(entity =>
            {
                entity.HasComment("Oil and gas information associated with this folio.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.FarmNumber).HasComment("A BCA farm identification number");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCOILG_FK");
            });

            modelBuilder.Entity<BcaFolioRecord>(entity =>
            {
                entity.HasKey(e => e.RollNumber)
                    .HasName("BCAFOR_PK");

                entity.HasComment("Represents a data record for a single folio and its associated attributes.");

                entity.Property(e => e.RollNumber).HasComment("The unique identifier for the specific folio within its jurisdiction.");

                entity.Property(e => e.Action).HasComment("Specifies the folio action: Add, Delete.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.FolioStatus).HasComment("Indicates the status of the folio at the time the Data Advice XML document was generated.");

                entity.Property(e => e.FolioStatusDescription).HasComment("The short name/description of the folio status.");

                entity.HasOne(d => d.JurisdictionCodeNavigation)
                    .WithMany(p => p.BcaFolioRecords)
                    .HasForeignKey(d => d.JurisdictionCode)
                    .HasConstraintName("PIM_BCAJUR_PIM_BCAFOR_FK");
            });

            modelBuilder.Entity<BcaFolioSale>(entity =>
            {
                entity.Property(e => e.ConveyanceDate).HasComment("The date associated with this sale.");

                entity.Property(e => e.ConveyancePrice).HasComment("The amount of money exchanged as part of this sale.");

                entity.Property(e => e.ConveyanceType).HasComment("The conveyance type code identifying the characteristics of the sale.");

                entity.Property(e => e.ConveyanceTypeDescription).HasComment("A short description about the conveyance type.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DocumentNumber).HasComment("The LTSA Document Number assigned in regard to an Indefeasible State of Title Certificate, which indicates ownership or title.");

                entity.Property(e => e.RejectReasonCode).HasComment("The reject reason code identifying why a sale was rejected.");

                entity.Property(e => e.RejectReasonDescription).HasComment("A short description about the reject reason code.");

                entity.Property(e => e.SaleDate).HasComment("The date the LTSA document was received for registration at Land Title and Survey Authority, subject to adjustment by BCA.");

                entity.Property(e => e.SalePrice).HasComment("The price provided on the LTSA document as received for registration at Land Title and Survey Authority, subject to adjustment by BCA.");

                entity.Property(e => e.SaleStatusCode).HasComment("The code applied to the sale record by BCA property assessment staff to identify eligibility for inclusion into overall market analysis.");

                entity.Property(e => e.SaleStatusDescription).HasComment("A short description about the sale status code.");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCAFSA_FK");
            });

            modelBuilder.Entity<BcaFolioSchoolValue>(entity =>
            {
                entity.HasComment("Values summarized by property class and sub-class for school purposes.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GrossImprovementValue).HasComment("The gross improvement value.");

                entity.Property(e => e.GrossLandValue).HasComment("The gross land value.");

                entity.Property(e => e.NetImprovementValue).HasComment("The net improvement value.");

                entity.Property(e => e.NetLandValue).HasComment("The net land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the jurisdiction class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The jurisdiction class description.");

                entity.Property(e => e.PropertySubclassCode).HasComment("A code indicating the jurisdiction sub-class.");

                entity.Property(e => e.PropertySubclassDescription).HasComment("The jurisdiction sub-class description.");

                entity.Property(e => e.TaxExemptImprovementValue).HasComment("The tax exempt improvement value.");

                entity.Property(e => e.TaxExemptLandValue).HasComment("The tax exempt land value.");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCFSCV_FK");
            });

            modelBuilder.Entity<BcaFolioValuation>(entity =>
            {
                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ImprovementValue).HasComment("The improvement(s) value.");

                entity.Property(e => e.LandValue).HasComment("The land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the property class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The code class description.");

                entity.Property(e => e.TaxExemptCode).HasComment("The tax exemption code.");

                entity.Property(e => e.TaxExemptDescription).HasComment("The tax exemption code description.");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCAVAL_FK");
            });

            modelBuilder.Entity<BcaGeneralService>(entity =>
            {
                entity.HasComment("The General Services minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.MinorTaxingCode).HasComment("A code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingCodeShort).HasComment("A one-character code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingDescription).HasComment("The full name/description of the minor taxing jurisdiction (MTXJ).");

                entity.HasOne(d => d.MinorTaxing)
                    .WithMany()
                    .HasForeignKey(d => d.MinorTaxingId)
                    .HasConstraintName("PIM_BCMNTX_PIM_BCGSVC_FK");
            });

            modelBuilder.Entity<BcaImprovementDistrict>(entity =>
            {
                entity.HasComment("The Improvement Districts minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.MinorTaxingCode).HasComment("A code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingCodeShort).HasComment("A one-character code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingDescription).HasComment("The full name/description of the minor taxing jurisdiction (MTXJ).");

                entity.HasOne(d => d.MinorTaxing)
                    .WithMany()
                    .HasForeignKey(d => d.MinorTaxingId)
                    .HasConstraintName("PIM_BCMNTX_PIM_BCIMPD_FK");
            });

            modelBuilder.Entity<BcaIslandsTrust>(entity =>
            {
                entity.HasComment("The Islands Trusts minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.MinorTaxingCode).HasComment("A code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingCodeShort).HasComment("A one-character code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingDescription).HasComment("The full name/description of the minor taxing jurisdiction (MTXJ).");

                entity.HasOne(d => d.MinorTaxing)
                    .WithMany()
                    .HasForeignKey(d => d.MinorTaxingId)
                    .HasConstraintName("PIM_BCMNTX_PIM_BCISLT_FK");
            });

            modelBuilder.Entity<BcaJurisdiction>(entity =>
            {
                entity.HasKey(e => e.JurisdictionCode)
                    .HasName("BCAJUR_PK");

                entity.Property(e => e.JurisdictionCode).HasComment("The BCA code that identifies the jurisdiction.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("The short name/description of the jurisdiction.");

                entity.HasOne(d => d.AreaCodeNavigation)
                    .WithMany(p => p.BcaJurisdictions)
                    .HasForeignKey(d => d.AreaCode)
                    .HasConstraintName("PIM_BCASAR_PIM_BCAJUR_FK");
            });

            modelBuilder.Entity<BcaJurisdictionAmendment>(entity =>
            {
                entity.Property(e => e.AmendmentReasonCode).HasComment("A code indicating the amendment type.");

                entity.Property(e => e.AmendmentReasonDescription).HasComment("A short description of the amendment reason.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.FolioCount).HasComment("The folio count for the amendment type.");

                entity.HasOne(d => d.JurisdictionCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.JurisdictionCode)
                    .HasConstraintName("PIM_BCAJUR_PIM_BCJAMD_FK");
            });

            modelBuilder.Entity<BcaJurisdictionBctransitValue>(entity =>
            {
                entity.HasComment("Values summarized by property class and sub-class for BC Transit purposes.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GrossImprovementValue).HasComment("The gross improvement value.");

                entity.Property(e => e.GrossLandValue).HasComment("The gross land value.");

                entity.Property(e => e.NetImprovementValue).HasComment("The net improvement value.");

                entity.Property(e => e.NetLandValue).HasComment("The net land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the jurisdiction class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The jurisdiction class description.");

                entity.Property(e => e.PropertySubclassCode).HasComment("A code indicating the jurisdiction sub-class.");

                entity.Property(e => e.PropertySubclassDescription).HasComment("The jurisdiction sub-class description.");

                entity.Property(e => e.TaxExemptImprovementValue).HasComment("The tax exempt improvement value.");

                entity.Property(e => e.TaxExemptLandValue).HasComment("The tax exempt land value.");

                entity.HasOne(d => d.JurisdictionCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.JurisdictionCode)
                    .HasConstraintName("PIM_BCAJUR_PIM_BCJTRV_FK");
            });

            modelBuilder.Entity<BcaJurisdictionDelete>(entity =>
            {
                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DeleteReasonCode)
                    .HasDefaultValueSql("('UNKNOWN')")
                    .HasComment("A code indicating the delete reason.");

                entity.Property(e => e.DeleteReasonDescription).HasComment("A short description of the delete reason.");

                entity.Property(e => e.FolioCount).HasComment("The folio count for the delete reason.");

                entity.HasOne(d => d.JurisdictionCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.JurisdictionCode)
                    .HasConstraintName("PIM_BCAJUR_PIM_BCJDEL_FK");
            });

            modelBuilder.Entity<BcaJurisdictionGeneralValue>(entity =>
            {
                entity.HasComment("Values summarized by property class and sub-class for general purposes.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GrossImprovementValue).HasComment("The gross improvement value.");

                entity.Property(e => e.GrossLandValue).HasComment("The gross land value.");

                entity.Property(e => e.NetImprovementValue).HasComment("The net improvement value.");

                entity.Property(e => e.NetLandValue).HasComment("The net land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the jurisdiction class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The jurisdiction class description.");

                entity.Property(e => e.PropertySubclassCode).HasComment("A code indicating the jurisdiction sub-class.");

                entity.Property(e => e.PropertySubclassDescription).HasComment("The jurisdiction sub-class description.");

                entity.Property(e => e.TaxExemptImprovementValue).HasComment("The tax exempt improvement value.");

                entity.Property(e => e.TaxExemptLandValue).HasComment("The tax exempt land value.");

                entity.HasOne(d => d.JurisdictionCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.JurisdictionCode)
                    .HasConstraintName("PIM_BCAJUR_PIM_BCJGNV_FK");
            });

            modelBuilder.Entity<BcaJurisdictionSchoolValue>(entity =>
            {
                entity.HasComment("Values summarized by property class and sub-class for school purposes.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GrossImprovementValue).HasComment("The gross improvement value.");

                entity.Property(e => e.GrossLandValue).HasComment("The gross land value.");

                entity.Property(e => e.NetImprovementValue).HasComment("The net improvement value.");

                entity.Property(e => e.NetLandValue).HasComment("The net land value.");

                entity.Property(e => e.PropertyClassCode).HasComment("A code indicating the jurisdiction class.");

                entity.Property(e => e.PropertyClassDescription).HasComment("The jurisdiction class description.");

                entity.Property(e => e.PropertySubclassCode).HasComment("A code indicating the jurisdiction sub-class.");

                entity.Property(e => e.PropertySubclassDescription).HasComment("The jurisdiction sub-class description.");

                entity.Property(e => e.TaxExemptImprovementValue).HasComment("The tax exempt improvement value.");

                entity.Property(e => e.TaxExemptLandValue).HasComment("The tax exempt land value.");

                entity.HasOne(d => d.JurisdictionCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.JurisdictionCode)
                    .HasConstraintName("PIM_BCAJUR_PIM_BCJSCV_FK");
            });

            modelBuilder.Entity<BcaLocalArea>(entity =>
            {
                entity.HasComment("The Local Areas minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.MinorTaxingCode).HasComment("A code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingCodeShort).HasComment("A one-character code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingDescription).HasComment("The full name/description of the minor taxing jurisdiction (MTXJ).");

                entity.HasOne(d => d.MinorTaxing)
                    .WithMany()
                    .HasForeignKey(d => d.MinorTaxingId)
                    .HasConstraintName("PIM_BCMNTX_PIM_BCLCLA_FK");
            });

            modelBuilder.Entity<BcaMinorTaxing>(entity =>
            {
                entity.HasKey(e => e.MinorTaxingId)
                    .HasName("BCMNTX_PK");

                entity.Property(e => e.MinorTaxingId).HasDefaultValueSql("(NEXT VALUE FOR [BCA_MINOR_TAXING_ID_SEQ])");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany(p => p.BcaMinorTaxings)
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCMNTX_FK");
            });

            modelBuilder.Entity<BcaOwner>(entity =>
            {
                entity.HasKey(e => e.OwnerId)
                    .HasName("BCAOWN_PK");

                entity.Property(e => e.OwnerId).HasDefaultValueSql("(NEXT VALUE FOR [BCA_OWNER_ID_SEQ])");

                entity.Property(e => e.CompanyOrLastName).HasComment("The company name or last name of an owner.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.EquityType).HasComment("Identifies the type of relationship an owner/lessee/occupier has to the folio.");

                entity.Property(e => e.EquityTypeDescription).HasComment("The short name/description of the equity type.");

                entity.Property(e => e.FirstName).HasComment("The first name of an owner.");

                entity.Property(e => e.MiddleInitial).HasComment("The middle initial of an owner.");

                entity.Property(e => e.MiddleName).HasComment("The middle name of an owner.");

                entity.Property(e => e.OwnerSequenceId).HasComment("Identifies the sequence owners should be listed within an ownership group.");

                entity.HasOne(d => d.OwnershipGroup)
                    .WithMany(p => p.BcaOwners)
                    .HasForeignKey(d => d.OwnershipGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_BCAOWG_PIM_BCAOWN_FK");
            });

            modelBuilder.Entity<BcaOwnershipGroup>(entity =>
            {
                entity.HasKey(e => e.OwnershipGroupId)
                    .HasName("BCAOWG_PK");

                entity.HasComment("Represents a group of property owners with details in common.");

                entity.Property(e => e.OwnershipGroupId).HasComment("Identifies the group of property owners.");

                entity.Property(e => e.AssessmentNoticeReturned)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates whether the Assessment Notice for the current roll year was returned");

                entity.Property(e => e.AssessmentNoticeSuppressed)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates whether BCA has not sent out an Assessment Notice to this owner based");

                entity.Property(e => e.ChangeDate).HasComment("The date of the most recent change to the ownership information.");

                entity.Property(e => e.ChangeSource).HasComment("Identifies the source of the most recent change to the ownership information.");

                entity.Property(e => e.ChangeSourceDescription).HasComment("The short name/description of the ChangeSource.");

                entity.Property(e => e.ChangeType).HasComment("A code identifying the type of the most recent change to the ownership information.");

                entity.Property(e => e.ChangeTypeDescription).HasComment("The short name/description of the ChangeType.");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.FormattedMailingAddrLine1).HasComment("Formatted address line for mailing purposes: Line 1 consists of C/O label and value.");

                entity.Property(e => e.FormattedMailingAddrLine2).HasComment("Formatted address line for mailing purposes: Line 2 consists of Attention label and value.");

                entity.Property(e => e.FormattedMailingAddrLine3).HasComment("Formatted address line for mailing purposes: Line 3 consists of value of freeform address field.");

                entity.Property(e => e.FormattedMailingAddrLine4).HasComment("Formatted address line for mailing purposes: Line 4 consists of Unit and Floor Number label and value if there is not enough room on Line 5.");

                entity.Property(e => e.FormattedMailingAddrLine5).HasComment("Formatted address line for mailing purposes: Line 5 consists of street number, name, type and directional data.");

                entity.Property(e => e.FormattedMailingAddrLine6).HasComment("Formatted address line for mailing purposes: Line 6 consists of Site and Compartment label and values");

                entity.Property(e => e.MailingAddrAttention).HasComment("Attention, as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrBulkMailCode).HasComment("Identifies when five or more properties are linked to a name record or when five or more name records with a single mailing address are linked to a folio.");

                entity.Property(e => e.MailingAddrCareOf).HasComment("Care Of, as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrCity).HasComment("City, as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrCompartment).HasComment("Compartment, as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrCountry).HasComment("Country, as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrDeliveryInstallationType).HasComment("Delivery installation type, as required for mail to be sent to the owners mailing address.  E.g. Station, Post Office, Letter Carrier Depot, etc.");

                entity.Property(e => e.MailingAddrFloor).HasComment("Floor, as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrFreeForm).HasComment("Additional mailing address information not fitting in any of the predefined fields, as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrModeOfDelivery).HasComment("Mode of Delivery, as required for mail to be sent to the owners mailing address.  e.g. Rural Route, Post Office Box, General Delivery, etc.");

                entity.Property(e => e.MailingAddrModeOfDeliveryValue).HasComment("Mode of delivery value, as required for mail to be sent to the owners mailing address.  e.g. RR 876, PO Box 19, etc.");

                entity.Property(e => e.MailingAddrPostalZip).HasComment("Postal or zip code, as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrProvinceState).HasComment("Province or state, as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrSite).HasComment("Site value, as required for mail to be sent to the owners mailing address.  e.g. Site 10.");

                entity.Property(e => e.MailingAddrStreetDirectionPrefix).HasComment("Further refinement of the street name to facilitate location of building (pre-directional), as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrStreetDirectionSuffix).HasComment("Further refinement of the street name to facilitate location of building (post-directional), as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrStreetName).HasComment("Road name assigned by a local government (municipality), as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrStreetNumber).HasComment("Street number assigned by a local government (municipality), as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrStreetType).HasComment("Street type assigned by a local government (municipality), as required for mail to be sent to the owners mailing address.");

                entity.Property(e => e.MailingAddrUnitNumber).HasComment("Apartment or Suite or Unit Number, as required for mail to be sent to the owners mailing address.");

                entity.HasOne(d => d.RollNumberNavigation)
                    .WithMany(p => p.BcaOwnershipGroups)
                    .HasForeignKey(d => d.RollNumber)
                    .HasConstraintName("PIM_BCAFOR_PIM_BCAOWG_FK");
            });

            modelBuilder.Entity<BcaServiceArea>(entity =>
            {
                entity.HasComment("The Service Areas minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.MinorTaxingCode).HasComment("A code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingCodeShort).HasComment("A one-character code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingDescription).HasComment("The full name/description of the minor taxing jurisdiction (MTXJ).");

                entity.HasOne(d => d.MinorTaxing)
                    .WithMany()
                    .HasForeignKey(d => d.MinorTaxingId)
                    .HasConstraintName("PIM_BCMNTX_PIM_BCSVCA_FK");
            });

            modelBuilder.Entity<BcaSpecifiedMunicipal>(entity =>
            {
                entity.HasComment("The Specified Municipal minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.MinorTaxingCode).HasComment("A code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingCodeShort).HasComment("A one-character code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingDescription).HasComment("The full name/description of the minor taxing jurisdiction (MTXJ).");

                entity.HasOne(d => d.MinorTaxing)
                    .WithMany()
                    .HasForeignKey(d => d.MinorTaxingId)
                    .HasConstraintName("PIM_BCMNTX_PIM_BCSPMU_FK");
            });

            modelBuilder.Entity<BcaSpecifiedRegional>(entity =>
            {
                entity.HasComment("The Specified Regional minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.MinorTaxingCode).HasComment("A code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingCodeShort).HasComment("A one-character code identifying the minor taxing jurisdiction (MTXJ).");

                entity.Property(e => e.MinorTaxingDescription).HasComment("The full name/description of the minor taxing jurisdiction (MTXJ).");

                entity.HasOne(d => d.MinorTaxing)
                    .WithMany()
                    .HasForeignKey(d => d.MinorTaxingId)
                    .HasConstraintName("PIM_BCMNTX_PIM_BCSPRG_FK");
            });

            modelBuilder.Entity<PimsAccessRequest>(entity =>
            {
                entity.HasKey(e => e.AccessRequestId)
                    .HasName("ACRQST_PK");

                entity.Property(e => e.AccessRequestId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACCESS_REQUEST_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Note).HasComment("Note associated with this access request.");

                entity.Property(e => e.RegionCode).HasDefaultValueSql("((4))");

                entity.HasOne(d => d.AccessRequestStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsAccessRequests)
                    .HasForeignKey(d => d.AccessRequestStatusTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ARQSTT_PIM_ACRQST_FK");

                entity.HasOne(d => d.RegionCodeNavigation)
                    .WithMany(p => p.PimsAccessRequests)
                    .HasForeignKey(d => d.RegionCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_REGION_PIM_ACRQST_FK");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PimsAccessRequests)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("PIM_ROLE_PIM_ACRQST_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PimsAccessRequests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_USER_PIM_ACRQST_FK");
            });

            modelBuilder.Entity<PimsAccessRequestHist>(entity =>
            {
                entity.HasKey(e => e.AccessRequestHistId)
                    .HasName("PIMS_ACRQST_H_PK");

                entity.Property(e => e.AccessRequestHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACCESS_REQUEST_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsAccessRequestOrganization>(entity =>
            {
                entity.HasKey(e => e.AccessRequestOrganizationId)
                    .HasName("ACRQOR_PK");

                entity.Property(e => e.AccessRequestOrganizationId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACCESS_REQUEST_ORGANIZATION_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.AccessRequest)
                    .WithMany(p => p.PimsAccessRequestOrganizations)
                    .HasForeignKey(d => d.AccessRequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ACRQST_PIM_ACRQOR_FK");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.PimsAccessRequestOrganizations)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("PIM_ORG_PIM_ACRQOR_FK");
            });

            modelBuilder.Entity<PimsAccessRequestOrganizationHist>(entity =>
            {
                entity.HasKey(e => e.AccessRequestOrganizationHistId)
                    .HasName("PIMS_ACRQOR_H_PK");

                entity.Property(e => e.AccessRequestOrganizationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACCESS_REQUEST_ORGANIZATION_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsAccessRequestStatusType>(entity =>
            {
                entity.HasKey(e => e.AccessRequestStatusTypeCode)
                    .HasName("ARQSTT_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsAcqFlPersonProfileType>(entity =>
            {
                entity.HasKey(e => e.AcqFlPersonProfileTypeCode)
                    .HasName("AQFPPT_PK");

                entity.HasComment("Codified values for the acquistion file staff profile (role).");

                entity.Property(e => e.AcqFlPersonProfileTypeCode).HasComment("Code value for the acquistion file staff profile (role).");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the acquistion file staff profile (role).");

                entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");
            });

            modelBuilder.Entity<PimsAcqPhysFileStatusType>(entity =>
            {
                entity.HasKey(e => e.AcqPhysFileStatusTypeCode)
                    .HasName("ACQPFS_PK");

                entity.HasComment("Codified values for the acquistion physical file status type.");

                entity.Property(e => e.AcqPhysFileStatusTypeCode).HasComment("Code value for the acquistion physical file status type.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the acquistion physical file status type.");

                entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");
            });

            modelBuilder.Entity<PimsAcquisitionActivityInstance>(entity =>
            {
                entity.HasKey(e => e.AcquisitionActivityInstanceId)
                    .HasName("ACQAIN_PK");

                entity.Property(e => e.AcquisitionActivityInstanceId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_ACTIVITY_INSTANCE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the relationship is active.");

                entity.HasOne(d => d.AcquisitionFile)
                    .WithMany(p => p.PimsAcquisitionActivityInstances)
                    .HasForeignKey(d => d.AcquisitionFileId)
                    .HasConstraintName("PIM_ACQNFL_PIM_ACQAIN_FK");

                entity.HasOne(d => d.ActivityInstance)
                    .WithMany(p => p.PimsAcquisitionActivityInstances)
                    .HasForeignKey(d => d.ActivityInstanceId)
                    .HasConstraintName("PIM_ACTINS_PIM_ACQAIN_FK");
            });

            modelBuilder.Entity<PimsAcquisitionActivityInstanceHist>(entity =>
            {
                entity.HasKey(e => e.AcquisitionActivityInstanceHistId)
                    .HasName("PIMS_ACQAIN_H_PK");

                entity.Property(e => e.AcquisitionActivityInstanceHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_ACTIVITY_INSTANCE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsAcquisitionFile>(entity =>
            {
                entity.HasKey(e => e.AcquisitionFileId)
                    .HasName("ACQNFL_PK");

                entity.HasComment("Entity containing information regarding an acquisition file.");

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

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.CpsProductCode).HasComment("CPS product code.");

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
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("Formatted file number assigned to the acquisition file.  Format follows YY-XXXXXX-ZZ where YY = MoTI region number, XXXXXX = generated integer sequence number,  and ZZ = file suffix number (defaulting to '01')");

                entity.Property(e => e.FundingOther).HasComment("Description of other funding type.");

                entity.Property(e => e.LegacyFileNumber).HasComment("Legacy formatted file number assigned to the acquisition file.  Format follows YY-XXXXXX-ZZ where YY = MoTI region number, XXXXXX = generated integer sequence number,  and ZZ = file suffix number (defaulting to '01').   Required due to some files having t");

                entity.Property(e => e.MinistryProjectName).HasComment("Ministry project name.");

                entity.Property(e => e.MinistryProjectNumber).HasComment("Ministry project number.");

                entity.Property(e => e.PaimsAcquisitionFileId).HasComment("Legacy Acquisition File ID from the PAIMS system.");

                entity.Property(e => e.RegionCode)
                    .HasDefaultValueSql("((-1))")
                    .HasComment("Region responsible for oversight of the acquisition.");

                entity.HasOne(d => d.AcqPhysFileStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsAcquisitionFiles)
                    .HasForeignKey(d => d.AcqPhysFileStatusTypeCode)
                    .HasConstraintName("PIM_ACQPFS_PIM_ACQNFL_FK");

                entity.HasOne(d => d.AcquisitionFileStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsAcquisitionFiles)
                    .HasForeignKey(d => d.AcquisitionFileStatusTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ACQFST_PIM_ACQNFL_FK");

                entity.HasOne(d => d.AcquisitionFundingTypeCodeNavigation)
                    .WithMany(p => p.PimsAcquisitionFiles)
                    .HasForeignKey(d => d.AcquisitionFundingTypeCode)
                    .HasConstraintName("PIM_ACQFTY_PIM_ACQNFL_FK");

                entity.HasOne(d => d.AcquisitionTypeCodeNavigation)
                    .WithMany(p => p.PimsAcquisitionFiles)
                    .HasForeignKey(d => d.AcquisitionTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ACQTYP_PIM_ACQNFL_FK");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PimsAcquisitionFiles)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("PIM_PRODCT_PIM_ACQNFL_FK");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.PimsAcquisitionFiles)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("PIM_PROJCT_PIM_ACQNFL_FK");

                entity.HasOne(d => d.RegionCodeNavigation)
                    .WithMany(p => p.PimsAcquisitionFiles)
                    .HasForeignKey(d => d.RegionCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_REGION_PIM_ACQNFL_FK");
            });

            modelBuilder.Entity<PimsAcquisitionFileHist>(entity =>
            {
                entity.HasKey(e => e.AcquisitionFileHistId)
                    .HasName("PIMS_ACQNFL_H_PK");

                entity.Property(e => e.AcquisitionFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsAcquisitionFileNote>(entity =>
            {
                entity.HasKey(e => e.AcquisitionFileNoteId)
                    .HasName("ACQNOT_PK");

                entity.Property(e => e.AcquisitionFileNoteId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_NOTE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the relationship has been disabled.");

                entity.HasOne(d => d.AcquisitionFile)
                    .WithMany(p => p.PimsAcquisitionFileNotes)
                    .HasForeignKey(d => d.AcquisitionFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ACQNFL_PIM_ACQNOT_FK");

                entity.HasOne(d => d.Note)
                    .WithOne(p => p.PimsAcquisitionFileNote)
                    .HasForeignKey<PimsAcquisitionFileNote>(d => d.NoteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_NOTE_PIM_ACQNOT_FK");
            });

            modelBuilder.Entity<PimsAcquisitionFileNoteHist>(entity =>
            {
                entity.HasKey(e => e.AcquisitionFileNoteHistId)
                    .HasName("PIMS_ACQNOT_H_PK");

                entity.Property(e => e.AcquisitionFileNoteHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_NOTE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsAcquisitionFilePerson>(entity =>
            {
                entity.HasKey(e => e.AcquisitionFilePersonId)
                    .HasName("ACQPER_PK");

                entity.HasComment("Table to associate an acquisition file to a person.");

                entity.Property(e => e.AcquisitionFilePersonId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_PERSON_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the relationship is active.");

                entity.HasOne(d => d.AcqFlPersonProfileTypeCodeNavigation)
                    .WithMany(p => p.PimsAcquisitionFilePeople)
                    .HasForeignKey(d => d.AcqFlPersonProfileTypeCode)
                    .HasConstraintName("PIM_AQFPPT_PIM_ACQPER_FK");

                entity.HasOne(d => d.AcquisitionFile)
                    .WithMany(p => p.PimsAcquisitionFilePeople)
                    .HasForeignKey(d => d.AcquisitionFileId)
                    .HasConstraintName("PIM_ACQNFL_PIM_ACQPER_FK");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PimsAcquisitionFilePeople)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PERSON_PIM_ACQPER_FK");
            });

            modelBuilder.Entity<PimsAcquisitionFilePersonHist>(entity =>
            {
                entity.HasKey(e => e.AcquisitionFilePersonHistId)
                    .HasName("PIMS_ACQPER_H_PK");

                entity.Property(e => e.AcquisitionFilePersonHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_FILE_PERSON_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsAcquisitionFileStatusType>(entity =>
            {
                entity.HasKey(e => e.AcquisitionFileStatusTypeCode)
                    .HasName("ACQFST_PK");

                entity.HasComment("Codified values for the acquistion file status.");

                entity.Property(e => e.AcquisitionFileStatusTypeCode).HasComment("Code value for the acquistion file status.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the acquistion file status.");

                entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");
            });

            modelBuilder.Entity<PimsAcquisitionFundingType>(entity =>
            {
                entity.HasKey(e => e.AcquisitionFundingTypeCode)
                    .HasName("ACQFTY_PK");

                entity.HasComment("Codified values for the acquistion funding type.");

                entity.Property(e => e.AcquisitionFundingTypeCode).HasComment("Code value for the acquistion funding type.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the acquistion funding type.");

                entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");
            });

            modelBuilder.Entity<PimsAcquisitionOwner>(entity =>
            {
                entity.HasKey(e => e.AcquisitionOwnerId)
                    .HasName("ACQOWN_PK");

                entity.HasComment("Entity containing information regarding an acquisition file.");

                entity.Property(e => e.AcquisitionOwnerId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_OWNER_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GivenName).HasComment("Given name of the owner (person).");

                entity.Property(e => e.IncorporationNumber).HasComment("Incorporation number of the organization.");

                entity.Property(e => e.LastNameOrCorpName1)
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("Name of the owner (person or organization).  If person, surname.");

                entity.Property(e => e.LastNameOrCorpName2).HasComment("Optional.");

                entity.HasOne(d => d.AcquisitionFile)
                    .WithMany(p => p.PimsAcquisitionOwners)
                    .HasForeignKey(d => d.AcquisitionFileId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("PIM_ACQNFL_PIM_ACQOWN_FK");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.PimsAcquisitionOwners)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("PIM_ADDRSS_PIM_ACQOWN_FK");
            });

            modelBuilder.Entity<PimsAcquisitionOwnerHist>(entity =>
            {
                entity.HasKey(e => e.AcquisitionOwnerHistId)
                    .HasName("PIMS_ACQOWN_H_PK");

                entity.Property(e => e.AcquisitionOwnerHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACQUISITION_OWNER_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsAcquisitionType>(entity =>
            {
                entity.HasKey(e => e.AcquisitionTypeCode)
                    .HasName("ACQTYP_PK");

                entity.HasComment("Codified values for the acquistion type.");

                entity.Property(e => e.AcquisitionTypeCode).HasComment("Code value for the acquistion type.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the acquistion type.");

                entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");
            });

            modelBuilder.Entity<PimsActInstPropAcqFile>(entity =>
            {
                entity.HasKey(e => e.ActInstPropAcqFileId)
                    .HasName("AIPAFL_PK");

                entity.HasComment("File to associate an activity instance with a subset of properties associated with the superset of acquisition file properties.");

                entity.Property(e => e.ActInstPropAcqFileId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACT_INST_PROP_ACQ_FILE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the relationship has been disabled.");

                entity.HasOne(d => d.ActivityInstance)
                    .WithMany(p => p.PimsActInstPropAcqFiles)
                    .HasForeignKey(d => d.ActivityInstanceId)
                    .HasConstraintName("PIM_ACTINS_PIM_AIPAFL_FK");

                entity.HasOne(d => d.PropertyAcquisitionFile)
                    .WithMany(p => p.PimsActInstPropAcqFiles)
                    .HasForeignKey(d => d.PropertyAcquisitionFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRACQF_PIM_AIPAFL_FK");
            });

            modelBuilder.Entity<PimsActInstPropAcqFileHist>(entity =>
            {
                entity.HasKey(e => e.ActInstPropAcqFileHistId)
                    .HasName("PIMS_AIPAFL_H_PK");

                entity.Property(e => e.ActInstPropAcqFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACT_INST_PROP_ACQ_FILE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsActInstPropRsrchFile>(entity =>
            {
                entity.HasKey(e => e.ActInstPropRsrchFileId)
                    .HasName("AIPRFL_PK");

                entity.HasComment("File to associate an activity instance with a subset of properties associated with the superset of research file properties.");

                entity.Property(e => e.ActInstPropRsrchFileId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACT_INST_PROP_RSRCH_FILE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the relationship has been disabled.");

                entity.HasOne(d => d.ActivityInstance)
                    .WithMany(p => p.PimsActInstPropRsrchFiles)
                    .HasForeignKey(d => d.ActivityInstanceId)
                    .HasConstraintName("PIM_ACTINS_PIM_AIPRFL_FK");

                entity.HasOne(d => d.PropertyResearchFile)
                    .WithMany(p => p.PimsActInstPropRsrchFiles)
                    .HasForeignKey(d => d.PropertyResearchFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRSCRC_PIM_AIPRFL_FK");
            });

            modelBuilder.Entity<PimsActInstPropRsrchFileHist>(entity =>
            {
                entity.HasKey(e => e.ActInstPropRsrchFileHistId)
                    .HasName("PIMS_AIPRFL_H_PK");

                entity.Property(e => e.ActInstPropRsrchFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACT_INST_PROP_RSRCH_FILE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsActivityInstance>(entity =>
            {
                entity.HasKey(e => e.ActivityInstanceId)
                    .HasName("ACTINS_PK");

                entity.HasComment("Table to contain all applicable activity instances for the PIMS PSP system.");

                entity.Property(e => e.ActivityInstanceId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_INSTANCE_ID_SEQ])");

                entity.Property(e => e.ActivityDataJson)
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("JSON structure containing data specific to an activity instance.");

                entity.Property(e => e.ActivityInstanceStatusTypeCode).HasDefaultValueSql("('NOSTART')");

                entity.Property(e => e.ActivityTemplateId).HasDefaultValueSql("((-1))");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the activity instance.");

                entity.HasOne(d => d.ActivityInstanceStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsActivityInstances)
                    .HasForeignKey(d => d.ActivityInstanceStatusTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ACTIST_PIM_ACTINS_FK");

                entity.HasOne(d => d.ActivityTemplate)
                    .WithMany(p => p.PimsActivityInstances)
                    .HasForeignKey(d => d.ActivityTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ACTTMP_PIM_ACTINS_FK");
            });

            modelBuilder.Entity<PimsActivityInstanceDocument>(entity =>
            {
                entity.HasKey(e => e.ActivityInstanceDocumentId)
                    .HasName("ACTDOC_PK");

                entity.HasComment("Relates a document object to an activity instance.");

                entity.Property(e => e.ActivityInstanceDocumentId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_INSTANCE_DOCUMENT_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the relationship has been disabled.");

                entity.HasOne(d => d.ActivityInstance)
                    .WithMany(p => p.PimsActivityInstanceDocuments)
                    .HasForeignKey(d => d.ActivityInstanceId)
                    .HasConstraintName("PIM_ACTINS_PIM_ACTDOC_FK");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.PimsActivityInstanceDocuments)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_DOCMNT_PIM_ACTDOC_FK");
            });

            modelBuilder.Entity<PimsActivityInstanceDocumentHist>(entity =>
            {
                entity.HasKey(e => e.ActivityInstanceDocumentHistId)
                    .HasName("PIMS_ACTDOC_H_PK");

                entity.Property(e => e.ActivityInstanceDocumentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_INSTANCE_DOCUMENT_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsActivityInstanceHist>(entity =>
            {
                entity.HasKey(e => e.ActivityInstanceHistId)
                    .HasName("PIMS_ACTINS_H_PK");

                entity.Property(e => e.ActivityInstanceHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_INSTANCE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsActivityInstanceNote>(entity =>
            {
                entity.HasComment("Relates a note object to an activity instance.");

                entity.Property(e => e.PimsActivityInstanceNoteId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_INSTANCE_NOTE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the relationship has been disabled.");

                entity.HasOne(d => d.ActivityInstance)
                    .WithMany(p => p.PimsActivityInstanceNotes)
                    .HasForeignKey(d => d.ActivityInstanceId)
                    .HasConstraintName("PIM_ACTINS_PIM_ACTINN_FK");

                entity.HasOne(d => d.Note)
                    .WithMany(p => p.PimsActivityInstanceNotes)
                    .HasForeignKey(d => d.NoteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_NOTE_PIM_ACTINN_FK");
            });

            modelBuilder.Entity<PimsActivityInstanceNoteHist>(entity =>
            {
                entity.HasKey(e => e.ActivityInstanceNoteHistId)
                    .HasName("PIMS_ACTINN_H_PK");

                entity.Property(e => e.ActivityInstanceNoteHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_INSTANCE_NOTE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsActivityInstanceStatusType>(entity =>
            {
                entity.HasKey(e => e.ActivityInstanceStatusTypeCode)
                    .HasName("ACTIST_PK");

                entity.HasComment("Codified values for the activity instance status type.");

                entity.Property(e => e.ActivityInstanceStatusTypeCode).HasComment("Code value for the activity instance status type.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the activity instance status type.");

                entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");
            });

            modelBuilder.Entity<PimsActivityTemplate>(entity =>
            {
                entity.HasKey(e => e.ActivityTemplateId)
                    .HasName("ACTTMP_PK");

                entity.Property(e => e.ActivityTemplateId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_TEMPLATE_ID_SEQ])");

                entity.Property(e => e.ActivityTemplateJson)
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("JSON structure desribing how to construct the activity UI.");

                entity.Property(e => e.ActivityTemplateTypeCode).HasDefaultValueSql("('GENERAL')");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");

                entity.HasOne(d => d.ActivityTemplateTypeCodeNavigation)
                    .WithMany(p => p.PimsActivityTemplates)
                    .HasForeignKey(d => d.ActivityTemplateTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ACTTTY_PIM_ACTTMP_FK");
            });

            modelBuilder.Entity<PimsActivityTemplateDocument>(entity =>
            {
                entity.HasKey(e => e.ActivityTemplateDocumentId)
                    .HasName("ACTMDO_PK");

                entity.HasComment("Associative relationship between an activity template and a document.  Useful in determining with templates reference a document and conversely, which documents reference an activity template.");

                entity.Property(e => e.ActivityTemplateDocumentId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_TEMPLATE_DOCUMENT_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the relationship has been disabled.");

                entity.HasOne(d => d.ActivityTemplate)
                    .WithMany(p => p.PimsActivityTemplateDocuments)
                    .HasForeignKey(d => d.ActivityTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ACTTMP_PIM_ACTMDO_FK");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.PimsActivityTemplateDocuments)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_DOCMNT_PIM_ACTMDO_FK");
            });

            modelBuilder.Entity<PimsActivityTemplateDocumentHist>(entity =>
            {
                entity.HasKey(e => e.ActivityTemplateDocumentHistId)
                    .HasName("PIMS_ACTMDO_H_PK");

                entity.Property(e => e.ActivityTemplateDocumentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_TEMPLATE_DOCUMENT_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsActivityTemplateHist>(entity =>
            {
                entity.HasKey(e => e.ActivityTemplateHistId)
                    .HasName("PIMS_ACTTMP_H_PK");

                entity.Property(e => e.ActivityTemplateHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_TEMPLATE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsActivityTemplateType>(entity =>
            {
                entity.HasKey(e => e.ActivityTemplateTypeCode)
                    .HasName("ACTTTY_PK");

                entity.HasComment("Codified values for the activity type.");

                entity.Property(e => e.ActivityTemplateTypeCode).HasComment("Code value for the activity type.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the activity type.");

                entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");
            });

            modelBuilder.Entity<PimsAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId)
                    .HasName("ADDRSS_PK");

                entity.Property(e => e.AddressId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ADDRESS_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.OtherCountry).HasComment("Other country not listed in drop-down list");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.PimsAddresses)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("PIM_CNTRY_PIM_ADDRSS_FK");

                entity.HasOne(d => d.DistrictCodeNavigation)
                    .WithMany(p => p.PimsAddresses)
                    .HasForeignKey(d => d.DistrictCode)
                    .HasConstraintName("PIM_DSTRCT_PIM_ADDRSS_FK");

                entity.HasOne(d => d.ProvinceState)
                    .WithMany(p => p.PimsAddresses)
                    .HasForeignKey(d => d.ProvinceStateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PROVNC_PIM_ADDRSS_FK");

                entity.HasOne(d => d.RegionCodeNavigation)
                    .WithMany(p => p.PimsAddresses)
                    .HasForeignKey(d => d.RegionCode)
                    .HasConstraintName("PIM_REGION_PIM_ADDRSS_FK");
            });

            modelBuilder.Entity<PimsAddressHist>(entity =>
            {
                entity.HasKey(e => e.AddressHistId)
                    .HasName("PIMS_ADDRSS_H_PK");

                entity.Property(e => e.AddressHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ADDRESS_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsAddressUsageType>(entity =>
            {
                entity.HasKey(e => e.AddressUsageTypeCode)
                    .HasName("ADUSGT_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsAreaUnitType>(entity =>
            {
                entity.HasKey(e => e.AreaUnitTypeCode)
                    .HasName("ARUNIT_PK");

                entity.HasComment("The area unit used for measuring Properties.  The units must be in metric: square metres or hectares.");

                entity.Property(e => e.AreaUnitTypeCode).HasComment("The area unit used for measuring Properties.  The units must be in metric: square metres or hectares.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Translation of the code value into a description that can be displayed to the user.");

                entity.Property(e => e.DisplayOrder).HasComment("Order in which to display the code values, if required.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is still active or is now disabled.");
            });

            modelBuilder.Entity<PimsBusinessFunctionCode>(entity =>
            {
                entity.HasComment("Code and description of the business function codes.");

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
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("Name of the code.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

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
                entity.HasKey(e => e.BusinessFunctionCodeHistId)
                    .HasName("PIMS_BIZFCN_H_PK");

                entity.Property(e => e.BusinessFunctionCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_BUSINESS_FUNCTION_CODE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsChartOfAccountsCode>(entity =>
            {
                entity.HasComment("Code and description of the chart of accounts codes.");

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
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("Name of the code.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

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
                entity.HasKey(e => e.ChartOfAccountsCodeHistId)
                    .HasName("PIMS_CHRTAC_H_PK");

                entity.Property(e => e.ChartOfAccountsCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CHART_OF_ACCOUNTS_CODE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsClaim>(entity =>
            {
                entity.HasKey(e => e.ClaimId)
                    .HasName("CLMTYP_PK");

                entity.Property(e => e.ClaimId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CLAIM_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsClaimHist>(entity =>
            {
                entity.HasKey(e => e.ClaimHistId)
                    .HasName("PIMS_CLMTYP_H_PK");

                entity.Property(e => e.ClaimHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CLAIM_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsContactMethod>(entity =>
            {
                entity.HasKey(e => e.ContactMethodId)
                    .HasName("CNTMTH_PK");

                entity.Property(e => e.ContactMethodId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CONTACT_METHOD_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsPreferredMethod).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.ContactMethodTypeCodeNavigation)
                    .WithMany(p => p.PimsContactMethods)
                    .HasForeignKey(d => d.ContactMethodTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_CNTMTT_PIM_CNTMTH_FK");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.PimsContactMethods)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("PIM_ORG_PIM_CNTMTH_FK");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PimsContactMethods)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("PIM_PERSON_PIM_CNTMTH_FK");
            });

            modelBuilder.Entity<PimsContactMethodHist>(entity =>
            {
                entity.HasKey(e => e.ContactMethodHistId)
                    .HasName("PIMS_CNTMTH_H_PK");

                entity.Property(e => e.ContactMethodHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CONTACT_METHOD_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsContactMethodType>(entity =>
            {
                entity.HasKey(e => e.ContactMethodTypeCode)
                    .HasName("CNTMTT_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsContactMgrVw>(entity =>
            {
                entity.ToView("PIMS_CONTACT_MGR_VW");

                entity.Property(e => e.Id).IsUnicode(false);
            });

            modelBuilder.Entity<PimsCostTypeCode>(entity =>
            {
                entity.HasComment("Code and description of the cost type codes.");

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
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("Name of the code.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

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
                entity.HasKey(e => e.CostTypeCodeHistId)
                    .HasName("PIMS_COSTYP_H_PK");

                entity.Property(e => e.CostTypeCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_COST_TYPE_CODE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsCountry>(entity =>
            {
                entity.HasKey(e => e.CountryId)
                    .HasName("CNTRY_PK");

                entity.Property(e => e.CountryId).ValueGeneratedNever();

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            });

            modelBuilder.Entity<PimsDataSourceType>(entity =>
            {
                entity.HasKey(e => e.DataSourceTypeCode)
                    .HasName("PIDSRT_PK");

                entity.HasComment("Describes the source system of the data (PAIMS, LIS, etc.)");

                entity.Property(e => e.DataSourceTypeCode).HasComment("Code value of the source system of the data (PAIMS, LIS, etc.)");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the source system of the data (PAIMS, LIS, etc.)");

                entity.Property(e => e.DisplayOrder).HasComment("Defines the default display order of the descriptions");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is still in use");
            });

            modelBuilder.Entity<PimsDistrict>(entity =>
            {
                entity.HasKey(e => e.DistrictCode)
                    .HasName("DSTRCT_PK");

                entity.Property(e => e.DistrictCode).ValueGeneratedNever();

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.RegionCodeNavigation)
                    .WithMany(p => p.PimsDistricts)
                    .HasForeignKey(d => d.RegionCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_REGION_PIM_DSTRCT_FK");
            });

            modelBuilder.Entity<PimsDocument>(entity =>
            {
                entity.HasKey(e => e.DocumentId)
                    .HasName("DOCMNT_PK");

                entity.HasComment("Table describing the available document types.");

                entity.Property(e => e.DocumentId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DOCUMENT_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.FileName)
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("Name of the file stored on Mayan EDMS.");

                entity.Property(e => e.MayanId)
                    .HasDefaultValueSql("((-1))")
                    .HasComment("Mayan-generated document number used for retrieval from Mayan EDMS.");

                entity.HasOne(d => d.DocumentStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsDocuments)
                    .HasForeignKey(d => d.DocumentStatusTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_DOCSTY_PIM_DOCMNT_FK");

                entity.HasOne(d => d.DocumentType)
                    .WithMany(p => p.PimsDocuments)
                    .HasForeignKey(d => d.DocumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_DOCTYP_PIM_DOCMNT_FK");
            });

            modelBuilder.Entity<PimsDocumentHist>(entity =>
            {
                entity.HasKey(e => e.DocumentHistId)
                    .HasName("PIMS_DOCMNT_H_PK");

                entity.Property(e => e.DocumentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DOCUMENT_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsDocumentStatusType>(entity =>
            {
                entity.HasKey(e => e.DocumentStatusTypeCode)
                    .HasName("DOCSTY_PK");

                entity.HasComment("Table describing the available document types.");

                entity.Property(e => e.DocumentStatusTypeCode)
                    .HasDefaultValueSql("('NEXT VALUE FOR [PIMS_DOCUMENT_ID_SEQ]')")
                    .HasComment("Code value of the document status type.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Descriptive translation of the document status type code.");

                entity.Property(e => e.DisplayOrder).HasComment("Determines the default display order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is still in use.");
            });

            modelBuilder.Entity<PimsDocumentTyp>(entity =>
            {
                entity.HasKey(e => e.DocumentTypeId)
                    .HasName("DOCTYP_PK");

                entity.HasComment("Table describing the available document types.");

                entity.Property(e => e.DocumentTypeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DOCUMENT_TYPE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DocumentType)
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("Description of the available document types.");

                entity.Property(e => e.MayanId)
                    .HasDefaultValueSql("((-1))")
                    .HasComment("Mayan-generated document type number used for retrieval from Mayan EDMS.");
            });

            modelBuilder.Entity<PimsDocumentTypHist>(entity =>
            {
                entity.HasKey(e => e.DocumentTypHistId)
                    .HasName("PIMS_DOCTYP_H_PK");

                entity.Property(e => e.DocumentTypHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_DOCUMENT_TYP_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsFenceType>(entity =>
            {
                entity.HasKey(e => e.FenceTypeCode)
                    .HasName("FNCTYP_PK");

                entity.HasComment("Codified values for the fence type.  This is an unassociated table that is used in the UI to populate JSON attributes.");

                entity.Property(e => e.FenceTypeCode).HasComment("Code value for the fence type.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the fence type.");

                entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");
            });

            modelBuilder.Entity<PimsFinancialActivityCode>(entity =>
            {
                entity.HasComment("Code and description of the financial activity codes.");

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
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("Value of the code.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

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
                entity.HasKey(e => e.FinancialActivityCodeHistId)
                    .HasName("PIMS_FINACT_H_PK");

                entity.Property(e => e.FinancialActivityCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_FINANCIAL_ACTIVITY_CODE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsInsurance>(entity =>
            {
                entity.HasKey(e => e.InsuranceId)
                    .HasName("INSRNC_PK");

                entity.Property(e => e.InsuranceId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_INSURANCE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.CoverageDescription).HasComment("Description of the insurance coverage");

                entity.Property(e => e.CoverageLimit)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Monetary limit of the insurance coverage");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ExpiryDate).HasComment("Date the insurance expires");

                entity.Property(e => e.IsInsuranceInPlace)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicator that digital license exists");

                entity.Property(e => e.OtherInsuranceType).HasComment("Description of the non-standard insurance coverage type");

                entity.HasOne(d => d.InsuranceTypeCodeNavigation)
                    .WithMany(p => p.PimsInsurances)
                    .HasForeignKey(d => d.InsuranceTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_INSPYT_PIM_INSRNC_FK");

                entity.HasOne(d => d.Lease)
                    .WithMany(p => p.PimsInsurances)
                    .HasForeignKey(d => d.LeaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LEASE_PIM_INSRNC_FK");
            });

            modelBuilder.Entity<PimsInsuranceHist>(entity =>
            {
                entity.HasKey(e => e.InsuranceHistId)
                    .HasName("PIMS_INSRNC_H_PK");

                entity.Property(e => e.InsuranceHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_INSURANCE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsInsuranceType>(entity =>
            {
                entity.HasKey(e => e.InsuranceTypeCode)
                    .HasName("INSPYT_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLandSurveyorType>(entity =>
            {
                entity.HasKey(e => e.LandSurveyorTypeCode)
                    .HasName("LNSRVT_PK");

                entity.HasComment("Codified values for the land surveyor type.  This is an unassociated table that is used in the UI to populate JSON attributes.");

                entity.Property(e => e.LandSurveyorTypeCode).HasComment("Code value for the land surveyor type.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the land surveyor type.");

                entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");
            });

            modelBuilder.Entity<PimsLease>(entity =>
            {
                entity.HasKey(e => e.LeaseId)
                    .HasName("LEASE_PK");

                entity.HasComment("Details of a lease that is inventoried in PIMS system.");

                entity.Property(e => e.LeaseId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DocumentationReference).HasComment("Location of documents pertianing to the lease/license");

                entity.Property(e => e.HasDigitalFile).HasComment("Indicator that digital file exists");

                entity.Property(e => e.HasDigitalLicense).HasComment("Indicator that digital license exists");

                entity.Property(e => e.HasPhysicalFile).HasComment("Indicator that phyical file exists");

                entity.Property(e => e.HasPhysicialLicense).HasComment("Indicator that physical license exists");

                entity.Property(e => e.InspectionDate).HasComment("Inspection date");

                entity.Property(e => e.InspectionNotes).HasComment("Notes accompanying inspection");

                entity.Property(e => e.IsCommBldg)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is a commercial building");

                entity.Property(e => e.IsExpired)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Incidcator that lease/license has expired");

                entity.Property(e => e.IsOtherImprovement)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is improvement of another description");

                entity.Property(e => e.IsSubjectToRta)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is subject the Residential Tenancy Act");

                entity.Property(e => e.LFileNo).HasComment("Generated identifying lease/licence number");

                entity.Property(e => e.LeaseAmount).HasComment("Lease/licence amount");

                entity.Property(e => e.LeaseCategoryOtherDesc).HasComment("User-specified lease category description not included in standard set of lease purposes");

                entity.Property(e => e.LeaseDescription).HasComment("Manually etered lease description, not the legal description");

                entity.Property(e => e.LeaseNotes).HasComment("Notes accompanying lease");

                entity.Property(e => e.LeasePurposeOtherDesc).HasComment("User-specified lease purpose description not included in standard set of lease purposes");

                entity.Property(e => e.MotiContact).HasComment("Contact of the MoTI person associated with the lease");

                entity.Property(e => e.OrigExpiryDate).HasComment("Original expiry date of the lease/license");

                entity.Property(e => e.OrigStartDate).HasComment("Original start date of the lease/license");

                entity.Property(e => e.OtherLeaseLicenseType).HasComment("Description of a non-standard lease/license type");

                entity.Property(e => e.OtherLeaseProgramType).HasComment("Description of a non-standard lease program type");

                entity.Property(e => e.OtherLeasePurposeType).HasComment("Description of a non-standard lease purpose type");

                entity.Property(e => e.PsFileNo).HasComment("Sourced from t_fileSubOverrideData.PSFile_No");

                entity.Property(e => e.RegionCode).HasComment("MoTI region associated with the lease");

                entity.Property(e => e.ResponsibilityEffectiveDate).HasComment("Date current responsibility came into effect for this lease");

                entity.Property(e => e.ReturnNotes).HasComment("Notes accompanying lease");

                entity.Property(e => e.TfaFileNo).HasComment("Sourced from t_fileMain.TFA_File_Number");

                entity.Property(e => e.TfaFileNumber).HasComment("Sourced from t_fileMain.TFA_File_Number || - || t_fileSub.Subfile_Sequence_Code");

                entity.HasOne(d => d.LeaseCategoryTypeCodeNavigation)
                    .WithMany(p => p.PimsLeases)
                    .HasForeignKey(d => d.LeaseCategoryTypeCode)
                    .HasConstraintName("PIM_LSCATT_PIM_LEASE_FK");

                entity.HasOne(d => d.LeaseInitiatorTypeCodeNavigation)
                    .WithMany(p => p.PimsLeases)
                    .HasForeignKey(d => d.LeaseInitiatorTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LINITT_PIM_LEASE_FK");

                entity.HasOne(d => d.LeaseLicenseTypeCodeNavigation)
                    .WithMany(p => p.PimsLeases)
                    .HasForeignKey(d => d.LeaseLicenseTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LELIST_PIM_LEASE_FK");

                entity.HasOne(d => d.LeasePayRvblTypeCodeNavigation)
                    .WithMany(p => p.PimsLeases)
                    .HasForeignKey(d => d.LeasePayRvblTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LSPRTY_PIM_LEASE_FK");

                entity.HasOne(d => d.LeaseProgramTypeCodeNavigation)
                    .WithMany(p => p.PimsLeases)
                    .HasForeignKey(d => d.LeaseProgramTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LSPRGT_PIM_LEASE_FK");

                entity.HasOne(d => d.LeasePurposeTypeCodeNavigation)
                    .WithMany(p => p.PimsLeases)
                    .HasForeignKey(d => d.LeasePurposeTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LPRPTY_PIM_LEASE_FK");

                entity.HasOne(d => d.LeaseResponsibilityTypeCodeNavigation)
                    .WithMany(p => p.PimsLeases)
                    .HasForeignKey(d => d.LeaseResponsibilityTypeCode)
                    .HasConstraintName("PIM_LRESPT_PIM_LEASE_FK");

                entity.HasOne(d => d.LeaseStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsLeases)
                    .HasForeignKey(d => d.LeaseStatusTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LSSTYP_PIM_LEASE_FK");

                entity.HasOne(d => d.RegionCodeNavigation)
                    .WithMany(p => p.PimsLeases)
                    .HasForeignKey(d => d.RegionCode)
                    .HasConstraintName("PIM_REGION_PIM_LEASE_FK");
            });

            modelBuilder.Entity<PimsLeaseActivityInstance>(entity =>
            {
                entity.HasKey(e => e.LeaseActivityInstanceId)
                    .HasName("LSACIN_PK");

                entity.HasComment("Associative entity between leases/licenses and activity instances.");

                entity.Property(e => e.LeaseActivityInstanceId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_ACTIVITY_INSTANCE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.ActivityInstance)
                    .WithMany(p => p.PimsLeaseActivityInstances)
                    .HasForeignKey(d => d.ActivityInstanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ACTINS_PIM_LSACIN_FK");

                entity.HasOne(d => d.Lease)
                    .WithMany(p => p.PimsLeaseActivityInstances)
                    .HasForeignKey(d => d.LeaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LEASE_PIM_LSACIN_FK");
            });

            modelBuilder.Entity<PimsLeaseActivityInstanceHist>(entity =>
            {
                entity.HasKey(e => e.LeaseActivityInstanceHistId)
                    .HasName("PIMS_LSACIN_H_PK");

                entity.Property(e => e.LeaseActivityInstanceHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_ACTIVITY_INSTANCE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsLeaseCategoryType>(entity =>
            {
                entity.HasKey(e => e.LeaseCategoryTypeCode)
                    .HasName("LSCATT_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseHist>(entity =>
            {
                entity.HasKey(e => e.LeaseHistId)
                    .HasName("PIMS_LEASE_H_PK");

                entity.Property(e => e.LeaseHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsLeaseInitiatorType>(entity =>
            {
                entity.HasKey(e => e.LeaseInitiatorTypeCode)
                    .HasName("LINITT_PK");

                entity.HasComment("Describes the initiator of the lease");

                entity.Property(e => e.LeaseInitiatorTypeCode).HasComment("Code value of the initiator of the lease");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the initiator of the lease");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseLicenseType>(entity =>
            {
                entity.HasKey(e => e.LeaseLicenseTypeCode)
                    .HasName("LELIST_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeasePayRvblType>(entity =>
            {
                entity.HasKey(e => e.LeasePayRvblTypeCode)
                    .HasName("LSPRTY_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeasePayment>(entity =>
            {
                entity.HasKey(e => e.LeasePaymentId)
                    .HasName("LSPYMT_PK");

                entity.HasComment("Describes a payment associated with a lease term.");

                entity.Property(e => e.LeasePaymentId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_PAYMENT_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Note).HasComment("Notes regarding this payment");

                entity.Property(e => e.PaymentAmountGst).HasComment("GST owing on payment if applicable");

                entity.Property(e => e.PaymentAmountPreTax).HasComment("Principal amount of the payment before applicable taxes");

                entity.Property(e => e.PaymentAmountPst).HasComment("PST owing on payment if applicable");

                entity.Property(e => e.PaymentAmountTotal).HasComment("Total amount of payment including principal plus all applicable taxes");

                entity.Property(e => e.PaymentReceivedDate).HasComment("Date the payment was received or sent");

                entity.HasOne(d => d.LeasePaymentMethodTypeCodeNavigation)
                    .WithMany(p => p.PimsLeasePayments)
                    .HasForeignKey(d => d.LeasePaymentMethodTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LSPMMT_PIM_LSPYMT_FK");

                entity.HasOne(d => d.LeasePaymentStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsLeasePayments)
                    .HasForeignKey(d => d.LeasePaymentStatusTypeCode)
                    .HasConstraintName("PIM_LPSTST_PIM_LSPYMT_FK");

                entity.HasOne(d => d.LeaseTerm)
                    .WithMany(p => p.PimsLeasePayments)
                    .HasForeignKey(d => d.LeaseTermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LSTERM_PIM_LSPYMT_FK");
            });

            modelBuilder.Entity<PimsLeasePaymentHist>(entity =>
            {
                entity.HasKey(e => e.LeasePaymentHistId)
                    .HasName("PIMS_LSPYMT_H_PK");

                entity.Property(e => e.LeasePaymentHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_PAYMENT_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsLeasePaymentMethodType>(entity =>
            {
                entity.HasKey(e => e.LeasePaymentMethodTypeCode)
                    .HasName("LSPMMT_PK");

                entity.HasComment("Describes the type of payment method for a lease.");

                entity.Property(e => e.LeasePaymentMethodTypeCode).HasComment("Payment method type code");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Payment method type description");

                entity.Property(e => e.DisplayOrder).HasComment("Display order of the descriptions");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is this code disabled?");
            });

            modelBuilder.Entity<PimsLeasePaymentStatusType>(entity =>
            {
                entity.HasKey(e => e.LeasePaymentStatusTypeCode)
                    .HasName("LPSTST_PK");

                entity.HasComment("Describes the status of forecast payments");

                entity.Property(e => e.LeasePaymentStatusTypeCode).HasComment("Payment status type code");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Payment status type description");

                entity.Property(e => e.DisplayOrder).HasComment("Display order of the descriptions");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is this code disabled?");
            });

            modelBuilder.Entity<PimsLeasePmtFreqType>(entity =>
            {
                entity.HasKey(e => e.LeasePmtFreqTypeCode)
                    .HasName("LSPMTF_PK");

                entity.HasComment("Describes the frequency of payments for a lease.");

                entity.Property(e => e.LeasePmtFreqTypeCode).HasComment("Payment frequency type code");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Payment frequency type code description");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseProgramType>(entity =>
            {
                entity.HasKey(e => e.LeaseProgramTypeCode)
                    .HasName("LSPRGT_PK");

                entity.HasComment("Describes the program type associated with a lease.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeasePurposeType>(entity =>
            {
                entity.HasKey(e => e.LeasePurposeTypeCode)
                    .HasName("LPRPTY_PK");

                entity.HasComment("Describes the purpose type associated with a lease.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseResponsibilityType>(entity =>
            {
                entity.HasKey(e => e.LeaseResponsibilityTypeCode)
                    .HasName("LRESPT_PK");

                entity.HasComment("Describes which organization is responsible for this lease");

                entity.Property(e => e.LeaseResponsibilityTypeCode).HasComment("Code value of the organization responsible for this lease");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the organization responsible for this lease");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseStatusType>(entity =>
            {
                entity.HasKey(e => e.LeaseStatusTypeCode)
                    .HasName("LSSTYP_PK");

                entity.HasComment("Describes the status of the lease");

                entity.Property(e => e.LeaseStatusTypeCode).HasComment("Code value of the status of the lease");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the status of the lease");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseTenant>(entity =>
            {
                entity.HasKey(e => e.LeaseTenantId)
                    .HasName("TENANT_PK");

                entity.HasComment("Associates a tenant with a lease");

                entity.Property(e => e.LeaseTenantId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_TENANT_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.LessorTypeCode).HasDefaultValueSql("('UNK')");

                entity.Property(e => e.Note).HasComment("Notes associated with the lease/tenant relationship.");

                entity.Property(e => e.TenantTypeCode).HasDefaultValueSql("('UNK')");

                entity.HasOne(d => d.Lease)
                    .WithMany(p => p.PimsLeaseTenants)
                    .HasForeignKey(d => d.LeaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LEASE_PIM_TENANT_FK");

                entity.HasOne(d => d.LessorTypeCodeNavigation)
                    .WithMany(p => p.PimsLeaseTenants)
                    .HasForeignKey(d => d.LessorTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LSSRTY_PIM_TENANT_FK");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.PimsLeaseTenants)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("PIM_ORG_PIM_TENANT_FK");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PimsLeaseTenantPeople)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("PIM_PERSON_PIM_TENANT_FK");

                entity.HasOne(d => d.PrimaryContact)
                    .WithMany(p => p.PimsLeaseTenantPrimaryContacts)
                    .HasForeignKey(d => d.PrimaryContactId)
                    .HasConstraintName("PIM_PERSON_PIM_PRIMARY_CONTACT_FK");

                entity.HasOne(d => d.TenantTypeCodeNavigation)
                    .WithMany(p => p.PimsLeaseTenants)
                    .HasForeignKey(d => d.TenantTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_TENTYP_PIM_TENANT_FK");
            });

            modelBuilder.Entity<PimsLeaseTenantHist>(entity =>
            {
                entity.HasKey(e => e.LeaseTenantHistId)
                    .HasName("PIMS_TENANT_H_PK");

                entity.Property(e => e.LeaseTenantHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_TENANT_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsLeaseTerm>(entity =>
            {
                entity.HasKey(e => e.LeaseTermId)
                    .HasName("LSTERM_PK");

                entity.HasComment("Describes a term for the associated lease.");

                entity.Property(e => e.LeaseTermId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_TERM_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.GstAmount).HasComment("Calculated/entered GST portion of the payment.  Can be overridden by the user.");

                entity.Property(e => e.IsGstEligible).HasComment("Is the lease subject to GST?");

                entity.Property(e => e.IsTermExercised).HasComment("Has the lease term been exercised?");

                entity.Property(e => e.LeasePmtFreqTypeCode).HasComment("Foreign key to payment frequency values");

                entity.Property(e => e.PaymentAmount).HasComment("Agreed-to payment amount (exclusive of GST)");

                entity.Property(e => e.PaymentDueDate).HasComment("Anecdotal description of payment due date (e.g. 1st of month, end of month)");

                entity.Property(e => e.PaymentNote).HasComment("Notes regarding payment status for the lease term");

                entity.Property(e => e.TermExpiryDate).HasComment("Expiry date of the current term of the lease/licence");

                entity.Property(e => e.TermRenewalDate).HasComment("Renewal date of the current term of the lease/licence");

                entity.Property(e => e.TermStartDate).HasComment("Start date of the current term of the lease/licence");

                entity.HasOne(d => d.Lease)
                    .WithMany(p => p.PimsLeaseTerms)
                    .HasForeignKey(d => d.LeaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LEASE_PIM_LSTERM_FK");

                entity.HasOne(d => d.LeasePmtFreqTypeCodeNavigation)
                    .WithMany(p => p.PimsLeaseTerms)
                    .HasForeignKey(d => d.LeasePmtFreqTypeCode)
                    .HasConstraintName("PIM_LSPMTF_PIM_LSTERM_FK");

                entity.HasOne(d => d.LeaseTermStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsLeaseTerms)
                    .HasForeignKey(d => d.LeaseTermStatusTypeCode)
                    .HasConstraintName("PIM_LTRMST_PIM_LSTERM_FK");
            });

            modelBuilder.Entity<PimsLeaseTermHist>(entity =>
            {
                entity.HasKey(e => e.LeaseTermHistId)
                    .HasName("PIMS_LSTERM_H_PK");

                entity.Property(e => e.LeaseTermHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_TERM_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsLeaseTermStatusType>(entity =>
            {
                entity.HasKey(e => e.LeaseTermStatusTypeCode)
                    .HasName("LTRMST_PK");

                entity.HasComment("Describes the status of the lease term");

                entity.Property(e => e.LeaseTermStatusTypeCode).HasComment("Code value of the status of the lease term");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the status of the lease term");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLessorType>(entity =>
            {
                entity.HasKey(e => e.LessorTypeCode)
                    .HasName("LSSRTY_PK");

                entity.HasComment("Code table describing the type of lessor on a lease.");

                entity.Property(e => e.LessorTypeCode)
                    .HasDefaultValueSql("('UNK')")
                    .HasComment("Code representing the types of lessors on a lease.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .HasDefaultValueSql("('Unknown')")
                    .HasComment("Description of the types of lessors on a lease.");

                entity.Property(e => e.DisplayOrder).HasComment("Specifies a specific order to visually present the code.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is currently active.");
            });

            modelBuilder.Entity<PimsLetterType>(entity =>
            {
                entity.HasKey(e => e.LetterTypeCode)
                    .HasName("LTRTYP_PK");

                entity.HasComment("Codified values for the letter type.  This is an unassociated table that is used in the UI to populate JSON attributes.");

                entity.Property(e => e.LetterTypeCode).HasComment("Code value for the letter type.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the letter type.");

                entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");
            });

            modelBuilder.Entity<PimsNote>(entity =>
            {
                entity.HasKey(e => e.NoteId)
                    .HasName("NOTE_PK");

                entity.HasComment("Table to contain all applicable notes for the PIMS PSP system.");

                entity.Property(e => e.NoteId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_NOTE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsSystemGenerated)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicatesd if this note is system-generated.");

                entity.Property(e => e.NoteTxt)
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("Contents of the note.");
            });

            modelBuilder.Entity<PimsNoteHist>(entity =>
            {
                entity.HasKey(e => e.NoteHistId)
                    .HasName("PIMS_NOTE_H_PK");

                entity.Property(e => e.NoteHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_NOTE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsOrgIdentifierType>(entity =>
            {
                entity.HasKey(e => e.OrgIdentifierTypeCode)
                    .HasName("ORGIDT_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsOrganization>(entity =>
            {
                entity.HasKey(e => e.OrganizationId)
                    .HasName("ORG_PK");

                entity.HasComment("Information related to an organization identified in the PSP system.");

                entity.Property(e => e.OrganizationId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ORGANIZATION_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IncorporationNumber).HasComment("Incorporation number of the orgnization");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.DistrictCodeNavigation)
                    .WithMany(p => p.PimsOrganizations)
                    .HasForeignKey(d => d.DistrictCode)
                    .HasConstraintName("PIM_DSTRCT_PIM_ORG_FK");

                entity.HasOne(d => d.OrgIdentifierTypeCodeNavigation)
                    .WithMany(p => p.PimsOrganizations)
                    .HasForeignKey(d => d.OrgIdentifierTypeCode)
                    .HasConstraintName("PIM_ORGIDT_PIM_ORG_FK");

                entity.HasOne(d => d.OrganizationTypeCodeNavigation)
                    .WithMany(p => p.PimsOrganizations)
                    .HasForeignKey(d => d.OrganizationTypeCode)
                    .HasConstraintName("PIM_ORGTYP_PIM_ORG_FK");

                entity.HasOne(d => d.PrntOrganization)
                    .WithMany(p => p.InversePrntOrganization)
                    .HasForeignKey(d => d.PrntOrganizationId)
                    .HasConstraintName("PIM_ORG_PIM_PRNT_ORG_FK");

                entity.HasOne(d => d.RegionCodeNavigation)
                    .WithMany(p => p.PimsOrganizations)
                    .HasForeignKey(d => d.RegionCode)
                    .HasConstraintName("PIM_REGION_PIM_ORG_FK");
            });

            modelBuilder.Entity<PimsOrganizationAddress>(entity =>
            {
                entity.HasKey(e => e.OrganizationAddressId)
                    .HasName("ORGADD_PK");

                entity.HasComment("An associative entity to define multiple addresses for a person.");

                entity.Property(e => e.OrganizationAddressId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ORGANIZATION_ADDRESS_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.PimsOrganizationAddresses)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ADDRSS_PIM_ORGADD_FK");

                entity.HasOne(d => d.AddressUsageTypeCodeNavigation)
                    .WithMany(p => p.PimsOrganizationAddresses)
                    .HasForeignKey(d => d.AddressUsageTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ADUSGT_PIM_ORGADD_FK");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.PimsOrganizationAddresses)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ORG_PIM_ORGADD_FK");
            });

            modelBuilder.Entity<PimsOrganizationAddressHist>(entity =>
            {
                entity.HasKey(e => e.OrganizationAddressHistId)
                    .HasName("PIMS_ORGADD_H_PK");

                entity.Property(e => e.OrganizationAddressHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ORGANIZATION_ADDRESS_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsOrganizationHist>(entity =>
            {
                entity.HasKey(e => e.OrganizationHistId)
                    .HasName("PIMS_ORG_H_PK");

                entity.Property(e => e.OrganizationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ORGANIZATION_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsOrganizationType>(entity =>
            {
                entity.HasKey(e => e.OrganizationTypeCode)
                    .HasName("ORGTYP_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsPerson>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PERSON_PK");

                entity.Property(e => e.PersonId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.UseOrganizationAddress).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsPersonAddress>(entity =>
            {
                entity.HasKey(e => e.PersonAddressId)
                    .HasName("PERADD_PK");

                entity.HasComment("An associative entity to define multiple addresses for a person.");

                entity.Property(e => e.PersonAddressId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ADDRESS_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.PimsPersonAddresses)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ADDRSS_PIM_PERADD_FK");

                entity.HasOne(d => d.AddressUsageTypeCodeNavigation)
                    .WithMany(p => p.PimsPersonAddresses)
                    .HasForeignKey(d => d.AddressUsageTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ADUSGT_PIM_PERADD_FK");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PimsPersonAddresses)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PERSON_PIM_PERADD_FK");
            });

            modelBuilder.Entity<PimsPersonAddressHist>(entity =>
            {
                entity.HasKey(e => e.PersonAddressHistId)
                    .HasName("PIMS_PERADD_H_PK");

                entity.Property(e => e.PersonAddressHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ADDRESS_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPersonContactVw>(entity =>
            {
                entity.ToView("PIMS_PERSON_CONTACT_VW");
            });

            modelBuilder.Entity<PimsPersonHist>(entity =>
            {
                entity.HasKey(e => e.PersonHistId)
                    .HasName("PIMS_PERSON_H_PK");

                entity.Property(e => e.PersonHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPersonOrganization>(entity =>
            {
                entity.HasKey(e => e.PersonOrganizationId)
                    .HasName("PERORG_PK");

                entity.Property(e => e.PersonOrganizationId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ORGANIZATION_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.PimsPersonOrganizations)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("PIM_ORG_PIM_PERORG_FK");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PimsPersonOrganizations)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("PIM_PERSON_PIM_PERORG_FK");
            });

            modelBuilder.Entity<PimsPersonOrganizationHist>(entity =>
            {
                entity.HasKey(e => e.PersonOrganizationHistId)
                    .HasName("PIMS_PERORG_H_PK");

                entity.Property(e => e.PersonOrganizationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ORGANIZATION_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPphStatusType>(entity =>
            {
                entity.HasKey(e => e.PphStatusTypeCode)
                    .HasName("PPHSTT_PK");

                entity.HasComment("Code table to describe the Provincial Public Highway status.");

                entity.Property(e => e.PphStatusTypeCode).HasComment("Code indicating the Provincial Public Highway status");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the code indicating the purpose of the property research");

                entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled.");
            });

            modelBuilder.Entity<PimsPrfPropResearchPurposeType>(entity =>
            {
                entity.HasKey(e => e.PrfPropResearchPurposeId)
                    .HasName("PRSPRP_PK");

                entity.Property(e => e.PrfPropResearchPurposeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PRF_PROP_RESEARCH_PURPOSE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.PropResearchPurposeTypeCodeNavigation)
                    .WithMany(p => p.PimsPrfPropResearchPurposeTypes)
                    .HasForeignKey(d => d.PropResearchPurposeTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_RRESPT_PIM_PRSPRP_FK");

                entity.HasOne(d => d.PropertyResearchFile)
                    .WithMany(p => p.PimsPrfPropResearchPurposeTypes)
                    .HasForeignKey(d => d.PropertyResearchFileId)
                    .HasConstraintName("PIM_PRSCRC_PIM_PRSPRP_FK");
            });

            modelBuilder.Entity<PimsProduct>(entity =>
            {
                entity.HasComment("Code and description of a project.");

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

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

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

                entity.HasOne(d => d.ParentProject)
                    .WithMany(p => p.PimsProducts)
                    .HasForeignKey(d => d.ParentProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PROJCT_PIM_PRODCT_FK");
            });

            modelBuilder.Entity<PimsProductHist>(entity =>
            {
                entity.HasKey(e => e.ProductHistId)
                    .HasName("PIMS_PRODCT_H_PK");

                entity.Property(e => e.ProductHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PRODUCT_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsProject>(entity =>
            {
                entity.HasComment("Code and description of a project.");

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

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Project description.");

                entity.Property(e => e.Note).HasComment("Descriptive note relevant to the project.");

                entity.Property(e => e.ProjectStatusTypeCode).HasDefaultValueSql("('ACTIVE')");

                entity.HasOne(d => d.BusinessFunctionCode)
                    .WithMany(p => p.PimsProjects)
                    .HasForeignKey(d => d.BusinessFunctionCodeId)
                    .HasConstraintName("PIM_BIZFCN_PIM_PROJCT_FK");

                entity.HasOne(d => d.CostTypeCode)
                    .WithMany(p => p.PimsProjects)
                    .HasForeignKey(d => d.CostTypeCodeId)
                    .HasConstraintName("PIM_COSTYP_PIM_PROJCT_FK");

                entity.HasOne(d => d.ProjectStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsProjects)
                    .HasForeignKey(d => d.ProjectStatusTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRJSTS_PIM_PROJCT_FK");

                entity.HasOne(d => d.RegionCodeNavigation)
                    .WithMany(p => p.PimsProjects)
                    .HasForeignKey(d => d.RegionCode)
                    .HasConstraintName("PIM_REGION_PIM_PROJCT_FK");

                entity.HasOne(d => d.WorkActivityCode)
                    .WithMany(p => p.PimsProjects)
                    .HasForeignKey(d => d.WorkActivityCodeId)
                    .HasConstraintName("PIM_WRKACT_PIM_PROJCT_FK");
            });

            modelBuilder.Entity<PimsProjectHist>(entity =>
            {
                entity.HasKey(e => e.ProjectHistId)
                    .HasName("PIMS_PROJCT_H_PK");

                entity.Property(e => e.ProjectHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsProjectStatusType>(entity =>
            {
                entity.HasKey(e => e.ProjectStatusTypeCode)
                    .HasName("PRJSTY_PK");

                entity.HasComment("Codified values for the project status.");

                entity.Property(e => e.ProjectStatusTypeCode).HasComment("Code value for the project status.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the project status.");

                entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");
            });

            modelBuilder.Entity<PimsPropPropAdjacentLandType>(entity =>
            {
                entity.HasKey(e => e.PropPropAdjacentLandTypeId)
                    .HasName("PRPALT_PK");

                entity.HasComment("Resolves many-to-many relationship between PIMS_PROPERTY and PIMS_PROPERTY_ADJACENT_LAND_TYPE");

                entity.Property(e => e.PropPropAdjacentLandTypeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_PROP_ADJACENT_LAND_TYPE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.PropertyAdjacentLandTypeCodeNavigation)
                    .WithMany(p => p.PimsPropPropAdjacentLandTypes)
                    .HasForeignKey(d => d.PropertyAdjacentLandTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRADJL_PIM_PRPALT_FK");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsPropPropAdjacentLandTypes)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPRTY_PIM_PRPALT_FK");
            });

            modelBuilder.Entity<PimsPropPropAnomalyType>(entity =>
            {
                entity.HasKey(e => e.PropPropAnomalyTypeId)
                    .HasName("PRPRAT_PK");

                entity.HasComment("Resolves many-to-many relationship between PIMS_PROPERTY and PIMS_PROPERTY_ANOMALY_TYPE");

                entity.Property(e => e.PropPropAnomalyTypeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_PROP_ANOMALY_TYPE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.PropertyAnomalyTypeCodeNavigation)
                    .WithMany(p => p.PimsPropPropAnomalyTypes)
                    .HasForeignKey(d => d.PropertyAnomalyTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRANOM_PIM_PRPRAT_FK");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsPropPropAnomalyTypes)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPRTY_PIM_PRPRAT_FK");
            });

            modelBuilder.Entity<PimsPropPropRoadType>(entity =>
            {
                entity.HasKey(e => e.PropPropRoadTypeId)
                    .HasName("PRPRRT_PK");

                entity.HasComment("Resolves many-to-many relationship between PIMS_PROPERTY and PIMS_PROPERTY_ROAD_TYPE");

                entity.Property(e => e.PropPropRoadTypeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_PROP_ROAD_TYPE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsPropPropRoadTypes)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPRTY_PIM_PRPRRT_FK");

                entity.HasOne(d => d.PropertyRoadTypeCodeNavigation)
                    .WithMany(p => p.PimsPropPropRoadTypes)
                    .HasForeignKey(d => d.PropertyRoadTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRROAD_PIM_PRPRRT_FK");
            });

            modelBuilder.Entity<PimsPropPropTenureType>(entity =>
            {
                entity.HasKey(e => e.PropPropTenureTypeId)
                    .HasName("PRPRTT_PK");

                entity.HasComment("Resolves many-to-many relationship between PIMS_PROPERTY and PIMS_PROPERTY_TENURE_TYPE");

                entity.Property(e => e.PropPropTenureTypeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROP_PROP_TENURE_TYPE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.PropertyTenureTypeCode).HasDefaultValueSql("('UNKNOWN')");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsPropPropTenureTypes)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPRTY_PIM_PRPRTT_FK");

                entity.HasOne(d => d.PropertyTenureTypeCodeNavigation)
                    .WithMany(p => p.PimsPropPropTenureTypes)
                    .HasForeignKey(d => d.PropertyTenureTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPTNR_PIM_PRPRTT_FK");
            });

            modelBuilder.Entity<PimsPropResearchPurposeType>(entity =>
            {
                entity.HasKey(e => e.PropResearchPurposeTypeCode)
                    .HasName("RRESPT_PK");

                entity.HasComment("Code table to describe the purpose ot the property research");

                entity.Property(e => e.PropResearchPurposeTypeCode).HasComment("Code indicating the purpose of the property research");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the code indicating the purpose of the property research");

                entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled.");
            });

            modelBuilder.Entity<PimsProperty>(entity =>
            {
                entity.HasKey(e => e.PropertyId)
                    .HasName("PRPRTY_PK");

                entity.HasComment("Describes the attributes of a property.");

                entity.Property(e => e.PropertyId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Boundary).HasComment("Spatial bundary of land");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Property description");

                entity.Property(e => e.EncumbranceReason).HasComment("reason for property encumbreance");

                entity.Property(e => e.FileNumber).HasComment("The (ARCS/ORCS) number identifying the Property File.");

                entity.Property(e => e.FileNumberSuffix).HasComment("A suffix to distinguish between Property Files with the same number.");

                entity.Property(e => e.IsOwned)
                    .HasDefaultValueSql("(CONVERT([bit],(1)))")
                    .HasComment("Is the property currently owned?");

                entity.Property(e => e.IsPropertyOfInterest)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is this a property of interest to the Ministry?");

                entity.Property(e => e.IsProvincialPublicHwy).HasComment("Is this property a provincial public highway?");

                entity.Property(e => e.IsRwyBeltDomPatent)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if this property is original federal vs. provincial ownership.");

                entity.Property(e => e.IsSensitive)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is this a sensitive property?");

                entity.Property(e => e.IsVisibleToOtherAgencies)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is the property visible to other agencies?");

                entity.Property(e => e.IsVolumetricParcel)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is there a volumetric measurement for this parcel?");

                entity.Property(e => e.LandArea).HasComment("Area occupied by property");

                entity.Property(e => e.LandLegalDescription).HasComment("Legal description of property");

                entity.Property(e => e.Location).HasComment("Geospatial location (pin) of property");

                entity.Property(e => e.MunicipalZoning).HasComment("Municipal zoning that applies this property.");

                entity.Property(e => e.Name).HasComment("Property name");

                entity.Property(e => e.Notes).HasComment("Notes about the property");

                entity.Property(e => e.Pid).HasComment("Property ID");

                entity.Property(e => e.Pin).HasComment("Property number");

                entity.Property(e => e.PphStatusUpdateTimestamp).HasComment("Date / time that the Provincial Public Highway status was updated.");

                entity.Property(e => e.PphStatusUpdateUserGuid).HasComment("GUID of the user that updated the PPH status.");

                entity.Property(e => e.PphStatusUpdateUserid).HasComment("Userid that updated the Provincial Public Highway status.");

                entity.Property(e => e.PropertyDataSourceEffectiveDate).HasComment("Date the property was officially registered");

                entity.Property(e => e.SurplusDeclarationComment).HasComment("Comment regarding the surplus declaration");

                entity.Property(e => e.SurplusDeclarationDate).HasComment("Date the property was declared surplus");

                entity.Property(e => e.SurveyPlanNumber).HasComment("Property/Land Parcel survey plan number");

                entity.Property(e => e.VolumetricMeasurement).HasComment("Volumetric measurement of the parcel.");

                entity.Property(e => e.Zoning).HasComment("Current property zoning");

                entity.Property(e => e.ZoningPotential).HasComment("Potential property zoning");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("PIM_ADDRSS_PIM_PRPRTY_FK");

                entity.HasOne(d => d.DistrictCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.DistrictCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_DSTRCT_PIM_PRPRTY_FK");

                entity.HasOne(d => d.PphStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.PphStatusTypeCode)
                    .HasConstraintName("PIM_PPHSTT_PIM_PRPRTY_FK");

                entity.HasOne(d => d.PropMgmtOrg)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.PropMgmtOrgId)
                    .HasConstraintName("PIM_ORG_PIM_PRPRTY_FK");

                entity.HasOne(d => d.PropertyAreaUnitTypeCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.PropertyAreaUnitTypeCode)
                    .HasConstraintName("PIM_ARUNIT_PIM_PRPRTY_FK");

                entity.HasOne(d => d.PropertyClassificationTypeCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.PropertyClassificationTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPCLT_PIM_PRPRTY_FK");

                entity.HasOne(d => d.PropertyDataSourceTypeCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.PropertyDataSourceTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PIDSRT_PIM_PRPRTY_FK");

                entity.HasOne(d => d.PropertyManager)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.PropertyManagerId)
                    .HasConstraintName("PIM_PERSON_PIM_PRPRTY_FK");

                entity.HasOne(d => d.PropertyStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.PropertyStatusTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPSTS_PIM_PRPRTY_FK");

                entity.HasOne(d => d.PropertyTypeCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.PropertyTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPTYP_PIM_PRPRTY_FK");

                entity.HasOne(d => d.RegionCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.RegionCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_REGION_PIM_PRPRTY_FK");

                entity.HasOne(d => d.SurplusDeclarationTypeCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.SurplusDeclarationTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_SPDCLT_PIM_PRPRTY_FK");

                entity.HasOne(d => d.VolumeUnitTypeCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.VolumeUnitTypeCode)
                    .HasConstraintName("PIM_VOLUTY_PIM_PRPRTY_FK");

                entity.HasOne(d => d.VolumetricTypeCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.VolumetricTypeCode)
                    .HasConstraintName("PIM_PRVOLT_PIM_PRPRTY_FK");
            });

            modelBuilder.Entity<PimsPropertyAcquisitionFile>(entity =>
            {
                entity.HasKey(e => e.PropertyAcquisitionFileId)
                    .HasName("PRACQF_PK");

                entity.HasComment("Associates a property with an acquisition file.");

                entity.Property(e => e.PropertyAcquisitionFileId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACQUISITION_FILE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the relationship is active.");

                entity.Property(e => e.PropertyName).HasComment("Descriptive reference for the property associated with the acquisition file.");

                entity.HasOne(d => d.AcquisitionFile)
                    .WithMany(p => p.PimsPropertyAcquisitionFiles)
                    .HasForeignKey(d => d.AcquisitionFileId)
                    .HasConstraintName("PIM_ACQNFL_PIM_PRACQF_FK");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsPropertyAcquisitionFiles)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPRTY_PIM_PRACQF_FK");
            });

            modelBuilder.Entity<PimsPropertyAcquisitionFileHist>(entity =>
            {
                entity.HasKey(e => e.PropertyAcquisitionFileHistId)
                    .HasName("PIMS_PRACQF_H_PK");

                entity.Property(e => e.PropertyAcquisitionFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACQUISITION_FILE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPropertyAdjacentLandType>(entity =>
            {
                entity.HasKey(e => e.PropertyAdjacentLandTypeCode)
                    .HasName("PRADJL_PK");

                entity.HasComment("Code table to describe property adjacent land type.");

                entity.Property(e => e.PropertyAdjacentLandTypeCode).HasComment("Property adjacent land code.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Property adjacent land code description.");

                entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled.");
            });

            modelBuilder.Entity<PimsPropertyAnomalyType>(entity =>
            {
                entity.HasKey(e => e.PropertyAnomalyTypeCode)
                    .HasName("PRANOM_PK");

                entity.HasComment("Code table to describe property anomalies.");

                entity.Property(e => e.PropertyAnomalyTypeCode).HasComment("Property anomaly code.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Property anomaly code description.");

                entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled.");
            });

            modelBuilder.Entity<PimsPropertyBoundaryVw>(entity =>
            {
                entity.ToView("PIMS_PROPERTY_BOUNDARY_VW");

                entity.Property(e => e.PidPadded).IsUnicode(false);
            });

            modelBuilder.Entity<PimsPropertyClassificationType>(entity =>
            {
                entity.HasKey(e => e.PropertyClassificationTypeCode)
                    .HasName("PRPCLT_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsPropertyEvaluation>(entity =>
            {
                entity.HasKey(e => e.PropertyEvaluationId)
                    .HasName("PRPEVL_PK");

                entity.Property(e => e.PropertyEvaluationId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_EVALUATION_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsPropertyEvaluations)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPRTY_PIM_PRPEVL_FK");
            });

            modelBuilder.Entity<PimsPropertyEvaluationHist>(entity =>
            {
                entity.HasKey(e => e.PropertyEvaluationHistId)
                    .HasName("PIMS_PRPEVL_H_PK");

                entity.Property(e => e.PropertyEvaluationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_EVALUATION_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPropertyHist>(entity =>
            {
                entity.HasKey(e => e.PropertyHistId)
                    .HasName("PIMS_PRPRTY_H_PK");

                entity.Property(e => e.PropertyHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPropertyImprovement>(entity =>
            {
                entity.HasKey(e => e.PropertyImprovementId)
                    .HasName("PIMPRV_PK");

                entity.HasComment("Description of property improvements associated with the lease.");

                entity.Property(e => e.PropertyImprovementId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_IMPROVEMENT_ID_SEQ])");

                entity.Property(e => e.Address).HasComment("Addresses affected");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ImprovementDescription).HasComment("Description of the improvements");

                entity.Property(e => e.StructureSize).HasComment("Size of the structure (house, building, bridge, etc,)");

                entity.HasOne(d => d.Lease)
                    .WithMany(p => p.PimsPropertyImprovements)
                    .HasForeignKey(d => d.LeaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LEASE_PIM_PIMPRV_FK");

                entity.HasOne(d => d.PropertyImprovementTypeCodeNavigation)
                    .WithMany(p => p.PimsPropertyImprovements)
                    .HasForeignKey(d => d.PropertyImprovementTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PIMPRT_PIM_PIMPRV_FK");
            });

            modelBuilder.Entity<PimsPropertyImprovementHist>(entity =>
            {
                entity.HasKey(e => e.PropertyImprovementHistId)
                    .HasName("PIMS_PIMPRV_H_PK");

                entity.Property(e => e.PropertyImprovementHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_IMPROVEMENT_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPropertyImprovementType>(entity =>
            {
                entity.HasKey(e => e.PropertyImprovementTypeCode)
                    .HasName("PIMPRT_PK");

                entity.HasComment("Description of the types of improvements made to a property during the lease.");

                entity.Property(e => e.PropertyImprovementTypeCode).HasComment("Code value of the types of improvements made to a property during the lease.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Code description of the types of improvements made to a property during the lease.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled");
            });

            modelBuilder.Entity<PimsPropertyLease>(entity =>
            {
                entity.HasKey(e => e.PropertyLeaseId)
                    .HasName("PROPLS_PK");

                entity.Property(e => e.PropertyLeaseId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_LEASE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.LeaseArea).HasComment("Leased area measurement");

                entity.Property(e => e.Name).HasComment("Property/lease name");

                entity.HasOne(d => d.AreaUnitTypeCodeNavigation)
                    .WithMany(p => p.PimsPropertyLeases)
                    .HasForeignKey(d => d.AreaUnitTypeCode)
                    .HasConstraintName("PIM_ARUNIT_PIM_PROPLS_FK");

                entity.HasOne(d => d.Lease)
                    .WithMany(p => p.PimsPropertyLeases)
                    .HasForeignKey(d => d.LeaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LEASE_PIM_PROPLS_FK");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsPropertyLeases)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPRTY_PIM_PROPLS_FK");
            });

            modelBuilder.Entity<PimsPropertyLeaseHist>(entity =>
            {
                entity.HasKey(e => e.PropertyLeaseHistId)
                    .HasName("PIMS_PROPLS_H_PK");

                entity.Property(e => e.PropertyLeaseHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_LEASE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPropertyLocationVw>(entity =>
            {
                entity.ToView("PIMS_PROPERTY_LOCATION_VW");

                entity.Property(e => e.PidPadded).IsUnicode(false);
            });

            modelBuilder.Entity<PimsPropertyOrganization>(entity =>
            {
                entity.HasKey(e => e.PropertyOrganizationId)
                    .HasName("PRPORG_PK");

                entity.Property(e => e.PropertyOrganizationId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ORGANIZATION_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.PimsPropertyOrganizations)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ORG_PIM_PRPORG_FK");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsPropertyOrganizations)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPRTY_PIM_PRPORG_FK");
            });

            modelBuilder.Entity<PimsPropertyOrganizationHist>(entity =>
            {
                entity.HasKey(e => e.PropertyOrganizationHistId)
                    .HasName("PIMS_PRPORG_H_PK");

                entity.Property(e => e.PropertyOrganizationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ORGANIZATION_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPropertyPropertyServiceFile>(entity =>
            {
                entity.HasKey(e => e.PropertyPropertyServiceFileId)
                    .HasName("PRPRSF_PK");

                entity.Property(e => e.PropertyPropertyServiceFileId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_PROPERTY_SERVICE_FILE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsPropertyPropertyServiceFiles)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPRTY_PIM_PRPRSF_FK");

                entity.HasOne(d => d.PropertyServiceFile)
                    .WithMany(p => p.PimsPropertyPropertyServiceFiles)
                    .HasForeignKey(d => d.PropertyServiceFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPSVC_PIM_PRPRSF_FK");
            });

            modelBuilder.Entity<PimsPropertyPropertyServiceFileHist>(entity =>
            {
                entity.HasKey(e => e.PropertyPropertyServiceFileHistId)
                    .HasName("PIMS_PRPRSF_H_PK");

                entity.Property(e => e.PropertyPropertyServiceFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_PROPERTY_SERVICE_FILE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPropertyResearchFile>(entity =>
            {
                entity.HasKey(e => e.PropertyResearchFileId)
                    .HasName("PRSCRC_PK");

                entity.HasComment("Associates a property with a research file.");

                entity.Property(e => e.PropertyResearchFileId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_RESEARCH_FILE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");

                entity.Property(e => e.DocumentReference).HasComment("URL / reference to a LAN Drive");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled.");

                entity.Property(e => e.IsLegalOpinionObtained).HasComment("Indicates whether a legal opinion was obtained (0 = No, 1 = Yes, null = Unknown)");

                entity.Property(e => e.IsLegalOpinionRequired).HasComment("Indicates whether a legal opinion is required (0 = No, 1 = Yes, null = Unknown)");

                entity.Property(e => e.PropertyName).HasComment("Descriptive reference for the property being researched.");

                entity.Property(e => e.ResearchSummary).HasComment("Summary of the property research.");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsPropertyResearchFiles)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPRTY_PIM_PRSCRC_FK");

                entity.HasOne(d => d.ResearchFile)
                    .WithMany(p => p.PimsPropertyResearchFiles)
                    .HasForeignKey(d => d.ResearchFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_RESRCH_PIM_PRSCRC_FK");
            });

            modelBuilder.Entity<PimsPropertyResearchFileHist>(entity =>
            {
                entity.HasKey(e => e.PropertyResearchFileHistId)
                    .HasName("PIMS_PRSCRC_H_PK");

                entity.Property(e => e.PropertyResearchFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_RESEARCH_FILE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPropertyRoadType>(entity =>
            {
                entity.HasKey(e => e.PropertyRoadTypeCode)
                    .HasName("PRROAD_PK");

                entity.HasComment("Code table to describe property highway/road type.");

                entity.Property(e => e.PropertyRoadTypeCode).HasComment("Property highway/road code.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Property highway/road code description.");

                entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled.");
            });

            modelBuilder.Entity<PimsPropertyServiceFile>(entity =>
            {
                entity.HasKey(e => e.PropertyServiceFileId)
                    .HasName("PRPSVC_PK");

                entity.Property(e => e.PropertyServiceFileId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_SERVICE_FILE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.PropertyServiceFileTypeCodeNavigation)
                    .WithMany(p => p.PimsPropertyServiceFiles)
                    .HasForeignKey(d => d.PropertyServiceFileTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRSVFT_PIM_PRPSVC_FK");
            });

            modelBuilder.Entity<PimsPropertyServiceFileHist>(entity =>
            {
                entity.HasKey(e => e.PropertyServiceFileHistId)
                    .HasName("PIMS_PRPSVC_H_PK");

                entity.Property(e => e.PropertyServiceFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_SERVICE_FILE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPropertyServiceFileType>(entity =>
            {
                entity.HasKey(e => e.PropertyServiceFileTypeCode)
                    .HasName("PRSVFT_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsPropertyStatusType>(entity =>
            {
                entity.HasKey(e => e.PropertyStatusTypeCode)
                    .HasName("PRPSTS_PK");

                entity.HasComment("Code table to describe property status.");

                entity.Property(e => e.PropertyStatusTypeCode).HasComment("Property status code.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Property status code description.");

                entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled.");
            });

            modelBuilder.Entity<PimsPropertyTax>(entity =>
            {
                entity.HasKey(e => e.PropertyTaxId)
                    .HasName("PRPTAX_PK");

                entity.Property(e => e.PropertyTaxId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_TAX_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.TaxFolioNo).HasComment("Property tax folio number");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsPropertyTaxes)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPRTY_PIM_PRPTAX_FK");

                entity.HasOne(d => d.PropertyTaxRemitTypeCodeNavigation)
                    .WithMany(p => p.PimsPropertyTaxes)
                    .HasForeignKey(d => d.PropertyTaxRemitTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PTRMTT_PIM_PRPTAX_FK");
            });

            modelBuilder.Entity<PimsPropertyTaxHist>(entity =>
            {
                entity.HasKey(e => e.PropertyTaxHistId)
                    .HasName("PIMS_PRPTAX_H_PK");

                entity.Property(e => e.PropertyTaxHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_TAX_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsPropertyTaxRemitType>(entity =>
            {
                entity.HasKey(e => e.PropertyTaxRemitTypeCode)
                    .HasName("PTRMTT_PK");

                entity.HasComment("Description of property tax remittance types");

                entity.Property(e => e.PropertyTaxRemitTypeCode).HasComment("Code value of property tax remittance types");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Code description of property tax remittance types");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is this code value disabled?");
            });

            modelBuilder.Entity<PimsPropertyTenureType>(entity =>
            {
                entity.HasKey(e => e.PropertyTenureTypeCode)
                    .HasName("PRPTNR_PK");

                entity.HasComment("A code table to store property tenure codes. Tenure is defined as : \"The act, right, manner or term of holding something(as a landed property)\" In this case, tenure is required on Properties to indicate MoTI's legal tenure on the property. The land parcel");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsPropertyType>(entity =>
            {
                entity.HasKey(e => e.PropertyTypeCode)
                    .HasName("PRPTYP_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsProvinceState>(entity =>
            {
                entity.HasKey(e => e.ProvinceStateId)
                    .HasName("PROVNC_PK");

                entity.Property(e => e.ProvinceStateId).ValueGeneratedNever();

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.PimsProvinceStates)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_CNTRY_PIM_PROVNC_FK");
            });

            modelBuilder.Entity<PimsRegion>(entity =>
            {
                entity.HasKey(e => e.RegionCode)
                    .HasName("REGION_PK");

                entity.Property(e => e.RegionCode).ValueGeneratedNever();

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsRegionUser>(entity =>
            {
                entity.HasKey(e => e.RegionUserId)
                    .HasName("RGNUSR_PK");

                entity.Property(e => e.RegionUserId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_REGION_USER_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.RegionCode).HasDefaultValueSql("((4))");

                entity.HasOne(d => d.RegionCodeNavigation)
                    .WithMany(p => p.PimsRegionUsers)
                    .HasForeignKey(d => d.RegionCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_REGION_PIM_RGNUSR_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PimsRegionUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_USER_PIM_RGNUSR_FK");
            });

            modelBuilder.Entity<PimsRegionUserHist>(entity =>
            {
                entity.HasKey(e => e.RegionUserHistId)
                    .HasName("PIMS_RGNUSR_H_PK");

                entity.Property(e => e.RegionUserHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_REGION_USER_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsRequestSourceType>(entity =>
            {
                entity.HasKey(e => e.RequestSourceTypeCode)
                    .HasName("RQSRCT_PK");

                entity.HasComment("Code table to describe source ot the research request");

                entity.Property(e => e.RequestSourceTypeCode).HasComment("Code indicating the source of the research request.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the code indicating the source of the research request.");

                entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled.");
            });

            modelBuilder.Entity<PimsResearchActivityInstance>(entity =>
            {
                entity.HasKey(e => e.ResearchActivityInstanceId)
                    .HasName("RSCHAI_PK");

                entity.HasComment("Relates a research file to an activity instance.");

                entity.Property(e => e.ResearchActivityInstanceId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_ACTIVITY_INSTANCE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the relationship has been disabled.");

                entity.HasOne(d => d.ActivityInstance)
                    .WithMany(p => p.PimsResearchActivityInstances)
                    .HasForeignKey(d => d.ActivityInstanceId)
                    .HasConstraintName("PIM_ACTINS_PIM_RSCHAI_FK");

                entity.HasOne(d => d.ResearchFile)
                    .WithMany(p => p.PimsResearchActivityInstances)
                    .HasForeignKey(d => d.ResearchFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_RESRCH_PIM_RSCHAI_FK");
            });

            modelBuilder.Entity<PimsResearchActivityInstanceHist>(entity =>
            {
                entity.HasKey(e => e.ResearchActivityInstanceHistId)
                    .HasName("PIMS_RSCHAI_H_PK");

                entity.Property(e => e.ResearchActivityInstanceHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_ACTIVITY_INSTANCE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsResearchFile>(entity =>
            {
                entity.HasKey(e => e.ResearchFileId)
                    .HasName("RESRCH_PK");

                entity.HasComment("Property research file");

                entity.Property(e => e.ResearchFileId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

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

                entity.Property(e => e.ResearchFileStatusTypeCode).HasDefaultValueSql("('ACTIVE')");

                entity.Property(e => e.ResearchResult).HasComment("Result of the research request.");

                entity.Property(e => e.RfileNumber)
                    .HasDefaultValueSql("('RFILE-UNKNOWN')")
                    .HasComment("R-File number assigned to the research file, formatted value from PIMS_RFILE_NUMBER_SEQ sequence generator");

                entity.Property(e => e.RoadAlias).HasComment("Alias(es) of roads associated with this research request.");

                entity.Property(e => e.RoadName).HasComment("Name(s) of roads associated with this research request.");

                entity.HasOne(d => d.RequestSourceTypeCodeNavigation)
                    .WithMany(p => p.PimsResearchFiles)
                    .HasForeignKey(d => d.RequestSourceTypeCode)
                    .HasConstraintName("PIM_RQSRCT_PIM_RESRCH_FK");

                entity.HasOne(d => d.RequestorNameNavigation)
                    .WithMany(p => p.PimsResearchFiles)
                    .HasForeignKey(d => d.RequestorName)
                    .HasConstraintName("PIM_PERSON_PIM_RESRCH_FK");

                entity.HasOne(d => d.RequestorOrganizationNavigation)
                    .WithMany(p => p.PimsResearchFiles)
                    .HasForeignKey(d => d.RequestorOrganization)
                    .HasConstraintName("PIM_ORG_PIM_RESRCH_FK");

                entity.HasOne(d => d.ResearchFileStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsResearchFiles)
                    .HasForeignKey(d => d.ResearchFileStatusTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_RSRCHS_PIM_RESRCH_FK");
            });

            modelBuilder.Entity<PimsResearchFileHist>(entity =>
            {
                entity.HasKey(e => e.ResearchFileHistId)
                    .HasName("PIMS_RESRCH_H_PK");

                entity.Property(e => e.ResearchFileHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsResearchFilePurpose>(entity =>
            {
                entity.HasKey(e => e.ResearchFilePurposeId)
                    .HasName("RSFLPR_PK");

                entity.Property(e => e.ResearchFilePurposeId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_PURPOSE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ResearchPurposeTypeCode).HasDefaultValueSql("('GENENQ')");

                entity.HasOne(d => d.ResearchFile)
                    .WithMany(p => p.PimsResearchFilePurposes)
                    .HasForeignKey(d => d.ResearchFileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_RESRCH_PIM_RSFLPR_FK");

                entity.HasOne(d => d.ResearchPurposeTypeCodeNavigation)
                    .WithMany(p => p.PimsResearchFilePurposes)
                    .HasForeignKey(d => d.ResearchPurposeTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_RSHPRT_PIM_RSFLPR_FK");
            });

            modelBuilder.Entity<PimsResearchFilePurposeHist>(entity =>
            {
                entity.HasKey(e => e.ResearchFilePurposeHistId)
                    .HasName("PIMS_RSFLPR_H_PK");

                entity.Property(e => e.ResearchFilePurposeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESEARCH_FILE_PURPOSE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsResearchFileStatusType>(entity =>
            {
                entity.HasKey(e => e.ResearchFileStatusTypeCode)
                    .HasName("RSRCHS_PK");

                entity.HasComment("Code table to describe property adjacent land type.");

                entity.Property(e => e.ResearchFileStatusTypeCode).HasComment("Code indicating the status of the research file.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the code indicating the status of the research file.");

                entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled.");
            });

            modelBuilder.Entity<PimsResearchPurposeType>(entity =>
            {
                entity.HasKey(e => e.ResearchPurposeTypeCode)
                    .HasName("RSHPRT_PK");

                entity.HasComment("Code table to describe the purpose ot the research request");

                entity.Property(e => e.ResearchPurposeTypeCode).HasComment("Code indicating the purpose of the research request.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the code indicating the purpose of the research request.");

                entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled.");
            });

            modelBuilder.Entity<PimsResponsibilityCode>(entity =>
            {
                entity.HasComment("Code and description of the responsibility codes.");

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
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("Name of the code.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

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
                entity.HasKey(e => e.ResponsibilityCodeHistId)
                    .HasName("PIMS_RESPCD_H_PK");

                entity.Property(e => e.ResponsibilityCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_RESPONSIBILITY_CODE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("ROLE_PK");

                entity.Property(e => e.RoleId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ROLE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.IsPublic).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsRoleClaim>(entity =>
            {
                entity.HasKey(e => e.RoleClaimId)
                    .HasName("ROLCLM_PK");

                entity.Property(e => e.RoleClaimId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ROLE_CLAIM_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.Claim)
                    .WithMany(p => p.PimsRoleClaims)
                    .HasForeignKey(d => d.ClaimId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_CLMTYP_PIM_ROLCLM_FK");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PimsRoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ROLE_PIM_ROLCLM_FK");
            });

            modelBuilder.Entity<PimsRoleClaimHist>(entity =>
            {
                entity.HasKey(e => e.RoleClaimHistId)
                    .HasName("PIMS_ROLCLM_H_PK");

                entity.Property(e => e.RoleClaimHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ROLE_CLAIM_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsRoleHist>(entity =>
            {
                entity.HasKey(e => e.RoleHistId)
                    .HasName("PIMS_ROLE_H_PK");

                entity.Property(e => e.RoleHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ROLE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsSecurityDeposit>(entity =>
            {
                entity.HasKey(e => e.SecurityDepositId)
                    .HasName("SECDEP_PK");

                entity.HasComment("Description of a security deposit associated with a lease.");

                entity.Property(e => e.SecurityDepositId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_ID_SEQ])");

                entity.Property(e => e.AmountPaid).HasComment("Amount paid of this security deposit");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DepositDate).HasComment("Date of this security deposit");

                entity.Property(e => e.Description).HasComment("Descirption of this security deposit");

                entity.Property(e => e.OtherDepositTypeDesc).HasComment("Description of the deposit type If the SECURITY_DEPOSIT_TYPE_CODE has been chosen for this scurity deposit.");

                entity.HasOne(d => d.Lease)
                    .WithMany(p => p.PimsSecurityDeposits)
                    .HasForeignKey(d => d.LeaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LEASE_PIM_SECDEP_FK");

                entity.HasOne(d => d.SecurityDepositTypeCodeNavigation)
                    .WithMany(p => p.PimsSecurityDeposits)
                    .HasForeignKey(d => d.SecurityDepositTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_SECDPT_PIM_SECDEP_FK");
            });

            modelBuilder.Entity<PimsSecurityDepositHist>(entity =>
            {
                entity.HasKey(e => e.SecurityDepositHistId)
                    .HasName("PIMS_SECDEP_H_PK");

                entity.Property(e => e.SecurityDepositHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsSecurityDepositHolder>(entity =>
            {
                entity.HasKey(e => e.SecurityDepositHolderId)
                    .HasName("SCDPHL_PK");

                entity.Property(e => e.SecurityDepositHolderId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_HOLDER_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.PimsSecurityDepositHolders)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("PIM_ORG_PIM_SCDPHL_FK");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PimsSecurityDepositHolders)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("PIM_PERSON_PIM_SCDPHL_FK");

                entity.HasOne(d => d.SecurityDeposit)
                    .WithOne(p => p.PimsSecurityDepositHolder)
                    .HasForeignKey<PimsSecurityDepositHolder>(d => d.SecurityDepositId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_SECDEP_PIM_SCDPHL_FK");
            });

            modelBuilder.Entity<PimsSecurityDepositHolderHist>(entity =>
            {
                entity.HasKey(e => e.SecurityDepositHolderHistId)
                    .HasName("PIMS_SCDPHL_H_PK");

                entity.Property(e => e.SecurityDepositHolderHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_HOLDER_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsSecurityDepositReturn>(entity =>
            {
                entity.HasKey(e => e.SecurityDepositReturnId)
                    .HasName("SDRTRN_PK");

                entity.HasComment("Describes the details of the return of a security deposit.");

                entity.Property(e => e.SecurityDepositReturnId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_RETURN_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ClaimsAgainst).HasComment("Amount of claims against the deposit");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.InterestPaid).HasComment("Interest paid on the deposit to the deposit holder");

                entity.Property(e => e.ReturnAmount).HasComment("Amount returned minus claims");

                entity.Property(e => e.ReturnDate).HasComment("Date of deposit return");

                entity.Property(e => e.TerminationDate).HasComment("Date the lease/license was terminated or surrendered");

                entity.HasOne(d => d.SecurityDeposit)
                    .WithMany(p => p.PimsSecurityDepositReturns)
                    .HasForeignKey(d => d.SecurityDepositId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_SECDEP_PIM_SDRTRN_FK");
            });

            modelBuilder.Entity<PimsSecurityDepositReturnHist>(entity =>
            {
                entity.HasKey(e => e.SecurityDepositReturnHistId)
                    .HasName("PIMS_SDRTRN_H_PK");

                entity.Property(e => e.SecurityDepositReturnHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_RETURN_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsSecurityDepositReturnHolder>(entity =>
            {
                entity.HasKey(e => e.SecurityDepositReturnHolderId)
                    .HasName("SCDPRH_PK");

                entity.Property(e => e.SecurityDepositReturnHolderId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_RETURN_HOLDER_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.PimsSecurityDepositReturnHolders)
                    .HasForeignKey(d => d.OrganizationId)
                    .HasConstraintName("PIM_ORG_PIM_SCDPRH_FK");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PimsSecurityDepositReturnHolders)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("PIM_PERSON_PIM_SCDPRH_FK");

                entity.HasOne(d => d.SecurityDepositReturn)
                    .WithOne(p => p.PimsSecurityDepositReturnHolder)
                    .HasForeignKey<PimsSecurityDepositReturnHolder>(d => d.SecurityDepositReturnId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_SDRTRN_PIM_SCDPRH_FK");
            });

            modelBuilder.Entity<PimsSecurityDepositReturnHolderHist>(entity =>
            {
                entity.HasKey(e => e.SecurityDepositReturnHolderHistId)
                    .HasName("PIMS_SCDPRH_H_PK");

                entity.Property(e => e.SecurityDepositReturnHolderHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_RETURN_HOLDER_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsSecurityDepositType>(entity =>
            {
                entity.HasKey(e => e.SecurityDepositTypeCode)
                    .HasName("SECDPT_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsStaticVariable>(entity =>
            {
                entity.HasKey(e => e.StaticVariableName)
                    .HasName("STAVBL_PK");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");
            });

            modelBuilder.Entity<PimsStaticVariableHist>(entity =>
            {
                entity.HasKey(e => e.StaticVariableHistId)
                    .HasName("PIMS_STAVBL_H_PK");

                entity.Property(e => e.StaticVariableHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_STATIC_VARIABLE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsSurplusDeclarationType>(entity =>
            {
                entity.HasKey(e => e.SurplusDeclarationTypeCode)
                    .HasName("SPDCLT_PK");

                entity.HasComment("Description of the surplus property type.");

                entity.Property(e => e.SurplusDeclarationTypeCode).HasComment("Code value of the surplus property type");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Code description of the surplus property type");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates that the code value is disabled");
            });

            modelBuilder.Entity<PimsSurveyPlanType>(entity =>
            {
                entity.HasKey(e => e.SurveyPlanTypeCode)
                    .HasName("SRVPLT_PK");

                entity.HasComment("Codified values for the survey plan type.  This is an unassociated table that is used in the UI to populate JSON attributes.");

                entity.Property(e => e.SurveyPlanTypeCode).HasComment("Code value for the survey plan type.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the survey plan type.");

                entity.Property(e => e.DisplayOrder).HasComment("Designates a preferred presentation order of the code descriptions.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is inactive.");
            });

            modelBuilder.Entity<PimsTenant>(entity =>
            {
                entity.HasKey(e => e.TenantId)
                    .HasName("TENNTX_PK");

                entity.HasComment("Deprecated table to support legacy CITZ-PIMS application code.  This table will be removed once the code dependency is removed from the system.");

                entity.Property(e => e.TenantId)
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TENANT_ID_SEQ])")
                    .HasComment("Auto-sequenced unique key value");

                entity.Property(e => e.Code).HasComment("Code value for entry");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Description of the entry for display purposes");

                entity.Property(e => e.Name).HasComment("Name of the entry for display purposes");

                entity.Property(e => e.Settings).HasComment("Serialized JSON value for the configuration");
            });

            modelBuilder.Entity<PimsTenantType>(entity =>
            {
                entity.HasKey(e => e.TenantTypeCode)
                    .HasName("TENTYP_PK");

                entity.HasComment("Code table describing the type of tenant on a lease.");

                entity.Property(e => e.TenantTypeCode)
                    .HasDefaultValueSql("('UNK')")
                    .HasComment("Code representing the types of tenants on a lease.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .HasDefaultValueSql("('Unknown')")
                    .HasComment("Description of the types of tenants on a lease.");

                entity.Property(e => e.DisplayOrder).HasComment("Specifies a specific order to visually present the code.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is currently active.");
            });

            modelBuilder.Entity<PimsUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("USER_PK");

                entity.HasComment("Information associated with an identified PIMS system user.");

                entity.Property(e => e.UserId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ApprovedById).HasComment("Identifier of the person that approved the creation of this PIMS user.");

                entity.Property(e => e.BusinessIdentifierValue).HasComment("Accepted identifier of a user (e.g. IDIR)");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ExpiryDate).HasComment("Expiry date/time of this user account.");

                entity.Property(e => e.GuidIdentifierValue).HasComment("Unique GUID associated with the user.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if this user account is disabled.");

                entity.Property(e => e.IssueDate).HasComment("Date/time that this user was identified as a PIMS user,");

                entity.Property(e => e.LastLogin).HasComment("Last date/time the user was logged into PIMS.");

                entity.Property(e => e.Note).HasComment("Notes associated with this user.");

                entity.Property(e => e.Position).HasComment("Role/position assigned to the user.");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PimsUsers)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PERSON_PIM_USER_FK");
            });

            modelBuilder.Entity<PimsUserHist>(entity =>
            {
                entity.HasKey(e => e.UserHistId)
                    .HasName("PIMS_USER_H_PK");

                entity.Property(e => e.UserHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsUserOrganization>(entity =>
            {
                entity.HasKey(e => e.UserOrganizationId)
                    .HasName("USRORG_PK");

                entity.HasComment("Associates a user with an organization.");

                entity.Property(e => e.UserOrganizationId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ORGANIZATION_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if this association is disabled.");

                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.PimsUserOrganizations)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ORG_PIM_USRORG_FK");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PimsUserOrganizations)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ROLE_PIM_USRORG_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PimsUserOrganizations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_USER_PIM_USRORG_FK");
            });

            modelBuilder.Entity<PimsUserOrganizationHist>(entity =>
            {
                entity.HasKey(e => e.UserOrganizationHistId)
                    .HasName("PIMS_USRORG_H_PK");

                entity.Property(e => e.UserOrganizationHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ORGANIZATION_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsUserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId)
                    .HasName("USERRL_PK");

                entity.HasComment("Associates a user with an role.");

                entity.Property(e => e.UserRoleId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ROLE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if this association is disabled.");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PimsUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ROLE_PIM_USERRL_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PimsUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_USER_PIM_USERRL_FK");
            });

            modelBuilder.Entity<PimsUserRoleHist>(entity =>
            {
                entity.HasKey(e => e.UserRoleHistId)
                    .HasName("PIMS_USERRL_H_PK");

                entity.Property(e => e.UserRoleHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ROLE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsVolumeUnitType>(entity =>
            {
                entity.HasKey(e => e.VolumeUnitTypeCode)
                    .HasName("VOLUTY_PK");

                entity.HasComment("The volume unit used for measuring Properties.");

                entity.Property(e => e.VolumeUnitTypeCode).HasComment("The volume unit used for measuring Properties.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Translation of the code value into a description that can be displayed to the user.");

                entity.Property(e => e.DisplayOrder).HasComment("Order in which to display the code values, if required.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is still active or is now disabled.");
            });

            modelBuilder.Entity<PimsVolumetricType>(entity =>
            {
                entity.HasKey(e => e.VolumetricTypeCode)
                    .HasName("PRVOLT_PK");

                entity.HasComment("Code table to describe parcel/property volumetric type.");

                entity.Property(e => e.VolumetricTypeCode).HasComment("Property parcel/property volumetric code.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description).HasComment("Property parcel/property volumetric code description.");

                entity.Property(e => e.DisplayOrder).HasComment("Force the display order of the codes.");

                entity.Property(e => e.IsDisabled)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled.");
            });

            modelBuilder.Entity<PimsWorkActivityCode>(entity =>
            {
                entity.HasComment("Code and description of the work activity codes.");

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
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("Name of the code.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

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
                entity.HasKey(e => e.WorkActivityCodeHistId)
                    .HasName("PIMS_WRKACT_H_PK");

                entity.Property(e => e.WorkActivityCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_WORK_ACTIVITY_CODE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsYearlyFinancialCode>(entity =>
            {
                entity.HasComment("Code and description of the chart of accounts codes.");

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
                    .HasDefaultValueSql("('<Empty>')")
                    .HasComment("Standard Object of Expenditure (STOB) code.");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

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
                entity.HasKey(e => e.YearlyFinancialCodeHistId)
                    .HasName("PIMS_YRFINC_H_PK");

                entity.Property(e => e.YearlyFinancialCodeHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_YEARLY_FINANCIAL_CODE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.HasSequence("BCA_DATA_ADVICE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("BCA_MINOR_TAXING_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("BCA_OWNER_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACCESS_REQUEST_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACCESS_REQUEST_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACCESS_REQUEST_ORGANIZATION_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACCESS_REQUEST_ORGANIZATION_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACQUISITION_ACTIVITY_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACQUISITION_ACTIVITY_INSTANCE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACQUISITION_ACTIVITY_INSTANCE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence<int>("PIMS_ACQUISITION_FILE_NO_SEQ").HasMin(1);

            modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_NOTE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_NOTE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_PERSON_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACQUISITION_FILE_PERSON_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACQUISITION_OWNER_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACQUISITION_OWNER_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACT_INST_PROP_ACQ_FILE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACT_INST_PROP_ACQ_FILE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACT_INST_PROP_RSRCH_FILE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACT_INST_PROP_RSRCH_FILE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_INSTANCE_DOCUMENT_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_INSTANCE_DOCUMENT_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_INSTANCE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_INSTANCE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_INSTANCE_NOTE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_INSTANCE_NOTE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_MODEL_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_MODEL_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_MODEL_TASK_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_SERVICE_FILE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_TASK_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_TEMPLATE_DOCUMENT_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_TEMPLATE_DOCUMENT_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_TEMPLATE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_TEMPLATE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ADDRESS_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ADDRESS_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ASSET_EVALUATION_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_BUILDING_CONSTRUCTION_TYPE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_BUILDING_EVALUATION_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_BUILDING_FISCAL_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_BUILDING_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_BUILDING_OCCUPANT_TYPE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_BUILDING_PREDOMINATE_USE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_BUSINESS_FUNCTION_CODE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_BUSINESS_FUNCTION_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_CHART_OF_ACCOUNTS_CODE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_CHART_OF_ACCOUNTS_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_CLAIM_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_CLAIM_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_CONTACT_METHOD_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_CONTACT_METHOD_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_COST_TYPE_CODE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_COST_TYPE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_DOCUMENT_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_DOCUMENT_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_DOCUMENT_TYP_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_DOCUMENT_TYPE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_FINANCIAL_ACTIVITY_CODE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_FINANCIAL_ACTIVITY_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_GL_ACCOUNT_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_INSURANCE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_INSURANCE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_L_FILE_NO_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_ACTIVITY_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_ACTIVITY_INSTANCE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_ACTIVITY_INSTANCE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_ACTIVITY_PERIOD_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_PAYMENT_FORECAST_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_PAYMENT_FORECAST_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_PAYMENT_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_PAYMENT_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_PAYMENT_PERIOD_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_PAYMENT_PERIOD_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_TENANT_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_TENANT_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_TERM_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_LEASE_TERM_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_NOTE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_NOTE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ORGANIZATION_ADDRESS_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ORGANIZATION_ADDRESS_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ORGANIZATION_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ORGANIZATION_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PERSON_ADDRESS_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PERSON_ADDRESS_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PERSON_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PERSON_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PERSON_ORGANIZATION_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PERSON_ORGANIZATION_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PRF_PROP_RESEARCH_PURPOSE_ID_SEQ")
                .HasMin(1)
                .HasMax(21474483647);

            modelBuilder.HasSequence("PIMS_PRODUCT_BUSINESS_FUNCTION_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PRODUCT_COST_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PRODUCT_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PRODUCT_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PRODUCT_WORK_ACTIVITY_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROJECT_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROJECT_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROJECT_NOTE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROJECT_NOTE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROJECT_NUMBER_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROJECT_ORGANIZATION_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROJECT_PROPERTY_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROJECT_PROPERTY_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROJECT_STATUS_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROJECT_WORKFLOW_MODEL_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROJECT_WORKFLOW_MODEL_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROP_PROP_ADJACENT_LAND_TYPE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROP_PROP_ANOMALY_TYPE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROP_PROP_ROAD_TYPE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROP_PROP_TENURE_TYPE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_ACQUISITION_FILE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_ACQUISITION_FILE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_ACTIVITY_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_ACTIVITY_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_EVALUATION_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_EVALUATION_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_IMPROVEMENT_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_IMPROVEMENT_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_LEASE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_LEASE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_ORGANIZATION_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_ORGANIZATION_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_PROPERTY_SERVICE_FILE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_PROPERTY_SERVICE_FILE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_RESEARCH_FILE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_RESEARCH_FILE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_SERVICE_FILE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_SERVICE_FILE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_STRUCTURE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_TAX_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROPERTY_TAX_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_REGION_USER_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_REGION_USER_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_RESEARCH_ACTIVITY_INSTANCE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_RESEARCH_ACTIVITY_INSTANCE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_RESEARCH_FILE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_RESEARCH_FILE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_RESEARCH_FILE_PURPOSE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_RESEARCH_FILE_PURPOSE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_RESPONSIBILITY_CENTRE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_RESPONSIBILITY_CODE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_RESPONSIBILITY_CODE_SEQ")
                .StartsAt(991)
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_RESPONSIBILITY_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_RFILE_NUMBER_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ROLE_CLAIM_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ROLE_CLAIM_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ROLE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ROLE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_HOLDER_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_HOLDER_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_RETURN_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_RETURN_HOLDER_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_RETURN_HOLDER_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_RETURN_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_STATIC_VARIABLE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_STRUCTURE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_TASK_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_TASK_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_TASK_TEMPLATE_ACTIVITY_MODEL_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_TASK_TEMPLATE_ACTIVITY_MODEL_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_TASK_TEMPLATE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_TASK_TEMPLATE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_TENANT_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_USER_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_USER_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_USER_ORGANIZATION_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_USER_ORGANIZATION_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_USER_ROLE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_USER_ROLE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_USER_TASK_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_WORK_ACTIVITY_CODE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_WORK_ACTIVITY_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_WORKFLOW_MODEL_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_WORKFLOW_MODEL_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_YEARLY_FINANCIAL_CODE_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_YEARLY_FINANCIAL_CODE_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
