using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// TierLevelConfiguration class, provides a way to configure project tier levels in the database.
    ///</summary>
    public class TierLevelConfiguration : LookupEntityConfiguration<TierLevel>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<TierLevel> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Name).HasMaxLength(150).IsRequired()
                .HasComment("A unique human friendly name to identify the record");

            builder.Property(m => m.Description).HasMaxLength(1000)
                .HasComment("A description of the tier level");

            builder.HasIndex(m => new { m.Name }, "TRLEVL_NAME_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled, m.SortOrder }, "TRLEVL_IS_DISABLED_SORT_ORDER_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
