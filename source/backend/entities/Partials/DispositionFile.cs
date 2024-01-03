using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionFile class, provides an entity for the datamodel to manage disposition files.
    /// </summary>
    public partial class PimsDispositionFile : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.DispositionFileId; set => this.DispositionFileId = value; }
        #endregion
    }
}
