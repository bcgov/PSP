using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsVolumeUnitType class, provides an entity for the datamodel to manage a list of volume unit types.
    /// </summary>
    public partial class PimsVolumeUnitType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify volume unit type.
        /// </summary>
        [NotMapped]
        public string Id { get => VolumeUnitTypeCode; set => VolumeUnitTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsVolumeUnitType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsVolumeUnitType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
