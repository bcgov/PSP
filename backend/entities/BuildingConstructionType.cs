using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// BuildingConstructionType class, provides an entity for the datamodel to manage a list of building contruction types.
    /// </summary>
    [MotiTable("PIMS_BUILDING_CONSTRUCTION_TYPE", "BLCNTY")]
    public class BuildingConstructionType : LookupEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify building construction type.
        /// </summary>
        [Column("BUILDING_CONSTRUCTION_TYPE_ID")]
        public long Id { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a BuildingConstructionType class.
        /// </summary>
        public BuildingConstructionType() { }

        /// <summary>
        /// Create a new instance of a BuildingConstructionType class.
        /// </summary>
        /// <param name="name"></param>
        public BuildingConstructionType(string name) : base(name)
        {
        }
        #endregion
    }
}
