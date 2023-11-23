using System.Collections.Generic;
using Pims.Api.Concepts.Models.Base;
using Pims.Api.Concepts.Models.Concepts.Organization;
using Pims.Api.Concepts.Models.Concepts.Person;

namespace Pims.Api.Concepts.Models.Concepts.InterestHolder
{
    public class InterestHolderModel : BaseAuditModel
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
