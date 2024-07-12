
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Lease Renewal class, provides an entity for the datamodel to manage leases renewals.
    /// </summary>
    public partial class PimsLeaseRenewal : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.LeaseRenewalId; set => this.LeaseRenewalId = value; }
        #endregion
    }
}
