using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropertyNote class, provides an entity for the datamodel to manage Property notes.
    /// </summary>
    public partial class PimsPropertyNote : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.PropertyNoteId; set => this.PropertyNoteId = value; }
        #endregion
    }
}
