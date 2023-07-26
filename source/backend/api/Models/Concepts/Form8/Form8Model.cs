namespace Pims.Api.Models.Concepts
{
    public class Form8Model : BaseAppModel
    {
        public long? Id { get; set; }

        public long AcquisitionFileId { get; set; }

        public long? AcquisitionOwnerId { get; set; }

        public long? InterestHolderId { get; set; }

        public long? ExpropriatingAuthorityId { get; set; }

        public TypeModel<string> PaymentItemTypeCode { get; set; }

        public string Description { get; set; }

        public bool? IsGstRequired { get; set; }

        public decimal? PretaxAmount { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public bool? IsDisabled { get; set; }
    }
}
