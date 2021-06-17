using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// BuildingOccupantTypeConfiguration class, provides a way to configure building occupant type in the database.
    ///</summary>
    public class BuildingOccupantTypeConfiguration : LookupEntityConfiguration<BuildingOccupantType>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<BuildingOccupantType> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Name).HasMaxLength(150).IsRequired()
                .HasComment("A unique name to identify the record");

            builder.HasIndex(m => new { m.Name }, "BLOCCT_NAME_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled, m.SortOrder }, "BLOCCT_IS_DISABLED_SORT_ORDER_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
