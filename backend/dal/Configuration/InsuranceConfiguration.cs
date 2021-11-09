using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// InsuranceConfiguration class, provides a way to configure insurance in the database.
    ///</summary>
    public class InsuranceConfiguration : BaseAppEntityConfiguration<Insurance>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Insurance> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.LeaseId)
                .HasComment("Foreign key to lease");
            builder.Property(m => m.InsurerOrganizationId)
                .HasComment("Foreign key to organization");
            builder.Property(m => m.InsurerContactId)
                .HasComment("Foreign key to person");
            builder.Property(m => m.MotiRiskManagementContactId)
                .HasComment("Foreign key to person");
            builder.Property(m => m.BctfaRiskManagementContactId)
                .HasComment("Foreign key to person");

            builder.Property(m => m.InsuranceTypeId)
                .HasMaxLength(20)
                .IsRequired()
                .HasComment("Foreign key to insurance type");
            builder.Property(m => m.InsurancePayeeTypeId)
                .HasMaxLength(20)
                .IsRequired()
                .HasComment("Foreign key to insurance payee type");

            builder.Property(m => m.OtherInsuranceType)
                .HasMaxLength(200)
                .HasComment("The description of the insurance type if the type is other");
            builder.Property(m => m.CoverageDescription)
                .HasMaxLength(2000)
                .HasComment("The description of the insurance coverage");
            builder.Property(m => m.CoverageLimit)
                .HasColumnType("MONEY")
                .HasDefaultValue(0)
                .HasComment("The coverage limit of this insurance");
            builder.Property(m => m.InsuredValue)
                .HasColumnType("MONEY")
                .HasComment("The insured value");
            builder.Property(m => m.StartDate)
                .HasColumnType("Date")
                .HasComment("The effective start date of the insurance coverage");
            builder.Property(m => m.ExpiryDate)
                .HasColumnType("Date")
                .HasComment("The effective end date of the insurance coverage");
            builder.Property(m => m.RiskAssessmentCompletedDate)
                .HasColumnType("DateTime")
                .HasComment("The optional date the risk assessment was completed");

            builder.HasOne(m => m.Lease).WithMany(m => m.Insurances).HasForeignKey(m => m.LeaseId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LEASE_PIM_INSRNC_FK");
            builder.HasOne(m => m.InsurerOrganization).WithMany().HasForeignKey(m => m.InsurerOrganizationId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_ORG_PIM_INSRNC_FK");
            builder.HasOne(m => m.InsurerContact).WithMany().HasForeignKey(m => m.InsurerContactId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PERSON_PIM_INSRNC_INSURER_CONTACT_FK");
            builder.HasOne(m => m.MotiRiskManagementContact).WithMany().HasForeignKey(m => m.MotiRiskManagementContactId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PERSON_PIM_INSRNCMOTI_CONTACT_FK");
            builder.HasOne(m => m.BctfaRiskManagementContact).WithMany().HasForeignKey(m => m.BctfaRiskManagementContactId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PERSON_PIM_INSRNC_BCTFA_CONTACT_FK");
            builder.HasOne(m => m.InsuranceType).WithMany(m => m.Insurances).HasForeignKey(m => m.InsuranceTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_INSPYT_PIM_INSRNC_FK");
            builder.HasOne(m => m.InsurancePayeeType).WithMany(m => m.Insurances).HasForeignKey(m => m.InsurancePayeeTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_INSPAY_PIM_INSRNC_FK");

            builder.HasIndex(m => m.MotiRiskManagementContactId).HasDatabaseName("INSRNC_MOTI_RISK_MGMT_CONTACT_ID_IDX");
            builder.HasIndex(m => m.BctfaRiskManagementContactId).HasDatabaseName("INSRNC_BCTFA_RISK_MGMT_CONTACT_ID_IDX");
            builder.HasIndex(m => m.InsurancePayeeTypeId).HasDatabaseName("INSRNC_INSURANCE_PAYEE_TYPE_CODE_IDX");
            builder.HasIndex(m => m.InsuranceTypeId).HasDatabaseName("INSRNC_INSURANCE_TYPE_CODE_IDX");
            builder.HasIndex(m => m.InsurerContactId).HasDatabaseName("INSRNC_INSURER_CONTACT_ID_IDX");
            builder.HasIndex(m => m.InsurerOrganizationId).HasDatabaseName("INSRNC_INSURER_ORG_ID_IDX");
            builder.HasIndex(m => m.LeaseId).HasDatabaseName("INSRNC_LEASE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
