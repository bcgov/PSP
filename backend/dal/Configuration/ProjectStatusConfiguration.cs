using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectStatusConfiguration class, provides a way to configure project status in the database.
    ///</summary>
    public class ProjectStatusConfiguration : LookupEntityConfiguration<ProjectStatus>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectStatus> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Code).HasMaxLength(10).IsRequired()
                .HasComment("A unique human friendly code to identify the record");

            builder.Property(m => m.Name).HasMaxLength(150).IsRequired()
                .HasComment("A name to identify the record");
            builder.Property(m => m.GroupName).HasMaxLength(150)
                .HasComment("A name to group related records");

            builder.Property(m => m.Description).HasMaxLength(1000)
                .HasComment("A description of the project status");

            builder.Property(m => m.IsMilestone).HasDefaultValue(false)
                .HasComment("Whether this project status is a milestone");
            builder.Property(m => m.IsTerminal).HasDefaultValue(false)
                .HasComment("Whether this project status is a terminal status");

            builder.Property(m => m.Route).HasMaxLength(150).IsRequired()
                .HasComment("A path that represents this status");

            builder.HasIndex(m => new { m.Code }, "PRJSTS_CODE_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled, m.Name, m.SortOrder }, "PRJSTS_IS_DISABLED_NAME_SORT_ORDER_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
