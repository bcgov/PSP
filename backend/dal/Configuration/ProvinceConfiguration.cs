using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProvinceConfiguration class, provides a way to configure provinces in the database.
    ///</summary>
    public class ProvinceConfiguration : BaseEntityConfiguration<Province>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Code)
                .HasMaxLength(2)
                .IsRequired()
                .HasDefaultValueSql("''") // This should not be a default value however MOTI standards requires it to have a default value.
                .HasComment("A unique human friendly code to identify the record");
            builder.Property(m => m.Name)
                .HasMaxLength(150)
                .IsRequired()
                .HasComment("A unique name to identify the record");
            builder.Property(m => m.SortOrder)
                .HasDefaultValue(0)
                .HasComment("Sorting order of record");
            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this record is disabled");

            builder.HasIndex(m => new { m.Code }, "PROV_CODE_TUC").IsUnique();
            builder.HasIndex(m => new { m.Name }, "PROV_NAME_TUC").IsUnique();

            base.Configure(builder);
        }
        #endregion
    }
}
