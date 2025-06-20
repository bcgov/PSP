using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionFileNote class, provides an entity for the datamodel to manage disposition file notes.
    /// </summary>
    public partial class PimsDispositionFileNote : PimsNoteRelationship, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.DispositionFileNoteId; set => this.DispositionFileNoteId = value; }

        [NotMapped]
        public override long ParentId { get => this.DispositionFileId; set => this.DispositionFileId = value; }

        [NotMapped]
        public override long InternalNoteId { get => this.NoteId; set => this.NoteId = value; }
        #endregion
    }
}
