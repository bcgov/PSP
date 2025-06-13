using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionStatusType class, provides an entity for the datamodel to manage Disposition status types.
    /// </summary>
    public partial class PimsDispositionStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify disposition status type.
        /// </summary>
        [NotMapped]
        public string Id { get => DispositionStatusTypeCode; set => DispositionStatusTypeCode = value; }
        #endregion

        #region Constructors

        public PimsDispositionStatusType()
        {
        }

        /// <summary>
        /// Create a new instance of a PimsDispositionStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDispositionStatusType(string id)
        {
            Id = id;
        }
        #endregion
    }
}
