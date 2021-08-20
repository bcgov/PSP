using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ContactMethod class, provides an entity for the datamodel to manage personal contact method.
    /// </summary>
    [MotiTable("PIMS_CONTACT_METHOD", "CNTMTH")]
    public class ContactMethod : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key to uniquely identify contact method.
        /// </summary>
        [Column("CONTACT_METHOD_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the contact method type.
        /// </summary>
        [Column("CONTACT_METHOD_TYPE_CODE")]
        public string ContactMethodTypeId { get; set; }

        /// <summary>
        /// get/set - The contact method type.
        /// </summary>
        public ContactMethodType ContactMethodType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the person.
        /// </summary>
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }

        /// <summary>
        /// get/set - The person.
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// get/set - Foreign key to the organization.
        /// </summary>
        [Column("ORGANIZATION_ID")]
        public long? OrganizationId { get; set; }

        /// <summary>
        /// get/set - The organization.
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// get/set - The contact method information.
        /// </summary>
        [Column("CONTACT_METHOD_VALUE")]
        public string Value { get; set; }

        /// <summary>
        /// get/set - Whether this is the preferred method.
        /// </summary>
        [Column("IS_PREFERRED_METHOD")]
        public bool IsPreferredMethod { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ContactMethod class.
        /// </summary>
        public ContactMethod() { }

        /// <summary>
        /// Create a new instance of a ContactMethod class, initializes with specified arguments.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="organization"></param>
        /// <param name="methodTypeId"></param>
        /// <param name="value"></param>
        public ContactMethod(Person person, Organization organization, string methodTypeId, string value)
        {
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("Argument cannot be null, whitespace or empty.", nameof(value));

            this.Person = person ?? throw new ArgumentNullException(nameof(person));
            this.PersonId = person.Id;
            this.Organization = organization;
            this.OrganizationId = organization?.Id;
            this.ContactMethodTypeId = methodTypeId;
            this.Value = value;
        }

        /// <summary>
        /// Create a new instance of a ContactMethod class, initializes with specified arguments.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="organization"></param>
        /// <param name="methodTypeId"></param>
        /// <param name="value"></param>
        public ContactMethod(Person person, Organization organization, string methodTypeId, string value)
        {
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("Argument cannot be null, whitespace or empty.", nameof(value));

            this.Person = person ?? throw new ArgumentNullException(nameof(person));
            this.PersonId = person.Id;
            this.Organization = organization;
            this.OrganizationId = organization?.Id;
            this.ContactMethodTypeId = methodTypeId;
            this.Value = value;
        }

        /// <summary>
        /// Create a new instance of a ContactMethod class, initializes with specified arguments.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="organization"></param>
        /// <param name="methodType"></param>
        /// <param name="value"></param>
        public ContactMethod(Person person, Organization organization, ContactMethodType methodType, string value)
        {
            if (String.IsNullOrWhiteSpace(value)) throw new ArgumentException("Argument cannot be null, whitespace or empty.", nameof(value));

            this.Person = person ?? throw new ArgumentNullException(nameof(person));
            this.PersonId = person.Id;
            this.Organization = organization ?? throw new ArgumentNullException(nameof(organization));
            this.OrganizationId = organization.Id;
            this.ContactMethodType = methodType ?? throw new ArgumentNullException(nameof(methodType));
            this.ContactMethodTypeId = methodType.Id;
            this.Value = value;
        }
        #endregion
    }
}
