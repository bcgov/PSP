using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectPropertyConfiguration class, provides a way to configure project properties in the database.
    ///</summary>
    public class ProjectPropertyConfiguration : BaseAppEntityConfiguration<ProjectProperty>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectProperty> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ProjectId)
                .HasComment("Foreign key to project");
            builder.Property(m => m.PropertyId)
                .HasComment("Foreign key to property");

            builder.Property(m => m.IsDisabled)
                .HasComment("Whether this record is disabled");

            builder.HasOne(m => m.Project).WithMany(m => m.PropertiesManyToMany).HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_PROJCT_PIM_PRJPRP_FK");
            builder.HasOne(m => m.Property).WithMany(m => m.ProjectsManyToMany).HasForeignKey(m => m.PropertyId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_PRPRTY_PIM_PRJPRP_FK");

            builder.HasIndex(m => m.ProjectId).HasDatabaseName("PRJPRP_PROJECT_ID_IDX");
            builder.HasIndex(m => m.PropertyId).HasDatabaseName("PRJPRP_PROPERTY_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
