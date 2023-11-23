using Pims.Api.Concepts.Models.Base;
using Pims.Api.Concepts.Models.Concepts.Address;

namespace Pims.Api.Concepts.Models.Concepts.Organization
{
    public class OrganizationAddressModel : BaseConcurrentModel
    {
        #region Properties

        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The organization id associated with the address.
        /// </summary>
        public long OrganizationId { get; set; }

        /// <summary>
        /// get/set - The address.
        /// </summary>
        public AddressModel Address { get; set; }

        /// <summary>
        /// get/set - The address usage type.
        /// </summary>
        public TypeModel<string> AddressUsageType { get; set; }
        #endregion
    }
}
