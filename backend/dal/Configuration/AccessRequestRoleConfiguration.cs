using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// AccessRequestRoleConfiguration class, provides a way to configure AccessRequest Roles in the database.
    ///</summary>
    public class AccessRequestRoleConfiguration : BaseEntityConfiguration<AccessRequestRole>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<AccessRequestRole> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.AccessRequestId).IsRequired()
                .HasComment("Foreign key to the access request");
            builder.Property(m => m.RoleId).IsRequired()
                .HasComment("Foreign key to the role");

            builder.HasOne(m => m.AccessRequest).WithMany(m => m.Roles).HasForeignKey(m => m.AccessRequestId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("ACCRQR_ACCESS_REQUEST_ID_IDX");
            builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("ACCRQR_ROLE_ID_IDX");

            builder.HasIndex(m => new { m.AccessRequestId, m.RoleId }, "ACCRQR_ROLE_ACCESS_REQUEST_TUC").IsUnique();
            builder.HasIndex(m => m.AccessRequestId, "ACCRQR_ACCESS_REQUEST_ID_IDX");
            builder.HasIndex(m => m.RoleId, "ACCRQR_ROLE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
