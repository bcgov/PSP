using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ClaimConfiguration class, provides a way to configure claims in the database.
    ///</summary>
    public class ClaimConfiguration : BaseEntityConfiguration<Claim>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Key)
                .HasComment("A unique key to identify the record");
            builder.Property(m => m.KeycloakRoleId)
                .HasComment("A unique key to identify the associated role in keycloak");

            builder.Property(m => m.Name).HasMaxLength(100).IsRequired()
                .HasComment("A unique name to identify this record");

            builder.Property(m => m.Description).HasMaxLength(500)
                .HasComment("A description of the claim");
            builder.Property(m => m.IsDisabled)
                .HasComment("Whether this claim is disabled");

            builder.HasIndex(m => new { m.Key }, "CLAIM_CLAIM_UID_TUC").IsUnique();
            builder.HasIndex(m => new { m.Name }, "CLAIM_NAME_TUC").IsUnique();
            builder.HasIndex(m => new { m.IsDisabled }, "CLAIM_IS_DISABLED_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
