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
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.PersonId)
                .HasComment("Foreign key to person");

            builder.Property(m => m.BusinessIdentifier)
                .IsRequired()
                .HasMaxLength(30)
                .HasComment("User account business identifier");
            builder.Property(m => m.KeycloakUserId).IsRequired()
                .HasComment("Unique key to link to keycloak user account");
            builder.Property(m => m.Position)
                .HasMaxLength(100)
                .HasComment("The user's position or job title");
            builder.Property(m => m.Note)
                .HasMaxLength(1000)
                .HasComment("A note about the user");
            builder.Property(m => m.ApprovedBy)
                .HasMaxLength(30)
                .HasComment("User name who approved this account");
            builder.Property(m => m.LastLogin)
                .HasColumnType("DATETIME")
                .HasComment("The date the user last logged in");
            builder.Property(m => m.IssueOn)
                .IsRequired()
                .HasColumnType("DATETIME")
                .HasComment("When the user account was issued");
            builder.Property(m => m.ExpiryOn)
                .HasColumnType("DATETIME")
                .HasComment("When the user account will expire");
            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether the user account is disabled");

            builder.HasOne(m => m.Person).WithMany(m => m.Users).HasForeignKey(m => m.PersonId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_PERSON_PIM_USER_FK");

            builder.HasMany(m => m.Organizations).WithMany(r => r.Users).UsingEntity<UserOrganization>(
                m => m.HasOne(m => m.Organization).WithMany(m => m.UsersManyToMany).HasForeignKey(m => m.OrganizationId),
                m => m.HasOne(m => m.User).WithMany(m => m.OrganizationsManyToMany).HasForeignKey(m => m.UserId)
            );
            builder.HasMany(m => m.Roles).WithMany(r => r.Users).UsingEntity<UserRole>(
                m => m.HasOne(m => m.Role).WithMany(m => m.UsersManyToMany).HasForeignKey(m => m.RoleId),
                m => m.HasOne(m => m.User).WithMany(m => m.RolesManyToMany).HasForeignKey(m => m.UserId)
            );

            builder.HasIndex(m => m.PersonId).HasDatabaseName("USER_PERSON_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
