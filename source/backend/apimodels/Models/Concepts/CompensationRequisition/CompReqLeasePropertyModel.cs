using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Lease;

namespace Pims.Api.Models.Concepts.CompensationRequisition
{
    public class CompReqLeasePropertyModel : BaseAuditModel
    {
        public long? CompensationRequisitionPropertyId { get; set; }

        public long? CompensationRequisitionId { get; set; }

        public long PropertyLeaseId { get; set; }

        public PropertyLeaseModel LeaseProperty { get; set; }
    }
}
