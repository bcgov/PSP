using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionType class, provides an entity for the datamodel to manage Disposition types.
    /// </summary>
    public partial class PimsDispositionType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify disposition type.
        /// </summary>
        [NotMapped]
        public string Id { get => DispositionTypeCode; set => DispositionTypeCode = value; }
        #endregion

        #region Constructors

        public PimsDispositionType()
        {
        }

        /// <summary>
        /// Create a new instance of a PimsDispositionType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDispositionType(string id)
        {
            Id = id;
        }
        #endregion
    }
}
