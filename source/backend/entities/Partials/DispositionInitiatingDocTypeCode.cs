using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionInitiatingDocType class, provides an entity for the datamodel to manage Disposition initiating document types.
    /// </summary>
    public partial class PimsDispositionInitiatingDocType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify disposition initiating document type.
        /// </summary>
        [NotMapped]
        public string Id { get => DispositionInitiatingDocTypeCode; set => DispositionInitiatingDocTypeCode = value; }
        #endregion

        #region Constructors

        public PimsDispositionInitiatingDocType()
        {
        }

        /// <summary>
        /// Create a new instance of a PimsDispositionInitiatingDocType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDispositionInitiatingDocType(string id)
        {
            Id = id;
        }
        #endregion
    }
}
