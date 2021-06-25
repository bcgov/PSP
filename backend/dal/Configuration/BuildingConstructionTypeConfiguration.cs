using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// BuildingConstructionTypeConfiguration class, provides a way to configure building construction types in the database.
    ///</summary>
    public class BuildingConstructionTypeConfiguration : LookupEntityConfiguration<BuildingConstructionType>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<BuildingConstructionType> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Name).HasMaxLength(150).IsRequired()
                .HasComment("A unique name of the record");

            builder.HasIndex(m => new { m.Name }, "BLCNTY_NAME_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled, m.SortOrder }, "BLCNTY_IS_DISABLED_DISPLAY_ORDER_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
