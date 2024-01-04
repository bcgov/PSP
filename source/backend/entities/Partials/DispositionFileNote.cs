using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionFileNote class, provides an entity for the datamodel to manage disposition file notes.
    /// </summary>
    public partial class PimsDispositionFileNote : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.DispositionFileNoteId; set => this.DispositionFileNoteId = value; }
        #endregion
    }
}
