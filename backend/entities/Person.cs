using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Person class, provides an entity for the datamodel to manage persons.
    /// </summary>
    [MotiTable("PIMS_PERSON", "PERSON")]
    public class Person : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to uniquely identify person.
        /// </summary>
        [Column("PERSON_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The person's surname (last name).
        /// </summary>
        [Column("SURNAME")]
        public string Surname { get; set; }

        /// <summary>
        /// get/set - The person's first name.
        /// </summary>
        [Column("FIRST_NAME")]
        public string FirstName { get; set; }

        /// <summary>
        /// get/set - The person's middle name(s).
        /// </summary>
        [Column("MIDDLE_NAMES")]
        public string MiddleNames { get; set; }

        /// <summary>
        /// get/set - The person's name suffix (i.e. Mr, Mrs, Miss).
        /// </summary>
        [Column("NAME_SUFFIX")]
        public string NameSuffix { get; set; }

        /// <summary>
        /// get/set - The person's birth date.
        /// </summary>
        [Column("BIRTH_DATE")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// get/set - Whether the person's record is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - Foreign key to the address.
        /// </summary>
        [Column("ADDRESS_ID")]
        public long? AddressId { get; set; }

        /// <summary>
        /// get/set - The person's address.
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// get - A collection of contact methods.
        /// </summary>
        public ICollection<ContactMethod> ContactMethods { get; } = new List<ContactMethod>();

        /// <summary>
        /// get - A collection of organizations.
        /// </summary>
        public ICollection<Organization> Organizations { get; } = new List<Organization>();

        /// <summary>
        /// get - A collection of many-to-many relationships between person and organization.
        /// </summary>
        public ICollection<PersonOrganization> OrganizationsManyToMany { get; } = new List<PersonOrganization>();

        /// <summary>
        /// get - A collection of users.
        /// </summary>
        public ICollection<User> Users { get; } = new List<User>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Person class.
        /// </summary>
        public Person() { }

        /// <summary>
        /// Create a new instance of a Person class, initializes with specified arguments.
        /// </summary>
        /// <param name="surname"></param>
        /// <param name="firstname"></param>
        /// <param name="address"></param>
        public Person(string surname, string firstname, Address address = null)
        {
            if (String.IsNullOrWhiteSpace(surname)) throw new ArgumentException("Argument cannot be null, whitespace or empty.", nameof(surname));
            if (String.IsNullOrWhiteSpace(firstname)) throw new ArgumentException("Argument cannot be null, whitespace or empty.", nameof(firstname));

            this.Surname = surname;
            this.FirstName = firstname;
            this.Address = address;
            this.AddressId = address?.Id;
        }
        #endregion
    }
}
