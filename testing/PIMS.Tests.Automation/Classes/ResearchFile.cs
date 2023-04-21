
namespace PIMS.Tests.Automation.Classes
{
    public class ResearchFile
    {
        public string? ResearchFileName { get; set; } = null;
        public string? Status { get; set; } = null;
        public List<string>? Projects { get; set; } = new List<string>();
        public string? RoadName { get; set; } = null;
        public string? RoadAlias { get; set; } = null;
        public List<string>? ResearchPurpose { get; set; } = new List<string>();
        public string? RequestDate { get; set; } = null;
        public string? RequestSource { get; set; } = null;
        public string? Requester { get; set; } = null;
        public string? RequestDescription { get; set; } = null;
        public string? ResearchCompletedDate { get; set; } = null;
        public string? RequestResult { get; set; } = null;
        public Boolean Expropriation { get; set; } = false;
        public string? ExpropriationNotes { get; set; } = null;
        public List<string>? NotesTab { get; set; } = new List<string>();
        public int SearchPropertiesIndex { get; set; } = 0;
        public SearchProperty? SearchProperties { get; set; } = new SearchProperty();
        public int PropertyResearchRowStart { get; set; } = 0;
        public int PropertyResearchRowEnd { get; set; } = 0;
        public List<PropertyResearch>? PropertyResearch { get; set; } = new List<PropertyResearch>();
    }

    public class PropertyResearch
    {
        public string? DescriptiveName { get; set; } = null;
        public string? Purpose { get; set; } = null;
        public string? LegalOpinionRequest { get; set; } = null;
        public string? LegalOpinionObtained { get; set; } = null;
        public string? DocumentReference { get; set; } = null;
        public string? SummaryNotes { get; set; } = null;
    }
}
