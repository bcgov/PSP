using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Api.Models.Concepts.InterestHolder;
using Pims.Api.Models.Concepts.Organization;

namespace Pims.Api.Models.Concepts.ExpropriationPayment
{
    public class ExpropriationPaymentModel : BaseAuditModel
    {
        public long? Id { get; set; }

        public long AcquisitionFileId { get; set; }

        public long? AcquisitionOwnerId { get; set; }

        public AcquisitionFileOwnerModel AcquisitionOwner { get; set; }

        public long? InterestHolderId { get; set; }

        public InterestHolderModel InterestHolder { get; set; }

        public long? ExpropriatingAuthorityId { get; set; }

        public OrganizationModel ExpropriatingAuthority { get; set; }

        public DateOnly? AdvancedPaymentServedDate { get; set; }

        public string Description { get; set; }

        public bool? IsDisabled { get; set; }

        public List<ExpropriationPaymentItemModel> PaymentItems { get; set; }
    }
}
