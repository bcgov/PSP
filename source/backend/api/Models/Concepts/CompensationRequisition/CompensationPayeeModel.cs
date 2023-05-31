using System.Collections.Generic;
namespace Pims.Api.Models.Concepts
{
    public class CompensationPayeeModel : BaseAppModel
    {

        public long AcquisitionPayeeId { get; set; }

        public long CompensationRequisitionId { get; set; }

        public long? AcquisitionOwnerId { get; set; }

        public long? InterestHolderId { get; set; }

        public AcquisitionFileOwnerModel Owner { get; set; }

        public List<AcquisitionFilePayeeChequeModel> PayeeCheques { get; set; }

        // public long Id { get; set; }

        // public bool? IsPrimaryOwner { get; set; }

        // public bool? IsOrganization { get; set; }

        // public string? LastNameAndCorpName { get; set; }

        // public string? OtherName { get; set; }

        // public string? GivenName { get; set; }

        // public string? IncorporationNumber { get; set; }

        // public string? RegistrationNumber { get; set; }

        // public string? ContactEmailAddr { get; set; }

        // public string? ContactPhoneNum { get; set; }

    }
}