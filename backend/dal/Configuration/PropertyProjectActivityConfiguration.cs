using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyProjectActivityConfiguration class, provides a way to configure relationship between properties and activities in the database.
    ///</summary>
    public class PropertyProjectActivityConfiguration : BaseAppEntityConfiguration<PropertyProjectActivity>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyProjectActivity> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.PropertyId)
                .HasComment("Foreign key to parcel");
            builder.Property(m => m.ProjectActivityId)
                .HasComment("Foreign key to project activity");

            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this record is disabled");

            builder.HasOne(m => m.Property).WithMany(m => m.ProjectActivitiesManyToMany).HasForeignKey(m => m.PropertyId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_PRPRTY_PIM_PRPACT_FK");
            builder.HasOne(m => m.ProjectActivity).WithMany(m => m.PropertiesManyToMany).HasForeignKey(m => m.ProjectActivityId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_ACTVTY_PIM_PRPACT_FK");

            builder.HasIndex(m => new { m.PropertyId, m.ProjectActivityId }, "PRPACT_PROPERTY_ACTIVITY_TUC").IsUnique();

            builder.HasIndex(m => m.PropertyId).HasDatabaseName("PRPACT_PROPERTY_ID_IDX");
            builder.HasIndex(m => m.ProjectActivityId).HasDatabaseName("PRPACT_ACTIVITY_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
