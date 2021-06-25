using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// BaseEntityConfiguration class, provides a way to configure base entity in the database.
    ///</summary>
    public abstract class BaseEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase>
        where TBase : BaseEntity
    {
        #region Methods
        protected void BaseConfigure(EntityTypeBuilder<TBase> builder)
        {
            builder.Property(m => m.RowVersion)
                .HasColumnType("BIGINT")
                .HasDefaultValue(1L)
                .IsConcurrencyToken()
                .HasComment("Concurrency control number")
                .HasAnnotation("ColumnOrder", 100);
        }

        public virtual void Configure(EntityTypeBuilder<TBase> builder)
        {
            BaseConfigure(builder);
        }
        #endregion
    }
}
