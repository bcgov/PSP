using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionPayee class, provides an entity for the datamodel to manage Compensation requisition's Payees.
    /// </summary>
    public partial class PimsAcquisitionPayee : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => AcquisitionPayeeId; set => AcquisitionPayeeId = value; }

        [NotMapped]
        public bool HasPayeeAssigned => AcquisitionOwnerId != null || InterestHolderId != null || OwnerRepresentativeId != null || OwnerSolicitorId != null || AcquisitionFilePersonId != null;
    }
}
