using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDocument for Management Files.
    /// </summary>
    public partial class PimsManagementFileDocument : PimsFileDocument, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.ManagementFileDocumentId; set => this.ManagementFileDocumentId = value; }

        [NotMapped]
        public override long FileId { get => this.ManagementFileId; set => this.ManagementFileId = value; }
    }
}
