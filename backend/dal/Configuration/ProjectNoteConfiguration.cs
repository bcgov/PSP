using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectNoteConfiguration class, provides a way to configure project notes in the database.
    ///</summary>
    public class ProjectNoteConfiguration : BaseEntityConfiguration<ProjectNote>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectNote> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ProjectId).IsRequired()
                .HasComment("Foreign key to the project");

            builder.Property(m => m.NoteType).IsRequired()
                .HasComment("The type of note");
            builder.Property(m => m.Note).IsRequired()
                .HasComment("The message of the note");

            builder.HasOne(m => m.Project).WithMany(m => m.Notes).HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PROJNT_PROJECT_ID_IDX");

            builder.HasIndex(m => new { m.NoteType }, "PROJNT_NOTE_TYPE_IDX");
            builder.HasIndex(m => m.ProjectId, "PROJNT_PROJECT_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
