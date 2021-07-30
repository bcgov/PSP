using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;
using System;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// NotificationQueueConfiguration class, provides a way to manage a notification queue in the database.
    ///</summary>
    public class NotificationQueueConfiguration : BaseAppEntityConfiguration<NotificationQueue>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<NotificationQueue> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ToAgencyId)
                .HasComment("Foreign key to the agency the notification was sent to");
            builder.Property(m => m.TemplateId)
                .HasComment("Foreign key to the notification template");

            builder.Property(m => m.Key).HasMaxLength(50)
                .HasComment("A unique key to identify the notification");
            builder.Property(m => m.ChesMessageId)
                .HasComment("Common Hosted Email Service message key");
            builder.Property(m => m.ChesTransactionId)
                .HasComment("Common hosted Email Service transaction key");

            builder.Property(m => m.SendOn)
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()") // This should not be a default value however MOTI standards requires it to have a default value.
                .HasComment("The date the message will be sent on");

            builder.Property(m => m.Priority).HasMaxLength(50)
                .HasConversion(v => v.ToString(), v => (NotificationPriorities)Enum.Parse(typeof(NotificationPriorities), v))
                .HasComment("The email priority");
            builder.Property(m => m.Encoding).HasMaxLength(50)
                .HasConversion(v => v.ToString(), v => (NotificationEncodings)Enum.Parse(typeof(NotificationEncodings), v))
                .HasComment("The email encoding");
            builder.Property(m => m.BodyType).HasMaxLength(50)
                .HasConversion(v => v.ToString(), v => (NotificationBodyTypes)Enum.Parse(typeof(NotificationBodyTypes), v))
                .HasComment("The email body type");

            builder.Property(m => m.Subject).HasMaxLength(200).IsRequired()
                .HasComment("The subject of the notification");

            builder.Property(m => m.Body).IsRequired()
                .HasComment("The message body of the notification");

            builder.Property(m => m.Tag).HasMaxLength(50)
                .HasComment("A way to identify related notifications");

            builder.Property(m => m.To).HasMaxLength(500)
                .HasComment("One more more email addresses the notification was sent to");
            builder.Property(m => m.Cc).HasMaxLength(500)
                .HasComment("One more more email addresses the notification was carbon copied to");
            builder.Property(m => m.Bcc).HasMaxLength(500)
                .HasComment("One more more email addresses the notification was blind carbon copied to");

            builder.HasOne(m => m.ToAgency).WithMany(m => m.Notifications).HasForeignKey(m => m.ToAgencyId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("NOTIFQ_TO_AGENCY_ID_IDX");
            builder.HasOne(m => m.Template).WithMany(m => m.Notifications).HasForeignKey(m => m.TemplateId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("NOTIFQ_NOTIFICATION_TEMPLATE_ID_IDX");

            builder.HasIndex(m => new { m.Key }, "NOTIFQ_NOTIFICATION_UID_TUC").IsUnique();
            builder.HasIndex(m => new { m.Status, m.SendOn, m.Subject }, "NOTIFQ_STATUS_SEND_ON_SUBJECT_IDX");
            builder.HasIndex(m => m.ToAgencyId, "NOTIFQ_TO_AGENCY_ID_IDX");
            builder.HasIndex(m => m.TemplateId, "NOTIFQ_NOTIFICATION_TEMPLATE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
