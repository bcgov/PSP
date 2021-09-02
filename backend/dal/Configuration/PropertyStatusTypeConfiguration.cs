using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyStatusTypeConfiguration class, provides a way to configure property status types in the database.
    ///</summary>
    public class PropertyStatusTypeConfiguration : TypeEntityConfiguration<PropertyStatusType, string>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyStatusType> builder)
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
