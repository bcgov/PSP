using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// BuildingPredominateUseConfiguration class, provides a way to configure building predominate uses in the database.
    ///</summary>
    public class BuildingPredominateUseConfiguration : LookupEntityConfiguration<BuildingPredominateUse>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<BuildingPredominateUse> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Name).HasMaxLength(150).IsRequired()
                .HasComment("A unique name to identify this record");

            builder.HasIndex(m => new { m.Name }, "BLPRDU_NAME_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled, m.SortOrder }, "BLPRDU_IS_DISABLED_SORT_ORDER_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
