using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyPropertyServiceFileConfiguration class, provides a way to configure relationships between properties and service files in the database.
    ///</summary>
    public class PropertyPropertyServiceFileConfiguration : BaseAppEntityConfiguration<PropertyPropertyServiceFile>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyPropertyServiceFile> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.PropertyId)
                .IsRequired()
                .HasComment("Foreign key to the property");
            builder.Property(m => m.ServiceFileId)
                .IsRequired()
                .HasComment("Foreign key to property service file");

            builder.HasOne(m => m.Property).WithMany(m => m.ServiceFilesManyToMany).HasForeignKey(m => m.PropertyId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_PRPRTY_PIM_PRPRSF_FK");
            builder.HasOne(m => m.ServiceFile).WithMany(m => m.PropertiesManyToMany).HasForeignKey(m => m.ServiceFileId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_PRPSVC_PIM_PRPRSF_FK");

            builder.HasIndex(m => new { m.PropertyId, m.ServiceFileId }, "PRPPSF_PROPERTY_PROPERTY_SERVICE_FILE_TUC").IsUnique();

            builder.HasIndex(m => m.PropertyId).HasDatabaseName("PRPPSF_PROPERTY_ID_IDX");
            builder.HasIndex(m => m.ServiceFileId).HasDatabaseName("PRPPSF_PROPERTY_SERVICE_FILE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
