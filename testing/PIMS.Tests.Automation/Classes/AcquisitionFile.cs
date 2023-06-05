namespace PIMS.Tests.Automation.Classes
{
    public class AcquisitionFile
    {
        public string? AcquisitionStatus { get; set; } = String.Empty;
        public string? AcquisitionProjCode { get; set; } = String.Empty;
        public string? AcquisitionProject { get; set; } = String.Empty;
        public string? AcquisitionProjProduct { get; set; } = String.Empty;
        public string? AcquisitionProjFunding { get; set; } = String.Empty;
        public string? AcquisitionFundingOther { get; set; } = String.Empty;

        public string? DeliveryDate { get; set; } = String.Empty;
        public string? AcquisitionCompletedDate { get; set; } = String.Empty;
        public string AcquisitionFileName { get; set; } = null!;
        public string? HistoricalFileNumber { get; set; } = String.Empty;
        public string? PhysicalFileStatus { get; set; } = String.Empty;
        public string AcquisitionType { get; set; } = null!;
        public string MOTIRegion { get; set; } = null!;
        public int AcquisitionTeamStartRow { get; set; } = 0;
        public int AcquisitionTeamCount { get; set; } = 0;
        public List<AcquisitionTeamMember>? AcquisitionTeam { get; set; } = new List<AcquisitionTeamMember>();
        public int OwnerStartRow { get; set; } = 0; 
        public int OwnerCount { get; set; } = 0;
        public List<Owner>? AcquisitionOwners { get; set; } = new List<Owner>();
        public string? OwnerSolicitor { get; set; } = String.Empty; 
        public int SearchPropertiesIndex { get; set; } = 0;
        public SearchProperty? SearchProperties { get; set; } = new SearchProperty();
    }

    public class AcquisitionTeamMember
    {
        public string TeamRole { get; set; } = null!;
        public string ContactName { get; set; } = null!;
    }

    public class Owner
    {
        public string? ContactType { get; set; } = String.Empty;
        public bool isPrimary { get; set; } = false;
        public string? GivenNames { get; set; } = String.Empty;
        public string? LastName { get; set; } = String.Empty;
        public string? OtherName { get; set; } = String.Empty;
        public string? CorporationName { get; set; } = String.Empty;
        public string? IncorporationNumber { get; set; } = String.Empty;
        public string? RegistrationNumber { get; set; } = String.Empty;
        public string? MailAddressLine1 { get; set; } = String.Empty;
        public string? MailAddressLine2 { get; set; } = String.Empty;
        public string? MailAddressLine3 { get; set; } = String.Empty;
        public string? MailCity { get; set; } = String.Empty;
        public string? MailProvince { get; set; } = String.Empty;
        public string? MailCountry { get; set; } = String.Empty;
        public string? MailOtherCountry { get; set; } = String.Empty;
        public string? MailPostalCode { get; set; } = String.Empty;
        public string? Email { get; set; } = String.Empty;
        public string? Phone { get; set; } = String.Empty;
    }
}
