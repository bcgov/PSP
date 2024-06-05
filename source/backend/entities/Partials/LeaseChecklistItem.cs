using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeaseChecklistItem class, provides an entity for the datamodel to manage a Lease checklist item.
    /// </summary>
    public partial class PimsLeaseChecklistItem : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => LeaseChecklistItemId; set => LeaseChecklistItemId = value; }
    }
}
