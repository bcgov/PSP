namespace Pims.Api.Models.Concepts
{
    public class PersonAddressModel : BaseAppModel
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
        /// get/set - The person associated with the address.
        /// </summary>
        public PersonModel Person { get; set; }

        /// <summary>
        /// get/set - The address for the relationship.
        /// </summary>
        public AddressModel Address { get; set; }

        /// <summary>
        /// get/set - The address usage type.
        /// </summary>
        public TypeModel<string> AddressUsageType { get; set; }
        #endregion
    }
}
