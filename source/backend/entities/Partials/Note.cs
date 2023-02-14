using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsNote class, provides an entity for the datamodel to manage notes.
    /// </summary>
    public partial class PimsNote : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.NoteId; set => this.NoteId = value; }
        #endregion
    }
}
