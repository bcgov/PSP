
namespace PIMS.Tests.Automation.Classes
{
    public class FinancialCode
    {
        public string FinnCodeType { get; set; } = null!;
        public string FinnCodeValue { get; set; } = null!;
        public string FinnCodeDescription { get; set; } = null!;
        public string FinnEffectiveDate { get; set; } = null!;
        public string? FinnExpiryDate{ get; set; } = String.Empty;
        public string? FinnDisplayOrder { get; set; } = String.Empty;
    }
}
