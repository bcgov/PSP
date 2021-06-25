using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// BuildingFiscalConfiguration class, provides a way to configure building fiscals in the database.
    ///</summary>
    public class BuildingFiscalConfiguration : BaseAppEntityConfiguration<BuildingFiscal>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<BuildingFiscal> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.BuildingId).IsRequired()
                .HasComment("Foreign key to the building");

            builder.Property(m => m.FiscalYear).IsRequired()
                .HasComment("The fiscal year this value is for");
            builder.Property(m => m.EffectiveDate)
                .HasColumnType("DATE")
                .HasComment("The effective date this value is for");
            builder.Property(m => m.Key).IsRequired()
                .HasComment("The fiscal value type");
            builder.Property(m => m.Value)
                .HasColumnType("MONEY")
                .HasComment("The value of the building");
            builder.Property(m => m.Note).HasMaxLength(500)
                .HasComment("A note about this fiscal value");

            builder.HasOne(m => m.Building).WithMany(m => m.Fiscals).HasForeignKey(m => m.BuildingId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("BLDFSC_BUILDING_ID_IDX");

            builder.HasIndex(m => new { m.BuildingId, m.FiscalYear, m.Key }, "BLDFSC_BUILDING_ID_FISCAL_YEAR_KEY_TUC").IsUnique();
            builder.HasIndex(m => new { m.BuildingId }, "BLDFSC_BUILDING_ID_IDX");
            builder.HasIndex(m => new { m.Value }, "BLDFSC_VALUE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
