using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyConfiguration class, provides a way to configure properties in the database.
    ///</summary>
    public class PropertyConfiguration : BaseAppEntityConfiguration<Property>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Property> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.PropertyTypeId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to property type");
            builder.Property(m => m.StatusId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to property status type");
            builder.Property(m => m.ClassificationId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to property classification type");
            builder.Property(m => m.AddressId)
                .IsRequired()
                .HasComment("Foreign key to address");
            builder.Property(m => m.RegionId)
                .IsRequired()
                .HasColumnType("SMALLINT")
                .HasComment("Foreign key to region");
            builder.Property(m => m.DistrictId)
                .IsRequired()
                .HasColumnType("SMALLINT")
                .HasComment("Foreign key to district");
            builder.Property(m => m.TenureId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to property tenure type");
            builder.Property(m => m.AreaUnitId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to property area unit type");
            builder.Property(m => m.DataSourceId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to property data source type");

            builder.Property(m => m.DataSourceEffectiveDate)
                .HasColumnType("DATE")
                .HasComment("The date the data source is effective on");
            builder.Property(m => m.Name)
                .HasMaxLength(250)
                .HasComment("A friendly name to identify the property");
            builder.Property(m => m.Description)
                .HasMaxLength(2000)
                .HasComment("Description of the property");
            builder.Property(m => m.PID)
                .IsRequired()
                .HasComment("A unique identifier for titled property");
            builder.Property(m => m.PIN)
                .HasComment("A unique identifier for untitled property");
            builder.Property(m => m.LandArea)
                .IsRequired()
                .HasColumnType("REAL")
                .HasComment("The total land area in the specified area unit type");
            builder.Property(m => m.LandLegalDescription)
                .HasColumnType("NVARCHAR(MAX)")
                .HasComment("Titled legal land description");
            builder.Property(m => m.Boundary)
                .HasComment("A geo-spatial description of the building boundary");
            builder.Property(m => m.Location)
                .IsRequired()
                .HasComment("A geo-spatial point where the building is located");
            builder.Property(m => m.EncumbranceReason)
                .HasMaxLength(500)
                .HasComment("A description of the reason for encumbrance");
            builder.Property(m => m.IsOwned)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Whether this property is owned by the ministry");
            builder.Property(m => m.IsPropertyOfInterest)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Whether this property is a property of interest");
            builder.Property(m => m.IsVisibleToOtherAgencies)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Whether this property is visible to other agencies");
            builder.Property(m => m.IsSensitive)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Whether this property is associated with sensitive information");
            builder.Property(m => m.Zoning)
                .HasMaxLength(50)
                .HasComment("The current zoning");
            builder.Property(m => m.ZoningPotential)
                .HasMaxLength(50)
                .HasComment("The potential zoning");

            builder.HasOne(m => m.PropertyType).WithMany(m => m.Properties).HasForeignKey(m => m.PropertyTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PRPTYP_PIM_PRPRTY_FK");
            builder.HasOne(m => m.Status).WithMany(m => m.Properties).HasForeignKey(m => m.StatusId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PRPSTS_PIM_PRPRTY_FK");
            builder.HasOne(m => m.Classification).WithMany(m => m.Properties).HasForeignKey(m => m.ClassificationId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PRPCLT_PIM_PRPRTY_FK");
            builder.HasOne(m => m.Address).WithMany(m => m.Properties).HasForeignKey(m => m.AddressId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_ADDRSS_PIM_PRPRTY_FK");
            builder.HasOne(m => m.Region).WithMany(m => m.Properties).HasForeignKey(m => m.RegionId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_REGION_PIM_PRPRTY_FK");
            builder.HasOne(m => m.District).WithMany(m => m.Properties).HasForeignKey(m => m.DistrictId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_DSTRCT_PIM_PRPRTY_FK");
            builder.HasOne(m => m.Tenure).WithMany(m => m.Properties).HasForeignKey(m => m.TenureId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PRPTNR_PIM_PRPRTY_FK");
            builder.HasOne(m => m.AreaUnit).WithMany(m => m.Properties).HasForeignKey(m => m.AreaUnitId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_ARUNIT_PIM_PRPRTY_FK");
            builder.HasOne(m => m.DataSource).WithMany(m => m.Properties).HasForeignKey(m => m.DataSourceId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PIDSRT_PIM_PRPRTY_FK");

            builder.HasMany(m => m.ServiceFiles).WithMany(m => m.Properties).UsingEntity<PropertyPropertyServiceFile>(
                m => m.HasOne(m => m.ServiceFile).WithMany(m => m.PropertiesManyToMany).HasForeignKey(m => m.ServiceFileId),
                m => m.HasOne(m => m.Property).WithMany(m => m.ServiceFilesManyToMany).HasForeignKey(m => m.PropertyId)
            );
            builder.HasMany(m => m.Organizations).WithMany(m => m.Properties).UsingEntity<PropertyOrganization>(
                m => m.HasOne(m => m.Organization).WithMany(m => m.PropertiesManyToMany).HasForeignKey(m => m.OrganizationId),
                m => m.HasOne(m => m.Property).WithMany(m => m.OrganizationsManyToMany).HasForeignKey(m => m.PropertyId)
            );

            builder.HasIndex(m => m.PropertyTypeId).HasDatabaseName("PRPRTY_PROPERTY_TYPE_CODE_IDX");
            builder.HasIndex(m => m.StatusId).HasDatabaseName("PRPRTY_PROPERTY_STATUS_TYPE_CODE_IDX");
            builder.HasIndex(m => m.ClassificationId).HasDatabaseName("PRPRTY_PROPERTY_CLASSIFICATION_TYPE_CODE_IDX");
            builder.HasIndex(m => m.AddressId).HasDatabaseName("PRPRTY_ADDRESS_ID_IDX");
            builder.HasIndex(m => m.RegionId).HasDatabaseName("PRPRTY_REGION_CODE_IDX");
            builder.HasIndex(m => m.DistrictId).HasDatabaseName("PRPRTY_DISTRICT_CODE_IDX");
            builder.HasIndex(m => m.TenureId).HasDatabaseName("PRPRTY_PROPERTY_TENURE_TYPE_CODE_IDX");
            builder.HasIndex(m => m.AreaUnitId).HasDatabaseName("PRPRTY_PROPERTY_AREA_UNIT_TYPE_CODE_IDX");
            builder.HasIndex(m => m.DataSourceId).HasDatabaseName("PRPRTY_PROPERTY_DATA_SOURCE_TYPE_CODE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
