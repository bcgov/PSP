using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ProjectConfiguration class, provides a way to configure projects in the database.
    ///</summary>
    public class ProjectConfiguration : BaseEntityConfiguration<Project>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.WorkflowId)
                .HasComment("Foreign key to workflow this project is currently in");
            builder.Property(m => m.AgencyId)
                .HasComment("Foreign key to the owning agency");
            builder.Property(m => m.StatusId)
                .HasComment("Foreign key to the project status this project is currently in");
            builder.Property(m => m.TierLevelId)
                .HasComment("Foreign key to the tier level this project is in");
            builder.Property(m => m.RiskId)
                .HasComment("Foreign key to the project risk this project is in");

            builder.Property(m => m.ProjectType)
                .HasComment("The type of project");
            builder.Property(m => m.ProjectNumber).HasMaxLength(25).IsRequired()
                .HasComment("A unique human friendly number to identify this project");

            builder.Property(m => m.ReportedFiscalYear)
                .HasComment("The fiscal year the project was report in");
            builder.Property(m => m.ActualFiscalYear)
                .HasComment("The fiscal year the project was completed in");

            builder.Property(m => m.Name).HasMaxLength(100).IsRequired()
                .HasComment("A name to identify the project");

            builder.Property(m => m.Manager).HasMaxLength(150)
                .HasComment("The name of the project manager");
            builder.Property(m => m.Description).HasMaxLength(1000)
                .HasComment("A description of the project");

            builder.Property(m => m.Metadata)
                .HasComment("Serialized JSON data containing additional project information");

            builder.Property(m => m.SubmittedOn)
                .HasColumnType("DATETIME")
                .HasComment("The date when the project was submitted");
            builder.Property(m => m.ApprovedOn)
                .HasColumnType("DATETIME")
                .HasComment("The date when the project was approved");
            builder.Property(m => m.DeniedOn)
                .HasColumnType("DATETIME")
                .HasComment("The date when the project was denied");
            builder.Property(m => m.CancelledOn)
                .HasColumnType("DATETIME")
                .HasComment("The date when the project was cancelled");
            builder.Property(m => m.CompletedOn)
                .HasColumnType("DATETIME")
                .HasComment("The date when the project was completed");

            builder.Property(m => m.NetBook)
                .HasComment("The netbook value of the project");
            builder.Property(m => m.Market)
                .HasComment("The market value of the project");
            builder.Property(m => m.Assessed)
                .HasComment("The assessed value of the project");
            builder.Property(m => m.Appraised)
                .HasComment("The appraised value of the project");

            builder.HasOne(m => m.Status).WithMany(s => s.Projects).HasForeignKey(m => m.StatusId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PROJCT_PROJECT_STATUS_ID_IDX");
            builder.HasOne(m => m.Agency).WithMany(a => a.Projects).HasForeignKey(m => m.AgencyId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PROJCT_AGENCY_ID_IDX");
            builder.HasOne(m => m.TierLevel).WithMany(t => t.Projects).HasForeignKey(m => m.TierLevelId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PROJCT_TIER_LEVEL_ID_IDX");
            builder.HasOne(m => m.Workflow).WithMany(m => m.Projects).HasForeignKey(m => m.WorkflowId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("PROJCT_WORKFLOW_ID_IDX");
            builder.HasOne(m => m.Risk).WithMany(m => m.Projects).HasForeignKey(m => m.RiskId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PROJCT_PROJECT_RISK_ID_IDX");

            builder.HasIndex(m => m.ProjectNumber, "PROJCT_PROJECT_NUMBER_TUC").IsUnique();
            builder.HasIndex(m => new { m.Name, m.Assessed, m.ReportedFiscalYear, m.ActualFiscalYear }, "PROJCT_NAME_ASSESSED_REPORTED_FISCAL_ACTUAL_FISCAL_IDX");
            builder.HasIndex(m => m.StatusId, "PROJCT_PROJECT_STATUS_ID_IDX");
            builder.HasIndex(m => m.AgencyId, "PROJCT_AGENCY_ID_IDX");
            builder.HasIndex(m => m.TierLevelId, "PROJCT_TIER_LEVEL_ID_IDX");
            builder.HasIndex(m => m.WorkflowId, "PROJCT_WORKFLOW_ID_IDX");
            builder.HasIndex(m => m.RiskId, "PROJCT_PROJECT_RISK_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
