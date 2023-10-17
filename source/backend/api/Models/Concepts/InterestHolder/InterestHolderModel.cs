using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class InterestHolderModel : BaseAppModel
    {
        public long? InterestHolderId { get; set; }

        public long? AcquisitionFileId { get; set; }

        public long? PersonId { get; set; }

        public long? PrimaryContactId { get; set; }

        public PersonModel Person { get; set; }

        public long? OrganizationId { get; set; }

        public OrganizationModel Organization { get; set; }

        public virtual PersonModel PrimaryContact { get; set; }

        public string Comment { get; set; }

        public IEnumerable<InterestHolderPropertyModel> InterestHolderProperties { get; set; }

        public virtual TypeModel<string> InterestHolderType { get; set; }

        public bool IsDisabled { get; set; }
    }
}
