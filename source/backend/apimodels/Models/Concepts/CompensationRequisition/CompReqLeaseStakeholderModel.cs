using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Lease;

namespace Pims.Api.Models.Concepts.CompensationRequisition
{
    public class CompReqLeaseStakeholderModel : BaseAuditModel
    {
        public long? CompReqLeaseStakeholderId { get; set; }

        public long CompensationRequisitionId { get; set; }

        public long LeaseStakeholderId { get; set; }

        public LeaseStakeholderModel LeaseStakeholder { get; set; }
    }
}
