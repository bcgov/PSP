using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyServiceFileTypeConfiguration class, provides a way to configure property service file types in the database.
    ///</summary>
    public class PropertyServiceFileTypeConfiguration : TypeEntityConfiguration<PropertyServiceFileType, string>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyServiceFileType> builder)
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
