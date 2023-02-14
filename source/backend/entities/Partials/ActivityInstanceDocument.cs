using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsActivityInstanceDocument class, provides an entity for the datamodel to manage Activity document entities.
    /// </summary>
    public partial class PimsActivityInstanceDocument : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.ActivityInstanceDocumentId; set => this.ActivityInstanceDocumentId = value; }
        #endregion
    }
}
