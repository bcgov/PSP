using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Organization;
using Pims.Api.Models.Concepts.Person;

namespace Pims.Api.Models.Concepts.Lease
{
    public class LeaseTenantModel : BaseAuditModel
    {
        public long? LeaseTenantId { get; set; }

        public long LeaseId { get; set; }

        public long? PersonId { get; set; }

        public PersonModel Person { get; set; }

        public long? OrganizationId { get; set; }

        public OrganizationModel Organization { get; set; }

        public string Note { get; set; }

        public PersonModel PrimaryContact { get; set; }

        public long? PrimaryContactId { get; set; }

        public CodeTypeModel<string> LessorType { get; set; }

        public CodeTypeModel<string> TenantTypeCode { get; set; }
    }
}
