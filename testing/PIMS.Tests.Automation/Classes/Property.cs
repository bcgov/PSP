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

    public class PropertyManagement
    {
        public List<string> ManagementPropertyPurpose { get; set; } = new List<string>();
        public string ManagementUtilitiesPayable { get; set; } = null!;
        public string ManagementTaxesPayable { get; set; } = null!;
        public string? ManagementPropertyAdditionalDetails { get; set; } = String.Empty;
        public int ManagementPropertyContactsStartRow { get; set; } = 0;
        public int ManagementPropertyContactsStartCount { get; set; } = 0;
        public List<PropertyContact>? ManagementPropertyContacts { get; set; } = new List<PropertyContact>();
        public int ManagementPropertyActivitiesStartRow { get; set; } = 0;
        public int ManagementPropertyActivitiesCount { get; set; } = 0;
        public List<PropertyActivity>? ManagementPropertyActivities { get; set; } = new List<PropertyActivity>();
    }

    public class PropertyContact
    {
        public string PropertyContactFullName { get; set; } = null!;
        public string PropertyContactType { get; set; } = null!;
        public string PropertyPrimaryContact { get; set; } = String.Empty;
        public string PropertyContactPurposeDescription { get; set; } = null!;
    }

    public class PropertyActivity
    {
        public string PropertyActivityType { get; set; } = null!;
        public string PropertyActivitySubType { get; set; } = null!;
        public string PropertyActivityStatus { get; set; } = null!;
        public string PropertyActivityRequestedDate { get; set; } = null!;
        public string PropertyActivityCompletionDate { get; set; } = null!;
        public string PropertyActivityDescription { get; set; } = null!;
        public List<string>? PropertyActivityMinistryContact { get; set; } = new List<string>();
        public string? PropertyActivityRequestedSource { get; set; } = String.Empty;
        public List<string>? PropertyActivityInvolvedParties { get; set; } = new List<string>();
        public string? PropertyActivityServiceProvider { get; set; } = String.Empty;
        public int ManagementPropertyActivityInvoicesStartRow { get; set; } = 0;
        public int ManagementPropertyActivityInvoicesCount { get; set; } = 0;
        public List<ManagementPropertyActivityInvoice>? ManagementPropertyActivityInvoices { get; set; } = new List<ManagementPropertyActivityInvoice>();
        public string ManagementPropertyActivityTotalPreTax { get; set; } = null!;
        public string ManagementPropertyActivityTotalGST { get; set; } = null!;
        public string ManagementPropertyActivityTotalPST { get; set; } = null!;
        public string ManagementPropertyActivityGrandTotal { get; set; } = null!;
    }

    public class ManagementPropertyActivityInvoice
    {
        public string? PropertyActivityInvoiceNumber{ get; set; } = String.Empty;
        public string PropertyActivityInvoiceDate { get; set; } = null!;
        public string PropertyActivityInvoiceDescription { get; set; } = null!;
        public string PropertyActivityInvoicePretaxAmount { get; set; } = null!;
        public string PropertyActivityInvoiceGSTAmount { get; set; } = null!;
        public string PropertyActivityInvoicePSTApplicable { get; set; } = null!;
        public string? PropertyActivityInvoicePSTAmount { get; set; } = String.Empty;
        public string PropertyActivityInvoiceTotalAmount { get; set; } = null!;
    }
}
