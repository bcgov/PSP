using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// WorkflowConfiguration class, provides a way to configure workflow model types in the database.
    ///</summary>
    public class WorkflowConfiguration : BaseAppEntityConfiguration<Workflow>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Workflow> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.WorkflowTypeId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to workflow model type");

            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this workflow is disabled");

            builder.HasOne(m => m.WorkflowType).WithMany(m => m.Workflows).HasForeignKey(m => m.WorkflowTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_WFLMDT_PIM_WFLMDL_FK");

            builder.HasIndex(m => m.WorkflowTypeId).HasDatabaseName("WFLMDL_WORKFLOW_MODEL_TYPE_CODE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
