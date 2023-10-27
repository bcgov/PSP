using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    public partial class PimsResearchFileDocument : PimsFileDocument, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.ResearchFileDocumentId; set => this.ResearchFileDocumentId = value; }

        [NotMapped]
        public override long FileId { get => this.ResearchFileId; set => this.ResearchFileId = value; }
    }
}
