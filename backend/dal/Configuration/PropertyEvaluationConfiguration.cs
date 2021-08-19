using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyEvaluationConfiguration class, provides a way to configure property evaluations in the database.
    ///</summary>
    public class PropertyEvaluationConfiguration : BaseAppEntityConfiguration<PropertyEvaluation>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyEvaluation> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.PropertyId)
                .HasComment("Foreign key to property");
            builder.Property(m => m.EvaluatedOn)
                .IsRequired()
                .HasComment("The date the evaluation was taken on");
            builder.Property(m => m.Key)
                .IsRequired()
                .HasComment("A key to identify the type of evaluation");
            builder.Property(m => m.Value)
                .IsRequired()
                .HasColumnType("MONEY")
                .HasComment("The value of the evaluation");
            builder.Property(m => m.Note)
                .HasMaxLength(1000)
                .HasComment("Evaluation description note");

            builder.HasOne(m => m.Property).WithMany(m => m.Evaluations).HasForeignKey(m => m.PropertyId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_PRPRTY_PIM_PRPEVL_FK");

            builder.HasIndex(m => m.PropertyId).HasDatabaseName("PRPEVL_PROPERTY_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
