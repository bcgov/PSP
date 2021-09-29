using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// AccessRequestOrganizationConfiguration class, provides a way to configure Access Request organizations in the database.
    /// </summary>
    public class AccessRequestOrganizationConfiguration : BaseAppEntityConfiguration<AccessRequestOrganization>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<AccessRequestOrganization> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.AccessRequestId)
                .IsRequired()
                .HasComment("Foreign key to the access request");
            builder.Property(m => m.OrganizationId)
                .IsRequired()
                .HasComment("Foreign key to the organization");

            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this access request organization relationship is disabled");

            builder.HasOne(m => m.AccessRequest).WithMany(m => m.OrganizationsManyToMany).HasForeignKey(m => m.AccessRequestId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_ACRQST_PIM_ACRQOR_FK");
            builder.HasOne(m => m.Organization).WithMany(m => m.AccessRequestsManyToMany).HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_ORG_PIM_ACRQOR_FK");

            builder.HasIndex(m => new { m.AccessRequestId, m.OrganizationId }, "ACRQAG_ACCESS_REQUEST_ORGANIZATION_TUC").IsUnique();
            builder.HasIndex(m => m.AccessRequestId).HasDatabaseName("ACRQAG_ACCESS_REQUEST_ID_IDX");
            builder.HasIndex(m => m.OrganizationId).HasDatabaseName("ACRQAG_ORGANIZATION_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
