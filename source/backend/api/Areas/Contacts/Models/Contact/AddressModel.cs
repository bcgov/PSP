using Pims.Api.Models;

namespace Pims.Api.Areas.Contact.Models.Contact
{
    /// <summary>
    /// Provides a contact-oriented address model.
    /// </summary>
    public class AddressModel
    {
        #region Properties

        /// <summary>
        /// get/set - The primary key to identify the address.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The concurrency row version.
        /// </summary>
        public long RowVersion { get; set; }

        /// <summary>
        /// get/set - The address type.
        /// </summary>
        public TypeModel<string> AddressType { get; set; }

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
        /// get/set - The address's province.
        /// </summary>
        public ProvinceStateModel Province { get; set; }

        /// <summary>
        /// get/set - The address's country.
        /// </summary>
        public CountryModel Country { get; set; }

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
