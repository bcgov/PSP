namespace PIMS.Tests.Automation.Classes
{
    public class ManagementFile
    {
        public string ManagementMinistryProjectCode { get; set; } = null!;
        public string ManagementMinistryProject { get; set; } = null!;
        public string ManagementMinistryProduct { get; set; } = null!;
        public string ManagementMinistryFunding { get; set; } = null!;
        public string ManagementStatus { get; set; } = null!;
        public string ManagementName { get; set; } = null!;
        public string ManagementHistoricalFile { get; set; } = null!;
        public string ManagementPurpose { get; set; } = null!;
        public string ManagementAdditionalDetails { get; set; } = null!;
        public int ManagementTeamStartRow { get; set; } = 0;
        public int ManagementTeamCount { get; set; } = 0;
        public List<TeamMember> ManagementTeam { get; set; } = new List<TeamMember>() { };
        public int ManagementSearchPropertiesIndex { get; set; } = 0;
        public SearchProperty ManagementSearchProperties { get; set; } = new SearchProperty() { };
        public int ManagementTotalProperties { get; set; } = 0;
        public int ManagementActivityStartRow { get; set; } = 0;
        public int ManagementActivityCount { get; set; } = 0;
        public List<PropertyActivity> ManagementPropertyActivities { get; set; } = new List<PropertyActivity>();
    }
}
