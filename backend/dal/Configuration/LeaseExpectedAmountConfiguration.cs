using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// LeaseExpectedAmountConfiguration class, provides a way to configure lease expected amount in the database.
    ///</summary>
    public class LeaseExpectedAmountConfiguration : BaseAppEntityConfiguration<LeaseExpectedAmount>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<LeaseExpectedAmount> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.LeaseId)
                .HasComment("Foreign key to lease");
            builder.Property(m => m.PeriodId)
                .HasComment("Foreign key to lease activity period");

            builder.Property(m => m.Amount)
                .HasColumnType("MONEY")
                .HasComment("The expected amount for this lease period");

            builder.HasOne(m => m.Lease).WithMany(m => m.ExpectedAmounts).HasForeignKey(m => m.LeaseId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LEASE_PIM_EXPAMT_FK");
            builder.HasOne(m => m.Period).WithMany(m => m.ExpectedAmounts).HasForeignKey(m => m.PeriodId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LSACPR_PIM_EXPAMT_FK");

            builder.HasIndex(m => m.PeriodId).HasDatabaseName("EXPAMT_LEASE_ACTIVITY_PERIOD_ID_IDX");
            builder.HasIndex(m => m.LeaseId).HasDatabaseName("EXPAMT_LEASE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
