using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// LeaseConfiguration class, provides a way to configure leases in the database.
    ///</summary>
    public class LeaseConfiguration : BaseAppEntityConfiguration<Lease>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Lease> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.PropertyManagementOrganizationId)
                .HasComment("Foreign key to property management organization");
            builder.Property(m => m.PurposeTypeId)
                .HasComment("Foreign key to lease purpose type");
            builder.Property(m => m.PurposeSubtypeId)
                .HasComment("Foreign key to lease purpose subtype");
            builder.Property(m => m.StatusTypeId)
                .HasComment("Foreign key to lease status type");
            builder.Property(m => m.PaymentFrequencyTypeId)
                .HasComment("Foreign key to lease payment frequency type");
            builder.Property(m => m.ProgramTypeId)
                .HasComment("Foreign key to lease program type");
            builder.Property(m => m.PropertyManagerId)
                .HasComment("Foreign key to lease property manager person");
            builder.Property(m => m.TenantId)
                .HasComment("Foreign key to lease tenant person");

            builder.Property(m => m.LFileNo)
                .HasMaxLength(50)
                .HasComment("The LIS File #");
            builder.Property(m => m.TfaFileNo)
                .HasComment("The TFA File #");
            builder.Property(m => m.PsFileNo)
                .HasMaxLength(50)
                .HasComment("The PS File #");
            builder.Property(m => m.StartDate)
                .HasColumnType("DATETIME")
                .HasComment("The date this lease starts");
            builder.Property(m => m.RenewalDate)
                .HasColumnType("DATETIME")
                .HasComment("The date this lease renews");
            builder.Property(m => m.ExpiryDate)
                .HasColumnType("DATETIME")
                .HasComment("The date this lease expires");
            builder.Property(m => m.Amount)
                .HasMaxLength(40)
                .HasComment("The amount of the lease");
            builder.Property(m => m.InsuranceStartDate)
                .HasColumnType("DATETIME")
                .HasComment("The date this lease insurance starts");
            builder.Property(m => m.InsuranceEndDate)
                .HasColumnType("DATETIME")
                .HasComment("The date this lease insurance ends");
            builder.Property(m => m.SecurityStartDate)
                .HasColumnType("DATETIME")
                .HasComment("The date this lease security starts");
            builder.Property(m => m.SecurityEndDate)
                .HasColumnType("DATETIME")
                .HasComment("The date this lease security ends");
            builder.Property(m => m.InspectionDate)
                .HasColumnType("DATETIME")
                .HasComment("The date the property will be inspected");
            builder.Property(m => m.InspectionNote)
                .HasMaxLength(4000)
                .HasComment("A note on the inspection");
            builder.Property(m => m.Note)
                .HasMaxLength(4000)
                .HasComment("A note on the lease");
            builder.Property(m => m.Unit)
                .HasMaxLength(2000)
                .HasComment("A description of the unit");
            builder.Property(m => m.IsExpired)
                .HasDefaultValue(false)
                .HasComment("Whether this lease has expired");
            builder.Property(m => m.HasPhysicalFile)
                .HasDefaultValue(false)
                .HasComment("Whether this lease has a physical file");
            builder.Property(m => m.HasDigitalFile)
                .HasDefaultValue(false)
                .HasComment("Whether this lease has a digital file");
            builder.Property(m => m.HasPhysicalLicense)
                .HasDefaultValue(false)
                .HasComment("Whether this lease has a physical license");
            builder.Property(m => m.HasDigitalLicense)
                .HasDefaultValue(false)
                .HasComment("Whether this lease has a digital license");

            builder.HasOne(m => m.PropertyManagementOrganization).WithMany(m => m.Leases).HasForeignKey(m => m.PropertyManagementOrganizationId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_ORG_PIM_LEASE_FK");
            builder.HasOne(m => m.PurposeType).WithMany(m => m.Leases).HasForeignKey(m => m.PurposeTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LSPRTY_PIM_LEASE_FK");
            builder.HasOne(m => m.PurposeSubtype).WithMany(m => m.Leases).HasForeignKey(m => m.PurposeSubtypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LSPRST_PIM_LEASE_FK");
            builder.HasOne(m => m.StatusType).WithMany(m => m.Leases).HasForeignKey(m => m.StatusTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LSSTSY_PIM_LEASE_FK");
            builder.HasOne(m => m.PaymentFrequencyType).WithMany(m => m.Leases).HasForeignKey(m => m.PaymentFrequencyTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LSPMTF_PIM_LEASE_FK");
            builder.HasOne(m => m.ProgramType).WithMany(m => m.Leases).HasForeignKey(m => m.ProgramTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LSPRGT_PIM_LEASE_FK");
            builder.HasOne(m => m.PropertyManager).WithMany().HasForeignKey(m => m.PropertyManagerId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PERSON_PIM_LEASE_PM_CONTACT_FK");
            builder.HasOne(m => m.Tenant).WithMany().HasForeignKey(m => m.TenantId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PERSON_PIM_LEASE_TENANT_FK");

            builder.HasMany(m => m.Properties).WithMany(m => m.Leases).UsingEntity<PropertyLease>(
                m => m.HasOne(m => m.Property).WithMany(m => m.LeasesManyToMany).HasForeignKey(m => m.PropertyId),
                m => m.HasOne(m => m.Lease).WithMany(m => m.PropertiesManyToMany).HasForeignKey(m => m.LeaseId)
            );

            builder.HasIndex(m => m.PaymentFrequencyTypeId).HasDatabaseName("LEASE_LEASE_PMT_FREQ_TYPE_CODE_IDX");
            builder.HasIndex(m => m.ProgramTypeId).HasDatabaseName("LEASE_LEASE_PROGRAM_TYPE_CODE_IDX");
            builder.HasIndex(m => m.PurposeSubtypeId).HasDatabaseName("LEASE_LEASE_PURPOSE_SUBTYPE_CODE_IDX");
            builder.HasIndex(m => m.PurposeTypeId).HasDatabaseName("LEASE_LEASE_PURPOSE_TYPE_CODE_IDX");
            builder.HasIndex(m => m.StatusTypeId).HasDatabaseName("LEASE_LEASE_STATUS_TYPE_CODE_IDX");
            builder.HasIndex(m => m.PropertyManagementOrganizationId).HasDatabaseName("LEASE_PROP_MGMT_ORG_ID_IDX");
            builder.HasIndex(m => m.PropertyManagerId).HasDatabaseName("LEASE_PROPERTY_MANAGER_ID_IDX");
            builder.HasIndex(m => m.TenantId).HasDatabaseName("LEASE_TENANT_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
