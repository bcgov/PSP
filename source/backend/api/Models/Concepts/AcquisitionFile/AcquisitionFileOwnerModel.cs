namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileOwnerModel : BaseAppModel
    {
        public long Id { get; set; }

        public long AcquisitionFileId { get; set; }

        public AddressModel Address { get; set; }

        public string LastNameOrCorp1 { get; set; }

        public string LastNameOrCorp2 { get; set; }

        public string GivenName { get; set; }

        public string IncorporationNumber { get; set; }
    }
}
