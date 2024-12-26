using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDocumentQueueStatusType class, provides an entity for the datamodel to manage document queue status types.
    /// </summary>
    public partial class PimsDocumentQueueStatusType : ITypeEntity<string>
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify disposition type.
        /// </summary>
        [NotMapped]
        public string Id { get => DocumentQueueStatusTypeCode; set => DocumentQueueStatusTypeCode = value; }
        #endregion

        #region Constructors

        public PimsDocumentQueueStatusType() { }

        /// <summary>
        /// Create a new instance of a PimsDocumentQueueStatusType class.
        /// </summary>
        /// <param name="id"></param>
        public PimsDocumentQueueStatusType(string id)
        {
            Id = id;
        }
        #endregion
    }
}
