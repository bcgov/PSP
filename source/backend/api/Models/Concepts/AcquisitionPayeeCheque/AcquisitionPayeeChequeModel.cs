namespace Pims.Api.Models.Concepts
{
    public class AcquisitionPayeeChequeModel : BaseAppModel
    {
        public long? Id { get; set; }

        public long? AcquisitionPayeeId { get; set; }

        public bool? IsPaymentInTrust { get; set; }

        public decimal? PretaxAmout { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public string GSTNumber { get; set; }
    }
}
