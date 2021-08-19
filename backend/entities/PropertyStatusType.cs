using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyStatusType class, provides an entity for the datamodel to manage a list of property status types.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_STATUS_TYPE", "PRPSTS")]
    public class PropertyStatusType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify property type.
        /// </summary>
        [Column("PROPERTY_STATUS_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyStatusType class.
        /// </summary>
        public PropertyStatusType() { }

        /// <summary>
        /// Create a new instance of a PropertyStatusType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public PropertyStatusType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
