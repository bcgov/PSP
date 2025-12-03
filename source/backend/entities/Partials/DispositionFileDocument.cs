using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDocument for Disposition Files.
    /// </summary>
    public partial class PimsDispositionFileDocument : PimsFileDocument, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.DispositionFileDocumentId; set => this.DispositionFileDocumentId = value; }

        [NotMapped]
        public override long FileId { get => this.DispositionFileId; set => this.DispositionFileId = value; }

        [NotMapped]
        public override long? InternalDocumentId { get => this.DocumentId; set => this.DocumentId = value.GetValueOrDefault(); }
    }
}
