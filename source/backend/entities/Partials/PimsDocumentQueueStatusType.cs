using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDocumentQueueStatusType class, provides an entity for the datamodel to manage document queue status types.
    /// </summary>
    public partial class PimsDocumentQueueStatusType : ITypeEntity<string>
    {
        [NotMapped]
        public string Id { get => DocumentQueueStatusTypeCode; set => DocumentQueueStatusTypeCode = value; }

        public PimsDocumentQueueStatusType(string id)
            : this()
        {
            Id = id;
        }

        public PimsDocumentQueueStatusType()
        {
        }
    }
}
