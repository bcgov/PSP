using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsActivityInstanceNote class, provides an entity for the datamodel to manage acquisition file notes.
    /// </summary>
    public partial class PimsAcquisitionFileNote : PimsNoteRelationship, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.AcquisitionFileNoteId; set => this.AcquisitionFileNoteId = value; }

        [NotMapped]
        public override long ParentId { get => this.AcquisitionFileId; set => this.AcquisitionFileId = value; }

        [NotMapped]
        public override long InternalNoteId { get => this.NoteId; set => this.NoteId = value; }
        #endregion
    }
}
