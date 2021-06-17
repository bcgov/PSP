using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// BuildingPredominateUse class, provides an entity for the datamodel to manage a list of building predominate uses.
    /// </summary>
    [MotiTable("PIMS_BUILDING_PREDOMINATE_USE", "BLPRDU")]
    public class BuildingPredominateUse : LookupEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify building predominate use.
        /// </summary>
        [Column("BUILDING_PREDOMINATE_USE_ID")]
        public long Id { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a BuildingPredominateUse class.
        /// </summary>
        public BuildingPredominateUse() { }

        /// <summary>
        /// Create a new instance of a BuildingPredominateUse class.
        /// </summary>
        /// <param name="name"></param>
        public BuildingPredominateUse(string name) : base(name)
        {
        }
        #endregion
    }
}
