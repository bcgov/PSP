using System;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Api.Models.Concepts.InterestHolder;

namespace Pims.Api.Models.Concepts.ExpropriationEvent
{
    public class ExpropriationEventModel : BaseAuditModel
    {
        public long? Id { get; set; }

        public long AcquisitionFileId { get; set; }

        public long? AcquisitionOwnerId { get; set; }

        public AcquisitionFileOwnerModel AcquisitionOwner { get; set; }

        public long? InterestHolderId { get; set; }

        public InterestHolderModel InterestHolder { get; set; }

        public CodeTypeModel<string> EventType { get; set; }

        public DateOnly? EventDate { get; set; }
    }
}
