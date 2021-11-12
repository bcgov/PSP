using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// ContactConfiguration class, provides a way to configure the contact view in the database.
    ///</summary>
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        #region Methods
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToView("PIMS_CONTACT_VW");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.PersonId)
                .HasComment("Foreign key to contact person");
            builder.Property(m => m.OrganizationId)
                .HasComment("Foreign key to contact organization");

            builder.HasOne(m => m.Address).WithMany(m => m.Contacts).HasForeignKey(m => m.AddressId);
            builder.HasOne(m => m.Person).WithMany(m => m.Contacts).HasForeignKey(m => m.PersonId);
            builder.HasOne(m => m.Organization).WithMany(m => m.Contacts).HasForeignKey(m => m.OrganizationId);
        }
        #endregion
    }
}
