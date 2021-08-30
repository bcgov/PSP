using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ActivityTaskConfiguration class, provides a way to configure activity task associations in the database.
    ///</summary>
    public class ActivityTaskConfiguration : BaseAppEntityConfiguration<ActivityTask>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ActivityTask> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ActivityId)
                .HasComment("Foreign key to activity model");
            builder.Property(m => m.TaskId)
                .HasComment("Foreign key to task template");
            builder.Property(m => m.IsRequired)
                .HasDefaultValue(false)
                .HasComment("Whether this activity task is mandatory");
            builder.Property(m => m.IsDisabled)
                .HasComment("Whether this task template is disabled");
            builder.Property(m => m.DisplayOrder)
                .HasDefaultValue(0)
                .HasComment("The order this activity task should be implemented");

            builder.HasOne(m => m.Activity).WithMany(m => m.TasksManyToMany).HasForeignKey(m => m.ActivityId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_ACTMDL_PIM_TSKTAM_FK");
            builder.HasOne(m => m.Task).WithMany(m => m.ActivitiesManyToMany).HasForeignKey(m => m.TaskId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_TSKTMP_PIM_TSKTAM_FK");

            builder.HasIndex(m => m.ActivityId).HasDatabaseName("TSKTAM_ACTIVITY_MODEL_ID_IDX");
            builder.HasIndex(m => m.TaskId).HasDatabaseName("TSKTAM_TASK_TEMPLATE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
