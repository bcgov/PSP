using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// LeaseStatusTypeConfiguration class, provides a way to configure lease status types in the database.
    ///</summary>
    public class LeaseStatusTypeConfiguration : TypeEntityConfiguration<LeaseStatusType, string>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<LeaseStatusType> builder)
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
