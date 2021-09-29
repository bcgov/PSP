using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ClaimConfiguration class, provides a way to configure claims in the database.
    ///</summary>
    public class ClaimConfiguration : BaseAppEntityConfiguration<Claim>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Key)
                .HasComment("A unique key to identify the record");
            builder.Property(m => m.KeycloakRoleId)
                .HasComment("A unique key to identify the associated role in keycloak");
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("A unique name to identify this record");
            builder.Property(m => m.Description)
                .HasMaxLength(500)
                .HasComment("A description of the claim");
            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this claim is disabled");

            builder.HasIndex(m => new { m.Name }, "CLAIM_NAME_TUC").IsUnique();

            base.Configure(builder);
        }
        #endregion
    }
}
