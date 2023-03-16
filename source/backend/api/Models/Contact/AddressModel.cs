namespace Pims.Api.Models.Contact
{
    /// <summary>
    /// Provides a contact-oriented address model.
    /// </summary>
    public class AddressModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the address.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the address type.
        /// </summary>
        public TypeModel<string> AddressTypeId { get; set; }

        /// <summary>
        /// get/set - The street address.
        /// </summary>
        public string StreetAddress1 { get; set; }

        /// <summary>
        /// get/set - The street address.
        /// </summary>
        public string StreetAddress2 { get; set; }

        /// <summary>
        /// get/set - The street address.
        /// </summary>
        public string StreetAddress3 { get; set; }

        /// <summary>
        /// get/set - The name of the municipality name.
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// get/set - Foreign key to the region.
        /// </summary>
        public short? RegionId { get; set; }

        /// <summary>
        /// get/set - Foreign key to the district.
        /// </summary>
        public short? DistrictId { get; set; }

        /// <summary>
        /// get/set - Foreign key to the address's province.
        /// </summary>
        public short? ProvinceId { get; set; }

        /// <summary>
        /// get/set - Foreign key to the address's country.
        /// </summary>
        public short CountryId { get; set; }

        /// <summary>
        /// get/set - The free-form value of country when country code is "Other".
        /// </summary>
        public string CountryOther { get; set; }

        /// <summary>
        /// get/set - The postal code.
        /// </summary>
        public string Postal { get; set; }
        #endregion
    }
}
