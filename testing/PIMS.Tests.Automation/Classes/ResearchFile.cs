
namespace PIMS.Tests.Automation.Classes
{
    public class ResearchFile
    {
        public string? ResearchFileName { get; set; } = String.Empty;
        public string? Status { get; set; } = String.Empty;
        public List<string>? Projects { get; set; } = new List<string>();
        public string? RoadName { get; set; } = String.Empty;
        public string? RoadAlias { get; set; } = String.Empty;
        public List<string>? ResearchPurpose { get; set; } = new List<string>();
        public string? RequestDate { get; set; } = String.Empty;
        public string? RequestSource { get; set; } = String.Empty;
        public string? Requester { get; set; } = String.Empty;
        public string? RequestDescription { get; set; } = String.Empty;
        public string? ResearchCompletedDate { get; set; } = String.Empty;
        public string? RequestResult { get; set; } = String.Empty;
        public Boolean Expropriation { get; set; } = false;
        public string? ExpropriationNotes { get; set; } = String.Empty;
        public int SearchPropertiesIndex { get; set; } = 0;
        public SearchProperty? SearchProperties { get; set; } = new SearchProperty();
        public int PropertyResearchRowStart { get; set; } = 0;
        public int PropertyResearchRowEnd { get; set; } = 0;
        public List<PropertyResearch>? PropertyResearch { get; set; } = new List<PropertyResearch>();
    }

    public class PropertyResearch
    {
        public string? DescriptiveName { get; set; } = String.Empty;
        public string? Purpose { get; set; } = String.Empty;
        public string? LegalOpinionRequest { get; set; } = String.Empty;
        public string? LegalOpinionObtained { get; set; } = String.Empty;
        public string? DocumentReference { get; set; } = String.Empty;
        public string? SummaryNotes { get; set; } = String.Empty;
    }
}
