using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDocument for Properties.
    /// </summary>
    public partial class PimsPropertyDocument : PimsFileDocument, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.PropertyDocumentId; set => this.PropertyDocumentId = value; }

        [NotMapped]
        public override long FileId { get => this.PropertyId; set => this.PropertyId = value; }
    }
}
