using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectWorkflowConfiguration class, provides a way to configure the many-to-many relationship between projects and workflows in the database.
    ///</summary>
    public class ProjectWorkflowConfiguration : BaseAppEntityConfiguration<ProjectWorkflow>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectWorkflow> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.HasMotiSequence(m => m.ProjectId)
                .HasComment("Foreign key to the project");
            builder.HasMotiSequence(m => m.WorkflowId)
                .HasComment("Foreign key to the workflow model");

            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this project workflow is disabled");

            builder.HasOne(m => m.Project).WithMany(m => m.WorkflowsManyToMany).HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_PROJCT_PIM_PRWKMD_FK");
            builder.HasOne(m => m.Workflow).WithMany(m => m.ProjectsManyToMany).HasForeignKey(m => m.WorkflowId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_WFLMDL_PIM_PRWKMD_FK");

            builder.HasIndex(m => m.ProjectId).HasDatabaseName("PRWKMD_PROJECT_ID_IDX");
            builder.HasIndex(m => m.WorkflowId).HasDatabaseName("PRWKMD_WORKFLOW_MODEL_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
