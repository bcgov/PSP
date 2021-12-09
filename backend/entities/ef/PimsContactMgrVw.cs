
#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsContactMgrVw
    {
        public string Id { get; set; }
        public long? PersonId { get; set; }
        public long? OrganizationId { get; set; }
        public bool IsDisabled { get; set; }
        public string Summary { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string OrganizationName { get; set; }
        public long? AddressId { get; set; }
        public string MailingAddress { get; set; }
        public string MunicipalityName { get; set; }
        public string ProvinceState { get; set; }
    }
}
