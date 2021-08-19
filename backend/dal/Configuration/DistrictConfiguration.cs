using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// DistrictConfiguration class, provides a way to configure districts in the database.
    ///</summary>
    public class DistrictConfiguration : BaseEntityConfiguration<District>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<District> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.Property(m => m.Id)
                .HasColumnType("SMALLINT")
                .HasComment("Unique primary key value");

            builder.Property(m => m.RegionId)
                .HasColumnType("SMALLINT")
                .HasComment("Foreign key to the region");

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("The name of the region");
            builder.Property(m => m.DisplayOrder)
                .HasComment("Displaying order of record");
            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this record is disabled");

            builder.HasOne(m => m.Region).WithMany(m => m.Districts).HasForeignKey(m => m.RegionId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_REGION_PIM_DSTRCT_FK");

            builder.HasIndex(m => m.RegionId).HasDatabaseName("DSTRCT_REGION_CODE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
