
namespace PIMS.Tests.Automation.Classes
{
    public class Project
    {
        public string Name { get; set; } = null!;
        public string? Number { get; set; } = null;
        public string CodeName { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string MOTIRegion { get; set; } = null!;
        public string? Summary { get; set; } = null;
        public string CreatedBy { get; set; } = null!;
        public string UpdatedBy { get; set; } = null!;
        public int ProductsCount { get; set; } = 0;
        public int ProductsRowStart { get; set; } = 0;
        public string? UpdateName { get; set; } = null;
        public string? UpdateNumber { get; set; } = null;
        public string? UpdateCodeName { get; set; } = null;
        public string? UpdateStatus { get; set; } = null;
        public string? UpdateMOTIRegion { get; set; } = null;
        public string? UpdateSummary { get; set; } = null;
        public List<Product>? Products { get; set; } = new List<Product>();
    }

    public class Product
    {
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string ProductCodeName { get; set; } = null!;
        public string CostEstimate { get; set; } = null!;
        public string? EstimateDate { get; set; } = null;
        public string StartDate { get; set; } = null!;
        public string Objectives { get; set; } = null!;
        public string Scope { get; set; } = null!;
        public string UpdateProductCode { get; set; } = null!;
        public string UpdateProductName { get; set; } = null!;
        public string UpdateProductCodeName { get; set; } = null!;
        public string UpdateCostEstimate { get; set; } = null!;
        public string? UpdateEstimateDate { get; set; } = null;
        public string UpdateStartDate { get; set; } = null!;
        public string UpdateObjectives { get; set; } = null!;
        public string UpdateScope { get; set; } = null!;
    }
}
