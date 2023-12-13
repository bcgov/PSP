using Pims.Api.Models.Base;

using Pims.Api.Models.Concepts.Organization;
using Pims.Api.Models.Concepts.Person;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyActivityInvolvedPartyModel : BaseAuditModel
    {
        #region Properties

        public long Id { get; set; }

        public long? OrganizationId { get; set; }

        public OrganizationModel Organization { get; set; }

        public long? PersonId { get; set; }

        public PersonModel Person { get; set; }

        public long PropertyActivityId { get; set; }

        public PropertyActivityModel PropertyActivity { get; set; }

        #endregion
    }
}
