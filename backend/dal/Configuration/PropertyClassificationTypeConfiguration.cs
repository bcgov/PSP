using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyClassificationTypeConfiguration class, provides a way to configure property classification types in the database.
    ///</summary>
    public class PropertyClassificationTypeConfiguration : TypeEntityConfiguration<PropertyClassificationType, string>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyClassificationType> builder)
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
