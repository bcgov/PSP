using Pims.Api.Models.Concepts;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    public class TenantModel : Api.Models.BaseAppModel
    {
        public string Id { get; set; }
        public long? LeaseTenantId { get; set; }
        public long LeaseId { get; set; }
        public long? PersonId { get; set; }
        public PersonModel Person { get; set; }
        public long? OrganizationId { get; set; }
        public OrganizationModel Organization { get; set; }
        public string Note { get; set; }
    }
}
