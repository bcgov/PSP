using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropAcqFlCompReq class, provides an entity for the datamodel to manage the relationship between Lease File Properties and Compensation Requisitions.
    /// </summary>
    public partial class PimsPropLeaseCompReq : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => PropLeaseCompReqId; set => PropLeaseCompReqId = value; }
    }
}
