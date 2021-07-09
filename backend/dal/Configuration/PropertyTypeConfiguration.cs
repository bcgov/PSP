using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyTypeConfiguration class, provides a way to configure property types in the database.
    ///</summary>
    public class PropertyTypeConfiguration : BaseEntityConfiguration<PropertyType>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyType> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Name).HasMaxLength(150).IsRequired()
                .HasComment("A unique name to identify the record");
            builder.Property(m => m.SortOrder).HasDefaultValue(0)
                .HasComment("Sorting order of record");
            builder.Property(m => m.IsDisabled).HasDefaultValue(false)
                .HasComment("Whether this record is disabled");

            builder.HasIndex(m => new { m.Name }, "PRPTYP_NAME_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled, m.SortOrder }, "PRPTYP_IS_DISABLED_DISPLAY_ORDER_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
