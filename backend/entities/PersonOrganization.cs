using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PersonOrganization class, provides the many-to-many relationship between persons and organizations.
    /// </summary>
    [MotiTable("PIMS_PERSON_ORGANIZATION", "PERORG")]
    public class PersonOrganization : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the person organization.
        /// </summary>
        [Column("PERSON_ORGANIZATION_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Primary key: The foreign key to the person.
        /// </summary>
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }

        /// <summary>
        /// get/set - The person that the organization is located on.
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// get/set - Primary key: The foreign key to the organization.
        /// </summary>
        [Column("ORGANIZATION_ID")]
        public long? OrganizationId { get; set; }

        /// <summary>
        /// get/set - The organization located on the person.
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// get/set - Whether this person organization relationship is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a PersonOrganization object.
        /// </summary>
        public PersonOrganization() { }

        /// <summary>
        /// Creates a new instance of a PersonOrganization object, initializes with specified arguments.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="organization"></param>
        public PersonOrganization(Person person, Organization organization)
        {
            this.PersonId = person?.Id ?? throw new ArgumentNullException(nameof(person));
            this.Person = person;
            this.OrganizationId = organization?.Id ?? throw new ArgumentNullException(nameof(organization));
            this.Organization = organization;
        }
        #endregion
    }
}
