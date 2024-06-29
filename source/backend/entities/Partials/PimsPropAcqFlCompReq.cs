using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    public partial class PimsPropAcqFlCompReq : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => PropAcqFlCompReqId; set => PropAcqFlCompReqId = value; }
    }
}
