using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionPayeeModel : BaseAppModel
    {
        public long? Id { get; set; }

        public long? CompensationRequisitionId { get; set; }

        public long? AcquisitionOwnerId { get; set; }

        public long? InterestHolderId { get; set; }

        public long? OwnerRepresentativeId { get; set; }

        public long? OwnerSolicitorId { get; set; }

        public long? AcquisitionFilePersonId { get; set; }

        public AcquisitionFileOwnerModel AcquisitionOwner { get; set; }

        public List<AcquisitionPayeeChequeModel> Cheques { get; set; }

        public bool? IsDisabled { get; set; }
    }
}
