using Pims.Api.Concepts.Models.Base;
using Pims.Api.Concepts.Models.Concepts.Organization;

namespace Pims.Api.Concepts.Models.Concepts.Person
{
    public class PersonOrganizationModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship person id.
        /// </summary>
        public long PersonId { get; set; }

        /// <summary>
        /// get/set - The relationship organization.
        /// </summary>
        public OrganizationModel Organization { get; set; }
        #endregion
    }
}
