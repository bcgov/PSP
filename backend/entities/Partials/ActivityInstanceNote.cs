using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsActivityInstanceNote class, provides an entity for the datamodel to manage activity notes.
    /// </summary>
    public partial class PimsActivityInstanceNote : IdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Id { get => this.PimsActivityInstanceNoteId; set => this.PimsActivityInstanceNoteId = value; }
        #endregion
    }
}
