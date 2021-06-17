using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectAgencyResponseConfiguration class, provides a way to record agency responses to project notifications in the database.
    ///</summary>
    public class ProjectAgencyResponseConfiguration : BaseEntityConfiguration<ProjectAgencyResponse>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectAgencyResponse> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ProjectId)
                .HasComment("Foreign key to the project");
            builder.Property(m => m.AgencyId)
                .HasComment("Foreign key to the agency");
            builder.Property(m => m.NotificationId)
                .HasComment("Foreign key to the notification queue");

            builder.Property(m => m.Response)
                .HasComment("The response type");
            builder.Property(m => m.ReceivedOn)
                .HasColumnType("DATETIME")
                .HasComment("The date the response was received on");
            builder.Property(m => m.OfferAmount)
                .HasComment("The amount offered by the agency to purchase the property");
            builder.Property(m => m.Note).HasMaxLength(2000)
                .HasComment("A note about the agency response");

            builder.HasOne(m => m.Project).WithMany(m => m.Responses).HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRAGRP_PROJECT_ID_IDX");
            builder.HasOne(m => m.Agency).WithMany(m => m.ProjectResponses).HasForeignKey(m => m.AgencyId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PRAGRP_AGENCY_ID_IDX");
            builder.HasOne(m => m.Notification).WithMany(m => m.Responses).HasForeignKey(m => m.NotificationId).OnDelete(DeleteBehavior.ClientCascade).IsRequired(false).HasConstraintName("PRAGRP_NOTIFICATION_QUEUE_ID_IDX");

            builder.HasIndex(m => new { m.ProjectId, m.AgencyId }, "PRAGRP_PROJECT_AGENCY_TUC").IsUnique();
            builder.HasIndex(m => new { m.Response, m.ReceivedOn }, "PRAGRP_RESPONSE_RECEIVED_ON_IDX");
            builder.HasIndex(m => m.ProjectId, "PRAGRP_PROJECT_ID_IDX");
            builder.HasIndex(m => m.AgencyId, "PRAGRP_AGENCY_ID_IDX");
            builder.HasIndex(m => m.NotificationId, "PRAGRP_NOTIFICATION_QUEUE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
