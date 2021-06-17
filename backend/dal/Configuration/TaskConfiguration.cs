using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// TaskConfiguration class, provides a way to configure process tasks in the database.
    ///</summary>
    public class TaskConfiguration : LookupEntityConfiguration<Task>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.StatusId)
                .HasComment("Foreign key to the project status this task belongs to");

            builder.Property(m => m.Name).HasMaxLength(150).IsRequired()
                .HasComment("A name to identify the record");
            builder.Property(m => m.Description).HasMaxLength(1000)
                .HasComment("A description of the task");
            builder.Property(m => m.IsOptional)
                .HasComment("Whether this task is optional");

            builder.HasOne(m => m.Status).WithMany(m => m.Tasks).HasForeignKey(m => m.StatusId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("TASK_PROJECT_STATUS_ID_IDX");

            builder.HasIndex(m => new { m.IsDisabled, m.IsOptional, m.Name, m.SortOrder }, "TASK_IS_DISABLED_IS_OPTIONAL_NAME_SORT_ORDER_IDX");
            builder.HasIndex(m => m.StatusId, "TASK_PROJECT_STATUS_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
