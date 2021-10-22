using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseTenant class, provides the many-to-many relationship between leases and tenants.
    /// </summary>
    [MotiTable("PIMS_LEASE_TENANT", "LEATEN")]
    public class LeaseTenant : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the tenant lease.
        /// </summary>
        [Column("LEASE_TENANT_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Primary key: The foreign key to the lease.
        /// </summary>
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }

        /// <summary>
        /// get/set - The lease that the tenant is associated to.
        /// </summary>
        public Lease Lease { get; set; }

        /// <summary>
        /// get/set - Primary key: The foreign key to the person.
        /// </summary>
        [Column("PERSON_ID")]
        public long? PersonId { get; set; }

        /// <summary>
        /// get/set - The person tenant that the lease is associated to.
        /// </summary>
        public Person Person { get; set; }

        /// <summary>
        /// get/set - Primary key: The foreign key to the organization.
        /// </summary>
        [Column("ORGANIZATION_ID")]
        public long? OrganizationId { get; set; }

        /// <summary>
        /// get/set - The organization tenant the lease is associated to.
        /// </summary>
        public Organization Organization { get; set; }

        /// <summary>
        /// get/set - Primary key: The foreign key to the lessor type.
        /// </summary>
        [Column("LESSOR_TYPE_CODE")]
        public string LessorTypeId { get; set; }

        /// <summary>
        /// get/set - The lessor type for this lease tenant relationship
        /// </summary>
        public LessorType LessorType { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LeaseTenant object.
        /// </summary>
        public LeaseTenant() { }

        /// <summary>
        /// Creates a new instance of a LeaseTenant object, initializes with specified arguments.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="person"></param>
        /// <param name="organization"></param>
        public LeaseTenant(Lease lease, Person person, Organization organization, LessorType lessorType)
        {
            this.LeaseId = lease?.Id ?? throw new ArgumentNullException(nameof(lease));
            this.Lease = lease;
            this.PersonId = person?.Id ?? throw new ArgumentNullException(nameof(person));
            this.Person = person;
            this.OrganizationId = organization?.Id ?? throw new ArgumentNullException(nameof(organization));
            this.Organization = organization;
            this.LessorTypeId = lessorType?.Id ?? throw new ArgumentNullException(nameof(lessorType));
            this.LessorType = lessorType;
        }
        #endregion
    }
}
