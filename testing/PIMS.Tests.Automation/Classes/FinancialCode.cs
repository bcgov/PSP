
namespace PIMS.Tests.Automation.Classes
{
    public class FinancialCode
    {
        public string CodeType { get; set; } = null!;
        public string CodeValue { get; set; } = null!;
        public string CodeDescription { get; set; } = null!;
        public string EffectiveDate { get; set; } = null!;
        public string? ExpiryDate{ get; set; } = String.Empty;
        public string? DisplayOrder { get; set; } = String.Empty;
    }
}
