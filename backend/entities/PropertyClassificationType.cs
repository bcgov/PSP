using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyClassificationType class, provides an entity for the datamodel to manage a list of property classification types.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_CLASSIFICATION_TYPE", "PRPCLT")]
    public class PropertyClassificationType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to property classification types.
        /// </summary>
        [Column("PROPERTY_CLASSIFICATION_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyClassificationType class.
        /// </summary>
        public PropertyClassificationType() { }

        /// <summary>
        /// Create a new instance of a PropertyClassificationType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public PropertyClassificationType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
