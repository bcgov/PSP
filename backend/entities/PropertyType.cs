using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyType class, provides an entity for the datamodel to manage a list of property types.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_TYPE", "PRPTYP")]
    public class PropertyType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify property type.
        /// </summary>
        [Column("PROPERTY_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyType class.
        /// </summary>
        public PropertyType() { }

        /// <summary>
        /// Create a new instance of a PropertyType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public PropertyType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
