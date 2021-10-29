using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyImprovementType class, provides an entity for the datamodel to manage a list of property improvement types.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_IMPROVEMENT_TYPE", "PIMPRT")]
    public class PropertyImprovementType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify property improvement type.
        /// </summary>
        [Column("PROPERTY_IMPROVEMENT_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of Improvements.
        /// </summary>
        public ICollection<PropertyImprovement> Improvements { get; } = new List<PropertyImprovement>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyImprovementType class.
        /// </summary>
        public PropertyImprovementType() { }

        /// <summary>
        /// Create a new instance of a PropertyImprovementType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public PropertyImprovementType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
