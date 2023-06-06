namespace PIMS.Tests.Automation.Classes
{
    public class DigitalDocument
    {
        public string DocumentType { get; set; } = null!;
        public string DocumentStatus { get; set; } = null!;
        public string? CanadaLandSurvey { get; set; } = String.Empty;
        public string? CivicAddress { get; set; } = String.Empty;
        public string? CrownGrant { get; set; } = String.Empty;
        public string? Date { get; set; } = String.Empty;
        public string? DateSigned { get; set; } = String.Empty;
        public string? DistrictLot { get; set; } = String.Empty;
        public string? ElectoralDistrict { get; set; } = String.Empty;
        public string? EndDate { get; set; } = String.Empty;
        public string? FieldBook { get; set; } = String.Empty;
        public string? File { get; set; } = String.Empty;
        public string? GazetteDate { get; set; } = String.Empty;
        public string? GazettePage { get; set; } = String.Empty;
        public string? GazettePublishedDate { get; set; } = String.Empty;
        public string? GazetteType { get; set; } = String.Empty;
        public string? HighwayDistrict { get; set; } = String.Empty;
        public string? IndianReserveOrNationalPark { get; set; } = String.Empty;
        public string? Jurisdiction { get; set; } = String.Empty;
        public string? LandDistrict { get; set; } = String.Empty;
        public string? LegalSurveyPlan { get; set; } = String.Empty;
        public string? LTSAScheduleFiling { get; set; } = String.Empty;
        public string? MO { get; set; } = String.Empty;
        public string? MoTIFile { get; set; } = String.Empty;
        public string? MoTIPlan { get; set; } = String.Empty;
        public string? OIC { get; set; } = String.Empty;
        public string? OICRoute { get; set; } = String.Empty;
        public string? OICType { get; set; } = String.Empty;
        public string? Owner { get; set; } = String.Empty;
        public string? PhysicalLocation { get; set; } = String.Empty;
        public string? PIDNumber { get; set; } = String.Empty;
        public string? PINNumber { get; set; } = String.Empty;
        public string? Plan { get; set; } = String.Empty;
        public string? PlanRevision { get; set; } = String.Empty;
        public string? PlanType { get; set; } = String.Empty;
        public string? Project { get; set; } = String.Empty;
        public string? ProjectName { get; set; } = String.Empty;
        public string? PropertyIdentifier { get; set; } = String.Empty;
        public string? PublishedDate { get; set; } = String.Empty;
        public string? RelatedGazette { get; set; } = String.Empty;
        public string? RoadName { get; set; } = String.Empty;
        public string? Roll { get; set; } = String.Empty;
        public string? Section { get; set; } = String.Empty;
        public string? ShortDescriptor { get; set; } = String.Empty;
        public string? StartDate { get; set; } = String.Empty;
        public string? Title { get; set; } = String.Empty;
        public string? Transfer { get; set; } = String.Empty;
        public string? Year { get; set; } = String.Empty;
        public string? YearPrivyCouncil { get; set; } = String.Empty;
    }

    public class DocumentFile
    {
        public string Url { get; set; } = null!;
    }
}
