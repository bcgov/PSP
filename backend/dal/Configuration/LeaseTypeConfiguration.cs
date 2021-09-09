using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// LeaseTypeConfiguration class, provides a way to configure lease types in the database.
    ///</summary>
    public class LeaseTypeConfiguration : TypeEntityConfiguration<LeaseType, int>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<LeaseType> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.Property(m => m.Id)
                .HasColumnType("SMALLINT")
                .IsRequired()
                .HasComment("Primary key code to identify record");

            base.Configure(builder);
        }
        #endregion
    }
}
