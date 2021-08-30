using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyTenureType class, provides an entity for the datamodel to manage a list of property tenure types.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_TENURE_TYPE", "PRPTNR")]
    public class PropertyTenureType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify property tenure type.
        /// </summary>
        [Column("PROPERTY_TENURE_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyTenureType class.
        /// </summary>
        public PropertyTenureType() { }

        /// <summary>
        /// Create a new instance of a PropertyTenureType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public PropertyTenureType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
