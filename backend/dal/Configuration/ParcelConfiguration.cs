using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ParcelConfiguration class, provides a way to configure parcels in the database.
    ///</summary>
    public class ParcelConfiguration : PropertyConfiguration<Parcel>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Parcel> builder)
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
                .HasDefaultValue(PropertyTypes.Land)
                .HasComment("Foreign key to the property type");
            builder.Property(m => m.ClassificationId)
                .HasComment("Foreign key to the property classification");

            builder.Property(m => m.PID).IsRequired()
                .HasComment("A unique identifier for a titled property");
            builder.Property(m => m.PIN)
                .HasComment("A unique identifier for an non-titled property");
            builder.Property(m => m.Zoning).HasMaxLength(50)
                .HasComment("The current zoning of the property");
            builder.Property(m => m.ZoningPotential).HasMaxLength(50)
                .HasComment("The potential zoning of the property");
            builder.Property(m => m.LandArea)
                .HasComment("The area of the property");
            builder.Property(m => m.LandLegalDescription).HasMaxLength(500)
                .HasComment("The land legal description");
            builder.Property(m => m.NotOwned).HasDefaultValue(false)
                .HasComment("Whether this property is owned by an agency");

            builder.HasOne(m => m.Agency).WithMany(m => m.Parcels).HasForeignKey(m => m.AgencyId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("PARCEL_AGENCY_ID_IDX");
            builder.HasOne(m => m.Address).WithMany().HasForeignKey(m => m.AddressId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PARCEL_ADDRESS_ID_IDX");
            builder.HasOne(m => m.PropertyType).WithMany().HasForeignKey(m => m.PropertyTypeId).OnDelete(DeleteBehavior.ClientNoAction).HasConstraintName("PARCEL_PROPERTY_TYPE_ID_IDX");
            builder.HasOne(m => m.Classification).WithMany().HasForeignKey(m => m.ClassificationId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("PARCEL_PROPERTY_CLASSIFICATION_ID_IDX");
            builder.HasMany(m => m.Subdivisions).WithMany(m => m.Parcels).UsingEntity<ParcelParcel>(
                m => m.HasOne(m => m.Subdivision).WithMany(m => m.ParcelsManyToMany).HasForeignKey(m => m.SubdivisionId),
                m => m.HasOne(m => m.Parcel).WithMany(m => m.SubdivisionsManyToMany).HasForeignKey(m => m.ParcelId)
            );

            builder.HasIndex(m => new { m.PID, m.PIN }, "PARCEL_PID_PIN_TUC").IsUnique(); // This will allow for Crown Land to set ParcelId=0 and PIN=#######.
            builder.HasIndex(m => m.IsSensitive, "PARCEL_IS_SENSITIVE_IDX");
            builder.HasIndex(m => m.AgencyId, "PARCEL_AGENCY_ID_IDX");
            builder.HasIndex(m => m.AddressId, "PARCEL_ADDRESS_ID_IDX");
            builder.HasIndex(m => m.PropertyTypeId, "PARCEL_PROPERTY_TYPE_ID_IDX");
            builder.HasIndex(m => m.ClassificationId, "PARCEL_PROPERTY_CLASSIFICATION_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
