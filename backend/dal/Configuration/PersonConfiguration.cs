using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pims.Dal.Entities;
using Pims.Dal.Extensions;

namespace Pims.Dal.Configuration
{
    /// <summary>
    /// PersonConfiguration class, provides a way to configure persons in the database.
    ///</summary>
    public class PersonConfiguration : BaseAppEntityConfiguration<Person>
    {
        #region Methods
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToMotiTable();

            builder.HasMotiKey(m => m.Id);
            builder.HasMotiSequence(m => m.Id)
                .HasComment("Auto-sequenced unique key value");

            builder.Property(m => m.AddressId)
                .HasComment("Foreign key to address");

            builder.Property(m => m.Surname)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Person's last name.");
            builder.Property(m => m.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Person's first name.");
            builder.Property(m => m.MiddleNames)
                .HasMaxLength(200)
                .HasComment("Person's middle names.");
            builder.Property(m => m.NameSuffix)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Person's name suffix (Mr, Mrs, Miss).");
            builder.Property(m => m.BirthDate)
                .HasColumnType("DATE")
                .HasComment("Person's birdate.");

            builder.Property(m => m.IsDisabled)
                .HasComment("Whether this person is disabled");

            builder.HasOne(m => m.Address).WithMany(m => m.Persons).HasForeignKey(m => m.AddressId).OnDelete(DeleteBehavior.Cascade).HasConstraintName("PIM_ADDRSS_PIM_PERSON_FK");

            builder.HasMany(m => m.Organizations).WithMany(m => m.Persons).UsingEntity<PersonOrganization>(
                m => m.HasOne(m => m.Organization).WithMany(m => m.PersonsManyToMany).HasForeignKey(m => m.OrganizationId),
                m => m.HasOne(m => m.Person).WithMany(m => m.OrganizationsManyToMany).HasForeignKey(m => m.PersonId)
            );

            builder.HasIndex(m => m.AddressId).HasDatabaseName("PERSON_ADDRESS_ID_IDX");

            base.Configure(builder);
        }
        #endregion
    }
}
