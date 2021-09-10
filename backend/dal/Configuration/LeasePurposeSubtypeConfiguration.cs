using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// LeasePurposeSubtypeConfiguration class, provides a way to configure lease purpose subtypes in the database.
    ///</summary>
    public class LeasePurposeSubtypeConfiguration : TypeEntityConfiguration<LeasePurposeSubtype, int>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<LeasePurposeSubtype> builder)
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
