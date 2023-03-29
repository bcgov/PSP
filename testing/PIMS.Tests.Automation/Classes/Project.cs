
namespace PIMS.Tests.Automation.Classes
{
    public class Project
    {
        public string ProjectName { get; set; } = null!;
        public string? ProjectNumber { get; set; } = null;
        public string Status { get; set; } = null!;
        public string MOTIRegion { get; set; } = null!;
        public string? Summary { get; set; } = null;
        public List<Product>? Products { get; set; } = new List<Product>();
    }

    public class Product
    {
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string CostEstimate { get; set; } = null!;
        public string EstimateDate { get; set; } = null!;
        public string Objectives { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}
