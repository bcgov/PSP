using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropAcqFlCompReq class, provides an entity for the datamodel to manage the relationship between Acq. File Properties and Compensation Requisitions.
    /// </summary>
    public partial class PimsPropAcqFlCompReq : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => PropAcqFlCompReqId; set => PropAcqFlCompReqId = value; }
    }
}
