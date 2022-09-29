using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// AcqPhysFileStatusType class, provides an entity for the datamodel to manage acquisition physical file status types.
    /// </summary>
    public partial class PimsAcqPhysFileStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify the acquisition's physical file status type.
        /// </summary>
        [NotMapped]
        public string Id { get => AcqPhysFileStatusTypeCode; set => AcqPhysFileStatusTypeCode = value; }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of a PimsAcqPhysFileStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsAcqPhysFileStatusType(string id)
            : this()
        {
            Id = id;
        }
        #endregion
    }
}
