using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// LeaseActivityConfiguration class, provides a way to configure lease activities in the database.
    ///</summary>
    public class LeaseActivityConfiguration : BaseAppEntityConfiguration<LeaseActivity>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<LeaseActivity> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.LeaseId)
                .HasComment("Foreign key to lease");
            builder.Property(m => m.LeaseTypeId)
                .HasComment("Foreign key to lease type");
            builder.Property(m => m.SubtypeId)
                .HasComment("Foreign key to lease subtype");
            builder.Property(m => m.PeriodId)
                .HasComment("Foreign key to lease activity period");

            builder.Property(m => m.Amount)
                .HasColumnType("MONEY")
                .HasComment("The lease activity amount");
            builder.Property(m => m.Date)
                .HasComment("When the activity occurred");
            builder.Property(m => m.Comment)
                .HasMaxLength(40)
                .HasComment("A comment related to the activity");

            builder.HasOne(m => m.Lease).WithMany(m => m.Activities).HasForeignKey(m => m.LeaseId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LEASE_PIM_LSACTV_FK");
            builder.HasOne(m => m.LeaseType).WithMany(m => m.Activities).HasForeignKey(m => m.LeaseTypeId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_LSTYPE_PIM_LSACTV_FK");
            builder.HasOne(m => m.Subtype).WithMany(m => m.Activities).HasForeignKey(m => m.SubtypeId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_LSSTYP_PIM_LSACTV_FK");
            builder.HasOne(m => m.Period).WithMany(m => m.Activities).HasForeignKey(m => m.PeriodId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LSACPR_PIM_LSACTV_FK");

            builder.HasIndex(m => m.PeriodId).HasDatabaseName("LSACTV_LEASE_ACTIVITY_PERIOD_ID_IDX");
            builder.HasIndex(m => m.LeaseId).HasDatabaseName("LSACTV_LEASE_ID_IDX");
            builder.HasIndex(m => m.SubtypeId).HasDatabaseName("LSACTV_LEASE_SUBTYPE_CODE_ID_IDX");
            builder.HasIndex(m => m.LeaseTypeId).HasDatabaseName("LSACTV_LEASE_TYPE_CODE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
