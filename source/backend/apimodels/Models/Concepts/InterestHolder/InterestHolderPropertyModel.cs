using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.AcquisitionFile;

namespace Pims.Api.Models.Concepts.InterestHolder
{
    public class InterestHolderPropertyModel : BaseAuditModel
    {
        public long? InterestHolderPropertyId { get; set; }

        public long? InterestHolderId { get; set; }

        public long? AcquisitionFilePropertyId { get; set; }

        public AcquisitionFilePropertyModel AcquisitionFileProperty { get; set; }

        public List<TypeModel<string>> PropertyInterestTypes { get; set; }
    }
}
