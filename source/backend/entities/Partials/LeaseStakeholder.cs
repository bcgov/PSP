using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseStakeholder class, provides the many-to-many relationship between leases and their stakeholders.
    /// </summary>
    public partial class PimsLeaseStakeholder : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.LeaseStakeholderId; set => this.LeaseStakeholderId = value; }
        #endregion

        #region Constructors
        public PimsLeaseStakeholder()
        {
        }

        /// <summary>
        /// Creates a new instance of a LeaseStakeholder object, initializes with specified arguments.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="person"></param>
        /// <param name="organization"></param>
        public PimsLeaseStakeholder(PimsLease lease, PimsPerson person, PimsOrganization organization, PimsLessorType lessorType, PimsLeaseStakeholderType stakeholderType)
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
            this.LeaseStakeholderTypeCode = stakeholderType?.Id ?? throw new ArgumentNullException(nameof(stakeholderType));
            this.LeaseStakeholderTypeCodeNavigation = stakeholderType;
        }
        #endregion
    }
}
