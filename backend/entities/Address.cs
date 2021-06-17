using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Address class, provides an entity for the datamodel to manage property addresses.
    /// </summary>
    [MotiTable("PIMS_ADDRESS", "ADDR")]
    public class Address : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - The primary key IDENTITY SEED.
        /// </summary>
        [Column("ADDRESS_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The first address line.
        /// </summary>
        [Column("ADDRESS1")]
        public string Address1 { get; set; }

        /// <summary>
        /// get/set - The second address line.
        /// </summary>
        [Column("ADDRESS2")]
        public string Address2 { get; set; }

        /// <summary>
        /// get/set - The name of the location (city, municipality, area, etc.)
        /// </summary>
        [Column("ADMINISTRATIVE_AREA")]
        public string AdministrativeArea { get; set; }

        /// <summary>
        /// get/set - The foreign key to the province.
        /// </summary>
        [Column("PROVINCE_ID")]
        public long ProvinceId { get; set; }

        /// <summary>
        /// get/set - The province of the address.
        /// </summary>
        public Province Province { get; set; }

        /// <summary>
        /// get/set - The postal code.
        /// </summary>
        [Column("POSTAL")]
        public string Postal { get; set; }
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
        /// <param name="address2"></param>
        /// <param name="administrativeArea"></param>
        /// <param name="provinceId"></param>
        /// <param name="postal"></param>
        public Address(string address1, string address2, string administrativeArea, int provinceId, string postal)
        {
            this.Address1 = address1;
            this.Address2 = address2;
            this.AdministrativeArea = administrativeArea ?? throw new ArgumentNullException(nameof(administrativeArea));
            this.ProvinceId = provinceId;
            this.Postal = postal;
            this.CreatedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// Create a new instance of a Address class.
        /// </summary>
        /// <param name="address1"></param>
        /// <param name="address2"></param>
        /// <param name="administrativeArea"></param>
        /// <param name="province"></param>
        /// <param name="postal"></param>
        public Address(string address1, string address2, string administrativeArea, Province province, string postal)
        {
            this.Address1 = address1;
            this.Address2 = address2;
            this.AdministrativeArea = administrativeArea ?? throw new ArgumentNullException(nameof(administrativeArea));
            this.Province = province;
            this.ProvinceId = province?.Id ??
                throw new ArgumentNullException(nameof(province));
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
            return String.Join(", ", new[] { this.Address1, this.Address2, this.AdministrativeArea, this.Province?.Code, this.Postal }.Where(s => !String.IsNullOrWhiteSpace(s)));
        }
        #endregion
    }
}
