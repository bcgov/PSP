using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDocument for Leases.
    /// </summary>
    public partial class PimsLeaseDocument : PimsFileDocument, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => LeaseDocumentId; set => LeaseDocumentId = value; }

        [NotMapped]
        public override long FileId { get => this.LeaseId; set => this.LeaseId = value; }

        [NotMapped]
        public override long? InternalDocumentId { get => this.DocumentId; set => this.DocumentId = value.GetValueOrDefault(); }
    }
}
