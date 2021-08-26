using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// TypeEntityConfiguration class, provides a way to configure base type entity in the database.
    ///</summary>
    public abstract class TypeEntityConfiguration<TBase, KeyType> : BaseEntityConfiguration<TBase>
        where TBase : TypeEntity<KeyType>
    {
        #region Methods
        protected void TypeConfigure(EntityTypeBuilder<TBase> builder)
        {
            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValueSql("''")
                .HasComment("Friendly description of record");
            builder.Property(m => m.DisplayOrder)
                .HasComment("Sorting order of record");
            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this record is disabled");

            base.Configure(builder);
        }

        public override void Configure(EntityTypeBuilder<TBase> builder)
        {
            TypeConfigure(builder);
        }
        #endregion
    }
}
