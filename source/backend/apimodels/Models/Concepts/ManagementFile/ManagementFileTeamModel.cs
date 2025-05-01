using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Organization;
using Pims.Api.Models.Concepts.Person;

namespace Pims.Api.Models.Concepts.ManagementFile
{
    public class ManagementFileTeamModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - The unique identifier for the management file team.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The management file ID this team is associated with.
        /// </summary>
        public long ManagementFileId { get; set; }

        /// <summary>
        /// get/set - The person ID associated with the team.
        /// </summary>
        public long? PersonId { get; set; }

        /// <summary>
        /// get/set - The person associated with a management file as part of the management team.
        /// </summary>
        public PersonModel Person { get; set; }

        /// <summary>
        /// get/set - The organization ID associated with the team.
        /// </summary>
        public long? OrganizationId { get; set; }

        /// <summary>
        /// get/set - The organization associated with a management file as part of the management team.
        /// </summary>
        public OrganizationModel Organization { get; set; }

        /// <summary>
        /// get/set - The primary contact ID for the organization.
        /// </summary>
        public long? PrimaryContactId { get; set; }

        /// <summary>
        /// get/set - The primary contact associated with a management file as part of the management team.
        /// </summary>
        public PersonModel PrimaryContact { get; set; }

        /// <summary>
        /// get/set - The Team's profile type code.
        /// </summary>
        public string TeamProfileTypeCode { get; set; }

        /// <summary>
        /// get/set - The Team's profile type code.
        /// </summary>
        public CodeTypeModel<string> TeamProfileType { get; set; }

        #endregion
    }
}
