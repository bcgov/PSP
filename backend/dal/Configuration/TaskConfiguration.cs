using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// TaskConfiguration class, provides a way to configure task templates in the database.
    ///</summary>
    public class TaskConfiguration : BaseAppEntityConfiguration<Task>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.TaskTypeId)
                .IsRequired()
                .HasMaxLength(40)
                .HasComment("Foreign key to task template type");
            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this task template is disabled");

            builder.HasOne(m => m.TaskType).WithMany(m => m.Tasks).HasForeignKey(m => m.TaskTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_TSKTMT_PIM_TSKTMP_FK");

            builder.HasIndex(m => m.TaskTypeId).HasDatabaseName("TSKTMP_TASK_TEMPLATE_TYPE_CODE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
