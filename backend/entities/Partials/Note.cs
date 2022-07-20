using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsNote class, provides an entity for the datamodel to manage notes.
    /// </summary>
    public partial class PimsNote : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Id { get => this.NoteId; set => this.NoteId = value; }
        #endregion
    }
}
