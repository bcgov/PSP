using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionFileStatusType class, provides an entity for the datamodel to manage Disposition file status types.
    /// </summary>
    public partial class PimsDispositionFileStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify disposition file status type.
        /// </summary>
        [NotMapped]
        public string Id { get => DispositionFileStatusTypeCode; set => DispositionFileStatusTypeCode = value; }
        #endregion

        #region Constructors

        public PimsDispositionFileStatusType()
        {
        }

        /// <summary>
        /// Create a new instance of a PimsDispositionFileStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDispositionFileStatusType(string id)
        {
            Id = id;
        }
        #endregion
    }
}
