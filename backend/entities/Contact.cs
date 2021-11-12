using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Contact class, provides an entity for the datamodel to manage contacts (either persons or organizations).
    ///</summary>
    [NotMapped]
    public class Contact
    {
        #region Properties
        /// <summary>
        /// get/set - The concatenated primary key - "P{PERSON_ID}" or "O{ORGANIZATION_ID}"
        /// </summary>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// get/set - The primary key to uniquely identify contact method if contact is a person.
        /// </summary>
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }

        /// <summary>
        /// get/set - Optional contact person
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// get/set - The primary key to uniquely identify contact method if contact is an organization.
        /// </summary>
        [Column("ORGANIZATION_ID")]
        public long? OrganizationId { get; set; }

        /// <summary>
        /// get/set - Optional contact organization
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// get/set - The contact address
        /// </summary>
        [Column("ADDRESS_ID")]
        public long? AddressId { get; set; }
        public Address Address { get; set; }

        [Column("MUNICIPALITY_NAME")]
        public string Municipality { get; set; }

        [Column("PROVINCE_STATE")]
        public string ProvinceState { get; set; }

        [Column("MAILING_ADDRESS")]
        public string MailingAddress { get; set; }

        [Column("SURNAME")]
        public string Surname { get; set; }

        [Column("FIRST_NAME")]
        public string FirstName { get; set; }

        [Column("MIDDLE_NAMES")]
        public string MiddleNames { get; set; }

        [Column("ORGANIZATION_NAME")]
        public string OrganizationName { get; set; }

        /// <summary>
        /// Either $"{FirstName MiddleNames Surname}" or "{OrganizationName}"
        /// </summary>
        [Column("Summary")]
        public string Summary { get; set; }

        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ContactMethod class.
        /// </summary>
        public Contact() { }
        #endregion
    }
}
