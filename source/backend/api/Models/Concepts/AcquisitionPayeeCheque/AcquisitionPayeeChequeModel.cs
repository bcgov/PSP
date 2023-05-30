namespace Pims.Api.Models.Concepts
{
    public class AcquisitionPayeeChequeModel : BaseAppModel
    {
        public long? Id { get; set; }

        public long AcquisitionPayeeId { get; set; }

        public decimal PretaxAmout { get; set; }

        public bool IsGSTRequired { get; set; }

        public decimal? TaxAmount { get; set; }

        public string GSTNumber { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
