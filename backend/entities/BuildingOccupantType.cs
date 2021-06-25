using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// BuildingOccupantType class, provides an entity for the datamodel to manage a list of building occupant types.
    /// </summary>
    [MotiTable("PIMS_BUILDING_OCCUPANT_TYPE", "BLOCCT")]
    public class BuildingOccupantType : LookupEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify building occupant type.
        /// </summary>
        [Column("BUILDING_OCCUPANT_TYPE_ID")]
        public override long Id { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a BuildingOccupantType class.
        /// </summary>
        public BuildingOccupantType() { }

        /// <summary>
        /// Create a new instance of a BuildingOccupantType class.
        /// </summary>
        /// <param name="name"></param>
        public BuildingOccupantType(string name) : base(name)
        {
        }
        #endregion
    }
}
