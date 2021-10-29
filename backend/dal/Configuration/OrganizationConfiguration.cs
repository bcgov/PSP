using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// OrganizationConfiguration class, provides a way to configure organizations in the database.
    ///</summary>
    public class OrganizationConfiguration : BaseAppEntityConfiguration<Organization>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ParentId)
                .HasComment("Foreign key to the parent organization");
            builder.Property(m => m.AddressId)
                .HasComment("Foreign key to the address");
            builder.Property(m => m.RegionId)
                .HasComment("Foreign key to the region");
            builder.Property(m => m.DistrictId)
                .HasComment("Foreign key to the district");
            builder.Property(m => m.OrganizationTypeId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to the organization type");
            builder.Property(m => m.OrganizationIdentifierTypeId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to the organization identifier type");

            builder.Property(m => m.Identifier)
                .HasMaxLength(100)
                .HasComment("An identifier for the organization");
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("A name to identify the organization");
            builder.Property(m => m.Website)
                .HasMaxLength(200)
                .HasComment("Organization website URI");
            builder.Property(m => m.IsDisabled)
                .HasComment("Whether the organization is disabled");

            builder.HasOne(m => m.Parent).WithMany(m => m.Children).HasForeignKey(m => m.ParentId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("PIM_ORG_PIM_PRNT_ORG_FK");
            builder.HasOne(m => m.Address).WithMany(m => m.Organizations).HasForeignKey(m => m.AddressId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_ADDRSS_PIM_ORG_FK");
            builder.HasOne(m => m.Region).WithMany(m => m.Organizations).HasForeignKey(m => m.RegionId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_REGION_PIM_ORG_FK");
            builder.HasOne(m => m.District).WithMany(m => m.Organizations).HasForeignKey(m => m.DistrictId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_DSTRCT_PIM_ORG_FK");
            builder.HasOne(m => m.OrganizationType).WithMany(m => m.Organizations).HasForeignKey(m => m.OrganizationTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_ORGTYP_PIM_ORG_FK");
            builder.HasOne(m => m.OrganizationIdentifierType).WithMany(m => m.Organizations).HasForeignKey(m => m.OrganizationIdentifierTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_ORGIDT_PIM_ORG_FK");

            builder.HasIndex(m => m.ParentId).HasDatabaseName("ORG_PRNT_ORGANIZATION_ID_IDX");
            builder.HasIndex(m => m.AddressId).HasDatabaseName("ORG_ADDRESS_ID_IDX");
            builder.HasIndex(m => m.RegionId).HasDatabaseName("ORG_REGION_CODE_IDX");
            builder.HasIndex(m => m.DistrictId).HasDatabaseName("ORG_DISTRICT_CODE_IDX");
            builder.HasIndex(m => m.OrganizationTypeId).HasDatabaseName("ORG_ORGANIZATION_TYPE_CODE_IDX");
            builder.HasIndex(m => m.OrganizationIdentifierTypeId).HasDatabaseName("ORG_ORG_IDENTIFIER_TYPE_CODE_IDX");

            builder.HasMany(m => m.Persons).WithMany(m => m.Organizations).UsingEntity<PersonOrganization>(
                m => m.HasOne(m => m.Person).WithMany(m => m.OrganizationsManyToMany).HasForeignKey(m => m.PersonId),
                m => m.HasOne(m => m.Organization).WithMany(m => m.PersonsManyToMany).HasForeignKey(m => m.OrganizationId)
                
            );

            builder.HasMany(m => m.Leases).WithMany(m => m.Organizations).UsingEntity<LeaseTenant>(
                m => m.HasOne(m => m.Lease).WithMany(m => m.TenantsManyToMany).HasForeignKey(m => m.LeaseId),
                m => m.HasOne(m => m.Organization).WithMany(m => m.LeasesManyToMany).HasForeignKey(m => m.OrganizationId)
            );

            base.Configure(builder);
        }
        #endregion
    }
}
