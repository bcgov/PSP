using System;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Organization;
using Pims.Api.Models.Concepts.Person;

namespace Pims.Api.Models.Concepts.Lease
{
    public class ConsultationLeaseModel : BaseAuditModel
    {
        #region Properties

        public long Id { get; set; }

        public long LeaseId { get; set; }

        public LeaseModel Lease { get; set; }

        public long? PersonId { get; set; }

        public PersonModel Person { get; set; }

        public long? OrganizationId { get; set; }

        public OrganizationModel Organization { get; set; }

        public long? PrimaryContactId { get; set; }

        public PersonModel PrimaryContact { get; set; }

        public CodeTypeModel<string> ConsultationTypeCode { get; set; }

        public CodeTypeModel<string> ConsultationStatusTypeCode { get; set; }

        public CodeTypeModel<string> ConsultationOutcomeTypeCode { get; set; }

        public string OtherDescription { get; set; }

        public DateOnly? RequestedOn { get; set; }

        public bool? IsResponseReceived { get; set; }

        public DateOnly? ResponseReceivedDate { get; set; }

        public string Comment { get; set; }
        #endregion
    }
}
