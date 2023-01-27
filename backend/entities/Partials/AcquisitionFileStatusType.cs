using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AcquisitionFileStatusType class, provides an entity for the datamodel to manage acquisition status types.
    /// </summary>
    public partial class PimsAcquisitionFileStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify acquisition file status type.
        /// </summary>
        [NotMapped]
        public string Id { get => AcquisitionFileStatusTypeCode; set => AcquisitionFileStatusTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsAcquisitionFileStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcquisitionFileStatusType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
