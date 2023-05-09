namespace PIMS.Tests.Automation.Classes
{
    public class Property
    {
        public string? PropertyName { get; set; } = null;
        public Address? Address { get; set; } = new Address();
        public string? MOTIRegion { get; set; } = null;
        public string? HighwaysDistrict { get; set; } = null;
        public string? ElectoralDistrict { get; set; } = null;
        public string? AgriculturalLandReserve { get; set; } = null;
        public string? RailwayBelt { get; set; } = null;
        public string? LandParcelType { get; set; } = null;
        public string? MunicipalZoning { get; set; } = null;
        public List<string>? Anomalies { get; set; } = new List<string>();
        public List<string>? TenureStatus { get; set; } = new List<string>();
        public string? ProvincialPublicHwy { get; set; } = null;
        public List<string>? HighwayEstablishedBy { get; set; } = new List<string>();
        public List<string>? AdjacentLandType { get; set; } = new List<string>();
        public string? SqrMeters { get; set; } = null;
        public Boolean IsVolumetric { get; set; } = false;
        public string? Volume { get; set; } = null;
        public string? VolumeType { get; set; } = null;
        public string? PropertyNotes { get; set; } = null;
    }

    public class Address
    {
        public string? AddressLine1 { get; set; } = null;
        public string? AddressLine2 { get; set; } = null;
        public string? AddressLine3 { get; set; } = null;
        public string? City { get; set; } = null;
        public string? Province { get; set; } = null;
        public string? Country { get; set; } = null;
        public string? OtherCountry { get; set; } = null;
        public string? PostalCode { get; set; } = null;
    }

    public class SearchProperty
    {
        public string? PID { get; set; } = null;
        public string? PIN { get; set; } = null;
        public string? Address { get; set; } = null;
        public string? PlanNumber { get; set; } = null;
        public string? LegalDescription { get; set; } = null;
    }
}
