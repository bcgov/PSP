namespace Pims.Api.Models.Concepts
{
    public class LeaseTenantModel : Api.Models.BaseAppModel
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

        public TypeModel<string> LessorType { get; set; }

        public TypeModel<string> TenantTypeCode { get; set; }
    }
}
