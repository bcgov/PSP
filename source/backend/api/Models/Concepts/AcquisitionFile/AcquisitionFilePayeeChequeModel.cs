namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFilePayeeChequeModel : BaseAppModel
    {
        public long Id { get; set; }

        public long AcquisitionPayeeId { get; set; }

        public decimal? PretaxAmount { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? GstAmount { get; set; }
    }
}