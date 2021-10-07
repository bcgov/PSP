using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// BaseEntityConfiguration class, provides a way to configure base entity in the database.
    ///</summary>
    public abstract class BaseAppEntityConfiguration<TBase> : BaseEntityConfiguration<TBase>
        where TBase : BaseAppEntity
    {
        #region Methods
        protected void BaseAppConfigure(EntityTypeBuilder<TBase> builder)
        {
            #region Application
            builder.Property(m => m.CreatedOn)
                .IsRequired()
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("When this record was created")
                .HasAnnotation("ColumnOrder", 88);
            builder.Property(m => m.CreatedBy)
                .IsRequired()
                .HasDefaultValueSql("user_name()")
                .HasMaxLength(30)
                .HasComment("Reference to the username who created this record")
                .HasAnnotation("ColumnOrder", 89);
            builder.Property(m => m.CreatedByKey)
                .HasComment("Reference to the user uid who created this record")
                .HasAnnotation("ColumnOrder", 90);
            builder.Property(m => m.CreatedByDirectory)
                .IsRequired()
                .HasDefaultValueSql("user_name()")
                .HasMaxLength(30)
                .HasComment("Reference to the user directory who created this record [IDIR, BCeID]")
                .HasAnnotation("ColumnOrder", 91);

            builder.Property(m => m.UpdatedOn)
                .IsRequired()
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("When this record was last updated")
                .HasAnnotation("ColumnOrder", 92);
            builder.Property(m => m.UpdatedBy)
                .IsRequired()
                .HasDefaultValueSql("user_name()")
                .HasMaxLength(30)
                .HasComment("Reference to the user who last updated this record")
                .HasAnnotation("ColumnOrder", 93);
            builder.Property(m => m.UpdatedByKey)
                .HasComment("Reference to the user uid who updated this record")
                .HasAnnotation("ColumnOrder", 94);
            builder.Property(m => m.UpdatedByDirectory)
                .IsRequired()
                .HasDefaultValueSql("user_name()")
                .HasMaxLength(30)
                .HasComment("Reference to the user directory who updated this record [IDIR, BCeID]")
                .HasAnnotation("ColumnOrder", 95);
            #endregion

            base.Configure(builder);
        }

        public override void Configure(EntityTypeBuilder<TBase> builder)
        {
            BaseAppConfigure(builder);
        }
        #endregion
    }
}
