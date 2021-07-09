using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// TenantConfiguration class, provides a way to configure tenant settings in the database.
    ///</summary>
    public class TenantConfiguration : BaseEntityConfiguration<Tenant>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToMotiTable().HasAnnotation("ProductVersion", "2.0.0");

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.Code)
                .HasMaxLength(6)
                .IsRequired()
                .HasComment("Code value for entry");

            builder.Property(m => m.Name)
                .HasMaxLength(150)
                .IsRequired()
                .HasComment("Name of the entry for display purposes");

            builder.Property(m => m.Settings)
                .HasMaxLength(2000)
                .IsRequired()
                .HasComment("Serialized JSON value for the configuration");

            builder.Property(m => m.Description)
                .HasMaxLength(500)
                .HasComment("Description of the entry for display purposes");

            builder.HasIndex(m => new { m.Code }, "TENANT_CODE_TUC").IsUnique();

            base.Configure(builder);
        }
        #endregion
    }
}
