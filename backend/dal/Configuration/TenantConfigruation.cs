using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;

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
            builder.ToTable("Tenants");

            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).ValueGeneratedOnAdd();

            builder.Property(m => m.Code).HasMaxLength(6);
            builder.Property(m => m.Code).IsRequired();

            builder.Property(m => m.Name).HasMaxLength(150);
            builder.Property(m => m.Name).IsRequired();

            builder.Property(m => m.Description).HasMaxLength(500);
            builder.Property(m => m.Settings).HasMaxLength(2000);

            builder.HasIndex(m => new { m.Code }).IsUnique();

            base.BaseConfigure(builder);
        }
        #endregion
    }
}
