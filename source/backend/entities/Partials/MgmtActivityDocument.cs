using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsMgmtActivityDocument for Management Activities.
    /// </summary>
    public partial class PimsMgmtActivityDocument : PimsFileDocument, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => MgmtActivityDocumentId; set => MgmtActivityDocumentId = value; }

        [NotMapped]
        public override long FileId { get => ManagementActivityId; set => ManagementActivityId = value; }

        [NotMapped]
        public override long? InternalDocumentId { get => DocumentId; set => DocumentId = value.GetValueOrDefault(); }
    }
}
