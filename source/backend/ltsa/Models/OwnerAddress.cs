/* 
 * Title Direct Search Services
 *
 * Title Direct Search Services
 *
 * OpenAPI spec version: 4.0.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System.Runtime.Serialization;


namespace Pims.Ltsa.Models
{
    /// <summary>
    /// OwnerAddress
    /// </summary>
    [DataContract]
    public partial class OwnerAddress
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnerAddress" /> class.
        /// </summary>
        /// <param name="addressLine1">Mailing address destination.</param>
        /// <param name="addressLine2">Mailing address destination.</param>
        /// <param name="city">City.</param>
        /// <param name="province">province.</param>
        /// <param name="provinceName">Non-Canadian Province / geographic division.</param>
        /// <param name="country">Country.</param>
        /// <param name="postalCode">Postal Code.</param>
        public OwnerAddress(string addressLine1 = default, string addressLine2 = default, string city = default, CanadianProvince? province = default, string provinceName = default, string country = default, string postalCode = default)
        {
            this.AddressLine1 = addressLine1;
            this.AddressLine2 = addressLine2;
            this.City = city;
            this.Province = province;
            this.ProvinceName = provinceName;
            this.Country = country;
            this.PostalCode = postalCode;
        }

        /// <summary>
        /// Mailing address destination
        /// </summary>
        /// <value>Mailing address destination</value>
        [DataMember(Name = "addressLine1", EmitDefaultValue = false)]
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Mailing address destination
        /// </summary>
        /// <value>Mailing address destination</value>
        [DataMember(Name = "addressLine2", EmitDefaultValue = false)]
        public string AddressLine2 { get; set; }

        /// <summary>
        /// City
        /// </summary>
        /// <value>City</value>
        [DataMember(Name = "city", EmitDefaultValue = false)]
        public string City { get; set; }

        /// <summary>
        /// Gets or Sets Province
        /// </summary>
        [DataMember(Name = "province", EmitDefaultValue = false)]
        public CanadianProvince? Province { get; set; }

        /// <summary>
        /// Non-Canadian Province / geographic division
        /// </summary>
        /// <value>Non-Canadian Province / geographic division</value>
        [DataMember(Name = "provinceName", EmitDefaultValue = false)]
        public string ProvinceName { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        /// <value>Country</value>
        [DataMember(Name = "country", EmitDefaultValue = false)]
        public string Country { get; set; }

        /// <summary>
        /// Postal Code
        /// </summary>
        /// <value>Postal Code</value>
        [DataMember(Name = "postalCode", EmitDefaultValue = false)]
        public string PostalCode { get; set; }

    }
}
