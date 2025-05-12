namespace PIMS.Tests.Automation.Classes
{
    public class ManagementFile
    {
        public string MinistryProjectCode { get; set; } = null!;
        public string MinistryProject { get; set; } = null!;
        public string MinistryProduct { get; set; } = null!;
        public string MinistryFunding { get; set; } = null!;
        public string ManagementStatus { get; set; } = null!;
        public string ManagementName { get; set; } = null!;
        public string ManagementHistoricalFile { get; set; } = null!;
        public string ManagementPurpose { get; set; } = null!;
        public string ManagementAdditionalDetails { get; set; } = null!;
        public List<TeamMember> ManagementTeam { get; set; } = new List<TeamMember>() { };
        public int ManagementSearchPropertiesIndex { get; set; } = 0;
        public SearchProperty ManagementSearchProperties { get; set; } = new SearchProperty() { };
    }
}
