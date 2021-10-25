using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// LeasePurposeTypeConfiguration class, provides a way to configure lease purpose types in the database.
    ///</summary>
    public class LeasePurposeTypeConfiguration : TypeEntityConfiguration<LeasePurposeType, string>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<LeasePurposeType> builder)
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
