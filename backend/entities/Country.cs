using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Country class, provides an entity for the datamodel to manage a list of countries.
    /// </summary>
    [MotiTable("PIMS_COUNTRY", "CNTRY")]
    public class Country : BaseEntity, ICodeEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify entity.
        /// </summary>
        [Column("COUNTRY_ID")]
        public int Id { get; set; }

        /// <summary>
        /// get/set - A unique code for the lookup.
        /// </summary>
        [Column("COUNTRY_CODE")]
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
        /// get - A collection of provinces or states in this country.
        /// </summary>
        public ICollection<Province> Provinces { get; } = new List<Province>();

        /// <summary>
        /// get - A Collection of addresses in this country.
        /// </summary>
        public ICollection<Address> Addresses { get; } = new List<Address>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Country class.
        /// </summary>
        public Country() { }

        /// <summary>
        /// Create a new instance of a Country class.
        /// </summary>
        /// <param name="code"></param>
        public Country(string code)
        {
            if (String.IsNullOrWhiteSpace(code)) throw new ArgumentException($"Argument '{nameof(code)}' must have a valid value.", nameof(code));

            this.Code = code;
        }
        #endregion
    }
}
