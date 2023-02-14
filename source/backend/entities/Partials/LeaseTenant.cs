using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseTenant class, provides the many-to-many relationship between leases and tenants.
    /// </summary>
    public partial class PimsLeaseTenant : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.LeaseTenantId; set => this.LeaseTenantId = value; }
        #endregion

        #region Constructors
        public PimsLeaseTenant()
        {
        }

        /// <summary>
        /// Creates a new instance of a LeaseTenant object, initializes with specified arguments.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="person"></param>
        /// <param name="organization"></param>
        public PimsLeaseTenant(PimsLease lease, PimsPerson person, PimsOrganization organization, PimsLessorType lessorType)
            : this()
        {
            this.LeaseId = lease?.LeaseId ?? throw new ArgumentNullException(nameof(lease));
            this.Lease = lease;
            this.PersonId = person?.PersonId ?? throw new ArgumentNullException(nameof(person));
            this.Person = person;
            this.OrganizationId = organization?.Internal_Id ?? throw new ArgumentNullException(nameof(organization));
            this.Organization = organization;
            this.LessorTypeCode = lessorType?.Id ?? throw new ArgumentNullException(nameof(lessorType));
            this.LessorTypeCodeNavigation = lessorType;
        }
        #endregion
    }
}
