using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Person;

namespace Pims.Api.Models.Concepts.Organization
{
    public class OrganizationPersonModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship person.
        /// </summary>
        public PersonModel Person { get; set; }

        /// <summary>
        /// get/set - The relationship person id.
        /// </summary>
        public long PersonId { get; set; }

        /// <summary>
        /// get/set - The relationship organization id.
        /// </summary>
        public long OrganizationId { get; set; }
        #endregion
    }
}
