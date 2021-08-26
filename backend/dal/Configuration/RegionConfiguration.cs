using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// RegionConfiguration class, provides a way to configure regions in the database.
    ///</summary>
    public class RegionConfiguration : BaseEntityConfiguration<Region>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.Property(m => m.Id)
                .HasColumnType("SMALLINT")
                .HasComment("Unique primary key value");

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("The name of the region");
            builder.Property(m => m.DisplayOrder)
                .HasComment("Displaying order of record");
            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this record is disabled");

            base.Configure(builder);
        }
        #endregion
    }
}
