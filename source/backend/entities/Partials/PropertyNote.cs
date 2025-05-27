using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyNote class, provides an entity for the datamodel to manage Property notes.
    /// </summary>
    public partial class PimsPropertyNote : PimsNoteRelationship, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropertyNoteId; set => this.PropertyNoteId = value; }

        [NotMapped]
        public override long ParentId { get => this.PropertyId; set => this.PropertyId = value; }

        [NotMapped]
        public override long InternalNoteId { get => this.NoteId; set => this.NoteId = value; }
        #endregion
    }
}
