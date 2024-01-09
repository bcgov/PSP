using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Address;

namespace Pims.Api.Models.Concepts.Organization
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
