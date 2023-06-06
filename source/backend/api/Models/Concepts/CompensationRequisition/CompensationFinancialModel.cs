namespace Pims.Api.Models.Concepts
{
    public class CompensationFinancialModel : BaseAppModel
    {
        public long? Id { get; set; }

        public long FinancialActivityCodeId { get; set; }

        public FinancialCodeModel FinancialActivityCode { get; set; }

        public long CompensationId { get; set; }

        public decimal? PretaxAmount { get; set; }

        public bool? IsGstRequired { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public bool? IsDisabled { get; set; }

        public long? H120CategoryId { get; set; }
    }
}
