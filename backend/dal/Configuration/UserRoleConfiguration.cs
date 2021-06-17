using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// UserRoleConfiguration class, provides a way to configure user roles in the database.
    ///</summary>
    public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.UserId).IsRequired()
                .HasComment("Foreign key to the user");

            builder.Property(m => m.RoleId).IsRequired()
                .HasComment("Foreign key to the role");

            builder.HasOne(m => m.User).WithMany(m => m.Roles).HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("USRROL_USER_ID_IDX");
            builder.HasOne(m => m.Role).WithMany(m => m.Users).HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("USRROL_ROLE_ID_IDX");

            builder.HasIndex(m => new { m.UserId, m.RoleId }, "USRROL_USER_ROLE_TUC").IsUnique();
            builder.HasIndex(m => m.UserId, "USRROL_USER_ID_IDX");
            builder.HasIndex(m => m.RoleId, "USRROL_ROLE_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
