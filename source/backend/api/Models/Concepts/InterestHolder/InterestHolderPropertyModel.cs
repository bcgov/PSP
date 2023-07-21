using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class InterestHolderPropertyModel : BaseAppModel
    {
        public long? InterestHolderPropertyId { get; set; }

        public long? InterestHolderId { get; set; }

        public long? AcquisitionFilePropertyId { get; set; }

        public AcquisitionFilePropertyModel AcquisitionFileProperty { get; set; }

        public List<TypeModel<string>> PropertyInterestTypes { get; set; }

        public bool IsDisabled { get; set; }
    }
}
