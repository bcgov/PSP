namespace Pims.Api.Models.Concepts.ExpropriationPayment
{
    public class ExpropriationPaymentItemModel : BaseAppModel
    {
        public long? Id { get; set; }

        public long? ExpropriationPaymentId { get; set; }

        public string PaymentItemTypeCode { get; set; }

        public TypeModel<string> PaymentItemType { get; set; }

        public bool? IsGstRequired { get; set; }

        public decimal? PretaxAmount { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public bool? IsDisabled { get; set; }
    }
}
