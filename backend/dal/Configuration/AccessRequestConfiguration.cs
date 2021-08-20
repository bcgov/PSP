using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// AccessRequestConfiguration class, provides a way to configure users access requests in the database.
    ///</summary>
    public class AccessRequestConfiguration : BaseAppEntityConfiguration<AccessRequest>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<AccessRequest> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.UserId)
                .HasComment("Foreign key to the user who submitted the request");
            builder.Property(m => m.RoleId)
                .HasComment("Foreign key to the role");
            builder.Property(m => m.StatusId)
                .HasComment("foreign key to the access request status type");

            builder.Property(m => m.Note)
                .HasMaxLength(1000)
                .HasComment("A note to describe the access request reason");

            builder.HasOne(m => m.User).WithMany(u => u.AccessRequests).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_USER_PIM_ACRQST_FK");
            builder.HasOne(m => m.Role).WithMany(u => u.AccessRequests).HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_ROLE_PIM_ACRQST_FK");
            builder.HasOne(m => m.Status).WithMany(u => u.AccessRequests).HasForeignKey(m => m.StatusId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_ARQSTT_PIM_ACRQST_FK");

            builder.HasMany(m => m.Organizations).WithMany(m => m.AccessRequests).UsingEntity<AccessRequestOrganization>(
                m => m.HasOne(m => m.Organization).WithMany(m => m.AccessRequestsManyToMany).HasForeignKey(m => m.OrganizationId),
                m => m.HasOne(m => m.AccessRequest).WithMany(m => m.OrganizationsManyToMany).HasForeignKey(m => m.AccessRequestId)
            );

            builder.HasIndex(m => m.UserId).HasDatabaseName("ACCRQT_USER_ID_IDX");
            builder.HasIndex(m => m.RoleId).HasDatabaseName("ACCRQT_ROLE_ID_IDX");
            builder.HasIndex(m => m.StatusId).HasDatabaseName("ACCRQT_ACCESS_REQUEST_STATUS_TYPE_CODE_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
