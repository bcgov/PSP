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
        public virtual DbSet<PimsInsurancePayeeType> PimsInsurancePayeeTypes { get; set; }
        public virtual DbSet<PimsInsuranceType> PimsInsuranceTypes { get; set; }
        public virtual DbSet<PimsLease> PimsLeases { get; set; }
        public virtual DbSet<PimsLeaseCategoryType> PimsLeaseCategoryTypes { get; set; }
        public virtual DbSet<PimsLeaseHist> PimsLeaseHists { get; set; }
        public virtual DbSet<PimsLeaseInitiatorType> PimsLeaseInitiatorTypes { get; set; }
        public virtual DbSet<PimsLeaseLicenseType> PimsLeaseLicenseTypes { get; set; }
        public virtual DbSet<PimsLeasePayRvblType> PimsLeasePayRvblTypes { get; set; }
        public virtual DbSet<PimsLeasePayment> PimsLeasePayments { get; set; }
        public virtual DbSet<PimsLeasePaymentForecast> PimsLeasePaymentForecasts { get; set; }
        public virtual DbSet<PimsLeasePaymentForecastHist> PimsLeasePaymentForecastHists { get; set; }
        public virtual DbSet<PimsLeasePaymentHist> PimsLeasePaymentHists { get; set; }
        public virtual DbSet<PimsLeasePaymentMethodType> PimsLeasePaymentMethodTypes { get; set; }
        public virtual DbSet<PimsLeasePaymentPeriod> PimsLeasePaymentPeriods { get; set; }
        public virtual DbSet<PimsLeasePaymentPeriodHist> PimsLeasePaymentPeriodHists { get; set; }
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
        public virtual DbSet<PimsSecDepHolderType> PimsSecDepHolderTypes { get; set; }
        public virtual DbSet<PimsSecurityDeposit> PimsSecurityDeposits { get; set; }
        public virtual DbSet<PimsSecurityDepositHist> PimsSecurityDepositHists { get; set; }
        public virtual DbSet<PimsSecurityDepositReturn> PimsSecurityDepositReturns { get; set; }
        public virtual DbSet<PimsSecurityDepositReturnHist> PimsSecurityDepositReturnHists { get; set; }
        public virtual DbSet<PimsSecurityDepositType> PimsSecurityDepositTypes { get; set; }
        public virtual DbSet<PimsStaticVariable> PimsStaticVariables { get; set; }
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

                entity.ToTable("PIMS_ACCESS_REQUEST");

                entity.HasIndex(e => e.AccessRequestStatusTypeCode, "ACRQST_ACCESS_REQUEST_STATUS_TYPE_CODE_IDX");

                entity.HasIndex(e => e.RoleId, "ACRQST_ROLE_ID_IDX");

                entity.HasIndex(e => e.UserId, "ACRQST_USER_ID_IDX");

                entity.Property(e => e.AccessRequestId)
                    .HasColumnName("ACCESS_REQUEST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACCESS_REQUEST_ID_SEQ])");

                entity.Property(e => e.AccessRequestStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ACCESS_REQUEST_STATUS_TYPE_CODE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Note).HasColumnName("NOTE");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

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

                entity.ToTable("PIMS_ACCESS_REQUEST_HIST");

                entity.HasIndex(e => new { e.AccessRequestHistId, e.EndDateHist }, "PIMS_ACRQST_H_UK")
                    .IsUnique();

                entity.Property(e => e.AccessRequestHistId)
                    .HasColumnName("_ACCESS_REQUEST_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACCESS_REQUEST_H_ID_SEQ])");

                entity.Property(e => e.AccessRequestId).HasColumnName("ACCESS_REQUEST_ID");

                entity.Property(e => e.AccessRequestStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ACCESS_REQUEST_STATUS_TYPE_CODE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");
            });

            modelBuilder.Entity<PimsAccessRequestOrganization>(entity =>
            {
                entity.HasKey(e => e.AccessRequestOrganizationId)
                    .HasName("ACRQOR_PK");

                entity.ToTable("PIMS_ACCESS_REQUEST_ORGANIZATION");

                entity.HasIndex(e => e.AccessRequestId, "ACRQOR_ACCESS_REQUEST_ID_IDX");

                entity.HasIndex(e => new { e.OrganizationId, e.AccessRequestId }, "ACRQOR_ORGANIZATION_ACCESS_REQUEST_TUC")
                    .IsUnique();

                entity.HasIndex(e => e.OrganizationId, "ACRQOR_ORGANIZATION_ID_IDX");

                entity.Property(e => e.AccessRequestOrganizationId)
                    .HasColumnName("ACCESS_REQUEST_ORGANIZATION_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACCESS_REQUEST_ORGANIZATION_ID_SEQ])");

                entity.Property(e => e.AccessRequestId).HasColumnName("ACCESS_REQUEST_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

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

                entity.ToTable("PIMS_ACCESS_REQUEST_ORGANIZATION_HIST");

                entity.HasIndex(e => new { e.AccessRequestOrganizationHistId, e.EndDateHist }, "PIMS_ACRQOR_H_UK")
                    .IsUnique();

                entity.Property(e => e.AccessRequestOrganizationHistId)
                    .HasColumnName("_ACCESS_REQUEST_ORGANIZATION_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACCESS_REQUEST_ORGANIZATION_H_ID_SEQ])");

                entity.Property(e => e.AccessRequestId).HasColumnName("ACCESS_REQUEST_ID");

                entity.Property(e => e.AccessRequestOrganizationId).HasColumnName("ACCESS_REQUEST_ORGANIZATION_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");
            });

            modelBuilder.Entity<PimsAccessRequestStatusType>(entity =>
            {
                entity.HasKey(e => e.AccessRequestStatusTypeCode)
                    .HasName("ARQSTT_PK");

                entity.ToTable("PIMS_ACCESS_REQUEST_STATUS_TYPE");

                entity.Property(e => e.AccessRequestStatusTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("ACCESS_REQUEST_STATUS_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsActivity>(entity =>
            {
                entity.HasKey(e => e.ActivityId)
                    .HasName("ACTVTY_PK");

                entity.ToTable("PIMS_ACTIVITY");

                entity.HasIndex(e => e.ActivityModelId, "ACTVTY_ACTIVITY_MODEL_ID_IDX");

                entity.HasIndex(e => e.ProjectId, "ACTVTY_PROJECT_ID_IDX");

                entity.HasIndex(e => e.WorkflowId, "ACTVTY_WORKFLOW_ID_IDX");

                entity.Property(e => e.ActivityId)
                    .HasColumnName("ACTIVITY_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_ID_SEQ])");

                entity.Property(e => e.ActivityModelId).HasColumnName("ACTIVITY_MODEL_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.WorkflowId).HasColumnName("WORKFLOW_ID");

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

                entity.ToTable("PIMS_ACTIVITY_HIST");

                entity.HasIndex(e => new { e.ActivityHistId, e.EndDateHist }, "PIMS_ACTVTY_H_UK")
                    .IsUnique();

                entity.Property(e => e.ActivityHistId)
                    .HasColumnName("_ACTIVITY_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_H_ID_SEQ])");

                entity.Property(e => e.ActivityId).HasColumnName("ACTIVITY_ID");

                entity.Property(e => e.ActivityModelId).HasColumnName("ACTIVITY_MODEL_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.WorkflowId).HasColumnName("WORKFLOW_ID");
            });

            modelBuilder.Entity<PimsActivityModel>(entity =>
            {
                entity.HasKey(e => e.ActivityModelId)
                    .HasName("ACTMDL_PK");

                entity.ToTable("PIMS_ACTIVITY_MODEL");

                entity.Property(e => e.ActivityModelId)
                    .HasColumnName("ACTIVITY_MODEL_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_MODEL_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsActivityModelHist>(entity =>
            {
                entity.HasKey(e => e.ActivityModelHistId)
                    .HasName("PIMS_ACTMDL_H_PK");

                entity.ToTable("PIMS_ACTIVITY_MODEL_HIST");

                entity.HasIndex(e => new { e.ActivityModelHistId, e.EndDateHist }, "PIMS_ACTMDL_H_UK")
                    .IsUnique();

                entity.Property(e => e.ActivityModelHistId)
                    .HasColumnName("_ACTIVITY_MODEL_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ACTIVITY_MODEL_H_ID_SEQ])");

                entity.Property(e => e.ActivityModelId).HasColumnName("ACTIVITY_MODEL_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");
            });

            modelBuilder.Entity<PimsAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId)
                    .HasName("ADDRSS_PK");

                entity.ToTable("PIMS_ADDRESS");

                entity.HasIndex(e => e.CountryId, "ADDRSS_COUNTRY_ID_IDX");

                entity.HasIndex(e => e.DistrictCode, "ADDRSS_DISTRICT_CODE_IDX");

                entity.HasIndex(e => e.ProvinceStateId, "ADDRSS_PROVINCE_STATE_ID_IDX");

                entity.HasIndex(e => e.RegionCode, "ADDRSS_REGION_CODE_IDX");

                entity.Property(e => e.AddressId)
                    .HasColumnName("ADDRESS_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ADDRESS_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2000)
                    .HasColumnName("COMMENT");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CountryId).HasColumnName("COUNTRY_ID");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DistrictCode).HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.Latitude)
                    .HasColumnType("numeric(8, 6)")
                    .HasColumnName("LATITUDE");

                entity.Property(e => e.Longitude)
                    .HasColumnType("numeric(9, 6)")
                    .HasColumnName("LONGITUDE");

                entity.Property(e => e.MunicipalityName)
                    .HasMaxLength(200)
                    .HasColumnName("MUNICIPALITY_NAME");

                entity.Property(e => e.OtherCountry)
                    .HasMaxLength(200)
                    .HasColumnName("OTHER_COUNTRY")
                    .HasComment("Other country not listed in drop-down list");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(20)
                    .HasColumnName("POSTAL_CODE");

                entity.Property(e => e.ProvinceStateId).HasColumnName("PROVINCE_STATE_ID");

                entity.Property(e => e.RegionCode).HasColumnName("REGION_CODE");

                entity.Property(e => e.StreetAddress1)
                    .HasMaxLength(200)
                    .HasColumnName("STREET_ADDRESS_1");

                entity.Property(e => e.StreetAddress2)
                    .HasMaxLength(200)
                    .HasColumnName("STREET_ADDRESS_2");

                entity.Property(e => e.StreetAddress3)
                    .HasMaxLength(200)
                    .HasColumnName("STREET_ADDRESS_3");

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

                entity.ToTable("PIMS_ADDRESS_HIST");

                entity.HasIndex(e => new { e.AddressHistId, e.EndDateHist }, "PIMS_ADDRSS_H_UK")
                    .IsUnique();

                entity.Property(e => e.AddressHistId)
                    .HasColumnName("_ADDRESS_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ADDRESS_H_ID_SEQ])");

                entity.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2000)
                    .HasColumnName("COMMENT");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.CountryId).HasColumnName("COUNTRY_ID");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.DistrictCode).HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.Latitude)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LATITUDE");

                entity.Property(e => e.Longitude)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("LONGITUDE");

                entity.Property(e => e.MunicipalityName)
                    .HasMaxLength(200)
                    .HasColumnName("MUNICIPALITY_NAME");

                entity.Property(e => e.OtherCountry)
                    .HasMaxLength(200)
                    .HasColumnName("OTHER_COUNTRY");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(20)
                    .HasColumnName("POSTAL_CODE");

                entity.Property(e => e.ProvinceStateId).HasColumnName("PROVINCE_STATE_ID");

                entity.Property(e => e.RegionCode).HasColumnName("REGION_CODE");

                entity.Property(e => e.StreetAddress1)
                    .HasMaxLength(200)
                    .HasColumnName("STREET_ADDRESS_1");

                entity.Property(e => e.StreetAddress2)
                    .HasMaxLength(200)
                    .HasColumnName("STREET_ADDRESS_2");

                entity.Property(e => e.StreetAddress3)
                    .HasMaxLength(200)
                    .HasColumnName("STREET_ADDRESS_3");
            });

            modelBuilder.Entity<PimsAddressUsageType>(entity =>
            {
                entity.HasKey(e => e.AddressUsageTypeCode)
                    .HasName("ADUSGT_PK");

                entity.ToTable("PIMS_ADDRESS_USAGE_TYPE");

                entity.Property(e => e.AddressUsageTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("ADDRESS_USAGE_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsAreaUnitType>(entity =>
            {
                entity.HasKey(e => e.AreaUnitTypeCode)
                    .HasName("ARUNIT_PK");

                entity.ToTable("PIMS_AREA_UNIT_TYPE");

                entity.HasComment("The area unit used for measuring Properties.  The units must be in metric: square metres or hectares.");

                entity.Property(e => e.AreaUnitTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("AREA_UNIT_TYPE_CODE")
                    .HasComment("The area unit used for measuring Properties.  The units must be in metric: square metres or hectares.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION")
                    .HasComment("Translation of the code value into a description that can be displayed to the user.");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("DISPLAY_ORDER")
                    .HasComment("Order in which to display the code values, if required.");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code value is still active or is now disabled.");
            });

            modelBuilder.Entity<PimsClaim>(entity =>
            {
                entity.HasKey(e => e.ClaimId)
                    .HasName("CLMTYP_PK");

                entity.ToTable("PIMS_CLAIM");

                entity.HasIndex(e => e.ClaimUid, "CLMTYP_CLAIM_UID_IDX");

                entity.HasIndex(e => e.KeycloakRoleId, "CLMTYP_KEYCLOAK_ROLE_ID_IDX");

                entity.Property(e => e.ClaimId)
                    .HasColumnName("CLAIM_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CLAIM_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ClaimUid).HasColumnName("CLAIM_UID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.KeycloakRoleId).HasColumnName("KEYCLOAK_ROLE_ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<PimsClaimHist>(entity =>
            {
                entity.HasKey(e => e.ClaimHistId)
                    .HasName("PIMS_CLMTYP_H_PK");

                entity.ToTable("PIMS_CLAIM_HIST");

                entity.HasIndex(e => new { e.ClaimHistId, e.EndDateHist }, "PIMS_CLMTYP_H_UK")
                    .IsUnique();

                entity.Property(e => e.ClaimHistId)
                    .HasColumnName("_CLAIM_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CLAIM_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ClaimId).HasColumnName("CLAIM_ID");

                entity.Property(e => e.ClaimUid).HasColumnName("CLAIM_UID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.KeycloakRoleId).HasColumnName("KEYCLOAK_ROLE_ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("NAME");
            });

            modelBuilder.Entity<PimsContactMethod>(entity =>
            {
                entity.HasKey(e => e.ContactMethodId)
                    .HasName("CNTMTH_PK");

                entity.ToTable("PIMS_CONTACT_METHOD");

                entity.HasIndex(e => e.ContactMethodTypeCode, "CNTMTH_CONTACT_METHOD_TYPE_CODE_IDX");

                entity.HasIndex(e => e.OrganizationId, "CNTMTH_ORGANIZATION_ID_IDX");

                entity.HasIndex(e => e.PersonId, "CNTMTH_PERSON_ID_IDX");

                entity.Property(e => e.ContactMethodId)
                    .HasColumnName("CONTACT_METHOD_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CONTACT_METHOD_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ContactMethodTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("CONTACT_METHOD_TYPE_CODE");

                entity.Property(e => e.ContactMethodValue)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("CONTACT_METHOD_VALUE");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsPreferredMethod)
                    .HasColumnName("IS_PREFERRED_METHOD")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");

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

                entity.ToTable("PIMS_CONTACT_METHOD_HIST");

                entity.HasIndex(e => new { e.ContactMethodHistId, e.EndDateHist }, "PIMS_CNTMTH_H_UK")
                    .IsUnique();

                entity.Property(e => e.ContactMethodHistId)
                    .HasColumnName("_CONTACT_METHOD_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_CONTACT_METHOD_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.ContactMethodId).HasColumnName("CONTACT_METHOD_ID");

                entity.Property(e => e.ContactMethodTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("CONTACT_METHOD_TYPE_CODE");

                entity.Property(e => e.ContactMethodValue)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("CONTACT_METHOD_VALUE");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsPreferredMethod).HasColumnName("IS_PREFERRED_METHOD");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");
            });

            modelBuilder.Entity<PimsContactMethodType>(entity =>
            {
                entity.HasKey(e => e.ContactMethodTypeCode)
                    .HasName("CNTMTT_PK");

                entity.ToTable("PIMS_CONTACT_METHOD_TYPE");

                entity.Property(e => e.ContactMethodTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("CONTACT_METHOD_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsContactMgrVw>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PIMS_CONTACT_MGR_VW");

                entity.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("FIRST_NAME");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.MailingAddress)
                    .HasMaxLength(200)
                    .HasColumnName("MAILING_ADDRESS");

                entity.Property(e => e.MiddleNames)
                    .HasMaxLength(200)
                    .HasColumnName("MIDDLE_NAMES");

                entity.Property(e => e.MunicipalityName)
                    .HasMaxLength(200)
                    .HasColumnName("MUNICIPALITY_NAME");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

                entity.Property(e => e.OrganizationName)
                    .HasMaxLength(200)
                    .HasColumnName("ORGANIZATION_NAME");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");

                entity.Property(e => e.ProvinceState)
                    .HasMaxLength(20)
                    .HasColumnName("PROVINCE_STATE");

                entity.Property(e => e.Summary)
                    .HasMaxLength(302)
                    .HasColumnName("SUMMARY");

                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .HasColumnName("SURNAME");
            });

            modelBuilder.Entity<PimsCountry>(entity =>
            {
                entity.HasKey(e => e.CountryId)
                    .HasName("CNTRY_PK");

                entity.ToTable("PIMS_COUNTRY");

                entity.Property(e => e.CountryId)
                    .ValueGeneratedNever()
                    .HasColumnName("COUNTRY_ID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("COUNTRY_CODE");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");
            });

            modelBuilder.Entity<PimsDataSourceType>(entity =>
            {
                entity.HasKey(e => e.DataSourceTypeCode)
                    .HasName("PIDSRT_PK");

                entity.ToTable("PIMS_DATA_SOURCE_TYPE");

                entity.HasComment("Describes the source system of the data (PAIMS, LIS, etc.)");

                entity.Property(e => e.DataSourceTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("DATA_SOURCE_TYPE_CODE")
                    .HasComment("Code val;ue of the source system of the data (PAIMS, LIS, etc.)");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION")
                    .HasComment("Description of the source system of the data (PAIMS, LIS, etc.)");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("DISPLAY_ORDER")
                    .HasComment("Defines the default display order of the descriptions");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is still in use");
            });

            modelBuilder.Entity<PimsDistrict>(entity =>
            {
                entity.HasKey(e => e.DistrictCode)
                    .HasName("DSTRCT_PK");

                entity.ToTable("PIMS_DISTRICT");

                entity.HasIndex(e => e.RegionCode, "DSTRCT_REGION_CODE_IDX");

                entity.Property(e => e.DistrictCode)
                    .ValueGeneratedNever()
                    .HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.DistrictName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DISTRICT_NAME");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.RegionCode).HasColumnName("REGION_CODE");

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

                entity.ToTable("PIMS_INSURANCE");

                entity.HasIndex(e => e.BctfaRiskMgmtContactId, "INSRNC_BCTFA_RISK_MGMT_CONTACT_ID_IDX");

                entity.HasIndex(e => e.InsurancePayeeTypeCode, "INSRNC_INSURANCE_PAYEE_TYPE_CODE_IDX");

                entity.HasIndex(e => e.InsuranceTypeCode, "INSRNC_INSURANCE_TYPE_CODE_IDX");

                entity.HasIndex(e => e.InsurerContactId, "INSRNC_INSURER_CONTACT_ID_IDX");

                entity.HasIndex(e => e.InsurerOrgId, "INSRNC_INSURER_ORG_ID_IDX");

                entity.HasIndex(e => e.LeaseId, "INSRNC_LEASE_ID_IDX");

                entity.HasIndex(e => e.MotiRiskMgmtContactId, "INSRNC_MOTI_RISK_MGMT_CONTACT_ID_IDX");

                entity.Property(e => e.InsuranceId)
                    .HasColumnName("INSURANCE_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_INSURANCE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.BctfaRiskMgmtContactId).HasColumnName("BCTFA_RISK_MGMT_CONTACT_ID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CoverageDescription)
                    .HasMaxLength(2000)
                    .HasColumnName("COVERAGE_DESCRIPTION");

                entity.Property(e => e.CoverageLimit)
                    .HasColumnType("money")
                    .HasColumnName("COVERAGE_LIMIT")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("date")
                    .HasColumnName("EXPIRY_DATE");

                entity.Property(e => e.InsurancePayeeTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("INSURANCE_PAYEE_TYPE_CODE");

                entity.Property(e => e.InsuranceTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("INSURANCE_TYPE_CODE");

                entity.Property(e => e.InsuredValue)
                    .HasColumnType("money")
                    .HasColumnName("INSURED_VALUE");

                entity.Property(e => e.InsurerContactId).HasColumnName("INSURER_CONTACT_ID");

                entity.Property(e => e.InsurerOrgId).HasColumnName("INSURER_ORG_ID");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.MotiRiskMgmtContactId).HasColumnName("MOTI_RISK_MGMT_CONTACT_ID");

                entity.Property(e => e.OtherInsuranceType)
                    .HasMaxLength(200)
                    .HasColumnName("OTHER_INSURANCE_TYPE");

                entity.Property(e => e.RiskAssessmentCompletedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("RISK ASSESSMENT_COMPLETED_DATE");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("START_DATE");

                entity.HasOne(d => d.BctfaRiskMgmtContact)
                    .WithMany(p => p.PimsInsuranceBctfaRiskMgmtContacts)
                    .HasForeignKey(d => d.BctfaRiskMgmtContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PERSON_PIM_INSRNC_BCTFA_CONTACT_FK");

                entity.HasOne(d => d.InsurancePayeeTypeCodeNavigation)
                    .WithMany(p => p.PimsInsurances)
                    .HasForeignKey(d => d.InsurancePayeeTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_INSPAY_PIM_INSRNC_FK");

                entity.HasOne(d => d.InsuranceTypeCodeNavigation)
                    .WithMany(p => p.PimsInsurances)
                    .HasForeignKey(d => d.InsuranceTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_INSPYT_PIM_INSRNC_FK");

                entity.HasOne(d => d.InsurerContact)
                    .WithMany(p => p.PimsInsuranceInsurerContacts)
                    .HasForeignKey(d => d.InsurerContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PERSON_PIM_INSRNC_INSURER_CONTACT_FK");

                entity.HasOne(d => d.InsurerOrg)
                    .WithMany(p => p.PimsInsurances)
                    .HasForeignKey(d => d.InsurerOrgId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_ORG_PIM_INSRNC_FK");

                entity.HasOne(d => d.Lease)
                    .WithMany(p => p.PimsInsurances)
                    .HasForeignKey(d => d.LeaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LEASE_PIM_INSRNC_FK");

                entity.HasOne(d => d.MotiRiskMgmtContact)
                    .WithMany(p => p.PimsInsuranceMotiRiskMgmtContacts)
                    .HasForeignKey(d => d.MotiRiskMgmtContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_PERSON_PIM_INSRNCMOTI_CONTACT_FK");
            });

            modelBuilder.Entity<PimsInsuranceHist>(entity =>
            {
                entity.HasKey(e => e.InsuranceHistId)
                    .HasName("PIMS_INSRNC_H_PK");

                entity.ToTable("PIMS_INSURANCE_HIST");

                entity.HasIndex(e => new { e.InsuranceHistId, e.EndDateHist }, "PIMS_INSRNC_H_UK")
                    .IsUnique();

                entity.Property(e => e.InsuranceHistId)
                    .HasColumnName("_INSURANCE_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_INSURANCE_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.BctfaRiskMgmtContactId).HasColumnName("BCTFA_RISK_MGMT_CONTACT_ID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.CoverageDescription)
                    .HasMaxLength(2000)
                    .HasColumnName("COVERAGE_DESCRIPTION");

                entity.Property(e => e.CoverageLimit)
                    .HasColumnType("money")
                    .HasColumnName("COVERAGE_LIMIT");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("date")
                    .HasColumnName("EXPIRY_DATE");

                entity.Property(e => e.InsuranceId).HasColumnName("INSURANCE_ID");

                entity.Property(e => e.InsurancePayeeTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("INSURANCE_PAYEE_TYPE_CODE");

                entity.Property(e => e.InsuranceTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("INSURANCE_TYPE_CODE");

                entity.Property(e => e.InsuredValue)
                    .HasColumnType("money")
                    .HasColumnName("INSURED_VALUE");

                entity.Property(e => e.InsurerContactId).HasColumnName("INSURER_CONTACT_ID");

                entity.Property(e => e.InsurerOrgId).HasColumnName("INSURER_ORG_ID");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.MotiRiskMgmtContactId).HasColumnName("MOTI_RISK_MGMT_CONTACT_ID");

                entity.Property(e => e.OtherInsuranceType)
                    .HasMaxLength(200)
                    .HasColumnName("OTHER_INSURANCE_TYPE");

                entity.Property(e => e.RiskAssessmentCompletedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("RISK ASSESSMENT_COMPLETED_DATE");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("START_DATE");
            });

            modelBuilder.Entity<PimsInsurancePayeeType>(entity =>
            {
                entity.HasKey(e => e.InsurancePayeeTypeCode)
                    .HasName("INSPAY_PK");

                entity.ToTable("PIMS_INSURANCE_PAYEE_TYPE");

                entity.Property(e => e.InsurancePayeeTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("INSURANCE_PAYEE_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsInsuranceType>(entity =>
            {
                entity.HasKey(e => e.InsuranceTypeCode)
                    .HasName("INSPYT_PK");

                entity.ToTable("PIMS_INSURANCE_TYPE");

                entity.Property(e => e.InsuranceTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("INSURANCE_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLease>(entity =>
            {
                entity.HasKey(e => e.LeaseId)
                    .HasName("LEASE_PK");

                entity.ToTable("PIMS_LEASE");

                entity.HasIndex(e => e.LeaseCategoryTypeCode, "LEASE_LEASE_CATEGORY_TYPE_CODE_IDX");

                entity.HasIndex(e => e.LeaseInitiatorTypeCode, "LEASE_LEASE_INITIATOR_TYPE_CODE_IDX");

                entity.HasIndex(e => e.LeaseLicenseTypeCode, "LEASE_LEASE_LICENSE_TYPE_CODE_IDX");

                entity.HasIndex(e => e.LeasePayRvblTypeCode, "LEASE_LEASE_PAY_RVBL_TYPE_CODE_IDX");

                entity.HasIndex(e => e.LeasePmtFreqTypeCode, "LEASE_LEASE_PMT_FREQ_TYPE_CODE_IDX");

                entity.HasIndex(e => e.LeaseProgramTypeCode, "LEASE_LEASE_PROGRAM_TYPE_CODE_IDX");

                entity.HasIndex(e => e.LeasePurposeTypeCode, "LEASE_LEASE_PURPOSE_TYPE_CODE_IDX");

                entity.HasIndex(e => e.LeaseResponsibilityTypeCode, "LEASE_LEASE_RESPONSIBILITY_TYPE_CODE_IDX");

                entity.HasIndex(e => e.LeaseStatusTypeCode, "LEASE_LEASE_STATUS_TYPE_CODE_IDX");

                entity.HasIndex(e => e.LFileNo, "LEASE_L_FILE_NO_IDX");

                entity.HasIndex(e => e.PsFileNo, "LEASE_PS_FILE_NO_IDX");

                entity.HasIndex(e => e.TfaFileNo, "LEASE_TFA_FILE_NO_IDX");

                entity.Property(e => e.LeaseId)
                    .HasColumnName("LEASE_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DocumentationReference)
                    .HasMaxLength(500)
                    .HasColumnName("DOCUMENTATION_REFERENCE")
                    .HasComment("Location of documents pertianing to the lease/license");

                entity.Property(e => e.HasDigitalFile)
                    .IsRequired()
                    .HasColumnName("HAS_DIGITAL_FILE")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicator that digital file exists");

                entity.Property(e => e.HasDigitalLicense)
                    .IsRequired()
                    .HasColumnName("HAS_DIGITAL_LICENSE")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicator that digital license exists");

                entity.Property(e => e.HasPhysicalFile)
                    .IsRequired()
                    .HasColumnName("HAS_PHYSICAL_FILE")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicator that phyical file exists");

                entity.Property(e => e.HasPhysicialLicense)
                    .IsRequired()
                    .HasColumnName("HAS_PHYSICIAL_LICENSE")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicator that physical license exists");

                entity.Property(e => e.InspectionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("INSPECTION_DATE")
                    .HasComment("Inspection date");

                entity.Property(e => e.InspectionNotes)
                    .HasColumnName("INSPECTION_NOTES")
                    .HasComment("Notes accompanying inspection");

                entity.Property(e => e.IsCommBldg)
                    .HasColumnName("IS_COMM_BLDG")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is a commercial building");

                entity.Property(e => e.IsExpired)
                    .IsRequired()
                    .HasColumnName("IS_EXPIRED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Incidcator that lease/license has expired");

                entity.Property(e => e.IsOtherImprovement)
                    .HasColumnName("IS_OTHER_IMPROVEMENT")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is improvement of another description");

                entity.Property(e => e.IsSubjectToRta)
                    .HasColumnName("IS_SUBJECT_TO_RTA")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is subject the Residential Tenancy Act");

                entity.Property(e => e.LFileNo)
                    .HasMaxLength(50)
                    .HasColumnName("L_FILE_NO")
                    .HasComment("Generated identifying lease/licence number");

                entity.Property(e => e.LeaseAmount)
                    .HasColumnType("money")
                    .HasColumnName("LEASE_AMOUNT")
                    .HasComment("Lease/licence amount");

                entity.Property(e => e.LeaseCategoryOtherDesc)
                    .HasMaxLength(200)
                    .HasColumnName("LEASE_CATEGORY_OTHER_DESC")
                    .HasComment("User-specified lease category description not included in standard set of lease purposes");

                entity.Property(e => e.LeaseCategoryTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_CATEGORY_TYPE_CODE");

                entity.Property(e => e.LeaseDescription)
                    .HasColumnName("LEASE_DESCRIPTION")
                    .HasComment("Manually etered lease description, not the legal description");

                entity.Property(e => e.LeaseInitiatorTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_INITIATOR_TYPE_CODE");

                entity.Property(e => e.LeaseLicenseTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_LICENSE_TYPE_CODE");

                entity.Property(e => e.LeaseNotes)
                    .HasColumnName("LEASE_NOTES")
                    .HasComment("Notes accompanying lease");

                entity.Property(e => e.LeasePayRvblTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PAY_RVBL_TYPE_CODE");

                entity.Property(e => e.LeasePmtFreqTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PMT_FREQ_TYPE_CODE");

                entity.Property(e => e.LeaseProgramTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PROGRAM_TYPE_CODE");

                entity.Property(e => e.LeasePurposeOtherDesc)
                    .HasMaxLength(200)
                    .HasColumnName("LEASE_PURPOSE_OTHER_DESC")
                    .HasComment("User-specified lease purpose description not included in standard set of lease purposes");

                entity.Property(e => e.LeasePurposeTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PURPOSE_TYPE_CODE");

                entity.Property(e => e.LeaseResponsibilityTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_RESPONSIBILITY_TYPE_CODE");

                entity.Property(e => e.LeaseStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_STATUS_TYPE_CODE");

                entity.Property(e => e.MotiContact)
                    .HasMaxLength(200)
                    .HasColumnName("MOTI_CONTACT")
                    .HasComment("Contact of the MoTI person associated with the lease");

                entity.Property(e => e.MotiRegion)
                    .HasMaxLength(200)
                    .HasColumnName("MOTI_REGION")
                    .HasComment("MoTI region associated with the lease");

                entity.Property(e => e.OrigExpiryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ORIG_EXPIRY_DATE")
                    .HasComment("Original expiry date of the lease/license");

                entity.Property(e => e.OrigStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ORIG_START_DATE")
                    .HasComment("Original start date of the lease/license");

                entity.Property(e => e.OtherLeaseLicenseType)
                    .HasMaxLength(200)
                    .HasColumnName("OTHER_LEASE_LICENSE_TYPE")
                    .HasComment("Description of a non-standard lease/license type");

                entity.Property(e => e.OtherLeaseProgramType)
                    .HasMaxLength(200)
                    .HasColumnName("OTHER_LEASE_PROGRAM_TYPE")
                    .HasComment("Description of a non-standard lease program type");

                entity.Property(e => e.OtherLeasePurposeType)
                    .HasMaxLength(200)
                    .HasColumnName("OTHER_LEASE_PURPOSE_TYPE")
                    .HasComment("Description of a non-standard lease purpose type");

                entity.Property(e => e.PsFileNo)
                    .HasMaxLength(50)
                    .HasColumnName("PS_FILE_NO")
                    .HasComment("Sourced from t_fileSubOverrideData.PSFile_No");

                entity.Property(e => e.ResponsibilityEffectiveDate)
                    .HasColumnType("datetime")
                    .HasColumnName("RESPONSIBILITY_EFFECTIVE_DATE")
                    .HasComment("Date current responsibility came into effect for this lease");

                entity.Property(e => e.ReturnNotes)
                    .HasColumnName("RETURN_NOTES")
                    .HasComment("Notes accompanying lease");

                entity.Property(e => e.TfaFileNo)
                    .HasColumnName("TFA_FILE_NO")
                    .HasComment("Sourced from t_fileMain.TFA_File_Number");

                entity.HasOne(d => d.LeaseCategoryTypeCodeNavigation)
                    .WithMany(p => p.PimsLeases)
                    .HasForeignKey(d => d.LeaseCategoryTypeCode)
                    .HasConstraintName("PIM_LSCATT_PIM_LEASE_FK");

                entity.HasOne(d => d.LeaseInitiatorTypeCodeNavigation)
                    .WithMany(p => p.PimsLeases)
                    .HasForeignKey(d => d.LeaseInitiatorTypeCode)
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

                entity.HasOne(d => d.LeasePmtFreqTypeCodeNavigation)
                    .WithMany(p => p.PimsLeases)
                    .HasForeignKey(d => d.LeasePmtFreqTypeCode)
                    .HasConstraintName("PIM_LSPMTF_PIM_LEASE_FK");

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
            });

            modelBuilder.Entity<PimsLeaseCategoryType>(entity =>
            {
                entity.HasKey(e => e.LeaseCategoryTypeCode)
                    .HasName("LSCATT_PK");

                entity.ToTable("PIMS_LEASE_CATEGORY_TYPE");

                entity.Property(e => e.LeaseCategoryTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_CATEGORY_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseHist>(entity =>
            {
                entity.HasKey(e => e.LeaseHistId)
                    .HasName("PIMS_LEASE_H_PK");

                entity.ToTable("PIMS_LEASE_HIST");

                entity.HasIndex(e => new { e.LeaseHistId, e.EndDateHist }, "PIMS_LEASE_H_UK")
                    .IsUnique();

                entity.Property(e => e.LeaseHistId)
                    .HasColumnName("_LEASE_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.DocumentationReference)
                    .HasMaxLength(500)
                    .HasColumnName("DOCUMENTATION_REFERENCE");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.HasDigitalFile).HasColumnName("HAS_DIGITAL_FILE");

                entity.Property(e => e.HasDigitalLicense).HasColumnName("HAS_DIGITAL_LICENSE");

                entity.Property(e => e.HasPhysicalFile).HasColumnName("HAS_PHYSICAL_FILE");

                entity.Property(e => e.HasPhysicialLicense).HasColumnName("HAS_PHYSICIAL_LICENSE");

                entity.Property(e => e.InspectionDate)
                    .HasColumnType("datetime")
                    .HasColumnName("INSPECTION_DATE");

                entity.Property(e => e.IsCommBldg).HasColumnName("IS_COMM_BLDG");

                entity.Property(e => e.IsExpired).HasColumnName("IS_EXPIRED");

                entity.Property(e => e.IsOtherImprovement).HasColumnName("IS_OTHER_IMPROVEMENT");

                entity.Property(e => e.IsSubjectToRta).HasColumnName("IS_SUBJECT_TO_RTA");

                entity.Property(e => e.LFileNo)
                    .HasMaxLength(50)
                    .HasColumnName("L_FILE_NO");

                entity.Property(e => e.LeaseAmount)
                    .HasColumnType("money")
                    .HasColumnName("LEASE_AMOUNT");

                entity.Property(e => e.LeaseCategoryOtherDesc)
                    .HasMaxLength(200)
                    .HasColumnName("LEASE_CATEGORY_OTHER_DESC");

                entity.Property(e => e.LeaseCategoryTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_CATEGORY_TYPE_CODE");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.LeaseInitiatorTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_INITIATOR_TYPE_CODE");

                entity.Property(e => e.LeaseLicenseTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_LICENSE_TYPE_CODE");

                entity.Property(e => e.LeasePayRvblTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PAY_RVBL_TYPE_CODE");

                entity.Property(e => e.LeasePmtFreqTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PMT_FREQ_TYPE_CODE");

                entity.Property(e => e.LeaseProgramTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PROGRAM_TYPE_CODE");

                entity.Property(e => e.LeasePurposeOtherDesc)
                    .HasMaxLength(200)
                    .HasColumnName("LEASE_PURPOSE_OTHER_DESC");

                entity.Property(e => e.LeasePurposeTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PURPOSE_TYPE_CODE");

                entity.Property(e => e.LeaseResponsibilityTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_RESPONSIBILITY_TYPE_CODE");

                entity.Property(e => e.LeaseStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_STATUS_TYPE_CODE");

                entity.Property(e => e.MotiContact)
                    .HasMaxLength(200)
                    .HasColumnName("MOTI_CONTACT");

                entity.Property(e => e.MotiRegion)
                    .HasMaxLength(200)
                    .HasColumnName("MOTI_REGION");

                entity.Property(e => e.OrigExpiryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ORIG_EXPIRY_DATE");

                entity.Property(e => e.OrigStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ORIG_START_DATE");

                entity.Property(e => e.OtherLeaseLicenseType)
                    .HasMaxLength(200)
                    .HasColumnName("OTHER_LEASE_LICENSE_TYPE");

                entity.Property(e => e.OtherLeaseProgramType)
                    .HasMaxLength(200)
                    .HasColumnName("OTHER_LEASE_PROGRAM_TYPE");

                entity.Property(e => e.OtherLeasePurposeType)
                    .HasMaxLength(200)
                    .HasColumnName("OTHER_LEASE_PURPOSE_TYPE");

                entity.Property(e => e.PsFileNo)
                    .HasMaxLength(50)
                    .HasColumnName("PS_FILE_NO");

                entity.Property(e => e.ResponsibilityEffectiveDate)
                    .HasColumnType("datetime")
                    .HasColumnName("RESPONSIBILITY_EFFECTIVE_DATE");

                entity.Property(e => e.TfaFileNo).HasColumnName("TFA_FILE_NO");
            });

            modelBuilder.Entity<PimsLeaseInitiatorType>(entity =>
            {
                entity.HasKey(e => e.LeaseInitiatorTypeCode)
                    .HasName("LINITT_PK");

                entity.ToTable("PIMS_LEASE_INITIATOR_TYPE");

                entity.HasComment("Describes the initiator of the lease");

                entity.Property(e => e.LeaseInitiatorTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_INITIATOR_TYPE_CODE")
                    .HasComment("Code value of the initiator of the lease");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION")
                    .HasComment("Description of the initiator of the lease");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseLicenseType>(entity =>
            {
                entity.HasKey(e => e.LeaseLicenseTypeCode)
                    .HasName("LELIST_PK");

                entity.ToTable("PIMS_LEASE_LICENSE_TYPE");

                entity.Property(e => e.LeaseLicenseTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_LICENSE_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeasePayRvblType>(entity =>
            {
                entity.HasKey(e => e.LeasePayRvblTypeCode)
                    .HasName("LSPRTY_PK");

                entity.ToTable("PIMS_LEASE_PAY_RVBL_TYPE");

                entity.Property(e => e.LeasePayRvblTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PAY_RVBL_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeasePayment>(entity =>
            {
                entity.HasKey(e => e.LeasePaymentId)
                    .HasName("LSPYMT_PK");

                entity.ToTable("PIMS_LEASE_PAYMENT");

                entity.HasIndex(e => e.LeasePaymentMethodTypeCode, "LSPYMT_LEASE_PAYMENT_METHOD_TYPE_CODE_IDX");

                entity.HasIndex(e => e.LeasePaymentPeriodId, "LSPYMT_LEASE_PAYMENT_PERIOD_ID_IDX");

                entity.HasIndex(e => e.LeaseTermId, "LSPYMT_LEASE_TERM_ID_IDX");

                entity.Property(e => e.LeasePaymentId)
                    .HasColumnName("LEASE_PAYMENT_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_PAYMENT_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.LeasePaymentMethodTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PAYMENT_METHOD_TYPE_CODE");

                entity.Property(e => e.LeasePaymentPeriodId).HasColumnName("LEASE_PAYMENT_PERIOD_ID");

                entity.Property(e => e.LeaseTermId).HasColumnName("LEASE_TERM_ID");

                entity.Property(e => e.Note)
                    .HasMaxLength(2000)
                    .HasColumnName("NOTE");

                entity.Property(e => e.PaymentAmountGst)
                    .HasColumnType("money")
                    .HasColumnName("PAYMENT_AMOUNT_GST");

                entity.Property(e => e.PaymentAmountPreTax)
                    .HasColumnType("money")
                    .HasColumnName("PAYMENT_AMOUNT_PRE_TAX");

                entity.Property(e => e.PaymentAmountPst)
                    .HasColumnType("money")
                    .HasColumnName("PAYMENT_AMOUNT_PST");

                entity.Property(e => e.PaymentAmountTotal)
                    .HasColumnType("money")
                    .HasColumnName("PAYMENT_AMOUNT_TOTAL");

                entity.Property(e => e.PaymentReceivedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("PAYMENT_RECEIVED_DATE");

                entity.HasOne(d => d.LeasePaymentMethodTypeCodeNavigation)
                    .WithMany(p => p.PimsLeasePayments)
                    .HasForeignKey(d => d.LeasePaymentMethodTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LSPMMT_PIM_LSPYMT_FK");

                entity.HasOne(d => d.LeasePaymentPeriod)
                    .WithMany(p => p.PimsLeasePayments)
                    .HasForeignKey(d => d.LeasePaymentPeriodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LPYPER_PIM_LSPYMT_FK");

                entity.HasOne(d => d.LeaseTerm)
                    .WithMany(p => p.PimsLeasePayments)
                    .HasForeignKey(d => d.LeaseTermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LSTERM_PIM_LSPYMT_FK");
            });

            modelBuilder.Entity<PimsLeasePaymentForecast>(entity =>
            {
                entity.HasKey(e => e.LeasePaymentForecastId)
                    .HasName("LPFCST_PK");

                entity.ToTable("PIMS_LEASE_PAYMENT_FORECAST");

                entity.HasIndex(e => e.LeasePaymentPeriodId, "LPFCST_LEASE_PAYMENT_PERIOD_ID_IDX");

                entity.HasIndex(e => e.LeasePaymentStatusTypeCode, "LPFCST_LEASE_PAYMENT_STATUS_TYPE_CODE_IDX");

                entity.HasIndex(e => e.LeaseTermId, "LPFCST_LEASE_TERM_ID_IDX");

                entity.Property(e => e.LeasePaymentForecastId)
                    .HasColumnName("LEASE_PAYMENT_FORECAST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_PAYMENT_FORECAST_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ForecastPaymentGst)
                    .HasColumnType("money")
                    .HasColumnName("FORECAST_PAYMENT_GST");

                entity.Property(e => e.ForecastPaymentPreTax)
                    .HasColumnType("money")
                    .HasColumnName("FORECAST_PAYMENT_PRE_TAX");

                entity.Property(e => e.ForecastPaymentPst)
                    .HasColumnType("money")
                    .HasColumnName("FORECAST_PAYMENT_PST");

                entity.Property(e => e.ForecastPaymentTotal)
                    .HasColumnType("money")
                    .HasColumnName("FORECAST_PAYMENT_TOTAL");

                entity.Property(e => e.LeasePaymentPeriodId).HasColumnName("LEASE_PAYMENT_PERIOD_ID");

                entity.Property(e => e.LeasePaymentStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PAYMENT_STATUS_TYPE_CODE");

                entity.Property(e => e.LeaseTermId).HasColumnName("LEASE_TERM_ID");

                entity.Property(e => e.PaymentDueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("PAYMENT_DUE_DATE");

                entity.HasOne(d => d.LeasePaymentPeriod)
                    .WithMany(p => p.PimsLeasePaymentForecasts)
                    .HasForeignKey(d => d.LeasePaymentPeriodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LPYPER_PIM_LPFCST_FK");

                entity.HasOne(d => d.LeasePaymentStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsLeasePaymentForecasts)
                    .HasForeignKey(d => d.LeasePaymentStatusTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LPSTST_PIM_LPFCST_FK");

                entity.HasOne(d => d.LeaseTerm)
                    .WithMany(p => p.PimsLeasePaymentForecasts)
                    .HasForeignKey(d => d.LeaseTermId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LSTERM_PIM_LPFCST_FK");
            });

            modelBuilder.Entity<PimsLeasePaymentForecastHist>(entity =>
            {
                entity.HasKey(e => e.LeasePaymentForecastHistId)
                    .HasName("PIMS_LPFCST_H_PK");

                entity.ToTable("PIMS_LEASE_PAYMENT_FORECAST_HIST");

                entity.HasIndex(e => new { e.LeasePaymentForecastHistId, e.EndDateHist }, "PIMS_LPFCST_H_UK")
                    .IsUnique();

                entity.Property(e => e.LeasePaymentForecastHistId)
                    .HasColumnName("_LEASE_PAYMENT_FORECAST_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_PAYMENT_FORECAST_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.ForecastPaymentGst)
                    .HasColumnType("money")
                    .HasColumnName("FORECAST_PAYMENT_GST");

                entity.Property(e => e.ForecastPaymentPreTax)
                    .HasColumnType("money")
                    .HasColumnName("FORECAST_PAYMENT_PRE_TAX");

                entity.Property(e => e.ForecastPaymentPst)
                    .HasColumnType("money")
                    .HasColumnName("FORECAST_PAYMENT_PST");

                entity.Property(e => e.ForecastPaymentTotal)
                    .HasColumnType("money")
                    .HasColumnName("FORECAST_PAYMENT_TOTAL");

                entity.Property(e => e.LeasePaymentForecastId).HasColumnName("LEASE_PAYMENT_FORECAST_ID");

                entity.Property(e => e.LeasePaymentPeriodId).HasColumnName("LEASE_PAYMENT_PERIOD_ID");

                entity.Property(e => e.LeasePaymentStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PAYMENT_STATUS_TYPE_CODE");

                entity.Property(e => e.LeaseTermId).HasColumnName("LEASE_TERM_ID");

                entity.Property(e => e.PaymentDueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("PAYMENT_DUE_DATE");
            });

            modelBuilder.Entity<PimsLeasePaymentHist>(entity =>
            {
                entity.HasKey(e => e.LeasePaymentHistId)
                    .HasName("PIMS_LSPYMT_H_PK");

                entity.ToTable("PIMS_LEASE_PAYMENT_HIST");

                entity.HasIndex(e => new { e.LeasePaymentHistId, e.EndDateHist }, "PIMS_LSPYMT_H_UK")
                    .IsUnique();

                entity.Property(e => e.LeasePaymentHistId)
                    .HasColumnName("_LEASE_PAYMENT_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_PAYMENT_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.LeasePaymentId).HasColumnName("LEASE_PAYMENT_ID");

                entity.Property(e => e.LeasePaymentMethodTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PAYMENT_METHOD_TYPE_CODE");

                entity.Property(e => e.LeasePaymentPeriodId).HasColumnName("LEASE_PAYMENT_PERIOD_ID");

                entity.Property(e => e.LeaseTermId).HasColumnName("LEASE_TERM_ID");

                entity.Property(e => e.Note)
                    .HasMaxLength(2000)
                    .HasColumnName("NOTE");

                entity.Property(e => e.PaymentAmountGst)
                    .HasColumnType("money")
                    .HasColumnName("PAYMENT_AMOUNT_GST");

                entity.Property(e => e.PaymentAmountPreTax)
                    .HasColumnType("money")
                    .HasColumnName("PAYMENT_AMOUNT_PRE_TAX");

                entity.Property(e => e.PaymentAmountPst)
                    .HasColumnType("money")
                    .HasColumnName("PAYMENT_AMOUNT_PST");

                entity.Property(e => e.PaymentAmountTotal)
                    .HasColumnType("money")
                    .HasColumnName("PAYMENT_AMOUNT_TOTAL");

                entity.Property(e => e.PaymentReceivedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("PAYMENT_RECEIVED_DATE");
            });

            modelBuilder.Entity<PimsLeasePaymentMethodType>(entity =>
            {
                entity.HasKey(e => e.LeasePaymentMethodTypeCode)
                    .HasName("LSPMMT_PK");

                entity.ToTable("PIMS_LEASE_PAYMENT_METHOD_TYPE");

                entity.Property(e => e.LeasePaymentMethodTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PAYMENT_METHOD_TYPE_CODE")
                    .HasComment("Payment method type code");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION")
                    .HasComment("Payment method type description");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("DISPLAY_ORDER")
                    .HasComment("Display order of the descriptions");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is this code disabled?");
            });

            modelBuilder.Entity<PimsLeasePaymentPeriod>(entity =>
            {
                entity.HasKey(e => e.LeasePaymentPeriodId)
                    .HasName("LPYPER_PK");

                entity.ToTable("PIMS_LEASE_PAYMENT_PERIOD");

                entity.Property(e => e.LeasePaymentPeriodId)
                    .HasColumnName("LEASE_PAYMENT_PERIOD_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_PAYMENT_PERIOD_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsPeriodClosed)
                    .IsRequired()
                    .HasColumnName("IS_PERIOD_CLOSED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.PeriodStartDate)
                    .HasColumnType("date")
                    .HasColumnName("PERIOD_START_DATE");
            });

            modelBuilder.Entity<PimsLeasePaymentPeriodHist>(entity =>
            {
                entity.HasKey(e => e.LeasePaymentPeriodHistId)
                    .HasName("PIMS_LPYPER_H_PK");

                entity.ToTable("PIMS_LEASE_PAYMENT_PERIOD_HIST");

                entity.HasIndex(e => new { e.LeasePaymentPeriodHistId, e.EndDateHist }, "PIMS_LPYPER_H_UK")
                    .IsUnique();

                entity.Property(e => e.LeasePaymentPeriodHistId)
                    .HasColumnName("_LEASE_PAYMENT_PERIOD_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_PAYMENT_PERIOD_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsPeriodClosed).HasColumnName("IS_PERIOD_CLOSED");

                entity.Property(e => e.LeasePaymentPeriodId).HasColumnName("LEASE_PAYMENT_PERIOD_ID");

                entity.Property(e => e.PeriodStartDate)
                    .HasColumnType("date")
                    .HasColumnName("PERIOD_START_DATE");
            });

            modelBuilder.Entity<PimsLeasePaymentStatusType>(entity =>
            {
                entity.HasKey(e => e.LeasePaymentStatusTypeCode)
                    .HasName("LPSTST_PK");

                entity.ToTable("PIMS_LEASE_PAYMENT_STATUS_TYPE");

                entity.HasComment("Describes the status of forecast payments");

                entity.Property(e => e.LeasePaymentStatusTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PAYMENT_STATUS_TYPE_CODE")
                    .HasComment("Payment status type code");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION")
                    .HasComment("Payment status type description");

                entity.Property(e => e.DisplayOrder)
                    .HasColumnName("DISPLAY_ORDER")
                    .HasComment("Display order of the descriptions");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is this code disabled?");
            });

            modelBuilder.Entity<PimsLeasePmtFreqType>(entity =>
            {
                entity.HasKey(e => e.LeasePmtFreqTypeCode)
                    .HasName("LSPMTF_PK");

                entity.ToTable("PIMS_LEASE_PMT_FREQ_TYPE");

                entity.Property(e => e.LeasePmtFreqTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PMT_FREQ_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseProgramType>(entity =>
            {
                entity.HasKey(e => e.LeaseProgramTypeCode)
                    .HasName("LSPRGT_PK");

                entity.ToTable("PIMS_LEASE_PROGRAM_TYPE");

                entity.Property(e => e.LeaseProgramTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PROGRAM_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeasePurposeType>(entity =>
            {
                entity.HasKey(e => e.LeasePurposeTypeCode)
                    .HasName("LPRPTY_PK");

                entity.ToTable("PIMS_LEASE_PURPOSE_TYPE");

                entity.Property(e => e.LeasePurposeTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_PURPOSE_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseResponsibilityType>(entity =>
            {
                entity.HasKey(e => e.LeaseResponsibilityTypeCode)
                    .HasName("LRESPT_PK");

                entity.ToTable("PIMS_LEASE_RESPONSIBILITY_TYPE");

                entity.HasComment("Describes which organization is responsible for this lease");

                entity.Property(e => e.LeaseResponsibilityTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_RESPONSIBILITY_TYPE_CODE")
                    .HasComment("Code value of the organization responsible for this lease");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION")
                    .HasComment("Description of the organization responsible for this lease");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseStatusType>(entity =>
            {
                entity.HasKey(e => e.LeaseStatusTypeCode)
                    .HasName("LSSTYP_PK");

                entity.ToTable("PIMS_LEASE_STATUS_TYPE");

                entity.HasComment("Describes the status of the lease");

                entity.Property(e => e.LeaseStatusTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_STATUS_TYPE_CODE")
                    .HasComment("Code value of the status of the lease");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION")
                    .HasComment("Description of the status of the lease");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLeaseTenant>(entity =>
            {
                entity.HasKey(e => e.LeaseTenantId)
                    .HasName("TENANT_PK");

                entity.ToTable("PIMS_LEASE_TENANT");

                entity.HasIndex(e => e.LeaseId, "TENANT_LEASE_ID_IDX");

                entity.HasIndex(e => e.LessorTypeCode, "TENANT_LESSOR_TYPE_CODE_IDX");

                entity.HasIndex(e => e.OrganizationId, "TENANT_ORGANIZATION_ID_IDX");

                entity.HasIndex(e => e.PersonId, "TENANT_PERSON_ID_IDX");

                entity.Property(e => e.LeaseTenantId)
                    .HasColumnName("LEASE_TENANT_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_TENANT_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.LessorTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LESSOR_TYPE_CODE");

                entity.Property(e => e.Note)
                    .HasMaxLength(2000)
                    .HasColumnName("NOTE");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");

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

                entity.ToTable("PIMS_LEASE_TENANT_HIST");

                entity.HasIndex(e => new { e.LeaseTenantHistId, e.EndDateHist }, "PIMS_TENANT_H_UK")
                    .IsUnique();

                entity.Property(e => e.LeaseTenantHistId)
                    .HasColumnName("_LEASE_TENANT_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_TENANT_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.LeaseTenantId).HasColumnName("LEASE_TENANT_ID");

                entity.Property(e => e.LessorTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LESSOR_TYPE_CODE");

                entity.Property(e => e.Note)
                    .HasMaxLength(2000)
                    .HasColumnName("NOTE");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");
            });

            modelBuilder.Entity<PimsLeaseTerm>(entity =>
            {
                entity.HasKey(e => e.LeaseTermId)
                    .HasName("LSTERM_PK");

                entity.ToTable("PIMS_LEASE_TERM");

                entity.HasIndex(e => e.LeaseId, "LSTERM_LEASE_ID_IDX");

                entity.HasIndex(e => e.LeaseTermStatusTypeCode, "LSTERM_LEASE_TERM_STATUS_TYPE_CODE_IDX");

                entity.Property(e => e.LeaseTermId)
                    .HasColumnName("LEASE_TERM_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_TERM_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.LeaseTermStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_TERM_STATUS_TYPE_CODE");

                entity.Property(e => e.TermExpiryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("TERM_EXPIRY_DATE")
                    .HasComment("Expiry date of the current term of the lease/licence");

                entity.Property(e => e.TermRenewalDate)
                    .HasColumnType("datetime")
                    .HasColumnName("TERM_RENEWAL_DATE")
                    .HasComment("Renewal date of the current term of the lease/licence");

                entity.Property(e => e.TermStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("TERM_START_DATE")
                    .HasComment("Start date of the current term of the lease/licence");

                entity.HasOne(d => d.Lease)
                    .WithMany(p => p.PimsLeaseTerms)
                    .HasForeignKey(d => d.LeaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LEASE_PIM_LSTERM_FK");

                entity.HasOne(d => d.LeaseTermStatusTypeCodeNavigation)
                    .WithMany(p => p.PimsLeaseTerms)
                    .HasForeignKey(d => d.LeaseTermStatusTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LTRMST_PIM_LSTERM_FK");
            });

            modelBuilder.Entity<PimsLeaseTermHist>(entity =>
            {
                entity.HasKey(e => e.LeaseTermHistId)
                    .HasName("PIMS_LSTERM_H_PK");

                entity.ToTable("PIMS_LEASE_TERM_HIST");

                entity.HasIndex(e => new { e.LeaseTermHistId, e.EndDateHist }, "PIMS_LSTERM_H_UK")
                    .IsUnique();

                entity.Property(e => e.LeaseTermHistId)
                    .HasColumnName("_LEASE_TERM_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_LEASE_TERM_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.LeaseTermId).HasColumnName("LEASE_TERM_ID");

                entity.Property(e => e.LeaseTermStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_TERM_STATUS_TYPE_CODE");

                entity.Property(e => e.TermExpiryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("TERM_EXPIRY_DATE");

                entity.Property(e => e.TermRenewalDate)
                    .HasColumnType("datetime")
                    .HasColumnName("TERM_RENEWAL_DATE");

                entity.Property(e => e.TermStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("TERM_START_DATE");
            });

            modelBuilder.Entity<PimsLeaseTermStatusType>(entity =>
            {
                entity.HasKey(e => e.LeaseTermStatusTypeCode)
                    .HasName("LTRMST_PK");

                entity.ToTable("PIMS_LEASE_TERM_STATUS_TYPE");

                entity.HasComment("Describes the status of the lease term");

                entity.Property(e => e.LeaseTermStatusTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LEASE_TERM_STATUS_TYPE_CODE")
                    .HasComment("Code value of the status of the lease term");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION")
                    .HasComment("Description of the status of the lease term");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsLessorType>(entity =>
            {
                entity.HasKey(e => e.LessorTypeCode)
                    .HasName("LSSRTY_PK");

                entity.ToTable("PIMS_LESSOR_TYPE");

                entity.Property(e => e.LessorTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("LESSOR_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsOrgIdentifierType>(entity =>
            {
                entity.HasKey(e => e.OrgIdentifierTypeCode)
                    .HasName("ORGIDT_PK");

                entity.ToTable("PIMS_ORG_IDENTIFIER_TYPE");

                entity.Property(e => e.OrgIdentifierTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("ORG_IDENTIFIER_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsOrganization>(entity =>
            {
                entity.HasKey(e => e.OrganizationId)
                    .HasName("ORG_PK");

                entity.ToTable("PIMS_ORGANIZATION");

                entity.HasComment("Information related to an organization identified in the PSP system.");

                entity.HasIndex(e => e.DistrictCode, "ORG_DISTRICT_CODE_IDX");

                entity.HasIndex(e => e.OrganizationTypeCode, "ORG_ORGANIZATION_TYPE_CODE_IDX");

                entity.HasIndex(e => e.OrgIdentifierTypeCode, "ORG_ORG_IDENTIFIER_TYPE_CODE_IDX");

                entity.HasIndex(e => e.PrntOrganizationId, "ORG_PRNT_ORGANIZATION_ID_IDX");

                entity.HasIndex(e => e.RegionCode, "ORG_REGION_CODE_IDX");

                entity.Property(e => e.OrganizationId)
                    .HasColumnName("ORGANIZATION_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ORGANIZATION_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2000)
                    .HasColumnName("COMMENT");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DistrictCode).HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.IncorporationNumber)
                    .HasMaxLength(50)
                    .HasColumnName("INCORPORATION_NUMBER")
                    .HasComment("Incorporation number of the orgnization");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.OrgIdentifierTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ORG_IDENTIFIER_TYPE_CODE");

                entity.Property(e => e.OrganizationAlias)
                    .HasMaxLength(200)
                    .HasColumnName("ORGANIZATION_ALIAS");

                entity.Property(e => e.OrganizationIdentifier)
                    .HasMaxLength(100)
                    .HasColumnName("ORGANIZATION_IDENTIFIER");

                entity.Property(e => e.OrganizationName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("ORGANIZATION_NAME");

                entity.Property(e => e.OrganizationTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ORGANIZATION_TYPE_CODE");

                entity.Property(e => e.PrntOrganizationId).HasColumnName("PRNT_ORGANIZATION_ID");

                entity.Property(e => e.RegionCode).HasColumnName("REGION_CODE");

                entity.Property(e => e.Website)
                    .HasMaxLength(200)
                    .HasColumnName("WEBSITE");

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

                entity.ToTable("PIMS_ORGANIZATION_ADDRESS");

                entity.HasComment("An associative entity to define multiple addresses for a person.");

                entity.HasIndex(e => e.AddressId, "ORGADD_ADDRESS_ID_IDX");

                entity.HasIndex(e => e.AddressUsageTypeCode, "ORGADD_ADDRESS_USAGE_TYPE_CODE_IDX");

                entity.HasIndex(e => e.OrganizationId, "ORGADD_ORGANIZATION_ID_IDX");

                entity.HasIndex(e => new { e.OrganizationId, e.AddressId, e.AddressUsageTypeCode }, "ORGADD_UNQ_ADDR_TYPE_TUC")
                    .IsUnique();

                entity.Property(e => e.OrganizationAddressId)
                    .HasColumnName("ORGANIZATION_ADDRESS_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ORGANIZATION_ADDRESS_ID_SEQ])");

                entity.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");

                entity.Property(e => e.AddressUsageTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ADDRESS_USAGE_TYPE_CODE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

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

                entity.ToTable("PIMS_ORGANIZATION_ADDRESS_HIST");

                entity.HasIndex(e => new { e.OrganizationAddressHistId, e.EndDateHist }, "PIMS_ORGADD_H_UK")
                    .IsUnique();

                entity.Property(e => e.OrganizationAddressHistId)
                    .HasColumnName("_ORGANIZATION_ADDRESS_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ORGANIZATION_ADDRESS_H_ID_SEQ])");

                entity.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");

                entity.Property(e => e.AddressUsageTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ADDRESS_USAGE_TYPE_CODE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.OrganizationAddressId).HasColumnName("ORGANIZATION_ADDRESS_ID");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");
            });

            modelBuilder.Entity<PimsOrganizationHist>(entity =>
            {
                entity.HasKey(e => e.OrganizationHistId)
                    .HasName("PIMS_ORG_H_PK");

                entity.ToTable("PIMS_ORGANIZATION_HIST");

                entity.HasIndex(e => new { e.OrganizationHistId, e.EndDateHist }, "PIMS_ORG_H_UK")
                    .IsUnique();

                entity.Property(e => e.OrganizationHistId)
                    .HasColumnName("_ORGANIZATION_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ORGANIZATION_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2000)
                    .HasColumnName("COMMENT");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.DistrictCode).HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IncorporationNumber)
                    .HasMaxLength(50)
                    .HasColumnName("INCORPORATION_NUMBER");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.OrgIdentifierTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ORG_IDENTIFIER_TYPE_CODE");

                entity.Property(e => e.OrganizationAlias)
                    .HasMaxLength(200)
                    .HasColumnName("ORGANIZATION_ALIAS");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

                entity.Property(e => e.OrganizationIdentifier)
                    .HasMaxLength(100)
                    .HasColumnName("ORGANIZATION_IDENTIFIER");

                entity.Property(e => e.OrganizationName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("ORGANIZATION_NAME");

                entity.Property(e => e.OrganizationTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ORGANIZATION_TYPE_CODE");

                entity.Property(e => e.PrntOrganizationId).HasColumnName("PRNT_ORGANIZATION_ID");

                entity.Property(e => e.RegionCode).HasColumnName("REGION_CODE");

                entity.Property(e => e.Website)
                    .HasMaxLength(200)
                    .HasColumnName("WEBSITE");
            });

            modelBuilder.Entity<PimsOrganizationType>(entity =>
            {
                entity.HasKey(e => e.OrganizationTypeCode)
                    .HasName("ORGTYP_PK");

                entity.ToTable("PIMS_ORGANIZATION_TYPE");

                entity.Property(e => e.OrganizationTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("ORGANIZATION_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsPerson>(entity =>
            {
                entity.HasKey(e => e.PersonId)
                    .HasName("PERSON_PK");

                entity.ToTable("PIMS_PERSON");

                entity.Property(e => e.PersonId)
                    .HasColumnName("PERSON_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("date")
                    .HasColumnName("BIRTH_DATE");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2000)
                    .HasColumnName("COMMENT");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("FIRST_NAME");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.MiddleNames)
                    .HasMaxLength(200)
                    .HasColumnName("MIDDLE_NAMES");

                entity.Property(e => e.NameSuffix)
                    .HasMaxLength(50)
                    .HasColumnName("NAME_SUFFIX");

                entity.Property(e => e.PreferredName)
                    .HasMaxLength(200)
                    .HasColumnName("PREFERRED_NAME");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("SURNAME");
            });

            modelBuilder.Entity<PimsPersonAddress>(entity =>
            {
                entity.HasKey(e => e.PersonAddressId)
                    .HasName("PERADD_PK");

                entity.ToTable("PIMS_PERSON_ADDRESS");

                entity.HasComment("An associative entity to define multiple addresses for a person.");

                entity.HasIndex(e => e.AddressId, "PERADD_ADDRESS_ID_IDX");

                entity.HasIndex(e => e.AddressUsageTypeCode, "PERADD_ADDRESS_USAGE_TYPE_CODE_IDX");

                entity.HasIndex(e => e.PersonId, "PERADD_PERSON_ID_IDX");

                entity.HasIndex(e => new { e.PersonId, e.AddressId, e.AddressUsageTypeCode }, "PERADD_UNQ_ADDR_TYPE_TUC")
                    .IsUnique();

                entity.Property(e => e.PersonAddressId)
                    .HasColumnName("PERSON_ADDRESS_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ADDRESS_ID_SEQ])");

                entity.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");

                entity.Property(e => e.AddressUsageTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ADDRESS_USAGE_TYPE_CODE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");

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

                entity.ToTable("PIMS_PERSON_ADDRESS_HIST");

                entity.HasIndex(e => new { e.PersonAddressHistId, e.EndDateHist }, "PIMS_PERADD_H_UK")
                    .IsUnique();

                entity.Property(e => e.PersonAddressHistId)
                    .HasColumnName("_PERSON_ADDRESS_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ADDRESS_H_ID_SEQ])");

                entity.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");

                entity.Property(e => e.AddressUsageTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("ADDRESS_USAGE_TYPE_CODE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.PersonAddressId).HasColumnName("PERSON_ADDRESS_ID");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");
            });

            modelBuilder.Entity<PimsPersonHist>(entity =>
            {
                entity.HasKey(e => e.PersonHistId)
                    .HasName("PIMS_PERSON_H_PK");

                entity.ToTable("PIMS_PERSON_HIST");

                entity.HasIndex(e => new { e.PersonHistId, e.EndDateHist }, "PIMS_PERSON_H_UK")
                    .IsUnique();

                entity.Property(e => e.PersonHistId)
                    .HasColumnName("_PERSON_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("date")
                    .HasColumnName("BIRTH_DATE");

                entity.Property(e => e.Comment)
                    .HasMaxLength(2000)
                    .HasColumnName("COMMENT");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("FIRST_NAME");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.MiddleNames)
                    .HasMaxLength(200)
                    .HasColumnName("MIDDLE_NAMES");

                entity.Property(e => e.NameSuffix)
                    .HasMaxLength(50)
                    .HasColumnName("NAME_SUFFIX");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");

                entity.Property(e => e.PreferredName)
                    .HasMaxLength(200)
                    .HasColumnName("PREFERRED_NAME");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("SURNAME");
            });

            modelBuilder.Entity<PimsPersonOrganization>(entity =>
            {
                entity.HasKey(e => e.PersonOrganizationId)
                    .HasName("PERORG_PK");

                entity.ToTable("PIMS_PERSON_ORGANIZATION");

                entity.HasIndex(e => e.OrganizationId, "PERORG_ORGANIZATION_ID_IDX");

                entity.HasIndex(e => e.PersonId, "PERORG_PERSON_ID_IDX");

                entity.HasIndex(e => new { e.OrganizationId, e.PersonId }, "PERORG_PERSON_ORGANIZATION_TUC")
                    .IsUnique();

                entity.Property(e => e.PersonOrganizationId)
                    .HasColumnName("PERSON_ORGANIZATION_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ORGANIZATION_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");

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

                entity.ToTable("PIMS_PERSON_ORGANIZATION_HIST");

                entity.HasIndex(e => new { e.PersonOrganizationHistId, e.EndDateHist }, "PIMS_PERORG_H_UK")
                    .IsUnique();

                entity.Property(e => e.PersonOrganizationHistId)
                    .HasColumnName("_PERSON_ORGANIZATION_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PERSON_ORGANIZATION_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");

                entity.Property(e => e.PersonOrganizationId).HasColumnName("PERSON_ORGANIZATION_ID");
            });

            modelBuilder.Entity<PimsProject>(entity =>
            {
                entity.HasKey(e => e.ProjectId)
                    .HasName("PROJCT_PK");

                entity.ToTable("PIMS_PROJECT");

                entity.HasIndex(e => e.ProjectRiskTypeCode, "PROJCT_PROJECT_RISK_TYPE_CODE_IDX");

                entity.HasIndex(e => e.ProjectStatusTypeCode, "PROJCT_PROJECT_STATUS_TYPE_CODE_IDX");

                entity.HasIndex(e => e.ProjectTierTypeCode, "PROJCT_PROJECT_TIER_TYPE_CODE_IDX");

                entity.HasIndex(e => e.ProjectTypeCode, "PROJCT_PROJECT_TYPE_CODE_IDX");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("PROJECT_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ProjectRiskTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_RISK_TYPE_CODE");

                entity.Property(e => e.ProjectStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_STATUS_TYPE_CODE");

                entity.Property(e => e.ProjectTierTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_TIER_TYPE_CODE");

                entity.Property(e => e.ProjectTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_TYPE_CODE");

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

                entity.ToTable("PIMS_PROJECT_HIST");

                entity.HasIndex(e => new { e.ProjectHistId, e.EndDateHist }, "PIMS_PROJCT_H_UK")
                    .IsUnique();

                entity.Property(e => e.ProjectHistId)
                    .HasColumnName("_PROJECT_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.ProjectRiskTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_RISK_TYPE_CODE");

                entity.Property(e => e.ProjectStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_STATUS_TYPE_CODE");

                entity.Property(e => e.ProjectTierTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_TIER_TYPE_CODE");

                entity.Property(e => e.ProjectTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_TYPE_CODE");
            });

            modelBuilder.Entity<PimsProjectNote>(entity =>
            {
                entity.HasKey(e => e.ProjectNoteId)
                    .HasName("PROJNT_PK");

                entity.ToTable("PIMS_PROJECT_NOTE");

                entity.HasIndex(e => e.ProjectId, "PROJNT_PROJECT_ID_IDX");

                entity.Property(e => e.ProjectNoteId)
                    .HasColumnName("PROJECT_NOTE_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_NOTE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

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

                entity.ToTable("PIMS_PROJECT_NOTE_HIST");

                entity.HasIndex(e => new { e.ProjectNoteHistId, e.EndDateHist }, "PIMS_PROJNT_H_UK")
                    .IsUnique();

                entity.Property(e => e.ProjectNoteHistId)
                    .HasColumnName("_PROJECT_NOTE_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_NOTE_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.ProjectNoteId).HasColumnName("PROJECT_NOTE_ID");
            });

            modelBuilder.Entity<PimsProjectProperty>(entity =>
            {
                entity.HasKey(e => e.ProjectPropertyId)
                    .HasName("PRJPRP_PK");

                entity.ToTable("PIMS_PROJECT_PROPERTY");

                entity.HasIndex(e => e.ProjectId, "PRJPRP_PROJECT_ID_IDX");

                entity.HasIndex(e => new { e.PropertyId, e.ProjectId }, "PRJPRP_PROJECT_PROPERTY_TUC")
                    .IsUnique();

                entity.HasIndex(e => e.PropertyId, "PRJPRP_PROPERTY_ID_IDX");

                entity.Property(e => e.ProjectPropertyId)
                    .HasColumnName("PROJECT_PROPERTY_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_PROPERTY_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

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

                entity.ToTable("PIMS_PROJECT_PROPERTY_HIST");

                entity.HasIndex(e => new { e.ProjectPropertyHistId, e.EndDateHist }, "PIMS_PRJPRP_H_UK")
                    .IsUnique();

                entity.Property(e => e.ProjectPropertyHistId)
                    .HasColumnName("_PROJECT_PROPERTY_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_PROPERTY_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.ProjectPropertyId).HasColumnName("PROJECT_PROPERTY_ID");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");
            });

            modelBuilder.Entity<PimsProjectRiskType>(entity =>
            {
                entity.HasKey(e => e.ProjectRiskTypeCode)
                    .HasName("PRJRSK_PK");

                entity.ToTable("PIMS_PROJECT_RISK_TYPE");

                entity.Property(e => e.ProjectRiskTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_RISK_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsProjectStatusType>(entity =>
            {
                entity.HasKey(e => e.ProjectStatusTypeCode)
                    .HasName("PRJSTY_PK");

                entity.ToTable("PIMS_PROJECT_STATUS_TYPE");

                entity.Property(e => e.ProjectStatusTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_STATUS_TYPE_CODE");

                entity.Property(e => e.CodeGroup)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("CODE_GROUP");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.IsMilestone)
                    .IsRequired()
                    .HasColumnName("IS_MILESTONE")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.IsTerminal)
                    .IsRequired()
                    .HasColumnName("IS_TERMINAL")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("TEXT");
            });

            modelBuilder.Entity<PimsProjectTierType>(entity =>
            {
                entity.HasKey(e => e.ProjectTierTypeCode)
                    .HasName("PROJTR_PK");

                entity.ToTable("PIMS_PROJECT_TIER_TYPE");

                entity.Property(e => e.ProjectTierTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_TIER_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsProjectType>(entity =>
            {
                entity.HasKey(e => e.ProjectTypeCode)
                    .HasName("PRJTYP_PK");

                entity.ToTable("PIMS_PROJECT_TYPE");

                entity.Property(e => e.ProjectTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("PROJECT_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsProjectWorkflowModel>(entity =>
            {
                entity.HasKey(e => e.ProjectWorkflowModelId)
                    .HasName("PRWKMD_PK");

                entity.ToTable("PIMS_PROJECT_WORKFLOW_MODEL");

                entity.HasIndex(e => e.ProjectId, "PRWKMD_PROJECT_ID_IDX");

                entity.HasIndex(e => new { e.ProjectId, e.WorkflowModelId }, "PRWKMD_PROJECT_WORKFLOW_MODEL_TUC")
                    .IsUnique();

                entity.HasIndex(e => e.WorkflowModelId, "PRWKMD_WORKFLOW_MODEL_ID_IDX");

                entity.Property(e => e.ProjectWorkflowModelId)
                    .HasColumnName("PROJECT_WORKFLOW_MODEL_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_WORKFLOW_MODEL_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.WorkflowModelId).HasColumnName("WORKFLOW_MODEL_ID");

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

                entity.ToTable("PIMS_PROJECT_WORKFLOW_MODEL_HIST");

                entity.HasIndex(e => new { e.ProjectWorkflowModelHistId, e.EndDateHist }, "PIMS_PRWKMD_H_UK")
                    .IsUnique();

                entity.Property(e => e.ProjectWorkflowModelHistId)
                    .HasColumnName("_PROJECT_WORKFLOW_MODEL_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROJECT_WORKFLOW_MODEL_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.ProjectId).HasColumnName("PROJECT_ID");

                entity.Property(e => e.ProjectWorkflowModelId).HasColumnName("PROJECT_WORKFLOW_MODEL_ID");

                entity.Property(e => e.WorkflowModelId).HasColumnName("WORKFLOW_MODEL_ID");
            });

            modelBuilder.Entity<PimsProperty>(entity =>
            {
                entity.HasKey(e => e.PropertyId)
                    .HasName("PRPRTY_PK");

                entity.ToTable("PIMS_PROPERTY");

                entity.HasIndex(e => e.AddressId, "PRPRTY_ADDRESS_ID_IDX");

                entity.HasIndex(e => e.DistrictCode, "PRPRTY_DISTRICT_CODE_IDX");

                entity.HasIndex(e => e.PropertyAreaUnitTypeCode, "PRPRTY_PROPERTY_AREA_UNIT_TYPE_CODE_IDX");

                entity.HasIndex(e => e.PropertyClassificationTypeCode, "PRPRTY_PROPERTY_CLASSIFICATION_TYPE_CODE_IDX");

                entity.HasIndex(e => e.PropertyDataSourceTypeCode, "PRPRTY_PROPERTY_DATA_SOURCE_TYPE_CODE_IDX");

                entity.HasIndex(e => e.PropertyManagerId, "PRPRTY_PROPERTY_MANAGER_ID_IDX");

                entity.HasIndex(e => e.PropertyStatusTypeCode, "PRPRTY_PROPERTY_STATUS_TYPE_CODE_IDX");

                entity.HasIndex(e => e.PropertyTenureTypeCode, "PRPRTY_PROPERTY_TENURE_TYPE_CODE_IDX");

                entity.HasIndex(e => e.PropertyTypeCode, "PRPRTY_PROPERTY_TYPE_CODE_IDX");

                entity.HasIndex(e => e.PropMgmtOrgId, "PRPRTY_PROP_MGMT_ORG_ID_IDX");

                entity.HasIndex(e => e.RegionCode, "PRPRTY_REGION_CODE_IDX");

                entity.HasIndex(e => e.SurplusDeclarationTypeCode, "PRPRTY_SURPLUS_DECLARATION_TYPE_CODE_IDX");

                entity.Property(e => e.PropertyId)
                    .HasColumnName("PROPERTY_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ID_SEQ])");

                entity.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.Boundary)
                    .HasColumnType("geometry")
                    .HasColumnName("BOUNDARY")
                    .HasComment("Spatial bundary of land");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .HasColumnName("DESCRIPTION")
                    .HasComment("Property description");

                entity.Property(e => e.DistrictCode).HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.EncumbranceReason)
                    .HasMaxLength(500)
                    .HasColumnName("ENCUMBRANCE_REASON")
                    .HasComment("reason for property encumbreance");

                entity.Property(e => e.IsOwned)
                    .IsRequired()
                    .HasColumnName("IS_OWNED")
                    .HasDefaultValueSql("(CONVERT([bit],(1)))")
                    .HasComment("Is the property currently owned?");

                entity.Property(e => e.IsPropertyOfInterest)
                    .IsRequired()
                    .HasColumnName("IS_PROPERTY_OF_INTEREST")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is this a property of interest to the Ministry?");

                entity.Property(e => e.IsSensitive)
                    .IsRequired()
                    .HasColumnName("IS_SENSITIVE")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is this a sensitive property?");

                entity.Property(e => e.IsVisibleToOtherAgencies)
                    .IsRequired()
                    .HasColumnName("IS_VISIBLE_TO_OTHER_AGENCIES")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is the property visible to other agencies?");

                entity.Property(e => e.LandArea)
                    .HasColumnName("LAND_AREA")
                    .HasComment("Area occupied by property");

                entity.Property(e => e.LandLegalDescription)
                    .HasColumnName("LAND_LEGAL_DESCRIPTION")
                    .HasComment("Legal description of property");

                entity.Property(e => e.Location)
                    .HasColumnType("geometry")
                    .HasColumnName("LOCATION")
                    .HasComment("Geospatial location (pin) of property");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("NAME")
                    .HasComment("Property name");

                entity.Property(e => e.Pid)
                    .HasColumnName("PID")
                    .HasComment("Property ID");

                entity.Property(e => e.Pin)
                    .HasColumnName("PIN")
                    .HasComment("Property number");

                entity.Property(e => e.PropMgmtOrgId).HasColumnName("PROP_MGMT_ORG_ID");

                entity.Property(e => e.PropertyAreaUnitTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_AREA_UNIT_TYPE_CODE");

                entity.Property(e => e.PropertyClassificationTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_CLASSIFICATION_TYPE_CODE");

                entity.Property(e => e.PropertyDataSourceEffectiveDate)
                    .HasColumnType("date")
                    .HasColumnName("PROPERTY_DATA_SOURCE_EFFECTIVE_DATE")
                    .HasComment("Date the property was officially registered");

                entity.Property(e => e.PropertyDataSourceTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_DATA_SOURCE_TYPE_CODE");

                entity.Property(e => e.PropertyManagerId).HasColumnName("PROPERTY_MANAGER_ID");

                entity.Property(e => e.PropertyStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_STATUS_TYPE_CODE");

                entity.Property(e => e.PropertyTenureTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TENURE_TYPE_CODE");

                entity.Property(e => e.PropertyTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TYPE_CODE");

                entity.Property(e => e.RegionCode).HasColumnName("REGION_CODE");

                entity.Property(e => e.SurplusDeclarationComment)
                    .HasMaxLength(2000)
                    .HasColumnName("SURPLUS_DECLARATION_COMMENT")
                    .HasComment("Comment regarding the surplus declaration");

                entity.Property(e => e.SurplusDeclarationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("SURPLUS_DECLARATION_DATE")
                    .HasComment("Date the property was declared surplus");

                entity.Property(e => e.SurplusDeclarationTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SURPLUS_DECLARATION_TYPE_CODE");

                entity.Property(e => e.Zoning)
                    .HasMaxLength(50)
                    .HasColumnName("ZONING")
                    .HasComment("Current property zoning");

                entity.Property(e => e.ZoningPotential)
                    .HasMaxLength(50)
                    .HasColumnName("ZONING_POTENTIAL")
                    .HasComment("Potential property zoning");

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

                entity.ToTable("PIMS_PROPERTY_ACTIVITY");

                entity.HasIndex(e => e.ActivityId, "PRPACT_ACTIVITY_ID_IDX");

                entity.HasIndex(e => new { e.PropertyId, e.ActivityId }, "PRPACT_PROPERTY_ACTIVITY_TUC")
                    .IsUnique();

                entity.HasIndex(e => e.PropertyId, "PRPACT_PROPERTY_ID_IDX");

                entity.Property(e => e.PropertyActivityId)
                    .HasColumnName("PROPERTY_ACTIVITY_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACTIVITY_ID_SEQ])");

                entity.Property(e => e.ActivityId).HasColumnName("ACTIVITY_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

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

                entity.ToTable("PIMS_PROPERTY_ACTIVITY_HIST");

                entity.HasIndex(e => new { e.PropertyActivityHistId, e.EndDateHist }, "PIMS_PRPACT_H_UK")
                    .IsUnique();

                entity.Property(e => e.PropertyActivityHistId)
                    .HasColumnName("_PROPERTY_ACTIVITY_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ACTIVITY_H_ID_SEQ])");

                entity.Property(e => e.ActivityId).HasColumnName("ACTIVITY_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.PropertyActivityId).HasColumnName("PROPERTY_ACTIVITY_ID");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");
            });

            modelBuilder.Entity<PimsPropertyBoundaryVw>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PIMS_PROPERTY_BOUNDARY_VW");

                entity.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("COUNTRY_CODE");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("COUNTRY_NAME");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DistrictCode).HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.EncumbranceReason)
                    .HasMaxLength(500)
                    .HasColumnName("ENCUMBRANCE_REASON");

                entity.Property(e => e.Geometry)
                    .HasColumnType("geometry")
                    .HasColumnName("GEOMETRY");

                entity.Property(e => e.IsOwned).HasColumnName("IS_OWNED");

                entity.Property(e => e.IsPropertyOfInterest).HasColumnName("IS_PROPERTY_OF_INTEREST");

                entity.Property(e => e.IsSensitive).HasColumnName("IS_SENSITIVE");

                entity.Property(e => e.IsVisibleToOtherAgencies).HasColumnName("IS_VISIBLE_TO_OTHER_AGENCIES");

                entity.Property(e => e.LandArea).HasColumnName("LAND_AREA");

                entity.Property(e => e.LandLegalDescription).HasColumnName("LAND_LEGAL_DESCRIPTION");

                entity.Property(e => e.MunicipalityName)
                    .HasMaxLength(200)
                    .HasColumnName("MUNICIPALITY_NAME");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("NAME");

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.PidPadded)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("PID_PADDED");

                entity.Property(e => e.Pin).HasColumnName("PIN");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(20)
                    .HasColumnName("POSTAL_CODE");

                entity.Property(e => e.PropertyAreaUnitTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_AREA_UNIT_TYPE_CODE");

                entity.Property(e => e.PropertyClassificationTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_CLASSIFICATION_TYPE_CODE");

                entity.Property(e => e.PropertyDataSourceEffectiveDate)
                    .HasColumnType("date")
                    .HasColumnName("PROPERTY_DATA_SOURCE_EFFECTIVE_DATE");

                entity.Property(e => e.PropertyDataSourceTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_DATA_SOURCE_TYPE_CODE");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

                entity.Property(e => e.PropertyStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_STATUS_TYPE_CODE");

                entity.Property(e => e.PropertyTenureTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TENURE_TYPE_CODE");

                entity.Property(e => e.PropertyTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TYPE_CODE");

                entity.Property(e => e.ProvinceName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("PROVINCE_NAME");

                entity.Property(e => e.ProvinceStateCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROVINCE_STATE_CODE");

                entity.Property(e => e.RegionCode).HasColumnName("REGION_CODE");

                entity.Property(e => e.StreetAddress1)
                    .HasMaxLength(200)
                    .HasColumnName("STREET_ADDRESS_1");

                entity.Property(e => e.StreetAddress2)
                    .HasMaxLength(200)
                    .HasColumnName("STREET_ADDRESS_2");

                entity.Property(e => e.StreetAddress3)
                    .HasMaxLength(200)
                    .HasColumnName("STREET_ADDRESS_3");

                entity.Property(e => e.Zoning)
                    .HasMaxLength(50)
                    .HasColumnName("ZONING");

                entity.Property(e => e.ZoningPotential)
                    .HasMaxLength(50)
                    .HasColumnName("ZONING_POTENTIAL");
            });

            modelBuilder.Entity<PimsPropertyClassificationType>(entity =>
            {
                entity.HasKey(e => e.PropertyClassificationTypeCode)
                    .HasName("PRPCLT_PK");

                entity.ToTable("PIMS_PROPERTY_CLASSIFICATION_TYPE");

                entity.Property(e => e.PropertyClassificationTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_CLASSIFICATION_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsPropertyEvaluation>(entity =>
            {
                entity.HasKey(e => e.PropertyEvaluationId)
                    .HasName("PRPEVL_PK");

                entity.ToTable("PIMS_PROPERTY_EVALUATION");

                entity.HasIndex(e => e.PropertyId, "PRPEVL_PROPERTY_ID_IDX");

                entity.Property(e => e.PropertyEvaluationId)
                    .HasColumnName("PROPERTY_EVALUATION_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_EVALUATION_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.EvaluationDate)
                    .HasColumnType("date")
                    .HasColumnName("EVALUATION_DATE");

                entity.Property(e => e.Key).HasColumnName("KEY");

                entity.Property(e => e.Note)
                    .HasMaxLength(1000)
                    .HasColumnName("NOTE");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

                entity.Property(e => e.Value)
                    .HasColumnType("money")
                    .HasColumnName("VALUE");

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

                entity.ToTable("PIMS_PROPERTY_EVALUATION_HIST");

                entity.HasIndex(e => new { e.PropertyEvaluationHistId, e.EndDateHist }, "PIMS_PRPEVL_H_UK")
                    .IsUnique();

                entity.Property(e => e.PropertyEvaluationHistId)
                    .HasColumnName("_PROPERTY_EVALUATION_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_EVALUATION_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.EvaluationDate)
                    .HasColumnType("date")
                    .HasColumnName("EVALUATION_DATE");

                entity.Property(e => e.Key).HasColumnName("KEY");

                entity.Property(e => e.Note)
                    .HasMaxLength(1000)
                    .HasColumnName("NOTE");

                entity.Property(e => e.PropertyEvaluationId).HasColumnName("PROPERTY_EVALUATION_ID");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

                entity.Property(e => e.Value)
                    .HasColumnType("money")
                    .HasColumnName("VALUE");
            });

            modelBuilder.Entity<PimsPropertyHist>(entity =>
            {
                entity.HasKey(e => e.PropertyHistId)
                    .HasName("PIMS_PRPRTY_H_PK");

                entity.ToTable("PIMS_PROPERTY_HIST");

                entity.HasIndex(e => new { e.PropertyHistId, e.EndDateHist }, "PIMS_PRPRTY_H_UK")
                    .IsUnique();

                entity.Property(e => e.PropertyHistId)
                    .HasColumnName("_PROPERTY_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_H_ID_SEQ])");

                entity.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DistrictCode).HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EncumbranceReason)
                    .HasMaxLength(500)
                    .HasColumnName("ENCUMBRANCE_REASON");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsOwned).HasColumnName("IS_OWNED");

                entity.Property(e => e.IsPropertyOfInterest).HasColumnName("IS_PROPERTY_OF_INTEREST");

                entity.Property(e => e.IsSensitive).HasColumnName("IS_SENSITIVE");

                entity.Property(e => e.IsVisibleToOtherAgencies).HasColumnName("IS_VISIBLE_TO_OTHER_AGENCIES");

                entity.Property(e => e.LandArea).HasColumnName("LAND_AREA");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("NAME");

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.Pin).HasColumnName("PIN");

                entity.Property(e => e.PropMgmtOrgId).HasColumnName("PROP_MGMT_ORG_ID");

                entity.Property(e => e.PropertyAreaUnitTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_AREA_UNIT_TYPE_CODE");

                entity.Property(e => e.PropertyClassificationTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_CLASSIFICATION_TYPE_CODE");

                entity.Property(e => e.PropertyDataSourceEffectiveDate)
                    .HasColumnType("date")
                    .HasColumnName("PROPERTY_DATA_SOURCE_EFFECTIVE_DATE");

                entity.Property(e => e.PropertyDataSourceTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_DATA_SOURCE_TYPE_CODE");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

                entity.Property(e => e.PropertyManagerId).HasColumnName("PROPERTY_MANAGER_ID");

                entity.Property(e => e.PropertyStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_STATUS_TYPE_CODE");

                entity.Property(e => e.PropertyTenureTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TENURE_TYPE_CODE");

                entity.Property(e => e.PropertyTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TYPE_CODE");

                entity.Property(e => e.RegionCode).HasColumnName("REGION_CODE");

                entity.Property(e => e.SurplusDeclarationComment)
                    .HasMaxLength(2000)
                    .HasColumnName("SURPLUS_DECLARATION_COMMENT");

                entity.Property(e => e.SurplusDeclarationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("SURPLUS_DECLARATION_DATE");

                entity.Property(e => e.SurplusDeclarationTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SURPLUS_DECLARATION_TYPE_CODE");

                entity.Property(e => e.Zoning)
                    .HasMaxLength(50)
                    .HasColumnName("ZONING");

                entity.Property(e => e.ZoningPotential)
                    .HasMaxLength(50)
                    .HasColumnName("ZONING_POTENTIAL");
            });

            modelBuilder.Entity<PimsPropertyImprovement>(entity =>
            {
                entity.HasKey(e => e.PropertyImprovementId)
                    .HasName("PIMPRV_PK");

                entity.ToTable("PIMS_PROPERTY_IMPROVEMENT");

                entity.HasComment("Description of property improvements associated with the lease.");

                entity.HasIndex(e => new { e.LeaseId, e.PropertyImprovementTypeCode }, "PIMPRV_LEASE_IMPROVEMENT_TUC")
                    .IsUnique();

                entity.HasIndex(e => e.PropertyImprovementTypeCode, "PIMPRV_PROPERTY_IMPROVEMENT_TYPE_CODE_IDX");

                entity.HasIndex(e => e.LeaseId, "PIMPRV_PROPERTY_LEASE_ID_IDX");

                entity.Property(e => e.PropertyImprovementId)
                    .HasColumnName("PROPERTY_IMPROVEMENT_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_IMPROVEMENT_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ImprovementDescription)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasColumnName("IMPROVEMENT_DESCRIPTION")
                    .HasComment("Description of the improvements");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.PropertyImprovementTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_IMPROVEMENT_TYPE_CODE");

                entity.Property(e => e.StructureSize)
                    .HasMaxLength(2000)
                    .HasColumnName("STRUCTURE_SIZE")
                    .HasComment("Size of the structure (house, building, bridge, etc,) ");

                entity.Property(e => e.Unit)
                    .HasMaxLength(2000)
                    .HasColumnName("UNIT")
                    .HasComment("Unit(s) affected");

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

                entity.ToTable("PIMS_PROPERTY_IMPROVEMENT_HIST");

                entity.HasIndex(e => new { e.PropertyImprovementHistId, e.EndDateHist }, "PIMS_PIMPRV_H_UK")
                    .IsUnique();

                entity.Property(e => e.PropertyImprovementHistId)
                    .HasColumnName("_PROPERTY_IMPROVEMENT_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_IMPROVEMENT_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.ImprovementDescription)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasColumnName("IMPROVEMENT_DESCRIPTION");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.PropertyImprovementId).HasColumnName("PROPERTY_IMPROVEMENT_ID");

                entity.Property(e => e.PropertyImprovementTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_IMPROVEMENT_TYPE_CODE");

                entity.Property(e => e.StructureSize)
                    .HasMaxLength(2000)
                    .HasColumnName("STRUCTURE_SIZE");

                entity.Property(e => e.Unit)
                    .HasMaxLength(2000)
                    .HasColumnName("UNIT");
            });

            modelBuilder.Entity<PimsPropertyImprovementType>(entity =>
            {
                entity.HasKey(e => e.PropertyImprovementTypeCode)
                    .HasName("PIMPRT_PK");

                entity.ToTable("PIMS_PROPERTY_IMPROVEMENT_TYPE");

                entity.HasComment("Description of the types of improvements made to a property during the lease.");

                entity.Property(e => e.PropertyImprovementTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_IMPROVEMENT_TYPE_CODE")
                    .HasComment("Code value of the types of improvements made to a property during the lease.");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION")
                    .HasComment("Code description of the types of improvements made to a property during the lease.");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates if the code is disabled");
            });

            modelBuilder.Entity<PimsPropertyLease>(entity =>
            {
                entity.HasKey(e => e.PropertyLeaseId)
                    .HasName("PROPLS_PK");

                entity.ToTable("PIMS_PROPERTY_LEASE");

                entity.HasIndex(e => e.LeaseId, "PROPLS_LEASE_ID_IDX");

                entity.HasIndex(e => e.PropertyId, "PROPLS_PROPERTY_ID_IDX");

                entity.Property(e => e.PropertyLeaseId)
                    .HasColumnName("PROPERTY_LEASE_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_LEASE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AreaUnitTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("AREA_UNIT_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.LeaseArea)
                    .HasColumnName("LEASE_AREA")
                    .HasComment("Leased area measurement");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

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

                entity.ToTable("PIMS_PROPERTY_LEASE_HIST");

                entity.HasIndex(e => new { e.PropertyLeaseHistId, e.EndDateHist }, "PIMS_PROPLS_H_UK")
                    .IsUnique();

                entity.Property(e => e.PropertyLeaseHistId)
                    .HasColumnName("_PROPERTY_LEASE_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_LEASE_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.AreaUnitTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("AREA_UNIT_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.LeaseArea).HasColumnName("LEASE_AREA");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

                entity.Property(e => e.PropertyLeaseId).HasColumnName("PROPERTY_LEASE_ID");
            });

            modelBuilder.Entity<PimsPropertyLocationVw>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("PIMS_PROPERTY_LOCATION_VW");

                entity.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("COUNTRY_CODE");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("COUNTRY_NAME");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DistrictCode).HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.EncumbranceReason)
                    .HasMaxLength(500)
                    .HasColumnName("ENCUMBRANCE_REASON");

                entity.Property(e => e.Geometry)
                    .HasColumnType("geometry")
                    .HasColumnName("GEOMETRY");

                entity.Property(e => e.IsOwned).HasColumnName("IS_OWNED");

                entity.Property(e => e.IsPropertyOfInterest).HasColumnName("IS_PROPERTY_OF_INTEREST");

                entity.Property(e => e.IsSensitive).HasColumnName("IS_SENSITIVE");

                entity.Property(e => e.IsVisibleToOtherAgencies).HasColumnName("IS_VISIBLE_TO_OTHER_AGENCIES");

                entity.Property(e => e.LandArea).HasColumnName("LAND_AREA");

                entity.Property(e => e.LandLegalDescription).HasColumnName("LAND_LEGAL_DESCRIPTION");

                entity.Property(e => e.MunicipalityName)
                    .HasMaxLength(200)
                    .HasColumnName("MUNICIPALITY_NAME");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("NAME");

                entity.Property(e => e.Pid).HasColumnName("PID");

                entity.Property(e => e.PidPadded)
                    .HasMaxLength(9)
                    .IsUnicode(false)
                    .HasColumnName("PID_PADDED");

                entity.Property(e => e.Pin).HasColumnName("PIN");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(20)
                    .HasColumnName("POSTAL_CODE");

                entity.Property(e => e.PropertyAreaUnitTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_AREA_UNIT_TYPE_CODE");

                entity.Property(e => e.PropertyClassificationTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_CLASSIFICATION_TYPE_CODE");

                entity.Property(e => e.PropertyDataSourceEffectiveDate)
                    .HasColumnType("date")
                    .HasColumnName("PROPERTY_DATA_SOURCE_EFFECTIVE_DATE");

                entity.Property(e => e.PropertyDataSourceTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_DATA_SOURCE_TYPE_CODE");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

                entity.Property(e => e.PropertyStatusTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_STATUS_TYPE_CODE");

                entity.Property(e => e.PropertyTenureTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TENURE_TYPE_CODE");

                entity.Property(e => e.PropertyTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TYPE_CODE");

                entity.Property(e => e.ProvinceName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("PROVINCE_NAME");

                entity.Property(e => e.ProvinceStateCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROVINCE_STATE_CODE");

                entity.Property(e => e.RegionCode).HasColumnName("REGION_CODE");

                entity.Property(e => e.StreetAddress1)
                    .HasMaxLength(200)
                    .HasColumnName("STREET_ADDRESS_1");

                entity.Property(e => e.StreetAddress2)
                    .HasMaxLength(200)
                    .HasColumnName("STREET_ADDRESS_2");

                entity.Property(e => e.StreetAddress3)
                    .HasMaxLength(200)
                    .HasColumnName("STREET_ADDRESS_3");

                entity.Property(e => e.Zoning)
                    .HasMaxLength(50)
                    .HasColumnName("ZONING");

                entity.Property(e => e.ZoningPotential)
                    .HasMaxLength(50)
                    .HasColumnName("ZONING_POTENTIAL");
            });

            modelBuilder.Entity<PimsPropertyOrganization>(entity =>
            {
                entity.HasKey(e => e.PropertyOrganizationId)
                    .HasName("PRPORG_PK");

                entity.ToTable("PIMS_PROPERTY_ORGANIZATION");

                entity.HasIndex(e => e.OrganizationId, "PRPORG_ORGANIZATION_ID_IDX");

                entity.HasIndex(e => e.PropertyId, "PRPORG_PROPERTY_ID_IDX");

                entity.HasIndex(e => new { e.PropertyId, e.OrganizationId }, "PRPORG_PROPERTY_ORGANIZATION_TUC")
                    .IsUnique();

                entity.Property(e => e.PropertyOrganizationId)
                    .HasColumnName("PROPERTY_ORGANIZATION_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ORGANIZATION_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

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

                entity.ToTable("PIMS_PROPERTY_ORGANIZATION_HIST");

                entity.HasIndex(e => new { e.PropertyOrganizationHistId, e.EndDateHist }, "PIMS_PRPORG_H_UK")
                    .IsUnique();

                entity.Property(e => e.PropertyOrganizationHistId)
                    .HasColumnName("_PROPERTY_ORGANIZATION_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_ORGANIZATION_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

                entity.Property(e => e.PropertyOrganizationId).HasColumnName("PROPERTY_ORGANIZATION_ID");
            });

            modelBuilder.Entity<PimsPropertyPropertyServiceFile>(entity =>
            {
                entity.HasKey(e => e.PropertyPropertyServiceFileId)
                    .HasName("PRPRSF_PK");

                entity.ToTable("PIMS_PROPERTY_PROPERTY_SERVICE_FILE");

                entity.HasIndex(e => e.PropertyId, "PRPRSF_PROPERTY_ID_IDX");

                entity.HasIndex(e => e.PropertyServiceFileId, "PRPRSF_PROPERTY_SERVICE_FILE_ID_IDX");

                entity.HasIndex(e => new { e.PropertyId, e.PropertyServiceFileId }, "PRPRSF_PROPERTY_SERVICE_FILE_TUC")
                    .IsUnique();

                entity.Property(e => e.PropertyPropertyServiceFileId)
                    .HasColumnName("PROPERTY_PROPERTY_SERVICE_FILE_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_PROPERTY_SERVICE_FILE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

                entity.Property(e => e.PropertyServiceFileId).HasColumnName("PROPERTY_SERVICE_FILE_ID");

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

                entity.ToTable("PIMS_PROPERTY_PROPERTY_SERVICE_FILE_HIST");

                entity.HasIndex(e => new { e.PropertyPropertyServiceFileHistId, e.EndDateHist }, "PIMS_PRPRSF_H_UK")
                    .IsUnique();

                entity.Property(e => e.PropertyPropertyServiceFileHistId)
                    .HasColumnName("_PROPERTY_PROPERTY_SERVICE_FILE_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_PROPERTY_SERVICE_FILE_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

                entity.Property(e => e.PropertyPropertyServiceFileId).HasColumnName("PROPERTY_PROPERTY_SERVICE_FILE_ID");

                entity.Property(e => e.PropertyServiceFileId).HasColumnName("PROPERTY_SERVICE_FILE_ID");
            });

            modelBuilder.Entity<PimsPropertyServiceFile>(entity =>
            {
                entity.HasKey(e => e.PropertyServiceFileId)
                    .HasName("PRPSVC_PK");

                entity.ToTable("PIMS_PROPERTY_SERVICE_FILE");

                entity.HasIndex(e => e.PropertyServiceFileTypeCode, "PRPSVC_PROPERTY_SERVICE_FILE_TYPE_CODE_IDX");

                entity.Property(e => e.PropertyServiceFileId)
                    .HasColumnName("PROPERTY_SERVICE_FILE_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_SERVICE_FILE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.PropertyServiceFileTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_SERVICE_FILE_TYPE_CODE");

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

                entity.ToTable("PIMS_PROPERTY_SERVICE_FILE_HIST");

                entity.HasIndex(e => new { e.PropertyServiceFileHistId, e.EndDateHist }, "PIMS_PRPSVC_H_UK")
                    .IsUnique();

                entity.Property(e => e.PropertyServiceFileHistId)
                    .HasColumnName("_PROPERTY_SERVICE_FILE_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_SERVICE_FILE_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.PropertyServiceFileId).HasColumnName("PROPERTY_SERVICE_FILE_ID");

                entity.Property(e => e.PropertyServiceFileTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_SERVICE_FILE_TYPE_CODE");
            });

            modelBuilder.Entity<PimsPropertyServiceFileType>(entity =>
            {
                entity.HasKey(e => e.PropertyServiceFileTypeCode)
                    .HasName("PRSVFT_PK");

                entity.ToTable("PIMS_PROPERTY_SERVICE_FILE_TYPE");

                entity.Property(e => e.PropertyServiceFileTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_SERVICE_FILE_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsPropertyStatusType>(entity =>
            {
                entity.HasKey(e => e.PropertyStatusTypeCode)
                    .HasName("PRPSTS_PK");

                entity.ToTable("PIMS_PROPERTY_STATUS_TYPE");

                entity.Property(e => e.PropertyStatusTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_STATUS_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsPropertyTax>(entity =>
            {
                entity.HasKey(e => e.PropertyTaxId)
                    .HasName("PRPTAX_PK");

                entity.ToTable("PIMS_PROPERTY_TAX");

                entity.HasIndex(e => e.PropertyId, "PRPTAX_PROPERTY_ID_IDX");

                entity.HasIndex(e => e.PropertyTaxRemitTypeCode, "PRPTAX_PROPERTY_TAX_REMIT_TYPE_CODE_IDX");

                entity.Property(e => e.PropertyTaxId)
                    .HasColumnName("PROPERTY_TAX_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_TAX_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.BctfaNotificationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("BCTFA_NOTIFICATION_DATE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.LastPaymentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("LAST_PAYMENT_DATE");

                entity.Property(e => e.PaymentAmount)
                    .HasColumnType("money")
                    .HasColumnName("PAYMENT_AMOUNT");

                entity.Property(e => e.PaymentNotes)
                    .HasColumnType("money")
                    .HasColumnName("PAYMENT_NOTES");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

                entity.Property(e => e.PropertyTaxRemitTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TAX_REMIT_TYPE_CODE");

                entity.Property(e => e.TaxFolioNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TAX_FOLIO_NO")
                    .HasComment("Property tax folio number");

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

                entity.ToTable("PIMS_PROPERTY_TAX_HIST");

                entity.HasIndex(e => new { e.PropertyTaxHistId, e.EndDateHist }, "PIMS_PRPTAX_H_UK")
                    .IsUnique();

                entity.Property(e => e.PropertyTaxHistId)
                    .HasColumnName("_PROPERTY_TAX_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_PROPERTY_TAX_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.BctfaNotificationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("BCTFA_NOTIFICATION_DATE");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.LastPaymentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("LAST_PAYMENT_DATE");

                entity.Property(e => e.PaymentAmount)
                    .HasColumnType("money")
                    .HasColumnName("PAYMENT_AMOUNT");

                entity.Property(e => e.PaymentNotes)
                    .HasColumnType("money")
                    .HasColumnName("PAYMENT_NOTES");

                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");

                entity.Property(e => e.PropertyTaxId).HasColumnName("PROPERTY_TAX_ID");

                entity.Property(e => e.PropertyTaxRemitTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TAX_REMIT_TYPE_CODE");

                entity.Property(e => e.TaxFolioNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("TAX_FOLIO_NO");
            });

            modelBuilder.Entity<PimsPropertyTaxRemitType>(entity =>
            {
                entity.HasKey(e => e.PropertyTaxRemitTypeCode)
                    .HasName("PTRMTT_PK");

                entity.ToTable("PIMS_PROPERTY_TAX_REMIT_TYPE");

                entity.HasComment("Description of property tax remittance types");

                entity.Property(e => e.PropertyTaxRemitTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TAX_REMIT_TYPE_CODE")
                    .HasComment("Code value of property tax remittance types");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION")
                    .HasComment("Code description of property tax remittance types");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Is this code value disabled?");
            });

            modelBuilder.Entity<PimsPropertyTenureType>(entity =>
            {
                entity.HasKey(e => e.PropertyTenureTypeCode)
                    .HasName("PRPTNR_PK");

                entity.ToTable("PIMS_PROPERTY_TENURE_TYPE");

                entity.HasComment("A code table to store property tenure codes. Tenure is defined as : \"The act, right, manner or term of holding something(as a landed property)\" In this case, tenure is required on Properties to indicate MoTI's legal tenure on the property. The land parcel still accurately describes the legal title of the land parcel but the individual properties each can have different tenures by MoTI.");

                entity.Property(e => e.PropertyTenureTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TENURE_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsPropertyType>(entity =>
            {
                entity.HasKey(e => e.PropertyTypeCode)
                    .HasName("PRPTYP_PK");

                entity.ToTable("PIMS_PROPERTY_TYPE");

                entity.Property(e => e.PropertyTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("PROPERTY_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsProvinceState>(entity =>
            {
                entity.HasKey(e => e.ProvinceStateId)
                    .HasName("PROVNC_PK");

                entity.ToTable("PIMS_PROVINCE_STATE");

                entity.HasIndex(e => e.CountryId, "PROVNC_COUNTRY_ID_IDX");

                entity.Property(e => e.ProvinceStateId)
                    .ValueGeneratedNever()
                    .HasColumnName("PROVINCE_STATE_ID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CountryId).HasColumnName("COUNTRY_ID");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.ProvinceStateCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("PROVINCE_STATE_CODE");

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

                entity.ToTable("PIMS_REGION");

                entity.Property(e => e.RegionCode)
                    .ValueGeneratedNever()
                    .HasColumnName("REGION_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.RegionName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("REGION_NAME");
            });

            modelBuilder.Entity<PimsRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("ROLE_PK");

                entity.ToTable("PIMS_ROLE");

                entity.HasIndex(e => e.KeycloakGroupId, "ROLE_KEYCLOAK_GROUP_ID_IDX");

                entity.HasIndex(e => e.RoleUid, "ROLE_ROLE_UID_IDX");

                entity.Property(e => e.RoleId)
                    .HasColumnName("ROLE_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ROLE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.IsPublic)
                    .IsRequired()
                    .HasColumnName("IS_PUBLIC")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.KeycloakGroupId).HasColumnName("KEYCLOAK_GROUP_ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("NAME");

                entity.Property(e => e.RoleUid).HasColumnName("ROLE_UID");

                entity.Property(e => e.SortOrder).HasColumnName("SORT_ORDER");
            });

            modelBuilder.Entity<PimsRoleClaim>(entity =>
            {
                entity.HasKey(e => e.RoleClaimId)
                    .HasName("ROLCLM_PK");

                entity.ToTable("PIMS_ROLE_CLAIM");

                entity.HasIndex(e => e.ClaimId, "ROLCLM_CLAIM_ID_IDX");

                entity.HasIndex(e => new { e.RoleId, e.ClaimId }, "ROLCLM_ROLE_CLAIM_TUC")
                    .IsUnique();

                entity.HasIndex(e => e.RoleId, "ROLCLM_ROLE_ID_IDX");

                entity.Property(e => e.RoleClaimId)
                    .HasColumnName("ROLE_CLAIM_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ROLE_CLAIM_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ClaimId).HasColumnName("CLAIM_ID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

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

                entity.ToTable("PIMS_ROLE_CLAIM_HIST");

                entity.HasIndex(e => new { e.RoleClaimHistId, e.EndDateHist }, "PIMS_ROLCLM_H_UK")
                    .IsUnique();

                entity.Property(e => e.RoleClaimHistId)
                    .HasColumnName("_ROLE_CLAIM_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ROLE_CLAIM_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ClaimId).HasColumnName("CLAIM_ID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.RoleClaimId).HasColumnName("ROLE_CLAIM_ID");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");
            });

            modelBuilder.Entity<PimsRoleHist>(entity =>
            {
                entity.HasKey(e => e.RoleHistId)
                    .HasName("PIMS_ROLE_H_PK");

                entity.ToTable("PIMS_ROLE_HIST");

                entity.HasIndex(e => new { e.RoleHistId, e.EndDateHist }, "PIMS_ROLE_H_UK")
                    .IsUnique();

                entity.Property(e => e.RoleHistId)
                    .HasColumnName("_ROLE_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_ROLE_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.IsPublic).HasColumnName("IS_PUBLIC");

                entity.Property(e => e.KeycloakGroupId).HasColumnName("KEYCLOAK_GROUP_ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("NAME");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.RoleUid).HasColumnName("ROLE_UID");

                entity.Property(e => e.SortOrder).HasColumnName("SORT_ORDER");
            });

            modelBuilder.Entity<PimsSecDepHolderType>(entity =>
            {
                entity.HasKey(e => e.SecDepHolderTypeCode)
                    .HasName("SCHLDT_PK");

                entity.ToTable("PIMS_SEC_DEP_HOLDER_TYPE");

                entity.Property(e => e.SecDepHolderTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("SEC_DEP_HOLDER_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsSecurityDeposit>(entity =>
            {
                entity.HasKey(e => e.SecurityDepositId)
                    .HasName("SECDEP_PK");

                entity.ToTable("PIMS_SECURITY_DEPOSIT");

                entity.HasIndex(e => e.LeaseId, "SECDEP_LEASE_ID_IDX");

                entity.HasIndex(e => e.SecurityDepositTypeCode, "SECDEP_SECURITY_DEPOSIT_TYPE_CODE_IDX");

                entity.HasIndex(e => e.SecDepHolderTypeCode, "SECDEP_SEC_DEP_HOLDER_TYPE_CODE_IDX");

                entity.Property(e => e.SecurityDepositId)
                    .HasColumnName("SECURITY_DEPOSIT_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_ID_SEQ])");

                entity.Property(e => e.AmountPaid)
                    .HasColumnType("money")
                    .HasColumnName("AMOUNT_PAID");

                entity.Property(e => e.AnnualInterestRate)
                    .HasColumnType("numeric(5, 2)")
                    .HasColumnName("ANNUAL_INTEREST_RATE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DepositDate)
                    .HasColumnType("date")
                    .HasColumnName("DEPOSIT_DATE");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.OtherDepHolderTypeDesc)
                    .HasMaxLength(100)
                    .HasColumnName("OTHER_DEP_HOLDER_TYPE_DESC");

                entity.Property(e => e.SecDepHolderTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SEC_DEP_HOLDER_TYPE_CODE");

                entity.Property(e => e.SecurityDepositTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SECURITY_DEPOSIT_TYPE_CODE");

                entity.HasOne(d => d.Lease)
                    .WithMany(p => p.PimsSecurityDeposits)
                    .HasForeignKey(d => d.LeaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LEASE_PIM_SECDEP_FK");

                entity.HasOne(d => d.SecDepHolderTypeCodeNavigation)
                    .WithMany(p => p.PimsSecurityDeposits)
                    .HasForeignKey(d => d.SecDepHolderTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_SCHLDT_PIM_SECDEP_FK");

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

                entity.ToTable("PIMS_SECURITY_DEPOSIT_HIST");

                entity.HasIndex(e => new { e.SecurityDepositHistId, e.EndDateHist }, "PIMS_SECDEP_H_UK")
                    .IsUnique();

                entity.Property(e => e.SecurityDepositHistId)
                    .HasColumnName("_SECURITY_DEPOSIT_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_H_ID_SEQ])");

                entity.Property(e => e.AmountPaid)
                    .HasColumnType("money")
                    .HasColumnName("AMOUNT_PAID");

                entity.Property(e => e.AnnualInterestRate)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("ANNUAL_INTEREST_RATE");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.DepositDate)
                    .HasColumnType("date")
                    .HasColumnName("DEPOSIT_DATE");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.OtherDepHolderTypeDesc)
                    .HasMaxLength(100)
                    .HasColumnName("OTHER_DEP_HOLDER_TYPE_DESC");

                entity.Property(e => e.SecDepHolderTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SEC_DEP_HOLDER_TYPE_CODE");

                entity.Property(e => e.SecurityDepositId).HasColumnName("SECURITY_DEPOSIT_ID");

                entity.Property(e => e.SecurityDepositTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SECURITY_DEPOSIT_TYPE_CODE");
            });

            modelBuilder.Entity<PimsSecurityDepositReturn>(entity =>
            {
                entity.HasKey(e => e.SecurityDepositReturnId)
                    .HasName("SDRTRN_PK");

                entity.ToTable("PIMS_SECURITY_DEPOSIT_RETURN");

                entity.HasIndex(e => e.LeaseId, "SDRTRN_LEASE_ID_IDX");

                entity.HasIndex(e => e.SecurityDepositTypeCode, "SDRTRN_SECURITY_DEPOSIT_TYPE_CODE_IDX");

                entity.Property(e => e.SecurityDepositReturnId)
                    .HasColumnName("SECURITY_DEPOSIT_RETURN_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_RETURN_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ChequeNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("CHEQUE_NUMBER")
                    .HasComment("Cheque number of the deposit return");

                entity.Property(e => e.ClaimsAgainst)
                    .HasColumnType("money")
                    .HasColumnName("CLAIMS_AGAINST")
                    .HasComment("Amount of claims against the deposit");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DepositTotal)
                    .HasColumnType("money")
                    .HasColumnName("DEPOSIT_TOTAL")
                    .HasComment("Total amount of the pet/security deposit (including interest)");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.PayeeAddress)
                    .HasMaxLength(500)
                    .HasColumnName("PAYEE_ADDRESS")
                    .HasComment("Address of cheque recipient");

                entity.Property(e => e.PayeeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("PAYEE_NAME")
                    .HasComment("Name of cheque recipient");

                entity.Property(e => e.ReturnAmount)
                    .HasColumnType("money")
                    .HasColumnName("RETURN_AMOUNT")
                    .HasComment("Amount returned minus claims");

                entity.Property(e => e.ReturnDate)
                    .HasColumnType("datetime")
                    .HasColumnName("RETURN_DATE")
                    .HasComment("Date of deposit return");

                entity.Property(e => e.SecurityDepositTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SECURITY_DEPOSIT_TYPE_CODE");

                entity.Property(e => e.TerminationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("TERMINATION_DATE")
                    .HasComment("Date the lease/license was terminated or surrendered");

                entity.HasOne(d => d.Lease)
                    .WithMany(p => p.PimsSecurityDepositReturns)
                    .HasForeignKey(d => d.LeaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PIM_LEASE_PIM_SDRTRN_FK");

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

                entity.ToTable("PIMS_SECURITY_DEPOSIT_RETURN_HIST");

                entity.HasIndex(e => new { e.SecurityDepositReturnHistId, e.EndDateHist }, "PIMS_SDRTRN_H_UK")
                    .IsUnique();

                entity.Property(e => e.SecurityDepositReturnHistId)
                    .HasColumnName("_SECURITY_DEPOSIT_RETURN_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_SECURITY_DEPOSIT_RETURN_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ChequeNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("CHEQUE_NUMBER");

                entity.Property(e => e.ClaimsAgainst)
                    .HasColumnType("money")
                    .HasColumnName("CLAIMS_AGAINST");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.DepositTotal)
                    .HasColumnType("money")
                    .HasColumnName("DEPOSIT_TOTAL");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.LeaseId).HasColumnName("LEASE_ID");

                entity.Property(e => e.PayeeAddress)
                    .HasMaxLength(500)
                    .HasColumnName("PAYEE_ADDRESS");

                entity.Property(e => e.PayeeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("PAYEE_NAME");

                entity.Property(e => e.ReturnAmount)
                    .HasColumnType("money")
                    .HasColumnName("RETURN_AMOUNT");

                entity.Property(e => e.ReturnDate)
                    .HasColumnType("datetime")
                    .HasColumnName("RETURN_DATE");

                entity.Property(e => e.SecurityDepositReturnId).HasColumnName("SECURITY_DEPOSIT_RETURN_ID");

                entity.Property(e => e.SecurityDepositTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("SECURITY_DEPOSIT_TYPE_CODE");

                entity.Property(e => e.TerminationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("TERMINATION_DATE");
            });

            modelBuilder.Entity<PimsSecurityDepositType>(entity =>
            {
                entity.HasKey(e => e.SecurityDepositTypeCode)
                    .HasName("SECDPT_PK");

                entity.ToTable("PIMS_SECURITY_DEPOSIT_TYPE");

                entity.Property(e => e.SecurityDepositTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("SECURITY_DEPOSIT_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsStaticVariable>(entity =>
            {
                entity.HasKey(e => e.StaticVariableName)
                    .HasName("STAVBL_PK");

                entity.ToTable("PIMS_STATIC_VARIABLE");

                entity.Property(e => e.StaticVariableName)
                    .HasMaxLength(100)
                    .HasColumnName("STATIC_VARIABLE_NAME");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.StaticVariableValue)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("STATIC_VARIABLE_VALUE");
            });

            modelBuilder.Entity<PimsSurplusDeclarationType>(entity =>
            {
                entity.HasKey(e => e.SurplusDeclarationTypeCode)
                    .HasName("SPDCLT_PK");

                entity.ToTable("PIMS_SURPLUS_DECLARATION_TYPE");

                entity.HasComment("Description of the surplus property type.");

                entity.Property(e => e.SurplusDeclarationTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("SURPLUS_DECLARATION_TYPE_CODE")
                    .HasComment("Code value of the surplus property type");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION")
                    .HasComment("Code description of the surplus property type");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))")
                    .HasComment("Indicates that the code value is disabled");
            });

            modelBuilder.Entity<PimsTask>(entity =>
            {
                entity.HasKey(e => e.TaskId)
                    .HasName("TASK_PK");

                entity.ToTable("PIMS_TASK");

                entity.HasIndex(e => e.ActivityId, "TASK_ACTIVITY_ID_IDX");

                entity.HasIndex(e => e.TaskTemplateId, "TASK_TASK_TEMPLATE_ID_IDX");

                entity.HasIndex(e => new { e.UserId, e.ActivityId, e.TaskTemplateId }, "TASK_TEMPLATE_ACTIVITY_USER_TUC")
                    .IsUnique();

                entity.HasIndex(e => e.UserId, "TASK_USER_ID_IDX");

                entity.Property(e => e.TaskId)
                    .HasColumnName("TASK_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TASK_ID_SEQ])");

                entity.Property(e => e.ActivityId).HasColumnName("ACTIVITY_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.TaskTemplateId).HasColumnName("TASK_TEMPLATE_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

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

                entity.ToTable("PIMS_TASK_HIST");

                entity.HasIndex(e => new { e.TaskHistId, e.EndDateHist }, "PIMS_TASK_H_UK")
                    .IsUnique();

                entity.Property(e => e.TaskHistId)
                    .HasColumnName("_TASK_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TASK_H_ID_SEQ])");

                entity.Property(e => e.ActivityId).HasColumnName("ACTIVITY_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.TaskId).HasColumnName("TASK_ID");

                entity.Property(e => e.TaskTemplateId).HasColumnName("TASK_TEMPLATE_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");
            });

            modelBuilder.Entity<PimsTaskTemplate>(entity =>
            {
                entity.HasKey(e => e.TaskTemplateId)
                    .HasName("TSKTMP_PK");

                entity.ToTable("PIMS_TASK_TEMPLATE");

                entity.HasIndex(e => e.TaskTemplateTypeCode, "TSKTMP_TASK_TEMPLATE_TYPE_CODE_IDX");

                entity.Property(e => e.TaskTemplateId)
                    .HasColumnName("TASK_TEMPLATE_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TASK_TEMPLATE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.TaskTemplateTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("TASK_TEMPLATE_TYPE_CODE");

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

                entity.ToTable("PIMS_TASK_TEMPLATE_ACTIVITY_MODEL");

                entity.HasIndex(e => e.ActivityModelId, "TSKTAM_ACTIVITY_MODEL_ID_IDX");

                entity.HasIndex(e => new { e.TaskTemplateId, e.ActivityModelId }, "TSKTAM_TASK_TEMPLATE_ACTIVITY_MODEL_TUC")
                    .IsUnique();

                entity.HasIndex(e => e.TaskTemplateId, "TSKTAM_TASK_TEMPLATE_ID_IDX");

                entity.Property(e => e.TaskTemplateActivityModelId)
                    .HasColumnName("TASK_TEMPLATE_ACTIVITY_MODEL_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TASK_TEMPLATE_ACTIVITY_MODEL_ID_SEQ])");

                entity.Property(e => e.ActivityModelId).HasColumnName("ACTIVITY_MODEL_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ImplementationOrder).HasColumnName("IMPLEMENTATION_ORDER");

                entity.Property(e => e.IsDisabled)
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.IsMandatory)
                    .IsRequired()
                    .HasColumnName("IS_MANDATORY")
                    .HasDefaultValueSql("(CONVERT([bit],(1)))");

                entity.Property(e => e.TaskTemplateId).HasColumnName("TASK_TEMPLATE_ID");

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

                entity.ToTable("PIMS_TASK_TEMPLATE_ACTIVITY_MODEL_HIST");

                entity.HasIndex(e => new { e.TaskTemplateActivityModelHistId, e.EndDateHist }, "PIMS_TSKTAM_H_UK")
                    .IsUnique();

                entity.Property(e => e.TaskTemplateActivityModelHistId)
                    .HasColumnName("_TASK_TEMPLATE_ACTIVITY_MODEL_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TASK_TEMPLATE_ACTIVITY_MODEL_H_ID_SEQ])");

                entity.Property(e => e.ActivityModelId).HasColumnName("ACTIVITY_MODEL_ID");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.ImplementationOrder).HasColumnName("IMPLEMENTATION_ORDER");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.IsMandatory).HasColumnName("IS_MANDATORY");

                entity.Property(e => e.TaskTemplateActivityModelId).HasColumnName("TASK_TEMPLATE_ACTIVITY_MODEL_ID");

                entity.Property(e => e.TaskTemplateId).HasColumnName("TASK_TEMPLATE_ID");
            });

            modelBuilder.Entity<PimsTaskTemplateHist>(entity =>
            {
                entity.HasKey(e => e.TaskTemplateHistId)
                    .HasName("PIMS_TSKTMP_H_PK");

                entity.ToTable("PIMS_TASK_TEMPLATE_HIST");

                entity.HasIndex(e => new { e.TaskTemplateHistId, e.EndDateHist }, "PIMS_TSKTMP_H_UK")
                    .IsUnique();

                entity.Property(e => e.TaskTemplateHistId)
                    .HasColumnName("_TASK_TEMPLATE_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TASK_TEMPLATE_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.TaskTemplateId).HasColumnName("TASK_TEMPLATE_ID");

                entity.Property(e => e.TaskTemplateTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("TASK_TEMPLATE_TYPE_CODE");
            });

            modelBuilder.Entity<PimsTaskTemplateType>(entity =>
            {
                entity.HasKey(e => e.TaskTemplateTypeCode)
                    .HasName("TSKTMT_PK");

                entity.ToTable("PIMS_TASK_TEMPLATE_TYPE");

                entity.Property(e => e.TaskTemplateTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("TASK_TEMPLATE_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsTenant>(entity =>
            {
                entity.HasKey(e => e.TenantId)
                    .HasName("PK__PIMS_TEN__5E0E988A42D52538");

                entity.ToTable("PIMS_TENANT");

                entity.Property(e => e.TenantId)
                    .HasColumnName("TENANT_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_TENANT_ID_SEQ])");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("NAME");

                entity.Property(e => e.Settings)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .HasColumnName("SETTINGS");
            });

            modelBuilder.Entity<PimsUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("USER_PK");

                entity.ToTable("PIMS_USER");

                entity.HasIndex(e => e.BusinessIdentifierValue, "USER_BUSINESS_IDENTIFIER_VALUE_IDX");

                entity.HasIndex(e => e.GuidIdentifierValue, "USER_GUID_IDENTIFIER_VALUE_IDX");

                entity.HasIndex(e => e.PersonId, "USER_PERSON_ID_IDX");

                entity.Property(e => e.UserId)
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ApprovedById)
                    .HasMaxLength(30)
                    .HasColumnName("APPROVED_BY_ID");

                entity.Property(e => e.BusinessIdentifierValue)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("BUSINESS_IDENTIFIER_VALUE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("EXPIRY_DATE");

                entity.Property(e => e.GuidIdentifierValue).HasColumnName("GUID_IDENTIFIER_VALUE");

                entity.Property(e => e.IsDisabled)
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.IssueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ISSUE_DATE");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("LAST_LOGIN");

                entity.Property(e => e.Note)
                    .HasMaxLength(1000)
                    .HasColumnName("NOTE");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");

                entity.Property(e => e.Position)
                    .HasMaxLength(100)
                    .HasColumnName("POSITION");

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

                entity.ToTable("PIMS_USER_HIST");

                entity.HasIndex(e => new { e.UserHistId, e.EndDateHist }, "PIMS_USER_H_UK")
                    .IsUnique();

                entity.Property(e => e.UserHistId)
                    .HasColumnName("_USER_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ApprovedById)
                    .HasMaxLength(30)
                    .HasColumnName("APPROVED_BY_ID");

                entity.Property(e => e.BusinessIdentifierValue)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("BUSINESS_IDENTIFIER_VALUE");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("EXPIRY_DATE");

                entity.Property(e => e.GuidIdentifierValue).HasColumnName("GUID_IDENTIFIER_VALUE");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.IssueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ISSUE_DATE");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("datetime")
                    .HasColumnName("LAST_LOGIN");

                entity.Property(e => e.Note)
                    .HasMaxLength(1000)
                    .HasColumnName("NOTE");

                entity.Property(e => e.PersonId).HasColumnName("PERSON_ID");

                entity.Property(e => e.Position)
                    .HasMaxLength(100)
                    .HasColumnName("POSITION");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");
            });

            modelBuilder.Entity<PimsUserOrganization>(entity =>
            {
                entity.HasKey(e => e.UserOrganizationId)
                    .HasName("USRORG_PK");

                entity.ToTable("PIMS_USER_ORGANIZATION");

                entity.HasIndex(e => e.OrganizationId, "USRORG_ORGANIZATION_ID_IDX");

                entity.HasIndex(e => e.RoleId, "USRORG_ROLE_ID_IDX");

                entity.HasIndex(e => e.UserId, "USRORG_USER_ID_IDX");

                entity.HasIndex(e => new { e.OrganizationId, e.UserId, e.RoleId }, "USRORG_USER_ROLE_ORGANIZATION_TUC")
                    .IsUnique();

                entity.Property(e => e.UserOrganizationId)
                    .HasColumnName("USER_ORGANIZATION_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ORGANIZATION_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

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

                entity.ToTable("PIMS_USER_ORGANIZATION_HIST");

                entity.HasIndex(e => new { e.UserOrganizationHistId, e.EndDateHist }, "PIMS_USRORG_H_UK")
                    .IsUnique();

                entity.Property(e => e.UserOrganizationHistId)
                    .HasColumnName("_USER_ORGANIZATION_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ORGANIZATION_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.OrganizationId).HasColumnName("ORGANIZATION_ID");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.UserOrganizationId).HasColumnName("USER_ORGANIZATION_ID");
            });

            modelBuilder.Entity<PimsUserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId)
                    .HasName("USERRL_PK");

                entity.ToTable("PIMS_USER_ROLE");

                entity.HasIndex(e => e.RoleId, "USERRL_ROLE_ID_IDX");

                entity.HasIndex(e => e.UserId, "USERRL_USER_ID_IDX");

                entity.HasIndex(e => new { e.UserId, e.RoleId }, "USERRL_USER_ROLE_TUC")
                    .IsUnique();

                entity.Property(e => e.UserRoleId)
                    .HasColumnName("USER_ROLE_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ROLE_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

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

                entity.ToTable("PIMS_USER_ROLE_HIST");

                entity.HasIndex(e => new { e.UserRoleHistId, e.EndDateHist }, "PIMS_USERRL_H_UK")
                    .IsUnique();

                entity.Property(e => e.UserRoleHistId)
                    .HasColumnName("_USER_ROLE_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_USER_ROLE_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");

                entity.Property(e => e.UserId).HasColumnName("USER_ID");

                entity.Property(e => e.UserRoleId).HasColumnName("USER_ROLE_ID");
            });

            modelBuilder.Entity<PimsWorkflowModel>(entity =>
            {
                entity.HasKey(e => e.WorkflowModelId)
                    .HasName("WFLMDL_PK");

                entity.ToTable("PIMS_WORKFLOW_MODEL");

                entity.HasIndex(e => e.WorkflowModelTypeCode, "WFLMDL_WORKFLOW_MODEL_TYPE_CODE_IDX");

                entity.Property(e => e.WorkflowModelId)
                    .HasColumnName("WORKFLOW_MODEL_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_WORKFLOW_MODEL_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.WorkflowModelTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("WORKFLOW_MODEL_TYPE_CODE");

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

                entity.ToTable("PIMS_WORKFLOW_MODEL_HIST");

                entity.HasIndex(e => new { e.WorkflowModelHistId, e.EndDateHist }, "PIMS_WFLMDL_H_UK")
                    .IsUnique();

                entity.Property(e => e.WorkflowModelHistId)
                    .HasColumnName("_WORKFLOW_MODEL_HIST_ID")
                    .HasDefaultValueSql("(NEXT VALUE FOR [PIMS_WORKFLOW_MODEL_H_ID_SEQ])");

                entity.Property(e => e.AppCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_CREATE_TIMESTAMP");

                entity.Property(e => e.AppCreateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USER_DIRECTORY");

                entity.Property(e => e.AppCreateUserGuid).HasColumnName("APP_CREATE_USER_GUID");

                entity.Property(e => e.AppCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_CREATE_USERID");

                entity.Property(e => e.AppLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("APP_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.AppLastUpdateUserDirectory)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USER_DIRECTORY");

                entity.Property(e => e.AppLastUpdateUserGuid).HasColumnName("APP_LAST_UPDATE_USER_GUID");

                entity.Property(e => e.AppLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("APP_LAST_UPDATE_USERID");

                entity.Property(e => e.ConcurrencyControlNumber).HasColumnName("CONCURRENCY_CONTROL_NUMBER");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID");

                entity.Property(e => e.EffectiveDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("EFFECTIVE_DATE_HIST")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EndDateHist)
                    .HasColumnType("datetime")
                    .HasColumnName("END_DATE_HIST");

                entity.Property(e => e.IsDisabled).HasColumnName("IS_DISABLED");

                entity.Property(e => e.WorkflowModelId).HasColumnName("WORKFLOW_MODEL_ID");

                entity.Property(e => e.WorkflowModelTypeCode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("WORKFLOW_MODEL_TYPE_CODE");
            });

            modelBuilder.Entity<PimsWorkflowModelType>(entity =>
            {
                entity.HasKey(e => e.WorkflowModelTypeCode)
                    .HasName("WFLMDT_PK");

                entity.ToTable("PIMS_WORKFLOW_MODEL_TYPE");

                entity.Property(e => e.WorkflowModelTypeCode)
                    .HasMaxLength(20)
                    .HasColumnName("WORKFLOW_MODEL_TYPE_CODE");

                entity.Property(e => e.ConcurrencyControlNumber)
                    .HasColumnName("CONCURRENCY_CONTROL_NUMBER")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DbCreateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_CREATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbCreateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_CREATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.DbLastUpdateTimestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("DB_LAST_UPDATE_TIMESTAMP")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DbLastUpdateUserid)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("DB_LAST_UPDATE_USERID")
                    .HasDefaultValueSql("(user_name())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayOrder).HasColumnName("DISPLAY_ORDER");

                entity.Property(e => e.IsDisabled)
                    .IsRequired()
                    .HasColumnName("IS_DISABLED")
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");
            });

            modelBuilder.Entity<PimsxTableDefinition>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PIMSX_TableDefinitions");

                entity.Property(e => e.Description).HasColumnName("DESCRIPTION");

                entity.Property(e => e.HistRequired)
                    .HasMaxLength(1)
                    .HasColumnName("HIST_REQUIRED");

                entity.Property(e => e.TableAlias)
                    .HasMaxLength(255)
                    .HasColumnName("TABLE_ALIAS");

                entity.Property(e => e.TableName)
                    .HasMaxLength(255)
                    .HasColumnName("TABLE_NAME");
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

            modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_RETURN_H_ID_SEQ")
                .HasMin(1)
                .HasMax(2147483647);

            modelBuilder.HasSequence("PIMS_SECURITY_DEPOSIT_RETURN_ID_SEQ")
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
