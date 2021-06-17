using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectStatusNotificationConfiguration class, provides a way to configure valid project status notifications in the database.
    ///</summary>
    public class ProjectStatusNotificationConfiguration : BaseEntityConfiguration<ProjectStatusNotification>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectStatusNotification> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.TemplateId)
                .HasComment("Foreign key to notification template");
            builder.Property(m => m.FromStatusId)
                .HasComment("Foreign key to project status which describes the from transition");
            builder.Property(m => m.ToStatusId)
                .HasComment("Foreign key to project status which describes the to transition");

            builder.Property(m => m.Priority)
                .HasComment("Priority of the notification");
            builder.Property(m => m.Delay)
                .HasComment("The type of delay this notification will have");
            builder.Property(m => m.DelayDays)
                .HasComment("The number of days to delay this notification");

            builder.HasOne(m => m.Template).WithMany(m => m.Status).HasForeignKey(m => m.TemplateId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRSTNT_NOTIFICATION_TEMPLATE_ID_IDX");
            builder.HasOne(m => m.FromStatus).WithMany().HasForeignKey(m => m.FromStatusId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRSTNT_FROM_PROJECT_STATUS_ID_IDX");
            builder.HasOne(m => m.ToStatus).WithMany().HasForeignKey(m => m.ToStatusId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRSTNT_TO_PROJECT_STATUS_ID_IDX");

            builder.HasIndex(m => new { m.FromStatusId, m.ToStatusId }, "PRSTNT_FROM_PROJECT_STATUS_ID_TO_PROJECT_STATUS_ID_IDX");
            builder.HasIndex(m => m.TemplateId, "PRSTNT_NOTIFICATION_TEMPLATE_ID_IDX");
            builder.HasIndex(m => m.FromStatusId, "PRSTNT_FROM_PROJECT_STATUS_ID_IDX");
            builder.HasIndex(m => m.ToStatusId, "PRSTNT_TO_PROJECT_STATUS_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
