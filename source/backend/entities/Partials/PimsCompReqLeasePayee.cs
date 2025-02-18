using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{

    /// <summary>
    /// PimsCompReqLeasePayee class, provides an entity for the datamodel to manage the relationship between Compensation Requisitions and the lease stakeholder.
    /// </summary>
    public partial class PimsCompReqLeasePayee : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => CompReqLeasePayeeId; set => CompReqLeasePayeeId = value; }
    }
}
