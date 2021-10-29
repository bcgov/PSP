using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// SurplusDeclarationType class, provides an entity for the datamodel to manage a list of surplus declaration types.
    /// </summary>
    [MotiTable("PIMS_SURPLUS_DECLARATION_TYPE", "SPDCLT")]
    public class PropertySurplusDeclarationType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify surplus declaration type.
        /// </summary>
        [Column("SURPLUS_DECLARATION_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a SurplusDeclarationType class.
        /// </summary>
        public PropertySurplusDeclarationType() { }

        /// <summary>
        /// Create a new instance of a SurplusDeclarationType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public PropertySurplusDeclarationType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
