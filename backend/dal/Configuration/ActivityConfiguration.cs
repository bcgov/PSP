using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ActivityConfiguration class, provides a way to configure activity models in the database.
    ///</summary>
    public class ActivityConfiguration : BaseAppEntityConfiguration<Activity>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("Description of activity model");

            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this activity model is disabled");

            builder.HasMany(m => m.Tasks).WithMany(m => m.Activities).UsingEntity<ActivityTask>(
                m => m.HasOne(m => m.Task).WithMany(m => m.ActivitiesManyToMany).HasForeignKey(m => m.TaskId),
                m => m.HasOne(m => m.Activity).WithMany(m => m.TasksManyToMany).HasForeignKey(m => m.ActivityId)
            );

            base.Configure(builder);
        }
        #endregion
    }
}
