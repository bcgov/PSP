using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectActivityConfiguration class, provides a way to configure project activities in the database.
    ///</summary>
    public class ProjectActivityConfiguration : BaseAppEntityConfiguration<ProjectActivity>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectActivity> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ProjectId)
                .HasComment("Foreign key to the project");
            builder.Property(m => m.ProjectWorkflowId)
                .HasComment("Foreign key to the project workflow");
            builder.Property(m => m.ActivityId)
                .HasComment("Foreign key to the activity model");

            builder.HasOne(m => m.Project).WithMany(m => m.ProjectActivities).HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PROJCT_PIM_ACTVTY_FK");
            builder.HasOne(m => m.ProjectWorkflow).WithMany(m => m.ProjectActivities).HasForeignKey(m => m.ProjectWorkflowId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PRWKMD_PIM_ACTVTY_FK");
            builder.HasOne(m => m.Activity).WithMany(m => m.ProjectActivities).HasForeignKey(m => m.ActivityId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_ACTMDL_PIM_ACTVTY_FK");

            builder.HasMany(m => m.Properties).WithMany(m => m.ProjectActivities).UsingEntity<PropertyProjectActivity>(
                m => m.HasOne(m => m.Property).WithMany(m => m.ProjectActivitiesManyToMany).HasForeignKey(m => m.PropertyId),
                m => m.HasOne(m => m.ProjectActivity).WithMany(m => m.PropertiesManyToMany).HasForeignKey(m => m.ProjectActivityId)
            );

            builder.HasIndex(m => m.ProjectId).HasDatabaseName("ACTVTY_PROJECT_ID_IDX");
            builder.HasIndex(m => m.ProjectWorkflowId).HasDatabaseName("ACTVTY_WORKFLOW_ID_IDX");
            builder.HasIndex(m => m.ActivityId).HasDatabaseName("ACTVTY_ACTIVITY_MODEL_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
