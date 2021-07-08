using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// RoleConfiguration class, provides a way to configure roles in the database.
    ///</summary>
    public class RoleConfiguration : BaseAppEntityConfiguration<Role>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Key)
                .HasComment("A unique key to identify the record");
            builder.Property(m => m.KeycloakGroupId)
                .HasComment("A key to the associated keycloak group");

            builder.Property(m => m.Name).HasMaxLength(100).IsRequired()
                .HasComment("A unique name to identify the record");
            builder.Property(m => m.SortOrder)
                .HasDefaultValue(0)
                .HasComment("Sorting order of record");
            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this record is disabled");

            builder.Property(m => m.Description).HasMaxLength(500)
                .HasComment("A description of the role");

            builder.Property(m => m.IsPublic)
                .HasComment("Whether this role is publicly available to users");

            builder.HasMany(m => m.Claims).WithMany(m => m.Roles).UsingEntity<RoleClaim>(
                m => m.HasOne(m => m.Claim).WithMany(m => m.RolesManyToMany).HasForeignKey(m => m.ClaimId),
                m => m.HasOne(m => m.Role).WithMany(m => m.ClaimsManyToMany).HasForeignKey(m => m.RoleId)
            );
            builder.HasMany(m => m.AccessRequests).WithMany(r => r.Roles).UsingEntity<AccessRequestRole>(
                m => m.HasOne(m => m.AccessRequest).WithMany(m => m.RolesManyToMany).HasForeignKey(m => m.AccessRequestId),
                m => m.HasOne(m => m.Role).WithMany(m => m.AccessRequestsManyToMany).HasForeignKey(m => m.RoleId)
            );

            builder.HasIndex(m => new { m.Key }, "ROLE_ROLE_UID_TUC").IsUnique();
            builder.HasIndex(m => new { m.Name }, "ROLE_NAME_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled }, "ROLE_IS_DISABLED_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
