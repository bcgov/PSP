namespace PIMS.Tests.Automation.Classes
{
    public class DispositionFile
    {
        public string? DispositionFileStatus { get; set; } = String.Empty;
        public string? DispositionProjFunding { get; set; } = String.Empty;
        public string? DispositionAssignedDate { get; set; } = String.Empty;
        public string? DispositionCompletedDate { get; set; } = String.Empty;
        public string? DispositionFileName { get; set; } = String.Empty;
        public string? DispositionReferenceNumber { get; set; } = String.Empty;
        public string? DispositionStatus { get; set; } = null!;
        public string? DispositionType { get; set; } = null!;
        public string? DispositionOtherTransferType { get; set; } = String.Empty;
        public string? InitiatingDocument { get; set; } = String.Empty;
        public string? OtherInitiatingDocument { get; set; } = String.Empty;
        public string? InitiatingDocumentDate { get; set; } = String.Empty;
        public string? PhysicalFileStatus { get; set; } = String.Empty;
        public string? InitiatingBranch { get; set; } = String.Empty;
        public string DispositionMOTIRegion { get; set; } = null!;
        public int DispositionTeamStartRow { get; set; } = 0;
        public int DispositionTeamCount { get; set; } = 0;
        public List<TeamMember>? DispositionTeam { get; set; } = new List<TeamMember>();
        public int DispositionSearchPropertiesIndex { get; set; } = 0;
        public SearchProperty? DispositionSearchProperties { get; set; } = new SearchProperty();
    }
}
