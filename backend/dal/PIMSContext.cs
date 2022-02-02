using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pims.Dal.Entities;

#nullable disable

namespace Pims.Dal
{
    public partial class PimsContext : DbContext
    {
        public PimsContext()
        {
        }

        public PimsContext(DbContextOptions<PimsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PimsAccessRequest> PimsAccessRequests { get; set; }
        public virtual DbSet<PimsAccessRequestHist> PimsAccessRequestHists { get; set; }
        public virtual DbSet<PimsAccessRequestOrganization> PimsAccessRequestOrganizations { get; set; }
        public virtual DbSet<PimsAccessRequestOrganizationHist> PimsAccessRequestOrganizationHists { get; set; }
        public virtual DbSet<PimsAccessRequestStatusType> PimsAccessRequestStatusTypes { get; set; }
        public virtual DbSet<PimsActivity> PimsActivities { get; set; }
        public virtual DbSet<PimsActivityHist> PimsActivityHists { get; set; }
        public virtual DbSet<PimsActivityModel> PimsActivityModels { get; set; }
        public virtual DbSet<PimsActivityModelHist> PimsActivityModelHists { get; set; }
        public virtual DbSet<PimsAddress> PimsAddresses { get; set; }
        public virtual DbSet<PimsAddressHist> PimsAddressHists { get; set; }
        public virtual DbSet<PimsAddressUsageType> PimsAddressUsageTypes { get; set; }
        public virtual DbSet<PimsAreaUnitType> PimsAreaUnitTypes { get; set; }
        public virtual DbSet<PimsClaim> PimsClaims { get; set; }
        public virtual DbSet<PimsClaimHist> PimsClaimHists { get; set; }
        public virtual DbSet<PimsContactMethod> PimsContactMethods { get; set; }
        public virtual DbSet<PimsContactMethodHist> PimsContactMethodHists { get; set; }
        public virtual DbSet<PimsContactMethodType> PimsContactMethodTypes { get; set; }
        public virtual DbSet<PimsContactMgrVw> PimsContactMgrVws { get; set; }
        public virtual DbSet<PimsCountry> PimsCountries { get; set; }
        public virtual DbSet<PimsDataSourceType> PimsDataSourceTypes { get; set; }
        public virtual DbSet<PimsDistrict> PimsDistricts { get; set; }
        public virtual DbSet<PimsInsurance> PimsInsurances { get; set; }
        public virtual DbSet<PimsInsuranceHist> PimsInsuranceHists { get; set; }
        public virtual DbSet<PimsInsuranceType> PimsInsuranceTypes { get; set; }
        public virtual DbSet<PimsLease> PimsLeases { get; set; }
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
        public virtual DbSet<PimsProject> PimsProjects { get; set; }
        public virtual DbSet<PimsProjectHist> PimsProjectHists { get; set; }
        public virtual DbSet<PimsProjectNote> PimsProjectNotes { get; set; }
        public virtual DbSet<PimsProjectNoteHist> PimsProjectNoteHists { get; set; }
        public virtual DbSet<PimsProjectProperty> PimsProjectProperties { get; set; }
        public virtual DbSet<PimsProjectPropertyHist> PimsProjectPropertyHists { get; set; }
        public virtual DbSet<PimsProjectRiskType> PimsProjectRiskTypes { get; set; }
        public virtual DbSet<PimsProjectStatusType> PimsProjectStatusTypes { get; set; }
        public virtual DbSet<PimsProjectTierType> PimsProjectTierTypes { get; set; }
        public virtual DbSet<PimsProjectType> PimsProjectTypes { get; set; }
        public virtual DbSet<PimsProjectWorkflowModel> PimsProjectWorkflowModels { get; set; }
        public virtual DbSet<PimsProjectWorkflowModelHist> PimsProjectWorkflowModelHists { get; set; }
        public virtual DbSet<PimsProperty> PimsProperties { get; set; }
        public virtual DbSet<PimsPropertyActivity> PimsPropertyActivities { get; set; }
        public virtual DbSet<PimsPropertyActivityHist> PimsPropertyActivityHists { get; set; }
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
        public virtual DbSet<PimsTask> PimsTasks { get; set; }
        public virtual DbSet<PimsTaskHist> PimsTaskHists { get; set; }
        public virtual DbSet<PimsTaskTemplate> PimsTaskTemplates { get; set; }
        public virtual DbSet<PimsTaskTemplateActivityModel> PimsTaskTemplateActivityModels { get; set; }
        public virtual DbSet<PimsTaskTemplateActivityModelHist> PimsTaskTemplateActivityModelHists { get; set; }
        public virtual DbSet<PimsTaskTemplateHist> PimsTaskTemplateHists { get; set; }
        public virtual DbSet<PimsTaskTemplateType> PimsTaskTemplateTypes { get; set; }
        public virtual DbSet<PimsTenant> PimsTenants { get; set; }
        public virtual DbSet<PimsUser> PimsUsers { get; set; }
        public virtual DbSet<PimsUserHist> PimsUserHists { get; set; }
        public virtual DbSet<PimsUserOrganization> PimsUserOrganizations { get; set; }
        public virtual DbSet<PimsUserOrganizationHist> PimsUserOrganizationHists { get; set; }
        public virtual DbSet<PimsUserRole> PimsUserRoles { get; set; }
        public virtual DbSet<PimsUserRoleHist> PimsUserRoleHists { get; set; }
        public virtual DbSet<PimsWorkflowModel> PimsWorkflowModels { get; set; }
        public virtual DbSet<PimsWorkflowModelHist> PimsWorkflowModelHists { get; set; }
        public virtual DbSet<PimsWorkflowModelType> PimsWorkflowModelTypes { get; set; }
        public virtual DbSet<PimsxTableDefinition> PimsxTableDefinitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

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

                entity.HasOne(d => d.AccessRequestStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsAccessRequests)
                    .HasForeignKey(d => d.AccessRequestStatusTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ARQSTT_PIM_ACRQST_FK");

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

            modelBuilder.Entity<PimsActivity>(entity =>
            {
                entity.HasKey(e => e.ActivityId)
                    .HasName("ACTVTY_PK");

                entity.Property(e => e.ActivityId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.ActivityModel)
                    .WithMany(p => p.PimsActivities)
                    .HasForeignKey(d => d.ActivityModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ACTMDL_PIM_ACTVTY_FK");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.PimsActivities)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("PIM_PROJCT_PIM_ACTVTY_FK");

                entity.HasOne(d => d.Workflow)
                    .WithMany(p => p.PimsActivities)
                    .HasForeignKey(d => d.WorkflowId)
                    .HasConstraintName("PIM_PRWKMD_PIM_ACTVTY_FK");
            });

            modelBuilder.Entity<PimsActivityHist>(entity =>
            {
                entity.HasKey(e => e.ActivityHistId)
                    .HasName("PIMS_ACTVTY_H_PK");

                entity.Property(e => e.ActivityHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsActivityModel>(entity =>
            {
                entity.HasKey(e => e.ActivityModelId)
                    .HasName("ACTMDL_PK");

                entity.Property(e => e.ActivityModelId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_MODEL_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsActivityModelHist>(entity =>
            {
                entity.HasKey(e => e.ActivityModelHistId)
                    .HasName("PIMS_ACTMDL_H_PK");

                entity.Property(e => e.ActivityModelHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_MODEL_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
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

                entity.Property(e => e.DataSourceTypeCode).HasComment("Code val;ue of the source system of the data (PAIMS, LIS, etc.)");

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

            modelBuilder.Entity<PimsLease>(entity =>
            {
                entity.HasKey(e => e.LeaseId)
                    .HasName("LEASE_PK");

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

                entity.Property(e => e.HasDigitalFile)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicator that digital file exists");

                entity.Property(e => e.HasDigitalLicense)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicator that digital license exists");

                entity.Property(e => e.HasPhysicalFile)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicator that phyical file exists");

                entity.Property(e => e.HasPhysicialLicense)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicator that physical license exists");

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

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseProgramType>(entity =>
            {
                entity.HasKey(e => e.LeaseProgramTypeCode)
                    .HasName("LSPRGT_PK");

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
                    .WithMany(p => p.PimsLeaseTenants)
                    .HasForeignKey(d => d.PersonId)
                    .HasConstraintName("PIM_PERSON_PIM_TENANT_FK");
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

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ORGIDT_PIM_ORG_FK");

                entity.HasOne(d => d.OrganizationTypeCodeNavigation)
                    .WithMany(p => p.PimsOrganizations)
                    .HasForeignKey(d => d.OrganizationTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

            modelBuilder.Entity<PimsProject>(entity =>
            {
                entity.HasKey(e => e.ProjectId)
                    .HasName("PROJCT_PK");

                entity.Property(e => e.ProjectId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.ProjectRiskTypeCodeNavigation)
                    .WithMany(p => p.PimsProjects)
                    .HasForeignKey(d => d.ProjectRiskTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRJRSK_PIM_PROJCT_FK");

                entity.HasOne(d => d.ProjectStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsProjects)
                    .HasForeignKey(d => d.ProjectStatusTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRJSTY_PIM_PROJCT_FK");

                entity.HasOne(d => d.ProjectTierTypeCodeNavigation)
                    .WithMany(p => p.PimsProjects)
                    .HasForeignKey(d => d.ProjectTierTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PROJTR_PIM_PROJCT_FK");

                entity.HasOne(d => d.ProjectTypeCodeNavigation)
                    .WithMany(p => p.PimsProjects)
                    .HasForeignKey(d => d.ProjectTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRJTYP_PIM_PROJCT_FK");
            });

            modelBuilder.Entity<PimsProjectHist>(entity =>
            {
                entity.HasKey(e => e.ProjectHistId)
                    .HasName("PIMS_PROJCT_H_PK");

                entity.Property(e => e.ProjectHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsProjectNote>(entity =>
            {
                entity.HasKey(e => e.ProjectNoteId)
                    .HasName("PROJNT_PK");

                entity.Property(e => e.ProjectNoteId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_NOTE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.PimsProjectNotes)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PROJCT_PIM_PROJNT_FK");
            });

            modelBuilder.Entity<PimsProjectNoteHist>(entity =>
            {
                entity.HasKey(e => e.ProjectNoteHistId)
                    .HasName("PIMS_PROJNT_H_PK");

                entity.Property(e => e.ProjectNoteHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_NOTE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsProjectProperty>(entity =>
            {
                entity.HasKey(e => e.ProjectPropertyId)
                    .HasName("PRJPRP_PK");

                entity.Property(e => e.ProjectPropertyId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_PROPERTY_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.PimsProjectProperties)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PROJCT_PIM_PRJPRP_FK");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsProjectProperties)
                    .HasForeignKey(d => d.PropertyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPRTY_PIM_PRJPRP_FK");
            });

            modelBuilder.Entity<PimsProjectPropertyHist>(entity =>
            {
                entity.HasKey(e => e.ProjectPropertyHistId)
                    .HasName("PIMS_PRJPRP_H_PK");

                entity.Property(e => e.ProjectPropertyHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_PROPERTY_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsProjectRiskType>(entity =>
            {
                entity.HasKey(e => e.ProjectRiskTypeCode)
                    .HasName("PRJRSK_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsProjectStatusType>(entity =>
            {
                entity.HasKey(e => e.ProjectStatusTypeCode)
                    .HasName("PRJSTY_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.IsMilestone).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.IsTerminal).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsProjectTierType>(entity =>
            {
                entity.HasKey(e => e.ProjectTierTypeCode)
                    .HasName("PROJTR_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsProjectType>(entity =>
            {
                entity.HasKey(e => e.ProjectTypeCode)
                    .HasName("PRJTYP_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsProjectWorkflowModel>(entity =>
            {
                entity.HasKey(e => e.ProjectWorkflowModelId)
                    .HasName("PRWKMD_PK");

                entity.Property(e => e.ProjectWorkflowModelId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_WORKFLOW_MODEL_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.PimsProjectWorkflowModels)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PROJCT_PIM_PRWKMD_FK");

                entity.HasOne(d => d.WorkflowModel)
                    .WithMany(p => p.PimsProjectWorkflowModels)
                    .HasForeignKey(d => d.WorkflowModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_WFLMDL_PIM_PRWKMD_FK");
            });

            modelBuilder.Entity<PimsProjectWorkflowModelHist>(entity =>
            {
                entity.HasKey(e => e.ProjectWorkflowModelHistId)
                    .HasName("PIMS_PRWKMD_H_PK");

                entity.Property(e => e.ProjectWorkflowModelHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_WORKFLOW_MODEL_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsProperty>(entity =>
            {
                entity.HasKey(e => e.PropertyId)
                    .HasName("PRPRTY_PK");

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

                entity.Property(e => e.IsOwned)
                    .HasDefaultValueSql("(CONVERT([bit],(1)))")
                    .HasComment("Is the property currently owned?");

                entity.Property(e => e.IsPropertyOfInterest)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is this a property of interest to the Ministry?");

                entity.Property(e => e.IsSensitive)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is this a sensitive property?");

                entity.Property(e => e.IsVisibleToOtherAgencies)
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is the property visible to other agencies?");

                entity.Property(e => e.LandArea).HasComment("Area occupied by property");

                entity.Property(e => e.LandLegalDescription).HasComment("Legal description of property");

                entity.Property(e => e.Location).HasComment("Geospatial location (pin) of property");

                entity.Property(e => e.Name).HasComment("Property name");

                entity.Property(e => e.Pid).HasComment("Property ID");

                entity.Property(e => e.Pin).HasComment("Property number");

                entity.Property(e => e.PropertyDataSourceEffectiveDate).HasComment("Date the property was officially registered");

                entity.Property(e => e.SurplusDeclarationComment).HasComment("Comment regarding the surplus declaration");

                entity.Property(e => e.SurplusDeclarationDate).HasComment("Date the property was declared surplus");

                entity.Property(e => e.Zoning).HasComment("Current property zoning");

                entity.Property(e => e.ZoningPotential).HasComment("Potential property zoning");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ADDRSS_PIM_PRPRTY_FK");

                entity.HasOne(d => d.DistrictCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.DistrictCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_DSTRCT_PIM_PRPRTY_FK");

                entity.HasOne(d => d.PropMgmtOrg)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.PropMgmtOrgId)
                    .HasConstraintName("PIM_ORG_PIM_PRPRTY_FK");

                entity.HasOne(d => d.PropertyAreaUnitTypeCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.PropertyAreaUnitTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

                entity.HasOne(d => d.PropertyTenureTypeCodeNavigation)
                    .WithMany(p => p.PimsProperties)
                    .HasForeignKey(d => d.PropertyTenureTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PRPTNR_PIM_PRPRTY_FK");

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
            });

            modelBuilder.Entity<PimsPropertyActivity>(entity =>
            {
                entity.HasKey(e => e.PropertyActivityId)
                    .HasName("PRPACT_PK");

                entity.Property(e => e.PropertyActivityId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACTIVITY_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.PimsPropertyActivities)
                    .HasForeignKey(d => d.ActivityId)
                    .HasConstraintName("PIM_ACTVTY_PIM_PRPACT_FK");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.PimsPropertyActivities)
                    .HasForeignKey(d => d.PropertyId)
                    .HasConstraintName("PIM_PRPRTY_PIM_PRPACT_FK");
            });

            modelBuilder.Entity<PimsPropertyActivityHist>(entity =>
            {
                entity.HasKey(e => e.PropertyActivityHistId)
                    .HasName("PIMS_PRPACT_H_PK");

                entity.Property(e => e.PropertyActivityHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACTIVITY_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
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

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
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

                entity.Property(e => e.SecurityDepositId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_ID_SEQ])");

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
                    .WithMany(p => p.PimsSecurityDepositHolders)
                    .HasForeignKey(d => d.SecurityDepositId)
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

                entity.Property(e => e.PayeeAddress).HasComment("Address of cheque recipient");

                entity.Property(e => e.PayeeName).HasComment("Name of cheque recipient");

                entity.Property(e => e.ReturnAmount).HasComment("Amount returned minus claims");

                entity.Property(e => e.ReturnDate).HasComment("Date of deposit return");

                entity.Property(e => e.TerminationDate).HasComment("Date the lease/license was terminated or surrendered");

                entity.HasOne(d => d.Lease)
                    .WithMany(p => p.PimsSecurityDepositReturns)
                    .HasForeignKey(d => d.LeaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LEASE_PIM_SDRTRN_FK");

                entity.HasOne(d => d.SecurityDeposit)
                    .WithMany(p => p.PimsSecurityDepositReturns)
                    .HasForeignKey(d => d.SecurityDepositId)
                    .HasConstraintName("PIM_SECDEP_PIM_SDRTRN_FK");

                entity.HasOne(d => d.SecurityDepositTypeCodeNavigation)
                    .WithMany(p => p.PimsSecurityDepositReturns)
                    .HasForeignKey(d => d.SecurityDepositTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_SECDPT_PIM_SDRTRN_FK");
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
                    .WithMany(p => p.PimsSecurityDepositReturnHolders)
                    .HasForeignKey(d => d.SecurityDepositReturnId)
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

            modelBuilder.Entity<PimsTask>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("TASK_PK");

                entity.Property(e => e.TaskId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TASK_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.PimsTasks)
                    .HasForeignKey(d => d.ActivityId)
                    .HasConstraintName("PIM_ACTVTY_PIM_TASK_FK");

                entity.HasOne(d => d.TaskTemplate)
                    .WithMany(p => p.PimsTasks)
                    .HasForeignKey(d => d.TaskTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_TSKTMP_PIM_TASK_FK");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PimsTasks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_USER_PIM_TASK_FK");
            });

            modelBuilder.Entity<PimsTaskHist>(entity =>
            {
                entity.HasKey(e => e.TaskHistId)
                    .HasName("PIMS_TASK_H_PK");

                entity.Property(e => e.TaskHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TASK_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsTaskTemplate>(entity =>
            {
                entity.HasKey(e => e.TaskTemplateId)
                    .HasName("TSKTMP_PK");

                entity.Property(e => e.TaskTemplateId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TASK_TEMPLATE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.TaskTemplateTypeCodeNavigation)
                    .WithMany(p => p.PimsTaskTemplates)
                    .HasForeignKey(d => d.TaskTemplateTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_TSKTMT_PIM_TSKTMP_FK");
            });

            modelBuilder.Entity<PimsTaskTemplateActivityModel>(entity =>
            {
                entity.HasKey(e => e.TaskTemplateActivityModelId)
                    .HasName("TSKTAM_PK");

                entity.Property(e => e.TaskTemplateActivityModelId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TASK_TEMPLATE_ACTIVITY_MODEL_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.IsMandatory).HasDefaultValueSql("(CONVERT([bit],(1)))");

                entity.HasOne(d => d.ActivityModel)
                    .WithMany(p => p.PimsTaskTemplateActivityModels)
                    .HasForeignKey(d => d.ActivityModelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ACTMDL_PIM_TSKTAM_FK");

                entity.HasOne(d => d.TaskTemplate)
                    .WithMany(p => p.PimsTaskTemplateActivityModels)
                    .HasForeignKey(d => d.TaskTemplateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_TSKTMP_PIM_TSKTAM_FK");
            });

            modelBuilder.Entity<PimsTaskTemplateActivityModelHist>(entity =>
            {
                entity.HasKey(e => e.TaskTemplateActivityModelHistId)
                    .HasName("PIMS_TSKTAM_H_PK");

                entity.Property(e => e.TaskTemplateActivityModelHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TASK_TEMPLATE_ACTIVITY_MODEL_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsTaskTemplateHist>(entity =>
            {
                entity.HasKey(e => e.TaskTemplateHistId)
                    .HasName("PIMS_TSKTMP_H_PK");

                entity.Property(e => e.TaskTemplateHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TASK_TEMPLATE_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsTaskTemplateType>(entity =>
            {
                entity.HasKey(e => e.TaskTemplateTypeCode)
                    .HasName("TSKTMT_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
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

            modelBuilder.Entity<PimsUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("USER_PK");

                entity.Property(e => e.UserId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

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

                entity.Property(e => e.UserOrganizationId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ORGANIZATION_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

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

                entity.Property(e => e.UserRoleId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ROLE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

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

            modelBuilder.Entity<PimsWorkflowModel>(entity =>
            {
                entity.HasKey(e => e.WorkflowModelId)
                    .HasName("WFLMDL_PK");

                entity.Property(e => e.WorkflowModelId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_WORKFLOW_MODEL_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.WorkflowModelTypeCodeNavigation)
                    .WithMany(p => p.PimsWorkflowModels)
                    .HasForeignKey(d => d.WorkflowModelTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_WFLMDT_PIM_WFLMDL_FK");
            });

            modelBuilder.Entity<PimsWorkflowModelHist>(entity =>
            {
                entity.HasKey(e => e.WorkflowModelHistId)
                    .HasName("PIMS_WFLMDL_H_PK");

                entity.Property(e => e.WorkflowModelHistId).HasDefaultValueSql("(NEXT VALUE FOR [PIMS_WORKFLOW_MODEL_H_ID_SEQ])");

                entity.Property(e => e.EffectiveDateHist).HasDefaultValueSql("(getutcdate())");
            });

            modelBuilder.Entity<PimsWorkflowModelType>(entity =>
            {
                entity.HasKey(e => e.WorkflowModelTypeCode)
                    .HasName("WFLMDT_PK");

                entity.Property(e => e.ConcurrencyControlNumber).HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp).HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid).HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled).HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

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

            modelBuilder.HasSequence("PIMS_ACTIVITY_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_ACTIVITY_ID_SEQ")
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

            modelBuilder.HasSequence("PIMS_PROJECT_WORKFLOW_MODEL_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_PROJECT_WORKFLOW_MODEL_ID_SEQ")
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

            modelBuilder.HasSequence("PIMS_WORKFLOW_MODEL_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_WORKFLOW_MODEL_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
