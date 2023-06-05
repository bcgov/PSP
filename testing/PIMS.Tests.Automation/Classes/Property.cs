namespace PIMS.Tests.Automation.Classes
{
    public class Property
    {
        public string? PropertyName { get; set; } = String.Empty;
        public Address? Address { get; set; } = new Address();
        public string? MOTIRegion { get; set; } = String.Empty;
        public string? HighwaysDistrict { get; set; } = String.Empty;
        public string? ElectoralDistrict { get; set; } = String.Empty;
        public string? AgriculturalLandReserve { get; set; } = String.Empty;
        public string? RailwayBelt { get; set; } = String.Empty;
        public string? LandParcelType { get; set; } = String.Empty;
        public string? MunicipalZoning { get; set; } = String.Empty;
        public List<string>? Anomalies { get; set; } = new List<string>();
        public List<string>? TenureStatus { get; set; } = new List<string>();
        public string? ProvincialPublicHwy { get; set; } = String.Empty;
        public List<string>? HighwayEstablishedBy { get; set; } = new List<string>();
        public List<string>? AdjacentLandType { get; set; } = new List<string>();
        public string? SqrMeters { get; set; } = String.Empty;
        public Boolean IsVolumetric { get; set; } = false;
        public string? Volume { get; set; } = String.Empty;
        public string? VolumeType { get; set; } = String.Empty;
        public string? PropertyNotes { get; set; } = String.Empty;
    }

    public class Address
    {
        public string? AddressLine1 { get; set; } = String.Empty;
        public string? AddressLine2 { get; set; } = String.Empty;
        public string? AddressLine3 { get; set; } = String.Empty;
        public string? City { get; set; } = String.Empty;
        public string? Province { get; set; } = String.Empty;
        public string? Country { get; set; } = String.Empty;
        public string? OtherCountry { get; set; } = String.Empty;
        public string? PostalCode { get; set; } = String.Empty;
    }

    public class SearchProperty
    {
        public string? PID { get; set; } = String.Empty;
        public string? PIN { get; set; } = String.Empty;
        public string? Address { get; set; } = String.Empty;
        public string? PlanNumber { get; set; } = String.Empty;
        public string? LegalDescription { get; set; } = String.Empty;
    }
}
