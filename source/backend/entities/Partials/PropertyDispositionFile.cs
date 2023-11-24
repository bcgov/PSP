using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyDispositionFile class, provides an entity for the datamodel to manage the relationship between Properties and Disposition Files.
    /// </summary>
    public partial class PimsPropertyDispositionFile : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropertyDispositionFileId; set => this.PropertyDispositionFileId = value; }
        #endregion
    }
}
