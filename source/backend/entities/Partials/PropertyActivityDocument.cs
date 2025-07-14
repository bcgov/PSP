using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyActivityDocument for Property Activities.
    /// </summary>
    public partial class PimsPropertyActivityDocument : PimsFileDocument, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => PropertyActivityDocumentId; set => PropertyActivityDocumentId = value; }

        [NotMapped]
        public override long FileId { get => this.PimsManagementActivityId; set => this.PimsManagementActivityId = value; }

        [NotMapped]
        public override long? InternalDocumentId { get => this.DocumentId; set => this.DocumentId = value.GetValueOrDefault(); }
    }
}
