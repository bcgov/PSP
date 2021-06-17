using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// WorkflowConfiguration class, provides a way to configure workflow in the database.
    ///</summary>
    public class WorkflowConfiguration : LookupEntityConfiguration<Workflow>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Workflow> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Name).HasMaxLength(150).IsRequired()
                .HasComment("A unique name to identify the record");

            builder.Property(m => m.Code).HasMaxLength(20).IsRequired()
                .HasComment("A human friendly unique code to identify the record");

            builder.Property(m => m.Description).HasMaxLength(500).IsRequired()
                .HasComment("A description of the workflow");

            builder.HasIndex(m => new { m.Name }, "WRKFLW_NAME_TUC").IsUnique();
            builder.HasIndex(m => new { m.Code }, "WRKFLW_CODE_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled, m.SortOrder }, "WRKFLOW_IS_DISABLED_SORT_ORDER_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
