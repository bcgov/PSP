using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDocument for Projects.
    /// </summary>
    public partial class PimsProjectDocument : PimsFileDocument, IDisableBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => ProjectDocumentId; set => ProjectDocumentId = value; }

        [NotMapped]
        public override long FileId { get => this.ProjectId; set => this.ProjectId = value; }
    }
}
