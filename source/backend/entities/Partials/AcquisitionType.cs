using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AcquisitionType class, provides an entity for the datamodel to manage acquisition types.
    /// </summary>
    public partial class PimsAcquisitionType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify acquisition type.
        /// </summary>
        [NotMapped]
        public string Id { get => AcquisitionTypeCode; set => AcquisitionTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsAcquisitionType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcquisitionType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
