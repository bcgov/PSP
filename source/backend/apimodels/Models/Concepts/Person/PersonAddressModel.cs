using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Address;

namespace Pims.Api.Models.Concepts.Person
{
    public class PersonAddressModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The person id associated with the address.
        /// </summary>
        public long PersonId { get; set; }

        /// <summary>
        /// get/set - The address for the relationship.
        /// </summary>
        public AddressModel Address { get; set; }

        /// <summary>
        /// get/set - The address usage type.
        /// </summary>
        public CodeTypeModel<string> AddressUsageType { get; set; }
        #endregion
    }
}
