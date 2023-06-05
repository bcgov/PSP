namespace Pims.Api.Models.Concepts
{
    public class InterestHolderModel : BaseAppModel
    {
        public long InterestHolderId { get; set; }

        public long? PersonId { get; set; }

        public long? OrganizationId { get; set; }

        public bool? IsDisabled { get; set; }

        public long AcquisitionFileId { get; set; }

        public AcquisitionFileModel AcquisitionFile { get; set; }

        public OrganizationModel Organization { get; set; }

        public PersonModel Person { get; set; }
    }
}