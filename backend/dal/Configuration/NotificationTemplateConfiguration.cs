using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;
using System;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// NotificationTemplateConfiguration class, provides a way to configure notification templates in the database.
    ///</summary>
    public class NotificationTemplateConfiguration : BaseEntityConfiguration<NotificationTemplate>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<NotificationTemplate> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Name).HasMaxLength(100).IsRequired()
                .HasComment("A unique name to identify the record");

            builder.Property(m => m.Description).HasMaxLength(500)
                .HasComment("A description to describe the record");

            builder.Property(m => m.Audience).HasMaxLength(50)
                .HasConversion(v => v.ToString(), v => (NotificationAudiences)Enum.Parse(typeof(NotificationAudiences), v))
                .HasComment("The audience who will receive the notification");
            builder.Property(m => m.Priority).HasMaxLength(50)
                .HasConversion(v => v.ToString(), v => (NotificationPriorities)Enum.Parse(typeof(NotificationPriorities), v))
                .HasComment("The notification priority");
            builder.Property(m => m.Encoding).HasMaxLength(50)
                .HasConversion(v => v.ToString(), v => (NotificationEncodings)Enum.Parse(typeof(NotificationEncodings), v))
                .HasComment("The encoding of the notification body");
            builder.Property(m => m.BodyType).HasMaxLength(50)
                .HasConversion(v => v.ToString(), v => (NotificationBodyTypes)Enum.Parse(typeof(NotificationBodyTypes), v))
                .HasComment("The notification body type");

            builder.Property(m => m.Subject).HasMaxLength(200).IsRequired()
                .HasComment("The subject of the notification");
            builder.Property(m => m.IsDisabled)
                .HasComment("Whether the notification template is disabled");

            builder.Property(m => m.To).HasMaxLength(500)
                .HasComment("One or more email address to send the notification to");
            builder.Property(m => m.Cc).HasMaxLength(500)
                .HasComment("One or more email address to carbon copy the notification to");
            builder.Property(m => m.Bcc).HasMaxLength(500)
                .HasComment("One or more email address to blind carbon copy the notification to");

            builder.Property(m => m.Tag).HasMaxLength(50)
                .HasComment("A way to identify related notifications");

            builder.HasIndex(m => m.Name, "NTTMPL_NAME_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled, m.Tag }, "NTTMPL_IS_DISABLED_TAG_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
