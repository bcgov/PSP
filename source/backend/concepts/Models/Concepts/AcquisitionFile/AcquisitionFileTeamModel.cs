using Pims.Api.Concepts.Models.Base;
using Pims.Api.Concepts.Models.Concepts.Organization;
using Pims.Api.Concepts.Models.Concepts.Person;

namespace Pims.Api.Concepts.Models.Concepts.AcquisitionFile
{
    public class AcquisitionFileTeamModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Parent Acquisition File.
        /// </summary>
        public long AcquisitionFileId { get; set; }

        /// <summary>
        /// get/set - The Id of the person associated with an acquisition file as part of the acquisition team.
        /// </summary>
        public long? PersonId { get; set; }

        /// <summary>
        /// get/set - The person associated with an acquisition file as part of the acquisition team.
        /// </summary>
        public PersonModel Person { get; set; }

        /// <summary>
        /// get/set - The Id of the organization associated with an acquisition file as part of the acquisition team.
        /// </summary>
        public long? OrganizationId { get; set; }

        /// <summary>
        /// get/set - The organization associated with an acquisition file as part of the acquisition team.
        /// </summary>
        public OrganizationModel Organization { get; set; }

        /// <summary>
        /// get/set - The Id of the primary contact associated with an acquisition file as part of the acquisition team.
        /// </summary>
        public long? PrimaryContactId { get; set; }

        /// <summary>
        /// get/set - The primary contact associated with an acquisition file as part of the acquisition team.
        /// </summary>
        public PersonModel PrimaryContact { get; set; }

        /// <summary>
        /// get/set - The Team's profile type code.
        /// </summary>
        public string TeamProfileTypeCode { get; set; }

        /// <summary>
        /// get/set - The Team's profile type code.
        /// </summary>
        public TypeModel<string> TeamProfileType { get; set; }

        #endregion
    }
}
