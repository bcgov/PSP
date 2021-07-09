using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// UserConfiguration class, provides a way to configure users in the database.
    ///</summary>
    public class UserConfiguration : BaseAppEntityConfiguration<User>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Key).IsRequired()
                .HasComment("A unique key to identify the user");

            builder.Property(m => m.Username).HasMaxLength(25).IsRequired()
                .HasComment("A unique username to identify the user");

            builder.Property(m => m.IsSystem)
                .HasComment("Whether this is a system user account");

            builder.Property(m => m.DisplayName).HasMaxLength(100).IsRequired()
                .HasComment("The user's display name");
            builder.Property(m => m.FirstName).HasMaxLength(100).IsRequired()
                .HasComment("The user's first name");
            builder.Property(m => m.MiddleName).HasMaxLength(100)
                .HasComment("The user's middle name");
            builder.Property(m => m.LastName).HasMaxLength(100).IsRequired()
                .HasComment("The user's last name");

            builder.Property(m => m.EmailVerified).HasDefaultValue(false)
                .HasComment("Whether the user's email has been verified");
            builder.Property(m => m.Email).HasMaxLength(100).IsRequired()
                .HasComment("The user's email address");

            builder.Property(m => m.Position).HasMaxLength(100)
                .HasComment("The user's position title");
            builder.Property(m => m.Note).HasMaxLength(1000)
                .HasComment("A note about the user");

            builder.Property(m => m.IsDisabled).HasDefaultValue(false)
                .HasComment("Whether the user account is disabled");

            builder.Property(m => m.ApprovedOn)
                .HasColumnType("DATETIME")
                .HasComment("When the user account was approved");
            builder.Property(m => m.ApprovedById)
                .HasComment("Foreign key to the user who approved this user account");

            builder.Property(m => m.LastLogin)
                .HasColumnType("DATETIME")
                .HasComment("The user's last login date");

            builder.HasOne(m => m.ApprovedBy).WithMany().HasForeignKey(m => m.ApprovedById).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("USER_USER_APPROVED_BY_ID_IDX");
            builder.HasMany(m => m.Roles).WithMany(r => r.Users).UsingEntity<UserRole>(
                m => m.HasOne(m => m.Role).WithMany(m => m.UsersManyToMany).HasForeignKey(m => m.RoleId),
                m => m.HasOne(m => m.User).WithMany(m => m.RolesManyToMany).HasForeignKey(m => m.UserId)
            );

            builder.HasIndex(m => new { m.Key }, "USER_USER_UID_TUC").IsUnique();
            builder.HasIndex(m => new { m.Email }, "USER_EMAIL_TUC").IsUnique();
            builder.HasIndex(m => new { m.Username }, "USER_USERNAME_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled, m.LastName, m.FirstName }, "USER_IS_DISABLED_LAST_NAME_FIRST_NAME_IDX");
            builder.HasIndex(m => m.ApprovedById, "USER_USER_APPROVED_BY_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
