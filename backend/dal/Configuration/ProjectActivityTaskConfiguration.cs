using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectActivityTaskConfiguration class, provides a way to configure project activity tasks in the database.
    ///</summary>
    public class ProjectActivityTaskConfiguration : BaseAppEntityConfiguration<ProjectActivityTask>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectActivityTask> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ProjectActivityId)
                .HasComment("Foreign key to the project activity");
            builder.Property(m => m.TaskId)
                .HasComment("Foreign key to the task template");
            builder.Property(m => m.UserId)
                .HasComment("Foreign key to the user");

            builder.HasOne(m => m.ProjectActivity).WithMany(m => m.ProjectActivityTasks).HasForeignKey(m => m.ProjectActivityId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_ACTVTY_PIM_TASK_FK");
            builder.HasOne(m => m.Task).WithMany(m => m.ProjectActivityTasks).HasForeignKey(m => m.TaskId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_TSKTMP_PIM_TASK_FK");
            builder.HasOne(m => m.User).WithMany(m => m.ProjectActivityTasks).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_USER_PIM_TASK_FK");

            builder.HasIndex(m => m.ProjectActivityId).HasDatabaseName("TASK_ACTIVITY_ID_IDX");
            builder.HasIndex(m => m.TaskId).HasDatabaseName("TASK_TASK_TEMPLATE_ID_IDX");
            builder.HasIndex(m => m.UserId).HasDatabaseName("TASK_USER_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
