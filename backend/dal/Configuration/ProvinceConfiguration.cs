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
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.Property(m => m.Id)
                .HasColumnType("SMALLINT")
                .HasComment("Unique primary key value");

            builder.Property(m => m.CountryId)
                .HasColumnType("SMALLINT")
                .HasComment("Foreign key to country");

            builder.Property(m => m.Code)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("''") // This should not be a default value however MOTI standards requires it to have a default value.
                .HasComment("A unique human friendly code to identify the record");
            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("A description of the province");
            builder.Property(m => m.DisplayOrder)
                .HasComment("Displaying order of record");
            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this record is disabled");

            builder.HasOne(m => m.Country).WithMany(m => m.Provinces).HasForeignKey(m => m.CountryId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_CNTRY_PIM_PROVNC_FK");

            builder.HasIndex(m => new { m.Code }, "PROVNC_CODE_TUC").IsUnique();

            builder.HasIndex(m => m.CountryId).HasDatabaseName("PROVNC_COUNTRY_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
