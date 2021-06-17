using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ParcelBuildingConfiguration class, provides a way to configure parcel and building relationships in the database.
    ///</summary>
    public class ParcelBuildingConfiguration : BaseEntityConfiguration<ParcelBuilding>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ParcelBuilding> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ParcelId).IsRequired()
                .HasComment("Foreign key to the parcel");
            builder.Property(m => m.BuildingId).IsRequired()
                .HasComment("Foreign key to the building");

            builder.HasOne(m => m.Parcel).WithMany(m => m.Buildings).HasForeignKey(m => m.ParcelId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRCLBL_PARCEL_ID_IDX");
            builder.HasOne(m => m.Building).WithMany(m => m.Parcels).HasForeignKey(m => m.BuildingId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRCLBL_BUILDING_ID_IDX");

            builder.HasIndex(m => new { m.ParcelId, m.BuildingId }, "PRCLBL_PARCEL_BUILDING_TUC").IsUnique();
            builder.HasIndex(m => m.ParcelId, "PRCLBL_PARCEL_ID_IDX");
            builder.HasIndex(m => m.BuildingId, "PRCLBL_BUILDING_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
