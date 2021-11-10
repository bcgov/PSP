using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Address class, provides an entity for the datamodel to manage property addresses.
    /// </summary>
    [MotiTable("PIMS_ADDRESS", "ADDRSS")]
    public class Address : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key IDENTITY SEED.
        /// </summary>
        [Column("ADDRESS_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to address usage type.
        /// </summary>
        [Column("ADDRESS_USAGE_TYPE_CODE")]
        public string AddressTypeId { get; set; }

        /// <summary>
        /// get/set - The address usage type.
        /// </summary>
        public AddressType AddressType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the region.
        /// </summary>
        [Column("REGION_CODE")]
        public int? RegionId { get; set; }

        /// <summary>
        /// get/set - The region.
        /// </summary>
        public Region Region { get; set; }

        /// <summary>
        /// get/set - Foreign key to the district.
        /// </summary>
        [Column("DISTRICT_CODE")]
        public int? DistrictId { get; set; }

        /// <summary>
        /// get/set - The district.
        /// </summary>
        public District District { get; set; }

        /// <summary>
        /// get/set - Foreign key to the province.
        /// </summary>
        [Column("PROVINCE_STATE_ID")]
        public int ProvinceId { get; set; }

        /// <summary>
        /// get/set - The province.
        /// </summary>
        public Province Province { get; set; }

        /// <summary>
        /// get/set - Foreign key to the country.
        /// </summary>
        [Column("COUNTRY_ID")]
        public int? CountryId { get; set; }

        /// <summary>
        /// get/set - The country.
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        /// get/set - The address line part 1.
        /// </summary>
        [Column("STREET_ADDRESS_1")]
        public string StreetAddress1 { get; set; }

        /// <summary>
        /// get/set - The address line part 2.
        /// </summary>
        [Column("STREET_ADDRESS_2")]
        public string StreetAddress2 { get; set; }

        /// <summary>
        /// get/set - The address line part 3.
        /// </summary>
        [Column("STREET_ADDRESS_3")]
        public string StreetAddress3 { get; set; }

        /// <summary>
        /// get/set - The name of the municipality.
        /// </summary>
        [Column("MUNICIPALITY_NAME")]
        public string Municipality { get; set; }

        /// <summary>
        /// get/set - The postal code.
        /// </summary>
        [Column("POSTAL_CODE")]
        public string Postal { get; set; }

        /// <summary>
        /// get/set - Latitude part of the location.
        /// </summary>
        [Column("LATITUDE")]
        public double? Latitude { get; set; }

        /// <summary>
        /// get/set - Longitude part of the location.
        /// </summary>
        [Column("LONGITUDE")]
        public double? Longitude { get; set; }

        /// <summary>
        /// get - A collection of organizations with this address.
        /// </summary>
        public ICollection<Organization> Organizations { get; } = new List<Organization>();

        /// <summary>
        /// get - A collection of persons with this address.
        /// </summary>
        public ICollection<Person> Persons { get; } = new List<Person>();

        /// <summary>
        /// get - A collection of property with this address.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();

        /// <summary>
        /// get - A collection of contacts with this address.
        /// </summary>
        public ICollection<Contact> Contacts { get; } = new List<Contact>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Address class.
        /// </summary>
        public Address() { }

        /// <summary>
        /// Create a new instance of a Address class.
        /// </summary>
        /// <param name="address1"></param>
        /// <param name="unitNumber"></param>
        /// <param name="municipality"></param>
        /// <param name="province"></param>
        /// <param name="district"></param>
        /// <param name="postal"></param>
        public Address(string address1, string address2, string municipality, Province province, District district, string postal)
        {
            if (String.IsNullOrWhiteSpace(address1)) throw new ArgumentException($"Argument '{nameof(address1)}' is required.", nameof(address1));

            this.StreetAddress1 = address1;
            this.StreetAddress2 = address2;
            this.Municipality = municipality ?? throw new ArgumentNullException(nameof(municipality));
            this.Province = province ?? throw new ArgumentNullException(nameof(province));
            this.ProvinceId = province.Id;
            this.Country = province.Country ?? throw new ArgumentException($"Argument '{nameof(province)}.{nameof(province.Country)}' is required.", nameof(province));
            this.CountryId = province.Country.Id;
            this.District = district;
            this.DistrictId = district?.Id;
            this.Region = district?.Region;
            this.RegionId = district?.Region?.Id;
            this.Postal = postal;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Return the address as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Join(", ", new[] { this.StreetAddress1, this.StreetAddress2, this.StreetAddress3, this.Municipality, this.Province?.Code, this.District?.Name, this.Region?.Name, this.Postal, this.Country?.Code }.Where(s => !String.IsNullOrWhiteSpace(s)));
        }
        #endregion
    }
}
