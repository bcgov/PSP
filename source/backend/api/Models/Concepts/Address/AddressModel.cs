namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// Provides a contact-oriented address model.
    /// </summary>
    public class AddressModel : BaseModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the address.
        /// </summary>
        public long? Id { get; set; }

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
        /// get/set - The address's Province Id.
        /// </summary>
        public short? ProvinceStateId { get; set; }

        /// <summary>
        /// get/set - The address's province.
        /// </summary>
        public CodeTypeModel Province { get; set; }

        /// <summary>
        /// get/set - The address's Country Id.
        /// </summary>
        public short? CountryId { get; set; }

        /// <summary>
        /// get/set - The address's country.
        /// </summary>
        public CodeTypeModel Country { get; set; }

        /// <summary>
        /// get/set - The address's district.
        /// </summary>
        public CodeTypeModel District { get; set; }

        /// <summary>
        /// get/set - The address's region.
        /// </summary>
        public CodeTypeModel Region { get; set; }

        /// <summary>
        /// get/set - The free-form value of country when country code is "Other".
        /// </summary>
        public string CountryOther { get; set; }

        /// <summary>
        /// get/set - The postal code.
        /// </summary>
        public string Postal { get; set; }

        /// <summary>
        /// get/set - Addresss latitude coordinate.
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// get/set - Addresss longitude coordinate.
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// get/set - Addresss comment.
        /// </summary>
        public string Comment { get; set; }
        #endregion
    }
}
