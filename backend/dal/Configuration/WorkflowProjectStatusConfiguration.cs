using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// WorkflowProjectStatusConfiguration class, provides a way to configure project properties in the database.
    ///</summary>
    public class WorkflowProjectStatusConfiguration : BaseEntityConfiguration<WorkflowProjectStatus>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<WorkflowProjectStatus> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.WorkflowId).IsRequired()
                .HasComment("Foreign key to the workflow");
            builder.Property(m => m.StatusId).IsRequired()
                .HasComment("Foreign key to the project status");

            builder.Property(m => m.IsOptional)
                .HasComment("Whether this workflow project status is optional");
            builder.Property(m => m.SortOrder)
                .HasComment("The sorting order to display this record");

            builder.HasOne(m => m.Workflow).WithMany(m => m.Status).HasForeignKey(m => m.WorkflowId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("WRPRST_WORKFLOW_ID_IDX");
            builder.HasOne(m => m.Status).WithMany(m => m.Workflows).HasForeignKey(m => m.StatusId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("WRPRST_PROJECT_STATUS_ID_IDX");

            builder.HasIndex(m => new { m.WorkflowId, m.StatusId }, "WRPRST_WORKFLOW_ID_PROJECT_STATUS_ID_TUC").IsUnique();
            builder.HasIndex(m => m.WorkflowId, "WRPRST_WORKFLOW_ID_IDX");
            builder.HasIndex(m => m.StatusId, "WRPRST_PROJECT_STATUS_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
