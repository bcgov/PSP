namespace Pims.Api.Models.Concepts
{
    public class CompensationFinancialModel : BaseAppModel
    {
        public long Id { get; set; }

        public long CompensationId { get; set; }

        public decimal? PretaxAmount { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public bool? IsDisabled { get; set; }

        // public long? H120CategoryId { get; set; } // TODO

        // public virtual PimsH120Category H120Category { get; set; } // TODO needs to mapped correctly once schema is defined.
    }
}
