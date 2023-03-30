using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDocument for Leases.
    /// </summary>
    public partial class PimsLeaseDocument : PimsFileDocument, IDisableBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => LeaseDocumentId; set => LeaseDocumentId = value; }

        [NotMapped]
        public override long FileId { get => this.LeaseId; set => this.LeaseId = value; }
    }
}
