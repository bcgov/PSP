using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// TypeEntity class, provides an entity for the datamodel to manage entities that represent type values.
    /// </summary>
    public abstract class TypeEntity<KeyType> : BaseEntity, ITypeEntity<KeyType>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify type.
        /// </summary>
        public abstract KeyType Id { get; set; }

        /// <summary>
        /// get/set - A description of the type.
        /// </summary>
        [Column("DESCRIPTION", Order = 93)]
        public string Description { get; set; }

        /// <summary>
        /// get/set - Whether this code is disabled.
        /// </summary>
        [Column("IS_DISABLED", Order = 95)]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get/set - The sort order of the lookup item.
        /// </summary>
        [Column("DISPLAY_ORDER", Order = 96)]
        public int? DisplayOrder { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a TypeEntity class.
        /// </summary>
        protected TypeEntity() { }

        /// <summary>
        /// Create a new instance of a TypeEntity class.
        /// </summary>
        /// <param name="description"></param>
        protected TypeEntity(string description)
        {
            this.Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        /// <summary>
        /// Create a new instance of a TypeEntity class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        protected TypeEntity(KeyType id, string description) : this(description)
        {
            if (typeof(KeyType) == typeof(string) && String.IsNullOrWhiteSpace(id as string)) throw new ArgumentException($"Argument '{nameof(id)}' must have a valid value.", nameof(id));

            this.Id = id ?? throw new ArgumentNullException(nameof(id));
        }
        #endregion
    }
}
