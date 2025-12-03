using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsProjectNote class, provides an entity for the datamodel to manage project notes.
    /// </summary>
    public partial class PimsProjectNote : PimsNoteRelationship, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.ProjectNoteId; set => this.ProjectNoteId = value; }

        [NotMapped]
        public override long ParentId { get => this.ProjectId; set => this.ProjectId = value; }

        [NotMapped]
        public override long InternalNoteId { get => this.NoteId; set => this.NoteId = value; }
    }
}
