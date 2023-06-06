namespace Pims.Api.Models.Concepts
{
    public class InterestHolderPropertyModel : BaseAppModel
    {
        public long? InterestHolderId { get; set; }

        public long? InterestHolderPropertyId { get; set; }

        public long? AcquisitionFilePropertyId { get; set; }

        public AcquisitionFilePropertyModel AcquisitionFileProperty { get; set; }

        public bool IsDisabled { get; set; }

        public TypeModel<string> InterestTypeCode { get; set; }
    }
}
