using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDocument class, provides an entity for the datamodel to manage document entities.
    /// </summary>
    public partial class PimsDocument : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.DocumentId; set => this.DocumentId = value; }
        #endregion
    }
}
