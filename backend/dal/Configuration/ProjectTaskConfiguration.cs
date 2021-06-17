using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectTaskConfiguration class, provides a way to configure project tasks in the database.
    ///</summary>
    public class ProjectTaskConfiguration : BaseEntityConfiguration<ProjectTask>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ProjectId).IsRequired()
                .HasComment("Foreign key to the project");
            builder.Property(m => m.TaskId).IsRequired()
                .HasComment("Foreign key to the task");

            builder.Property(m => m.IsCompleted)
                .HasComment("Whether this task is completed");
            builder.Property(m => m.CompletedOn)
                .HasColumnType("DATETIME")
                .HasComment("The date the task was completed");

            builder.HasOne(m => m.Project).WithMany(m => m.Tasks).HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PRJTSK_PROJECT_ID_IDX");
            builder.HasOne(m => m.Task).WithMany().HasForeignKey(m => m.TaskId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PRJTSK_TASK_ID_IDX");

            builder.HasIndex(m => new { m.ProjectId, m.TaskId }, "PRJTSK_PROJECT_TASK_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsCompleted, m.CompletedOn }, "PRJTSK_IS_COMPLETED_COMPLETED_ON_IDX");
            builder.HasIndex(m => m.ProjectId, "PRJTSK_PROJECT_ID_IDX");
            builder.HasIndex(m => m.TaskId, "PRJTSK_TASK_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
