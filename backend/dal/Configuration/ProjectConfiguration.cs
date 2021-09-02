using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectConfiguration class, provides a way to configure projects in the database.
    ///</summary>
    public class ProjectConfiguration : BaseAppEntityConfiguration<Project>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ProjectTypeId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to project type");
            builder.Property(m => m.StatusId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to project status type");
            builder.Property(m => m.RiskId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to project risk type");
            builder.Property(m => m.TierId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to project tier type");

            builder.HasOne(m => m.ProjectType).WithMany(m => m.Projects).HasForeignKey(m => m.ProjectTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PRJTYP_PIM_PROJCT_FK");
            builder.HasOne(m => m.Status).WithMany(m => m.Projects).HasForeignKey(m => m.StatusId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PRJSTY_PIM_PROJCT_FK");
            builder.HasOne(m => m.Risk).WithMany(m => m.Projects).HasForeignKey(m => m.RiskId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PRJRSK_PIM_PROJCT_FK");
            builder.HasOne(m => m.Tier).WithMany(m => m.Projects).HasForeignKey(m => m.TierId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PROJTR_PIM_PROJCT_FK");

            builder.HasMany(m => m.Workflows).WithMany(m => m.Projects).UsingEntity<ProjectWorkflow>(
                m => m.HasOne(m => m.Workflow).WithMany(m => m.ProjectsManyToMany).HasForeignKey(m => m.WorkflowId),
                m => m.HasOne(m => m.Project).WithMany(m => m.WorkflowsManyToMany).HasForeignKey(m => m.ProjectId)
            );

            builder.HasMany(m => m.Properties).WithMany(m => m.Projects).UsingEntity<ProjectProperty>(
                m => m.HasOne(m => m.Property).WithMany(m => m.ProjectsManyToMany).HasForeignKey(m => m.PropertyId),
                m => m.HasOne(m => m.Project).WithMany(m => m.PropertiesManyToMany).HasForeignKey(m => m.ProjectId)
            );

            builder.HasIndex(m => m.ProjectTypeId).HasDatabaseName("PROJCT_PROJECT_TYPE_CODE_IDX");
            builder.HasIndex(m => m.StatusId).HasDatabaseName("PROJCT_PROJECT_STATUS_TYPE_CODE_IDX");
            builder.HasIndex(m => m.RiskId).HasDatabaseName("PROJCT_PROJECT_RISK_TYPE_CODE_IDX");
            builder.HasIndex(m => m.TierId).HasDatabaseName("PROJCT_PROJECT_TIER_TYPE_CODE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
