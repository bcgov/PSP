using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectNoteConfiguration class, provides a way to configure project notes in the database.
    ///</summary>
    public class ProjectNoteConfiguration : BaseAppEntityConfiguration<ProjectNote>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectNote> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ProjectId)
                .HasComment("Foreign key to project");

            builder.HasOne(m => m.Project).WithMany(m => m.Notes).HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_PROJCT_PIM_PROJNT_FK");

            builder.HasIndex(m => m.ProjectId).HasDatabaseName("PROJNT_PROJECT_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
