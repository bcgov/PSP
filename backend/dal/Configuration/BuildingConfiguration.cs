using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// BuildingConfiguration class, provides a way to configure buildings in the database.
    ///</summary>
    public class BuildingConfiguration : PropertyConfiguration<Building>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Building> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.AgencyId)
                .HasComment("Foreign key to the owning agency");
            builder.Property(m => m.AddressId)
                .HasComment("Foreign key to the property address");
            builder.Property(m => m.PropertyTypeId)
                .HasComment("Foreign key to the property type");
            builder.Property(m => m.ClassificationId)
                .HasComment("Foreign key to the property classification");
            builder.Property(m => m.BuildingConstructionTypeId)
                .HasComment("Foreign key to the building construction type");
            builder.Property(m => m.BuildingOccupantTypeId)
                .HasComment("Foreign key to the building occupant type");
            builder.Property(m => m.BuildingPredominateUseId)
                .HasComment("Foreign key to the building predominate use");

            builder.Property(m => m.TotalArea)
                .HasComment("The total area of the building");
            builder.Property(m => m.RentableArea)
                .HasComment("The total rentable area of the building");

            builder.Property(m => m.LeasedLandMetadata)
                .HasComment("Contains JSON serialized data related to leased land");

            builder.Property(m => m.BuildingFloorCount).IsRequired()
                .HasComment("Number of floors the building has");
            builder.Property(m => m.BuildingTenancy).IsRequired()
                .HasComment("Type of tenancy in the building");
            builder.Property(m => m.LeaseExpiry)
                .HasColumnType("DATETIME")
                .HasComment("The date the lease expires");
            builder.Property(m => m.BuildingTenancyUpdatedOn)
                .HasColumnType("DATETIME")
                .HasComment("The date the building tenancy was updated on");
            builder.Property(m => m.TransferLeaseOnSale).HasDefaultValue(false)
                .HasComment("Whether the lease would transfer on sale");
            builder.Property(m => m.OccupantName).HasMaxLength(100)
                .HasComment("The name of the occupant");

            builder.HasOne(m => m.BuildingConstructionType).WithMany().HasForeignKey(m => m.BuildingConstructionTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("BUILDG_BUILDING_CONSTRUCTION_TYPE_ID_IDX");
            builder.HasOne(m => m.BuildingPredominateUse).WithMany().HasForeignKey(m => m.BuildingPredominateUseId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("BUILDG_BUILDING_PREDOMINATE_USE_ID_IDX");
            builder.HasOne(m => m.BuildingOccupantType).WithMany().HasForeignKey(m => m.BuildingOccupantTypeId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("BUILDG_BUILDING_OCCUPANT_TYPE_ID_IDX");

            builder.HasOne(m => m.Agency).WithMany(m => m.Buildings).HasForeignKey(m => m.AgencyId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("BUILDG_AGENCY_ID_IDX");
            builder.HasOne(m => m.PropertyType).WithMany().HasForeignKey(m => m.PropertyTypeId).OnDelete(DeleteBehavior.ClientNoAction).HasConstraintName("BUILDG_PROPERTY_TYPE_ID_IDX");
            builder.HasOne(m => m.Classification).WithMany().HasForeignKey(m => m.ClassificationId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("BUILDG_PROPERTY_CLASSIFICATION_ID_IDX");
            builder.HasOne(m => m.Address).WithMany().HasForeignKey(m => m.AddressId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("BUILDG_ADDRESS_ID_IDX");

            builder.HasIndex(m => new { m.IsSensitive, m.ProjectNumbers }, "BUILDG_IS_SENSITIVE_PROJECT_NUMBERS_IDX");
            builder.HasIndex(m => m.AgencyId, "BUILDG_AGENCY_ID_IDX");
            builder.HasIndex(m => m.PropertyTypeId, "BUILDG_PROPERTY_TYPE_ID_IDX");
            builder.HasIndex(m => m.ClassificationId, "BUILDG_PROPERTY_CLASSIFICATION_ID_IDX");
            builder.HasIndex(m => m.AddressId, "BUILDG_ADDRESS_ID_IDX");
            builder.HasIndex(m => m.BuildingConstructionTypeId, "BUILDG_BUILDING_CONSTRUCTION_TYPE_ID_IDX");
            builder.HasIndex(m => m.BuildingPredominateUseId, "BUILDG_BUILDING_PREDOMINATE_USE_ID_IDX");
            builder.HasIndex(m => m.BuildingOccupantTypeId, "BUILDG_BUILDING_OCCUPANT_TYPE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
