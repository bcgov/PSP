
namespace PIMS.Tests.Automation.Classes
{
    public class Contact
    {
        public string ContactStatus { get; set; } = null!;
        public string? Email1 { get; set; } = null;
        public string? EmailType1 { get; set; } = null;
        public string? EmailTypeDisplay1 { get; set; } = null;
        public string? Email2 { get; set; } = null;
        public string? EmailType2 { get; set; } = null;
        public string? EmailTypeDisplay2 { get; set; } = null;
        public string? Phone1 { get; set; } = null;
        public string? PhoneType1 { get; set; } = null;
        public string? PhoneTypeDisplay1 { get; set; } = null;
        public string? Phone2 { get; set; } = null;
        public string? PhoneType2 { get; set; } = null;
        public string? PhoneTypeDisplay2 { get; set; } = null;
        public string? MailAddressLine1 { get; set; } = null;
        public string? MailAddressLine2 { get; set; } = null;
        public string? MailAddressLine3 { get; set; } = null;
        public string? MailCity { get; set; } = null;
        public string? MailProvince { get; set; } = null;
        public string? MailProvDisplay { get; set; } = null;
        public string? MailCityProvinceView { get; set; } = null;
        public string? MailCountry { get; set; } = null;
        public string? MailOtherCountry { get; set; } = null;
        public string? MailPostalCode { get; set; } = null;
        public string? PropertyAddressLine1 { get; set; } = null;
        public string? PropertyAddressLine2 { get; set; } = null;
        public string? PropertyAddressLine3 { get; set; } = null;
        public string? PropertyCity { get; set; } = null;
        public string? PropertyProvince { get; set; } = null;
        public string? PropertyCityProvinceView { get; set; } = null;
        public string? PropertyCountry { get; set; } = null;
        public string? PropertyOtherCountry { get; set; } = null;
        public string? PropertyPostalCode { get; set; } = null;
        public string? BillingAddressLine1 { get; set; } = null;
        public string? BillingAddressLine2 { get; set; } = null;
        public string? BillingAddressLine3 { get; set; } = null;
        public string? BillingCity { get; set; } = null;
        public string? BillingProvince { get; set; } = null;
        public string? BillingCityProvinceView { get; set; } = null;
        public string? BillingCountry { get; set; } = null;
        public string? BillingOtherCountry { get; set; } = null;
        public string? BillingPostalCode { get; set; } = null;
        public string? Comments { get; set; } = null;
    }
    public class OrganizationContact: Contact
    {
        public string OrganizationName { get; set; } = null!;
        public string? Alias { get; set; } = null;
        public string? IncorporationNumber { get; set; } = null;
    }

    public class IndividualContact : Contact
    {
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; } = null;
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? PreferableName { get; set; } = null;
        public string? Organization { get; set; } = null;
    }
}
