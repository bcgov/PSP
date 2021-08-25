using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectStatusTypeConfiguration class, provides a way to configure project status types in the database.
    ///</summary>
    public class ProjectStatusTypeConfiguration : TypeEntityConfiguration<ProjectStatusType, string>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectStatusType> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.Property(m => m.Id)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Primary key code to identify record");

            builder.Property(m => m.CodeGroup)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("A code to identify a group of related status");
            builder.Property(m => m.Text)
                .IsRequired()
                .HasMaxLength(1000)
                .HasComment("Text to display the status");
            builder.Property(m => m.IsMilestone)
                .HasDefaultValue(false)
                .HasComment("Whether this status is a milestone");
            builder.Property(m => m.IsTerminal)
                .HasDefaultValue(false)
                .HasComment("Whether this status is terminal");

            base.Configure(builder);
        }
        #endregion
    }
}
