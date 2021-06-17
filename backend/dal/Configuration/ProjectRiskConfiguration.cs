using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectRiskConfiguration class, provides a way to configure project risks in the database.
    ///</summary>
    public class ProjectRiskConfiguration : CodeEntityConfiguration<ProjectRisk>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ProjectRisk> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Code).HasMaxLength(10).IsRequired()
                .HasComment("A unique human friendly code to identify the record");

            builder.Property(m => m.Name).HasMaxLength(150).IsRequired()
                .HasComment("A name to identify the record");

            builder.Property(m => m.Description).HasMaxLength(500)
                .HasComment("A description of the record");

            builder.HasIndex(m => new { m.Code }, "PRJRSK_CODE_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled, m.Name, m.SortOrder }, "PRJRSK_IS_DISABLED_NAME_SORT_ORDER_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
