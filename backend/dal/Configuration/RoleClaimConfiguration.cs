using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// RoleClaimConfiguration class, provides a way to configure user roles in the database.
    ///</summary>
    public class RoleClaimConfiguration : BaseAppEntityConfiguration<RoleClaim>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.RoleId)
                .IsRequired()
                .HasComment("Foreign key to the role");
            builder.Property(m => m.ClaimId)
                .IsRequired()
                .HasComment("Foreign key to the claim");

            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this record is disabled");

            builder.HasOne(m => m.Role).WithMany(m => m.ClaimsManyToMany).HasForeignKey(m => m.RoleId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_ROLE_PIM_ROLCLM_FK");
            builder.HasOne(m => m.Claim).WithMany(m => m.RolesManyToMany).HasForeignKey(m => m.ClaimId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_CLMTYP_PIM_ROLCLM_FK");

            builder.HasIndex(m => new { m.RoleId, m.ClaimId }, "ROLCLM_ROLE_CLAIM_TUC").IsUnique();

            builder.HasIndex(m => m.RoleId).HasDatabaseName("ROLCLM_ROLE_ID_IDX");
            builder.HasIndex(m => m.ClaimId).HasDatabaseName("ROLCLM_CLAIM_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
