using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    public partial class PimsAcquisitionFileDocument : PimsFileDocument, IDisableBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.AcquisitionFileDocumentId; set => this.AcquisitionFileDocumentId = value; }

        [NotMapped]
        public override long FileId { get => this.AcquisitionFileId; set => this.AcquisitionFileId = value; }
    }
}
