using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyConfiguration class, provides a way to configure properties in the database.
    ///</summary>
    public abstract class PropertyConfiguration<TBase> : BaseEntityConfiguration<TBase>
        where TBase : Property
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<TBase> builder)
        {
            builder.Property(m => m.AgencyId)
                .HasComment("Foreign key to the owning agency");
            builder.Property(m => m.PropertyTypeId)
                .HasComment("Foreign key to the property type");
            builder.Property(m => m.ClassificationId)
                .HasComment("Foreign key to the property classification");
            builder.Property(m => m.AddressId)
                .HasComment("Foreign key to the property address");

            builder.Property(m => m.Name).HasMaxLength(250)
                .HasComment("A name to identify this property");
            builder.Property(m => m.Description).HasMaxLength(2000)
                .HasComment("The property description");

            builder.Property(m => m.ProjectNumbers).HasMaxLength(2000)
                .HasComment("A comma-separated list of project numbers associated with this property");
            builder.Property(m => m.Boundary)
                .HasComment("A geo-spatial description of the building boundary");
            builder.Property(m => m.Location).IsRequired()
                .HasComment("A geo-spatial point where the building is located");
            builder.Property(m => m.IsVisibleToOtherAgencies).HasDefaultValue(false)
                .HasComment("Whether this building is visible to other agencies");
            builder.Property(m => m.IsSensitive).HasDefaultValue(false)
                .HasComment("Whether this building is sensitive to privacy impact statement");
            builder.Property(m => m.EncumbranceReason).HasMaxLength(500)
                .HasComment("The reason the property has an encumbrance");

            base.Configure(builder);
        }
        #endregion
    }
}
