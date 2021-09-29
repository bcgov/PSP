using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// UserRoleConfiguration class, provides a way to configure user roles in the database.
    ///</summary>
    public class UserRoleConfiguration : BaseAppEntityConfiguration<UserRole>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.UserId).IsRequired()
                .HasComment("Foreign key to the user");
            builder.Property(m => m.RoleId).IsRequired()
                .HasComment("Foreign key to the role");

            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this relationship between user and role is disabled");

            builder.HasOne(m => m.User).WithMany(m => m.RolesManyToMany).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_USER_PIM_USERRL_FK");
            builder.HasOne(m => m.Role).WithMany(m => m.UsersManyToMany).HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_ROLE_PIM_USERRL_FK");

            builder.HasIndex(m => new { m.UserId, m.RoleId }, "USRROL_USER_ROLE_TUC").IsUnique();

            builder.HasIndex(m => m.UserId).HasDatabaseName("USRROL_USER_ID_IDX");
            builder.HasIndex(m => m.RoleId).HasDatabaseName("USRROL_ROLE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
