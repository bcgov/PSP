using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// BaseEntityConfiguration class, provides a way to configure base entity in the database.
    ///</summary>
    public abstract class CodeEntityConfiguration<TBase> : BaseAppEntityConfiguration<TBase>
        where TBase : CodeEntity
    {
        #region Methods
        protected void CodeConfigure(EntityTypeBuilder<TBase> builder)
        {
            builder.Property(m => m.Code)
                .IsRequired()
                .HasComment("Unique human friendly code name to identity this record");
            builder.Property(m => m.DisplayOrder).HasDefaultValue(0)
                .HasComment("Sorting order of record");
            builder.Property(m => m.IsDisabled).HasDefaultValue(false)
                .HasComment("Whether this record is disabled");

            base.Configure(builder);
        }

        public override void Configure(EntityTypeBuilder<TBase> builder)
        {
            CodeConfigure(builder);
        }
        #endregion
    }
}
