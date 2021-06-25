using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// CodeEntity class, provides an entity for the datamodel to manage entities that represent codified values.
    /// </summary>
    public abstract class CodeEntity : BaseAppEntity, ICodeEntity
    {
        #region Properties
        /// <summary>
        /// get/set - A unique code for the lookup.
        /// </summary>
        [Column("CODE", Order = 94)]
        public string Code { get; set; }

        /// <summary>
        /// get/set - The name of the code.
        /// </summary>
        [Column("NAME", Order = 93)]
        public string Name { get; set; }

        /// <summary>
        /// get/set - Whether this code is disabled.
        /// </summary>
        [Column("IS_DISABLED", Order = 95)]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The sort order of the lookup item.
        /// </summary>
        [Column("DISPLAY_ORDER", Order = 96)]
        public int SortOrder { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a CodeEntity class.
        /// </summary>
        public CodeEntity() { }

        /// <summary>
        /// Create a new instance of a CodeEntity class.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        public CodeEntity(string code, string name)
        {
            if (String.IsNullOrWhiteSpace(code)) throw new ArgumentException($"Argument '{nameof(code)}' must have a valid value.", nameof(code));
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException($"Argument '{nameof(name)}' must have a valid value.", nameof(name));

            this.Code = code;
            this.Name = name;
        }
        #endregion
    }
}
