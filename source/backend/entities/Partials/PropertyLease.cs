using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyLease class, provides the many-to-many relationship between leases and properties.
    /// </summary>
    public partial class PimsPropertyLease : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropertyLeaseId; set => this.PropertyLeaseId = value; }
        #endregion

        #region Constructors
        public PimsPropertyLease()
        {
        }

        /// <summary>
        /// Creates a new instance of a PimsPropertyLease object, initializes with specified arguments.
        /// </summary>
        /// <param name="lease"></param>
        /// <param name="property"></param>
        public PimsPropertyLease(PimsLease lease, PimsProperty property)
            : this()
        {
            this.LeaseId = lease?.LeaseId ?? throw new ArgumentNullException(nameof(lease));
            this.Lease = lease;
            this.PropertyId = property?.PropertyId ?? throw new ArgumentNullException(nameof(property));
            this.Property = property;
        }
        #endregion
    }
}
