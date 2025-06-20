using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeaseNote partial class, extends the functionality of the EF definition.
    /// </summary>
    public partial class PimsLeaseNote : PimsNoteRelationship, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.LeaseNoteId; set => this.LeaseNoteId = value; }

        [NotMapped]
        public override long ParentId { get => this.LeaseId; set => this.LeaseId = value; }

        [NotMapped]
        public override long InternalNoteId { get => this.NoteId; set => this.NoteId = value; }
        #endregion
    }
}
