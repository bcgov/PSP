using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyImprovementConfiguration class, provides a way to configure property improvements in the database.
    ///</summary>
    public class PropertyImprovementConfiguration : BaseAppEntityConfiguration<PropertyImprovement>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyImprovement> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");
            builder.Property(m => m.StructureSize)
                .HasMaxLength(2000)
                .HasComment("The size of the structure of the improvement");
            builder.Property(m => m.Unit)
                .HasMaxLength(2000)
                .HasComment("Notes related to any units within the improvement");
            builder.Property(m => m.Description)
                .HasMaxLength(2000)
                .HasComment("A description of the improvement");

            builder.Property(m => m.LeaseId)
                .HasComment("Foreign key to lease");

            builder.HasOne(m => m.Lease).WithMany(m => m.Improvements).HasForeignKey(m => m.LeaseId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PROPLS_PIM_PIMPRV_FK");

            builder.HasOne(m => m.PropertyImprovementType).WithMany(m => m.Improvements).HasForeignKey(m => m.PropertyImprovementTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PIMPRT_PIM_PIMPRV_FK");

            builder.HasIndex(m => m.LeaseId).HasDatabaseName("PIMPRV_PROPERTY_LEASE_ID_IDX");
            builder.HasIndex(m => m.PropertyImprovementTypeId).HasDatabaseName("PIMPRV_PROPERTY_IMPROVEMENT_TYPE_CODE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
