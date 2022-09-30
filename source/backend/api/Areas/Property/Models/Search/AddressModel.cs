namespace Pims.Api.Areas.Property.Models.Search
{
    /// <summary>
    /// AddressModel class, provides a model to represent the address of a property.
    /// </summary>
    public class AddressModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the property.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        public long RowVersion { get; set; }

        /// <summary>
        /// get/set - Foreign key to the address type.
        /// </summary>
        public string AddressTypeId { get; set; }

        /// <summary>
        /// get/set - The address type description.
        /// </summary>
        public string AddressType { get; set; }

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
        /// get/set - Foreign key to the region.
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// get/set - The name of the region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// get/set - Foreign key to the district.
        /// </summary>
        public int? DistrictId { get; set; }

        /// <summary>
        /// get/set - The name of the district.
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// get/set - The name of the municipality name.
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// get/set - Foreign key to the province.
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// get/set - The name of the province.
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// get/set - Foreign key to country.
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// get/set - The name of the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// get/set - The postal code.
        /// </summary>
        public string Postal { get; set; }
        #endregion
    }
}
