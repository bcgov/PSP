
namespace PIMS.Tests.Automation.Classes
{
    public class OrganizationContact
    {
        public string OrganizationName { get; set; } = null!;
        public string Alias { get; set; } = null!;
        public string IncorporationNumber { get; set; } = null!;
        public string ContactStatus { get; set; } = null!;
        public string OrgEmail1 { get; set; } = null!;
        public string OrgEmailType1 { get; set; } = null!;
        public string OrgEmailTypeDisplay1 { get; set; } = null!;
        public string OrgEmail2 { get; set; } = null!;
        public string OrgEmailType2 { get; set; } = null!;
        public string OrgEmailTypeDisplay2 { get; set; } = null!;
        public string OrgPhone1 { get; set; } = null!;
        public string OrgPhoneType1 { get; set; } = null!;
        public string OrgPhoneTypeDisplay1 { get; set; } = null!;
        public string OrgPhone2 { get; set; } = null!;
        public string OrgPhoneType2 { get; set; } = null!;
        public string OrgPhoneTypeDisplay2 { get; set; } = null!;
        public Address OrgMailAddress { get; set; } = new Address();
        public Address OrgPropertyAddress { get; set; } = new Address();
        public Address OrgBillingAddress { get; set; } = new Address();
        public string OrgComments { get; set; } = null!;
    }

    public class IndividualContact
    {
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string PreferableName { get; set; } = null!;
        public string Organization { get; set; } = null!;
        public string ContactStatus { get; set; } = null!;
        public string IndEmail1 { get; set; } = null!;
        public string IndEmailType1 { get; set; } = null!;
        public string IndEmailTypeDisplay1 { get; set; } = null!;
        public string IndEmail2 { get; set; } = null!;
        public string IndEmailType2 { get; set; } = null!;
        public string IndEmailTypeDisplay2 { get; set; } = null!;
        public string IndPhone1 { get; set; } = null!;
        public string IndPhoneType1 { get; set; } = null!;
        public string IndPhoneTypeDisplay1 { get; set; } = null!;
        public string IndPhone2 { get; set; } = null!;
        public string IndPhoneType2 { get; set; } = null!;
        public string IndPhoneTypeDisplay2 { get; set; } = null!;
        public Address IndMailAddress { get; set; } = new Address();
        public Address IndPropertyAddress { get; set; } = new Address();
        public Address IndBillingAddress { get; set; } = new Address();
        public string IndComments { get; set; } = null!;
    }
}
