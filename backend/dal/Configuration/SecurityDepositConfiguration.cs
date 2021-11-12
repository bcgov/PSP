using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// SecurityDepositConfiguration class, provides a way to configure security deposits in the database.
    ///</summary>
    public class SecurityDepositConfiguration : BaseAppEntityConfiguration<SecurityDeposit>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<SecurityDeposit> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.LeaseId)
                .HasComment("Foreign key to lease");

            builder.Property(m => m.SecurityDepositHolderTypeId)
                .HasMaxLength(20)
                .IsRequired()
                .HasComment("Foreign key to security deposit holder type");
            builder.Property(m => m.SecurityDepositTypeId)
                .HasMaxLength(20)
                .IsRequired()
                .HasComment("Foreign key to security deposit type");

            builder.Property(m => m.Description)
                .HasMaxLength(2000)
                .HasComment("The description of the security deposit");
            builder.Property(m => m.AmountPaid)
                .HasColumnType("MONEY")
                .HasComment("The actual amount paid");
            builder.Property(m => m.TotalAmount)
                .HasColumnType("MONEY")
                .HasComment("The total amount of the security deposit");
            builder.Property(m => m.DepositDate)
                .HasColumnType("Date")
                .HasComment("The date of the deposit");
            builder.Property(m => m.AnnualInterestRate)
                .HasColumnType("NUMERIC(5,2)")
                .HasComment("The annual interest rate of this deposit");

            builder.HasOne(m => m.Lease).WithMany(m => m.SecurityDeposits).HasForeignKey(m => m.LeaseId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LEASE_PIM_SECDEP_FK");
            builder.HasOne(m => m.SecurityDepositHolderType).WithMany(m => m.SecurityDeposits).HasForeignKey(m => m.SecurityDepositHolderTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_SCHLDT_PIM_SECDEP_FK");
            builder.HasOne(m => m.SecurityDepositType).WithMany(m => m.SecurityDeposits).HasForeignKey(m => m.SecurityDepositTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_SECDPT_PIM_SECDEP_FK");

            builder.HasIndex(m => m.LeaseId).HasDatabaseName("SECDEP_LEASE_ID_IDX");
            builder.HasIndex(m => m.SecurityDepositHolderTypeId).HasDatabaseName("SECDEP_SEC_DEP_HOLDER_TYPE_CODE_IDX");
            builder.HasIndex(m => m.SecurityDepositTypeId).HasDatabaseName("SECDEP_SECURITY_DEPOSIT_TYPE_CODE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
