using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// CountryConfiguration class, provides a way to configure countries in the database.
    ///</summary>
    public class CountryConfiguration : BaseEntityConfiguration<Country>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.Property(m => m.Id)
                .HasColumnType("SMALLINT")
                .HasComment("Unique primary key value");

            builder.Property(m => m.Code)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValueSql("''") // This should not be a default value however MOTI standards requires it to have a default value.
                .HasComment("A unique human friendly code to identify the record");
            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("A description of the country");
            builder.Property(m => m.DisplayOrder)
                .HasComment("Displaying order of record");

            builder.HasIndex(m => new { m.Code }, "COUNTR_CODE_TUC").IsUnique();

            base.Configure(builder);
        }
        #endregion
    }
}
