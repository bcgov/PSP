namespace Pims.Api.Models.Concepts
{
    public class OrganizationAddressModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - The relationship id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The relationship's disabled status flag.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The organization associated with the address.
        /// </summary>
        public OrganizationModel Organization { get; set; }

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
