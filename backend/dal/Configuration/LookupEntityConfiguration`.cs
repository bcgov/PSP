using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// BaseEntityConfiguration class, provides a way to configure base entity in the database.
    ///</summary>
    public abstract class LookupEntityConfiguration<TBase> : BaseEntityConfiguration<TBase>
        where TBase : LookupEntity
    {
        #region Methods
        protected void LookupConfigure(EntityTypeBuilder<TBase> builder)
        {
            builder.Property(m => m.SortOrder).HasDefaultValue(0)
                .HasComment("Sorting order of record");
            builder.Property(m => m.IsDisabled).HasDefaultValue(false)
                .HasComment("Whether this record is disabled");

            base.Configure(builder);
        }

        public override void Configure(EntityTypeBuilder<TBase> builder)
        {
            LookupConfigure(builder);
        }
        #endregion
    }
}
