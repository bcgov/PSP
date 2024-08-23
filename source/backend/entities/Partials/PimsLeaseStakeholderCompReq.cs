using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{

    /// <summary>
    /// PimsLeaseStakeholderCompReq class, provides an entity for the datamodel to manage the relationship between Compensation Requisitions and the lease stakeholder.
    /// </summary>
    public partial class PimsLeaseStakeholderCompReq : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => LeaseStakeholderCompReqId; set => LeaseStakeholderCompReqId = value; }
    }
}
