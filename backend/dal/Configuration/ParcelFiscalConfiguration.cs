using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ParcelFiscalConfiguration class, provides a way to configure parcel fiscals in the database.
    ///</summary>
    public class ParcelFiscalConfiguration : BaseAppEntityConfiguration<ParcelFiscal>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ParcelFiscal> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ParcelId).IsRequired()
                .HasComment("Foreign key to the parcel");

            builder.Property(m => m.FiscalYear).IsRequired()
                .HasComment("The fiscal year this value is relevant to");
            builder.Property(m => m.EffectiveDate)
                .HasColumnType("DATE")
                .HasComment("The effective date of the value");
            builder.Property(m => m.Key).IsRequired()
                .HasComment("The fiscal value type");
            builder.Property(m => m.Value)
                .HasColumnType("MONEY")
                .HasComment("The value of the property");
            builder.Property(m => m.Note).HasMaxLength(500)
                .HasComment("A note about the fiscal value");

            builder.HasOne(m => m.Parcel).WithMany(m => m.Fiscals).HasForeignKey(m => m.ParcelId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRFSCL_PARCEL_ID_IDX");

            builder.HasIndex(m => new { m.ParcelId, m.FiscalYear, m.Key }, "PRFSCL_PARCEL_ID_FISCAL_YEAR_KEY_TUC").IsUnique();
            builder.HasIndex(m => new { m.Value }, "PRFSCL_VALUE_IDX");
            builder.HasIndex(m => m.ParcelId, "PRFSCL_PARCEL_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
