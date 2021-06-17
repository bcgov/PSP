using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectStatusTransitionConfiguration class, provides a way to configure project status transitions in the database.
    ///</summary>
    public class ProjectStatusTransitionConfiguration : BaseEntityConfiguration<ProjectStatusTransition>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectStatusTransition> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.FromWorkflowStatusId).IsRequired()
                .HasComment("Foreign key to the workflow status");
            builder.Property(m => m.ToWorkflowStatusId).IsRequired()
                .HasComment("Foreign key the the from workflow status");

            builder.Property(m => m.Action).HasMaxLength(100)
                .HasComment("Description of the action being performed in this transition");

            builder.HasOne(m => m.FromWorkflowStatus).WithMany(m => m.ToStatus).HasForeignKey(m => m.FromWorkflowStatusId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PRSTTX_FROM_WORKFLOW_PROJECT_STATUS_ID_IDX");
            builder.HasOne(m => m.ToWorkflowStatus).WithMany(m => m.FromStatus).HasForeignKey(m => m.ToWorkflowStatusId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRSTTX_TO_WORKFLOW_PROJECT_STATUS_ID_IDX");

            builder.HasIndex(m => new { m.FromWorkflowStatusId, m.ToWorkflowStatusId }, "PRSTTX_FROM_WORKFLOW_TO_WORKFLOW_TUC").IsUnique();
            builder.HasIndex(m => m.FromWorkflowStatusId, "PRSTTX_FROM_WORKFLOW_PROJECT_STATUS_ID_IDX");
            builder.HasIndex(m => m.ToWorkflowStatusId, "PRSTTX_TO_WORKFLOW_PROJECT_STATUS_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
