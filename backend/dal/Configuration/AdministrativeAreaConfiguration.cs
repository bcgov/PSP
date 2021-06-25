using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// AdministrativeAreaConfiguration class, provides a way to configure administrative areas in the database.
    ///</summary>
    public class AdministrativeAreaConfiguration : LookupEntityConfiguration<AdministrativeArea>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<AdministrativeArea> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Name).HasMaxLength(150).IsRequired()
                .HasComment("A name to identify this record");
            builder.Property(m => m.Abbreviation).HasMaxLength(100)
                .HasComment("An abbreviation of the name");
            builder.Property(m => m.BoundaryType).HasMaxLength(50)
                .HasComment("A boundary type representing this record");
            builder.Property(m => m.GroupName).HasMaxLength(250)
                .HasComment("A group name to associate multiple records");

            builder.HasIndex(m => new { m.IsDisabled, m.Name, m.SortOrder }, "ADMINA_IS_DISABLED_NAME_DISPLAY_ORDER_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
