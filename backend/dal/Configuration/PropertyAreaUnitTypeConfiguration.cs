using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyAreaUnitTypeConfiguration class, provides a way to configure property area unit types in the database.
    ///</summary>
    public class PropertyAreaUnitTypeConfiguration : TypeEntityConfiguration<PropertyAreaUnitType, string>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyAreaUnitType> builder)
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
