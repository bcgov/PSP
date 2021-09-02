using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PropertyOrganizationConfiguration class, provides a way to configure property organization relationships in the database.
    ///</summary>
    public class PropertyOrganizationConfiguration : BaseAppEntityConfiguration<PropertyOrganization>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PropertyOrganization> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.PropertyId)
                .IsRequired()
                .HasComment("Foreign key to the property");
            builder.Property(m => m.OrganizationId)
                .IsRequired()
                .HasComment("Foreign key to the organization");

            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this record is disabled");

            builder.HasOne(m => m.Property).WithMany(m => m.OrganizationsManyToMany).HasForeignKey(m => m.PropertyId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_PRPRTY_PIM_PRPORG_FK");
            builder.HasOne(m => m.Organization).WithMany(m => m.PropertiesManyToMany).HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_ORG_PIM_PRPORG_FK");

            builder.HasIndex(m => new { m.PropertyId, m.OrganizationId }, "PRPORG_PROPERTY_ORGANIZATION_TUC").IsUnique();

            builder.HasIndex(m => m.PropertyId).HasDatabaseName("PRPORG_PROPERTY_ID_IDX");
            builder.HasIndex(m => m.OrganizationId).HasDatabaseName("PRPORG_ORGANIZATION_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
