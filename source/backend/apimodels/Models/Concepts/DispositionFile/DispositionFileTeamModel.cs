/*
* Frontend model
* LINK @frontend/src\models\api\DispositionFile.ts:43
*/

using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Organization;
using Pims.Api.Models.Concepts.Person;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFileTeamModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Parent Disposition File.
        /// </summary>
        public long DispositionFileId { get; set; }

        /// <summary>
        /// get/set - The Id of the person associated with a disposition file as part of the disposition team.
        /// </summary>
        public long? PersonId { get; set; }

        /// <summary>
        /// get/set - The person associated with a disposition file as part of the disposition team.
        /// </summary>
        public PersonModel Person { get; set; }

        /// <summary>
        /// get/set - The Id of the organization associated with a disposition file as part of the disposition team.
        /// </summary>
        public long? OrganizationId { get; set; }

        /// <summary>
        /// get/set - The organization associated with a disposition file as part of the disposition team.
        /// </summary>
        public OrganizationModel Organization { get; set; }

        /// <summary>
        /// get/set - The Id of the primary contact associated with a disposition file as part of the disposition team.
        /// </summary>
        public long? PrimaryContactId { get; set; }

        /// <summary>
        /// get/set - The primary contact associated with a disposition file as part of the disposition team.
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
