using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyAreaUnitType class, provides an entity for the datamodel to manage a list of area unit types.
    /// </summary>
    [MotiTable("PIMS_AREA_UNIT_TYPE", "ARUNIT")]
    public class PropertyAreaUnitType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify area unit type.
        /// </summary>
        [Column("AREA_UNIT_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyAreaUnitType class.
        /// </summary>
        public PropertyAreaUnitType() { }

        /// <summary>
        /// Create a new instance of a PropertyAreaUnitType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public PropertyAreaUnitType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
