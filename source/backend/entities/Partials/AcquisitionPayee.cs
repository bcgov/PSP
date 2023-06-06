using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionPayee class, provides an entity for the datamodel to manage Compensation requisition's Payees.
    /// </summary>
    public partial class PimsAcquisitionPayee : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => AcquisitionPayeeId; set => AcquisitionPayeeId = value; }
    }
}
