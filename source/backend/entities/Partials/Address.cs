using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Address class, provides an entity for the datamodel to manage property addresses.
    /// </summary>
    public partial class PimsAddress : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.AddressId; set => this.AddressId = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a Address class.
        /// </summary>
        /// <param name="address1"></param>
        /// <param name="municipality"></param>
        /// <param name="province"></param>
        /// <param name="district"></param>
        /// <param name="postal"></param>
        public PimsAddress(string address1, string address2, string municipality, PimsProvinceState province, PimsDistrict district, string postal)
            : this()
        {
            if (string.IsNullOrWhiteSpace(address1))
            {
                throw new ArgumentException($"Argument '{nameof(address1)}' is required.", nameof(address1));
            }

            this.StreetAddress1 = address1;
            this.StreetAddress2 = address2;
            this.MunicipalityName = municipality ?? throw new ArgumentNullException(nameof(municipality));
            this.ProvinceState = province ?? throw new ArgumentNullException(nameof(province));
            this.ProvinceStateId = province.ProvinceStateId;
            this.Country = province.Country ?? throw new ArgumentException($"Argument '{nameof(province)}.{nameof(province.Country)}' is required.", nameof(province));
            this.CountryId = province.Country.CountryId;
            this.DistrictCodeNavigation = district;
            this.DistrictCode = district?.DistrictCode;
            this.RegionCodeNavigation = district?.RegionCodeNavigation;
            this.RegionCode = district?.RegionCode;
            this.PostalCode = postal;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Return the address as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(", ", new[] { this.StreetAddress1, this.StreetAddress2, this.StreetAddress3, this.MunicipalityName, this.ProvinceState?.ProvinceStateCode, this.DistrictCodeNavigation?.DistrictName, this.RegionCodeNavigation?.RegionName, this.PostalCode, this.Country?.CountryCode }.Where(s => !string.IsNullOrWhiteSpace(s)));
        }
        #endregion
    }
}
