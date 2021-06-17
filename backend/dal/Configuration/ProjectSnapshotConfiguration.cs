using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectSnapshotConfiguration class, provides a way to configure project Snapshot in the database.
    ///</summary>
    public class ProjectSnapshotConfiguration : BaseEntityConfiguration<ProjectSnapshot>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectSnapshot> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ProjectId).IsRequired()
                .HasComment("Foreign key to the project");

            builder.Property(m => m.SnapshotOn)
                .HasColumnType("DATETIME")
                .HasComment("The date the snapshot was taken");
            builder.Property(m => m.Market)
                .HasComment("The project market value");
            builder.Property(m => m.Assessed)
                .HasComment("The project assessed value");
            builder.Property(m => m.Appraised)
                .HasComment("The project appraised value");
            builder.Property(m => m.NetBook)
                .HasComment("The project netbook value");

            builder.Property(m => m.Metadata)
                .HasComment("A JSON serialized summary of the project");

            builder.HasOne(m => m.Project).WithMany(p => p.Snapshots).HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PRJSNP_PROJECT_ID_IDX");

            builder.HasIndex(m => new { m.SnapshotOn }, "PRJSNP_SNAPSHOT_ON_IDX");
            builder.HasIndex(m => m.ProjectId, "PRJSNP_PROJECT_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
