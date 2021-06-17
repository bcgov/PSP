using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// BuildingEvaluationConfiguration class, provides a way to configure building evaluations in the database.
    ///</summary>
    public class BuildingEvaluationConfiguration : BaseEntityConfiguration<BuildingEvaluation>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<BuildingEvaluation> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.BuildingId).IsRequired()
                .HasComment("Foreign key to the building");

            builder.Property(m => m.Date).IsRequired()
                .HasColumnType("DATETIME")
                .HasComment("The date this evaluation is for");

            builder.Property(m => m.Key).IsRequired()
                .HasComment("The type of evaluation taken");

            builder.Property(m => m.Value)
                .HasComment("The value of the evaluation");
            builder.Property(m => m.Note).HasMaxLength(500)
                .HasComment("A note about the evaluation");

            builder.HasOne(m => m.Building).WithMany(m => m.Evaluations).HasForeignKey(m => m.BuildingId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("BLDEVL_BUILDING_ID_IDX");

            builder.HasIndex(m => new { m.BuildingId, m.Date, m.Key }, "BLDEVL_BUILDING_ID_DATE_KEY_TUC").IsUnique();
            builder.HasIndex(m => new { m.BuildingId }, "BLDEVL_BUILDING_ID_IDX");
            builder.HasIndex(m => new { m.Date }, "BLDEVL_DATE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
