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
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Address1).HasMaxLength(150)
                .HasComment("The first line of the address");

            builder.Property(m => m.Address2).HasMaxLength(150)
                .HasComment("The second line of the address");

            builder.Property(m => m.Postal).HasMaxLength(6)
                .HasComment("The postal code of the address");

            builder.Property(m => m.ProvinceId).HasMaxLength(2).IsRequired()
                .HasComment("Foreign key to the province");

            builder.Property(m => m.AdministrativeArea).HasMaxLength(150).IsRequired()
                .HasComment("Administrative area name (city, district, region, etc.)");

            builder.HasOne(m => m.Province).WithMany().HasForeignKey(m => m.ProvinceId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("ADDR_PROVINCE_ID_IDX");

            builder.HasIndex(m => new { m.Address1, m.AdministrativeArea, m.Postal }, "ADDR_ADDRESS1_ADMINISTRATIVE_AREA_POSTAL_IDX").IncludeProperties(m => new { m.Address2 });
            builder.HasIndex(m => m.ProvinceId, "ADDR_PROVINCE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
