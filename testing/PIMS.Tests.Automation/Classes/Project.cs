
namespace PIMS.Tests.Automation.Classes
{
    public class Project
    {
        public string Name { get; set; } = null!;
        public string Number { get; set; } = null!;
        public string CodeName { get; set; } = null!;
        public string ProjectStatus { get; set; } = null!;
        public string ProjectMOTIRegion { get; set; } = null!;
        public string Summary { get; set; } = null!;
        public string CostType { get; set; } = null!;
        public string WorkActivity { get; set; } = null!;
        public string BusinessFunction { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string UpdatedBy { get; set; } = null!;
        public int ProductsCount { get; set; } = 0;
        public int ProductsRowStart { get; set; } = 0;
        public List<Product> Products { get; set; } = new List<Product>() { };
        public List<string> ProjectTeamMembers { get; set; } = new List<string>() { };
    }

    public class Product
    {
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string ProductCodeName { get; set; } = null!;
        public string CostEstimate { get; set; } = null!;
        public string EstimateDate { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string Objectives { get; set; } = null!;
        public string Scope { get; set; } = null!;
    }
}
