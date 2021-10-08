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

            builder.Property(m => m.PurposeTypeId)
                .IsRequired()
                .HasComment("Foreign key to lease purpose type");
            builder.Property(m => m.StatusTypeId)
                .HasComment("Foreign key to lease status type");
            builder.Property(m => m.PaymentFrequencyTypeId)
                .IsRequired()
                .HasComment("Foreign key to lease payment frequency type");
            builder.Property(m => m.ProgramTypeId)
                .IsRequired()
                .HasComment("Foreign key to lease program type");
            builder.Property(m => m.MotiNameId)
                .IsRequired()
                .HasComment("Foreign key to lease MOTI person");
            builder.Property(m => m.CategoryTypeId)
                .IsRequired()
                .HasComment("Foreign key to lease category type");
            builder.Property(m => m.LeaseTypeId)
                .IsRequired()
                .HasComment("Foreign key to lease type");

            builder.Property(m => m.IncludedRenewals)
                .HasColumnType("SMALLINT")
                .HasDefaultValue(0)
                .HasComment("The number of times this lease has been renewed");
            builder.Property(m => m.RenewalCount)
                .HasColumnType("SMALLINT")
                .HasComment("The number of times this lease has been renewed");
            builder.Property(m => m.RenewalTermMonths)
                .HasColumnType("SMALLINT")
                .HasDefaultValue(0)
                .HasComment("The term in months of each renewal for this lease");

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
            builder.Property(m => m.OrigStartDate)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("getdate()")
                .IsRequired()
                .HasComment("The original date this lease starts");
            builder.Property(m => m.RenewalDate)
                .HasColumnType("DATETIME")
                .HasComment("The date this lease renews");
            builder.Property(m => m.ExpiryDate)
                .HasColumnType("DATETIME")
                .HasComment("The date this lease expires");
            builder.Property(m => m.OrigExpiryDate)
                .HasColumnType("DATETIME")
                .HasComment("The original date this lease expires");
            builder.Property(m => m.Amount)
                .HasColumnType("MONEY")
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
            builder.Property(m => m.Description)
                .HasMaxLength(4000)
                .HasComment("A description of the lease");
            builder.Property(m => m.LeasePurposeOtherDesc)
                .HasMaxLength(200)
                .HasComment("A description of the lease");
            builder.Property(m => m.IsExpired)
                .HasComment("Whether this lease has expired");
            builder.Property(m => m.IsOrigExpiryRequired)
                .HasComment("Whether thie original expiry on the lease is required");
            builder.Property(m => m.HasPhysicalFile)
                .HasComment("Whether this lease has a physical file");
            builder.Property(m => m.HasDigitalFile)
                .HasComment("Whether this lease has a digital file");
            builder.Property(m => m.HasPhysicalLicense)
                .HasComment("Whether this lease has a physical license");
            builder.Property(m => m.HasDigitalLicense)
                .HasComment("Whether this lease has a digital license");

            builder.HasOne(m => m.PurposeType).WithMany(m => m.Leases).HasForeignKey(m => m.PurposeTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LSPRPTY_PIM_LEASE_FK");
            builder.HasOne(m => m.PaymentFrequencyType).WithMany(m => m.Leases).HasForeignKey(m => m.PaymentFrequencyTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LSPMTF_PIM_LEASE_FK");
            builder.HasOne(m => m.PaymentRvblType).WithMany(m => m.Leases).HasForeignKey(m => m.PaymentRvblTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LSPRTY_PIM_LEASE_FK");
            builder.HasOne(m => m.ProgramType).WithMany(m => m.Leases).HasForeignKey(m => m.ProgramTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LSPRGT_PIM_LEASE_FK");
            builder.HasOne(m => m.MotiName).WithMany().HasForeignKey(m => m.MotiNameId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PERSON_PIM_LEASE_FK");
            builder.HasOne(m => m.CategoryType).WithMany(m => m.Leases).HasForeignKey(m => m.CategoryTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LSCATT_PIM_LEASE_FK");
            builder.HasOne(m => m.LeaseLicenseType).WithMany(m => m.Leases).HasForeignKey(m => m.LeaseTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LELIST_PIM_LEASE_FK");

            builder.HasMany(m => m.Properties).WithMany(m => m.Leases).UsingEntity<PropertyLease>(
                m => m.HasOne(m => m.Property).WithMany(m => m.LeasesManyToMany).HasForeignKey(m => m.PropertyId),
                m => m.HasOne(m => m.Lease).WithMany(m => m.PropertiesManyToMany).HasForeignKey(m => m.LeaseId)
            );

            builder.HasMany(m => m.Persons).WithMany(m => m.Leases).UsingEntity<LeaseTenant>(
                m => m.HasOne(m => m.Person).WithMany(m => m.LeasesManyToMany).HasForeignKey(m => m.PersonId),
                m => m.HasOne(m => m.Lease).WithMany(m => m.TenantsManyToMany).HasForeignKey(m => m.LeaseId)
            );

            builder.HasMany(m => m.Organizations).WithMany(m => m.Leases).UsingEntity<LeaseTenant>(
                m => m.HasOne(m => m.Organization).WithMany(m => m.LeasesManyToMany).HasForeignKey(m => m.OrganizationId),
                m => m.HasOne(m => m.Lease).WithMany(m => m.TenantsManyToMany).HasForeignKey(m => m.LeaseId)
            );

            builder.HasIndex(m => m.PaymentFrequencyTypeId).HasDatabaseName("LEASE_LEASE_PMT_FREQ_TYPE_CODE_IDX");
            builder.HasIndex(m => m.PaymentRvblTypeId).HasDatabaseName("LEASE_LEASE_PAY_RVBL_TYPE_CODE_IDX");
            builder.HasIndex(m => m.ProgramTypeId).HasDatabaseName("LEASE_LEASE_PROGRAM_TYPE_CODE_IDX");
            builder.HasIndex(m => m.PurposeTypeId).HasDatabaseName("LEASE_LEASE_PURPOSE_TYPE_CODE_IDX");
            builder.HasIndex(m => m.CategoryTypeId).HasDatabaseName("LEASE_LEASE_CATEGORY_TYPE_CODE_IDX");
            builder.HasIndex(m => m.MotiNameId).HasDatabaseName("LEASE_MOTI_NAME_ID_IDX");
            builder.HasIndex(m => m.LFileNo).HasDatabaseName("LEASE_L_FILE_NO_IDX");
            builder.HasIndex(m => m.PsFileNo).HasDatabaseName("LEASE_PS_FILE_NO_IDX");
            builder.HasIndex(m => m.TfaFileNo).HasDatabaseName("LEASE_TFA_FILE_NO_IDX");
            builder.HasIndex(m => m.LeaseTypeId).HasDatabaseName("LEASE_LEASE_LICENSE_TYPE_CODE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
