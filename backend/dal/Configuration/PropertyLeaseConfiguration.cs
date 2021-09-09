using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyLeaseConfiguration class, provides a way to configure property lease in the database.
    ///</summary>
    public class PropertyLeaseConfiguration : BaseAppEntityConfiguration<PropertyLease>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyLease> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.LeaseId)
                .HasComment("Foreign key to lease");
            builder.Property(m => m.PropertyId)
                .HasComment("Foreign key to property");

            builder.HasOne(m => m.Property).WithMany(m => m.LeasesManyToMany).HasForeignKey(m => m.PropertyId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PRPRTY_PIM_PROPLS_FK");
            builder.HasOne(m => m.Lease).WithMany(m => m.PropertiesManyToMany).HasForeignKey(m => m.LeaseId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_LEASE_PIM_PROPLS_FK");

            builder.HasIndex(m => m.LeaseId).HasDatabaseName("PROPLS_LEASE_ID_IDX");
            builder.HasIndex(m => m.PropertyId).HasDatabaseName("PROPLS_PROPERTY_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
