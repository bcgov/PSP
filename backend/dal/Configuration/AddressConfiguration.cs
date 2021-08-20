using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// AddressConfiguration class, provides a way to configure addresses in the database.
    ///</summary>
    public class AddressConfiguration : BaseAppEntityConfiguration<Address>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.AddressTypeId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to address usage type");
            builder.Property(m => m.RegionId)
                .HasColumnType("SMALLINT")
                .HasComment("Foreign key to the region");
            builder.Property(m => m.DistrictId)
                .HasColumnType("SMALLINT")
                .HasComment("Foreign key to the district");
            builder.Property(m => m.ProvinceId)
                .HasColumnType("SMALLINT")
                .HasComment("Foreign key to the province");
            builder.Property(m => m.CountryId)
                .HasColumnType("SMALLINT")
                .HasComment("Foreign key to the country");

            builder.Property(m => m.StreetAddress1)
                .HasMaxLength(200)
                .HasComment("The street address part 1");
            builder.Property(m => m.StreetAddress2)
                .HasMaxLength(200)
                .HasComment("The street address part 2");
            builder.Property(m => m.StreetAddress3)
                .HasMaxLength(200)
                .HasComment("The street address part 3");
            builder.Property(m => m.Municipality)
                .HasMaxLength(200)
                .HasComment("The municipality location");
            builder.Property(m => m.Postal)
                .HasMaxLength(20)
                .HasComment("The postal code of the address");

            builder.Property(m => m.Latitude)
                .HasColumnType("NUMERIC(8,6)")
                .HasComment("GIS latitude location");
            builder.Property(m => m.Longitude)
                .HasColumnType("NUMERIC(9,6)")
                .HasComment("GIS longitude location");

            builder.HasOne(m => m.AddressType).WithMany(m => m.Addresses).HasForeignKey(m => m.AddressTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_ADUSGT_PIM_ADDRSS_FK");
            builder.HasOne(m => m.Region).WithMany(m => m.Addresses).HasForeignKey(m => m.RegionId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_REGION_PIM_ADDRSS_FK");
            builder.HasOne(m => m.District).WithMany(m => m.Addresses).HasForeignKey(m => m.DistrictId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_DSTRCT_PIM_ADDRSS_FK");
            builder.HasOne(m => m.Province).WithMany(m => m.Addresses).HasForeignKey(m => m.ProvinceId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PROVNC_PIM_ADDRSS_FK");
            builder.HasOne(m => m.Country).WithMany(m => m.Addresses).HasForeignKey(m => m.CountryId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_CNTRY_PIM_ADDRSS_FK");

            builder.HasIndex(m => m.AddressTypeId).HasDatabaseName("ADDRSS_ADDRESS_USAGE_TYPE_CODE_IDX");
            builder.HasIndex(m => m.RegionId).HasDatabaseName("ADDRSS_REGION_CODE_IDX");
            builder.HasIndex(m => m.DistrictId).HasDatabaseName("ADDRSS_DISTRICT_CODE_IDX");
            builder.HasIndex(m => m.ProvinceId).HasDatabaseName("ADDRSS_PROVINCE_STATE_ID_IDX");
            builder.HasIndex(m => m.CountryId).HasDatabaseName("ADDRSS_COUNTRY_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
