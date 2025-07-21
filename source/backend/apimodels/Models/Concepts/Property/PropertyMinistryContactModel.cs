using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Person;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyMinistryContactModel : BaseAuditModel
    {
        #region Properties

        public long Id { get; set; }

        public long PersonId { get; set; }

        public PersonModel Person { get; set; }

        public long ManagementActivityId { get; set; }

        public ManagementActivityModel ManagementActivity { get; set; }

        #endregion
    }
}
