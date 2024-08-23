using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.AcquisitionFile;

namespace Pims.Api.Models.Concepts.CompensationRequisition
{
    public class CompReqAcquisitionPropertyModel : BaseAuditModel
    {
        public long? CompensationRequisitionPropertyId { get; set; }

        public long? CompensationRequisitionId { get; set; }

        public long PropertyAcquisitionFileId { get; set; }

        public AcquisitionFilePropertyModel AcquisitionFileProperty { get; set; }
    }
}
