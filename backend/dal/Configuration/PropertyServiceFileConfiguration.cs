using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyServiceFileConfiguration class, provides a way to configure property service files in the database.
    ///</summary>
    public class PropertyServiceFileConfiguration : BaseAppEntityConfiguration<PropertyServiceFile>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyServiceFile> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.FileTypeId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to the property service file type");

            builder.HasOne(m => m.FileType).WithMany(m => m.ServiceFiles).HasForeignKey(m => m.FileTypeId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_PRSVFT_PIM_PRPSVC_FK");

            builder.HasIndex(m => m.FileTypeId).HasDatabaseName("PRPSVC_PROPERTY_SERVICE_FILE_TYPE_CODE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
