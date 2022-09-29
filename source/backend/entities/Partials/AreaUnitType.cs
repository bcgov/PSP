using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAreaUnitType class, provides an entity for the datamodel to manage a list of area unit types.
    /// </summary>
    public partial class PimsAreaUnitType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify area unit type.
        /// </summary>
        [NotMapped]
        public string Id { get => AreaUnitTypeCode; set => AreaUnitTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsAreaUnitType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAreaUnitType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
