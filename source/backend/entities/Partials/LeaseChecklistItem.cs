using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    public partial class PimsLeaseChecklistItem : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => LeaseChecklistItemId; set => LeaseChecklistItemId = value; }
    }
}
