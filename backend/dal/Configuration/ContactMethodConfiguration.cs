using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ContactMethodConfiguration class, provides a way to configure contact methods in the database.
    ///</summary>
    public class ContactMethodConfiguration : BaseAppEntityConfiguration<ContactMethod>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<ContactMethod> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.ContactMethodTypeId)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Foreign key to contact method type");
            builder.Property(m => m.PersonId)
                .HasComment("Foreign key to person");
            builder.Property(m => m.OrganizationId)
                .HasComment("Foreign key to organization");

            builder.Property(m => m.Value)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("Contact method value information (phone, email, fax, etc.)");
            builder.Property(m => m.IsPreferredMethod)
                .HasDefaultValue(false)
                .HasComment("Whether this contact method is the preferred type");

            builder.HasOne(m => m.ContactMethodType).WithMany(m => m.ContactMethods).HasForeignKey(m => m.ContactMethodTypeId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_CNTMTT_PIM_CNTMTH_FK");
            builder.HasOne(m => m.Person).WithMany(m => m.ContactMethods).HasForeignKey(m => m.PersonId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_PERSON_PIM_CNTMTH_FK");
            builder.HasOne(m => m.Organization).WithMany(m => m.ContactMethods).HasForeignKey(m => m.OrganizationId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("PIM_ORG_PIM_CNTMTH_FK");

            builder.HasIndex(m => m.ContactMethodTypeId).HasDatabaseName("CNTMTH_CONTACT_METHOD_TYPE_CODE_IDX");
            builder.HasIndex(m => m.PersonId).HasDatabaseName("CNTMTH_PERSON_ID_IDX");
            builder.HasIndex(m => m.OrganizationId).HasDatabaseName("CNTMTH_ORGANIZATION_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
