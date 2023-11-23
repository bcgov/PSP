using Pims.Api.Concepts.Models.Base;
using Pims.Api.Concepts.Models.Concepts.Address;

namespace Pims.Api.Concepts.Models.Concepts.AcquisitionFile
{
    public class AcquisitionFileOwnerModel : BaseAuditModel
    {
        public long Id { get; set; }

        public long AcquisitionFileId { get; set; }

        public bool IsOrganization { get; set; }

        public bool IsPrimaryContact { get; set; }

        public string LastNameAndCorpName { get; set; }

        public string OtherName { get; set; }

        public string GivenName { get; set; }

        public string IncorporationNumber { get; set; }

        public string RegistrationNumber { get; set; }

        public string ContactEmailAddr { get; set; }

        public string ContactPhoneNum { get; set; }

        public AddressModel Address { get; set; }
    }
}
