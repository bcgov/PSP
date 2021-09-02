using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyDataSourceType class, provides an entity for the datamodel to manage a list of property data source types.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_DATA_SOURCE_TYPE", "PRPDST")]
    public class PropertyDataSourceType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to property data source types.
        /// </summary>
        [Column("PROPERTY_DATA_SOURCE_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyDataSourceType class.
        /// </summary>
        public PropertyDataSourceType() { }

        /// <summary>
        /// Create a new instance of a PropertyDataSourceType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public PropertyDataSourceType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
