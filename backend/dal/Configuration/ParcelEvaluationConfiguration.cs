using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ParcelEvaluationConfiguration class, provides a way to configure parcel evaluations in the database.
    ///</summary>
    public class ParcelEvaluationConfiguration : BaseAppEntityConfiguration<ParcelEvaluation>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ParcelEvaluation> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ParcelId).IsRequired()
                .HasComment("Foreign key to parcel");

            builder.Property(m => m.Date).IsRequired()
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()") // This should not be a default value however MOTI standards requires it to have a default value.
                .HasComment("The date this evaluation was taken");
            builder.Property(m => m.Key).IsRequired()
                .HasComment("The evaluation type");
            builder.Property(m => m.Value)
                .HasColumnType("MONEY")
                .HasComment("The value of the evaluation");
            builder.Property(m => m.Note).HasMaxLength(500)
                .HasComment("A note about the evaluation");
            builder.Property(m => m.Firm).HasMaxLength(150)
                .HasComment("The firm or company name that provided the evaluation");

            builder.HasOne(m => m.Parcel).WithMany(m => m.Evaluations).HasForeignKey(m => m.ParcelId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PREVAL_PARCEL_ID_IDX");

            builder.HasIndex(m => new { m.ParcelId, m.Date, m.Key }, "PREVAL_PARCEL_ID_DATE_KEY_TUC").IsUnique();
            builder.HasIndex(m => new { m.Value }, "PREVAL_VALUE_IDX");
            builder.HasIndex(m => m.ParcelId, "PREVAL_PARCEL_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
