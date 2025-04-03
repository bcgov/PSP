using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Api.Models.Concepts.InterestHolder;

namespace Pims.Api.Models.Concepts.CompensationRequisition
{
    public class CompReqAcqPayeeModel : BaseAuditModel
    {
        public long? CompReqAcqPayeeId { get; set; }

        public long? CompensationRequisitionId { get; set; }

        public CompensationRequisitionModel CompensationRequisition { get; set; }

        public long? AcquisitionOwnerId { get; set; }

        public AcquisitionFileOwnerModel AcquisitionOwner { get; set; }

        public long? InterestHolderId { get; set; }

        public InterestHolderModel InterestHolder { get; set; }

        public long? AcquisitionFileTeamId { get; set; }

        public AcquisitionFileTeamModel AcquisitionFileTeam { get; set; }

        public string LegacyPayee { get; set; }
    }
}
