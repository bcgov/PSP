using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PersonOrganizationConfiguration class, provides a way to configure person and organization relationships in the database.
    ///</summary>
    public class PersonOrganizationConfiguration : BaseAppEntityConfiguration<PersonOrganization>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<PersonOrganization> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.PersonId)
                .HasComment("Foreign key to the person");
            builder.Property(m => m.OrganizationId)
                .HasComment("Foreign key to the organization");

            builder.Property(m => m.IsDisabled)
                .HasDefaultValue(false)
                .HasComment("Whether this person organization relationship is disabled");

            builder.HasOne(m => m.Person).WithMany(m => m.OrganizationsManyToMany).HasForeignKey(m => m.PersonId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_PERSON_PIM_PERORG_FK");
            builder.HasOne(m => m.Organization).WithMany(m => m.PersonsManyToMany).HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.ClientCascade).HasConstraintName("PIM_ORG_PIM_PERORG_FK");

            builder.HasIndex(m => m.PersonId).HasDatabaseName("PERORG_PERSON_ID_IDX");
            builder.HasIndex(m => m.OrganizationId).HasDatabaseName("PERORG_ORGANIZATION_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
