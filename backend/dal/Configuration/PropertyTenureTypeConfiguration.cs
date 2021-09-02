using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyTenureTypeConfiguration class, provides a way to configure property tenure types in the database.
    ///</summary>
    public class PropertyTenureTypeConfiguration : TypeEntityConfiguration<PropertyTenureType, string>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyTenureType> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.Property(m => m.Id)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Primary key code to identify record");

            base.Configure(builder);
        }
        #endregion
    }
}
