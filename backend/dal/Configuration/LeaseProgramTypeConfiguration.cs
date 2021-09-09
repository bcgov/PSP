using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// LeaseProgramTypeConfiguration class, provides a way to configure lease program types in the database.
    ///</summary>
    public class LeaseProgramTypeConfiguration : TypeEntityConfiguration<LeaseProgramType, string>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<LeaseProgramType> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.Property(m => m.Id)
                .IsRequired()
                .HasMaxLength(40)
                .HasComment("Primary key code to identify record");

            base.Configure(builder);
        }
        #endregion
    }
}
