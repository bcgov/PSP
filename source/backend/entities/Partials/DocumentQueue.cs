using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDocumentQueue class, provides an entity for the datamodel to manage document entities.
    /// </summary>
    public partial class PimsDocumentQueue : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.DocumentQueueId; set => this.DocumentQueueId = value; }
        #endregion
    }
}
