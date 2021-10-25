using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// LeaseTenantConfiguration class, provides a way to configure lease and tenant relationships in the database.
    ///</summary>
    public class LeaseTenantConfiguration : BaseAppEntityConfiguration<LeaseTenant>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<LeaseTenant> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.PersonId)
                .HasComment("Foreign key to the person");
            builder.Property(m => m.OrganizationId)
                .HasComment("Foreign key to the organization");
            builder.Property(m => m.LeaseId)
                .IsRequired()
                .HasComment("Foreign key to the lease");
            builder.Property(m => m.Note)
                .HasMaxLength(2000)
                .HasComment("A note on the lease tenant");


            builder.Property(m => m.LessorTypeId)
                .IsRequired()
                .HasComment("Foreign key to the lessor");

            builder.HasOne(m => m.LessorType).WithMany(m => m.Leases).HasForeignKey(m => m.LessorTypeId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_LSSRTY_PIM_TENANT_FK");
            builder.HasOne(m => m.Person).WithMany(m => m.LeasesManyToMany).HasForeignKey(m => m.PersonId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_PERSON_PIM_TENANT_FK");
            builder.HasOne(m => m.Organization).WithMany(m => m.LeasesManyToMany).HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_ORG_PIM_TENANT_FK");
            builder.HasOne(m => m.Lease).WithMany(m => m.TenantsManyToMany).HasForeignKey(m => m.LeaseId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_LEASE_PIM_TENANT_FK");

            builder.HasIndex(m => m.PersonId).HasDatabaseName("TENANT_PERSON_ID_IDX");
            builder.HasIndex(m => m.OrganizationId).HasDatabaseName("TENANT_ORGANIZATION_ID_IDX");
            builder.HasIndex(m => m.LeaseId).HasDatabaseName("TENANT_LEASE_ID_IDX");
            builder.HasIndex(m => m.LessorTypeId).HasDatabaseName("TENANT_LESSOR_TYPE_CODE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
