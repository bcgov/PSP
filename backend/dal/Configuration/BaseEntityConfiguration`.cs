using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// BaseEntityConfiguration class, provides a way to configure base entity in the database.
    ///</summary>
    public abstract class BaseEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : BaseEntity
    {
        #region Methods
        protected void BaseConfigure(EntityTypeBuilder<TBase> builder)
        {
            #region Application
            builder.Property(m => m.CreatedOn)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("When this record was created")
                .HasAnnotation("ColumnOrder", 88);
            builder.Property(m => m.CreatedBy)
                .HasMaxLength(30)
                .HasComment("Reference to the username who created this record")
                .HasAnnotation("ColumnOrder", 89);
            builder.Property(m => m.CreatedByKey)
                .HasComment("Reference to the user uid who created this record")
                .HasAnnotation("ColumnOrder", 90);
            builder.Property(m => m.CreatedByDirectory)
                .HasMaxLength(30)
                .HasComment("Reference to the user directory who created this record [IDIR, BCeID]")
                .HasAnnotation("ColumnOrder", 91);
            builder.Property(m => m.UpdatedOn)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("When this record was last updated")
                .HasAnnotation("ColumnOrder", 92);
            builder.Property(m => m.UpdatedBy)
                .HasMaxLength(30)
                .HasComment("Reference to the user who last updated this record")
                .HasAnnotation("ColumnOrder", 93);
            builder.Property(m => m.UpdatedByKey)
                .HasComment("Reference to the user uid who updated this record")
                .HasAnnotation("ColumnOrder", 94);
            builder.Property(m => m.UpdatedByDirectory)
                .HasMaxLength(30)
                .HasComment("Reference to the user directory who updated this record [IDIR, BCeID]")
                .HasAnnotation("ColumnOrder", 95);
            #endregion

            #region Database
            builder.Property(m => m.DbCreatedOn)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("When this record was created")
                .HasAnnotation("ColumnOrder", 96);
            builder.Property(m => m.DbCreatedBy)
                .HasMaxLength(30)
                .HasDefaultValueSql("user_name()")
                .HasComment("Reference to the user who created this record")
                .HasAnnotation("ColumnOrder", 97);
            builder.Property(m => m.DbUpdatedOn)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("When this record was last updated")
                .HasAnnotation("ColumnOrder", 98);
            builder.Property(m => m.DbUpdatedBy)
                .HasMaxLength(30)
                .HasDefaultValueSql("user_name()")
                .HasComment("Reference to the user who last updated this record")
                .HasAnnotation("ColumnOrder", 99);
            #endregion

            builder.Property(m => m.RowVersion)
                .HasColumnType("BIGINT")
                .HasDefaultValue(0)
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
