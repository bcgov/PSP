using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Country class, provides an entity for the datamodel to manage a list of countries.
    /// </summary>
    public partial class PimsCountry : IBaseEntity
    {
        #region Properties
        [NotMapped]
        public short Id { get => CountryId; set => CountryId = value; }

        [NotMapped]
        public string Code { get => CountryCode; set => CountryCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a Country class.
        /// </summary>
        /// <param name="code"></param>
        public PimsCountry(string code)
            : this()
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException($"Argument '{nameof(code)}' must have a valid value.", nameof(code));
            }

            this.CountryCode = code;
        }
        #endregion
    }
}
