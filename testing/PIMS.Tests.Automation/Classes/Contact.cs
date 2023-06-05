
namespace PIMS.Tests.Automation.Classes
{
    public class Contact
    {
        public string ContactStatus { get; set; } = null!;
        public string? Email1 { get; set; } = String.Empty;
        public string? EmailType1 { get; set; } = String.Empty;
        public string? EmailTypeDisplay1 { get; set; } = String.Empty;
        public string? Email2 { get; set; } = String.Empty;
        public string? EmailType2 { get; set; } = String.Empty;
        public string? EmailTypeDisplay2 { get; set; } = String.Empty;
        public string? Phone1 { get; set; } = String.Empty;
        public string? PhoneType1 { get; set; } = String.Empty;
        public string? PhoneTypeDisplay1 { get; set; } = String.Empty;
        public string? Phone2 { get; set; } = String.Empty;
        public string? PhoneType2 { get; set; } = String.Empty;
        public string? PhoneTypeDisplay2 { get; set; } = String.Empty;
        public string? MailAddressLine1 { get; set; } = String.Empty;
        public string? MailAddressLine2 { get; set; } = String.Empty;
        public string? MailAddressLine3 { get; set; } = String.Empty;
        public string? MailCity { get; set; } = String.Empty;
        public string? MailProvince { get; set; } = String.Empty;
        public string? MailProvDisplay { get; set; } = String.Empty;
        public string? MailCityProvinceView { get; set; } = String.Empty;
        public string? MailCountry { get; set; } = String.Empty;
        public string? MailOtherCountry { get; set; } = String.Empty;
        public string? MailPostalCode { get; set; } = String.Empty;
        public string? PropertyAddressLine1 { get; set; } = String.Empty;
        public string? PropertyAddressLine2 { get; set; } = String.Empty;
        public string? PropertyAddressLine3 { get; set; } = String.Empty;
        public string? PropertyCity { get; set; } = String.Empty;
        public string? PropertyProvince { get; set; } = String.Empty;
        public string? PropertyCityProvinceView { get; set; } = String.Empty;
        public string? PropertyCountry { get; set; } = String.Empty;
        public string? PropertyOtherCountry { get; set; } = String.Empty;
        public string? PropertyPostalCode { get; set; } = String.Empty;
        public string? BillingAddressLine1 { get; set; } = String.Empty;
        public string? BillingAddressLine2 { get; set; } = String.Empty;
        public string? BillingAddressLine3 { get; set; } = String.Empty;
        public string? BillingCity { get; set; } = String.Empty;
        public string? BillingProvince { get; set; } = String.Empty;
        public string? BillingCityProvinceView { get; set; } = String.Empty;
        public string? BillingCountry { get; set; } = String.Empty;
        public string? BillingOtherCountry { get; set; } = String.Empty;
        public string? BillingPostalCode { get; set; } = String.Empty;
        public string? Comments { get; set; } = String.Empty;
    }
    public class OrganizationContact: Contact
    {
        public string OrganizationName { get; set; } = null!;
        public string? Alias { get; set; } = String.Empty;
        public string? IncorporationNumber { get; set; } = String.Empty;
    }

    public class IndividualContact : Contact
    {
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; } = String.Empty;
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? PreferableName { get; set; } = String.Empty;
        public string? Organization { get; set; } = String.Empty;
    }
}
