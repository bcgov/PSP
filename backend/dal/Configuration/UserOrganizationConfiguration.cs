using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// UserOrganizationConfiguration class, provides a way to configure user organizations in the database.
    ///</summary>
    public class UserOrganizationConfiguration : BaseAppEntityConfiguration<UserOrganization>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<UserOrganization> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.UserId)
                .HasComment("Foreign key to the user");
            builder.Property(m => m.OrganizationId)
                .HasComment("Foreign key to the organization");
            builder.Property(m => m.RoleId)
                .HasComment("Foreign key to the role");

            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this user organization relationship is disabled");

            builder.HasOne(m => m.User).WithMany(m => m.OrganizationsManyToMany).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_USER_PIM_USRORG_FK");
            builder.HasOne(m => m.Organization).WithMany(m => m.UsersManyToMany).HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_ORG_PIM_USRORG_FK");
            builder.HasOne(m => m.Role).WithMany(m => m.OrganizationsManyToMany).HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_ROLE_PIM_USRORG_FK");

            builder.HasIndex(m => m.UserId).HasDatabaseName("PERORG_USER_ID_IDX");
            builder.HasIndex(m => m.OrganizationId).HasDatabaseName("PERORG_ORGANIZATION_ID_IDX");
            builder.HasIndex(m => m.RoleId).HasDatabaseName("PERORG_ROLE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
