using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Province class, provides an entity for the datamodel to manage a list of provinces.
    /// </summary>
    [MotiTable("PIMS_PROVINCE", "PROV")]
    public class Province : LookupEntity, ICodeEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify entity.
        /// </summary>
        [Column("PROVINCE_ID")]
        public override long Id { get; set; }

        /// <summary>
        /// get/set - A unique code for the lookup.
        /// </summary>
        [Column("PROVINCE_CODE", Order = 94)]
        public string Code { get; set; }
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
        /// <param name="name"></param>
        public Province(string code, string name)
        {
            if (String.IsNullOrWhiteSpace(code)) throw new ArgumentException($"Argument '{nameof(code)}' must have a valid value.", nameof(code));
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException($"Argument '{nameof(name)}' must have a valid value.", nameof(name));

            this.Code = code;
            this.Name = name;
        }
        #endregion
    }
}
