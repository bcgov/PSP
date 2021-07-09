using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyClassificationConfiguration class, provides a way to configure property classifications in the database.
    ///</summary>
    public class PropertyClassificationConfiguration : LookupEntityConfiguration<PropertyClassification>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyClassification> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Name).HasMaxLength(150).IsRequired()
                .HasComment("A unique name to identify the record");

            builder.Property(m => m.IsVisible)
                .HasComment("Whether this record is visible to users");

            builder.HasIndex(m => new { m.Name }, "PRPCLS_NAME_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled }, "PRPCLS_IS_DIABLED_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
