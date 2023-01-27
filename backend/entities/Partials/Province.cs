using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Province class, provides an entity for the datamodel to manage a list of provinces.
    /// </summary>
    public partial class PimsProvinceState : ICodeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify entity.
        /// </summary>
        [NotMapped]
        public short Id { get => ProvinceStateId; set => ProvinceStateId = value; }

        [NotMapped]
        public string Code { get => ProvinceStateCode; set => ProvinceStateCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a Province class.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="country"></param>
        public PimsProvinceState(string code, PimsCountry country)
            : this()
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException($"Argument '{nameof(code)}' must have a valid value.", nameof(code));
            }

            this.Code = code;
            this.Country = country ?? throw new ArgumentNullException(nameof(country));
            this.CountryId = country.Id;
        }
        #endregion
    }
}
