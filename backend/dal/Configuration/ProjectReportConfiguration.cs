using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectReportConfiguration class, provides a way to configure project reports in the database.
    ///</summary>
    public class ProjectReportConfiguration : BaseEntityConfiguration<ProjectReport>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectReport> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Name).HasMaxLength(250)
                .HasComment("A name to identify the record").IsNullable();
            builder.Property(m => m.ReportType)
                .HasComment("The type of report");

            builder.Property(m => m.From)
                .HasColumnType("DATETIME")
                .HasComment("The date this project period begins from");
            builder.Property(m => m.To).IsRequired()
                .HasColumnType("DATETIME")
                .HasComment("The date this project period ends at");

            builder.Property(m => m.IsFinal)
                .HasComment("Whether this report is considered final");

            builder.HasIndex(m => new { m.To, m.From, m.IsFinal }, "PRJRPT_TO_FROM_IS_FINAL_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
