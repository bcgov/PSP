using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Person;

namespace Pims.Api.Models.Concepts.Project
{
    public class ProjectPersonModel : BaseAuditModel
    {
        #region Properties

        public long? Id { get; set; }

        public long? ProjectId { get; set; }

        public ProjectModel Project { get; set; }

        public long PersonId { get; set; }

        public PersonModel Person { get; set; }

        #endregion
    }
}
