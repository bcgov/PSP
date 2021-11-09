using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// SecurityDepositReturnConfiguration class, provides a way to configure security deposit returns in the database.
    ///</summary>
    public class SecurityDepositReturnConfiguration : BaseAppEntityConfiguration<SecurityDepositReturn>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<SecurityDepositReturn> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.LeaseId)
                .HasComment("Foreign key to lease");

            builder.Property(m => m.SecurityDepositTypeId)
                .HasMaxLength(20)
                .IsRequired()
                .HasComment("Foreign key to security deposit type");

            builder.Property(m => m.TerminationDate)
                .HasColumnType("DateTime")
                .HasComment("The date the deposit was returned");
            builder.Property(m => m.DepositTotal)
                .HasColumnType("MONEY")
                .HasComment("The total deposit amount before claims");
            builder.Property(m => m.ClaimsAgainst)
                .HasColumnType("MONEY")
                .HasComment("The total amount of claims made against the deposit");
            builder.Property(m => m.ReturnAmount)
                .HasColumnType("MONEY")
                .HasComment("The total deposit amount less any claims");
            builder.Property(m => m.ReturnDate)
                .HasColumnType("DateTime")
                .HasComment("The date the deposit was returned");
            builder.Property(m => m.ChequeNumber)
                .HasMaxLength(50)
                .HasComment("The cheque number of the original deposit");
            builder.Property(m => m.PayeeName)
                .HasMaxLength(100)
                .HasComment("The deposit payee name");
            builder.Property(m => m.PayeeAddress)
                .HasMaxLength(500)
                .HasComment("The deposit payee address");
            builder.Property(m => m.TerminationNote)
                .HasMaxLength(4000)
                .HasComment("Any notes corresponding to the termination of this security deposit");

            builder.HasOne(m => m.Lease).WithMany(m => m.SecurityDepositReturns).HasForeignKey(m => m.LeaseId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LEASE_PIM_SDRTRN_FK");
            builder.HasOne(m => m.SecurityDepositType).WithMany(m => m.SecurityDepositReturns).HasForeignKey(m => m.SecurityDepositTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_SECDPT_PIM_SDRTRN_FK");

            builder.HasIndex(m => m.LeaseId).HasDatabaseName("SDRTRN_LEASE_ID_IDX");
            builder.HasIndex(m => m.SecurityDepositTypeId).HasDatabaseName("SDRTRN_SECURITY_DEPOSIT_TYPE_CODE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
