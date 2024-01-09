using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Organization;

namespace Pims.Api.Models.Concepts.Person
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
