using System.Collections.Generic;
using Pims.Api.Models.Concepts.ExpropriationPayment;

namespace Pims.Api.Models.Concepts
{
    public class ExpropriationPaymentModel : BaseAppModel
    {
        public long? Id { get; set; }

        public long AcquisitionFileId { get; set; }

        public long? AcquisitionOwnerId { get; set; }

        public AcquisitionFileOwnerModel AcquisitionOwner { get; set; }

        public long? InterestHolderId { get; set; }

        public InterestHolderModel InterestHolder { get; set; }

        public long? ExpropriatingAuthorityId { get; set; }

        public OrganizationModel ExpropriatingAuthority { get; set; }

        public string Description { get; set; }

        public bool? IsDisabled { get; set; }

        public List<ExpropriationPaymentItemModel> PaymentItems { get; set; }
    }
}
