using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Province class, provides an entity for the datamodel to manage a list of provinces.
    /// </summary>
    [MotiTable("PIMS_PROVINCE_STATE", "PROVNC")]
    public class Province : BaseEntity, ICodeEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify entity.
        /// </summary>
        [Column("PROVINCE_STATE_ID")]
        public int Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to country.
        /// </summary>
        [Column("COUNTRY_ID")]
        public int CountryId { get; set; }

        /// <summary>
        /// get/set - The country.
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        /// get/set - A unique code for the lookup.
        /// </summary>
        [Column("PROVINCE_STATE_CODE")]
        public string Code { get; set; }

        /// <summary>
        /// get/set - A description of the country.
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// get/set - The order to display the countries.
        /// </summary>
        [Column("DISPLAY_ORDER")]
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// get/set - Whether this province record is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get - A collection of addresses in this province or state.
        /// </summary>
        public ICollection<Address> Addresses { get; } = new List<Address>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Province class.
        /// </summary>
        public Province() { }

        /// <summary>
        /// Create a new instance of a Province class.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="country"></param>
        public Province(string code, Country country)
        {
            if (String.IsNullOrWhiteSpace(code)) throw new ArgumentException($"Argument '{nameof(code)}' must have a valid value.", nameof(code));

            this.Code = code;
            this.Country = country ?? throw new ArgumentNullException(nameof(country));
            this.CountryId = country.Id;
        }
        #endregion
    }
}
