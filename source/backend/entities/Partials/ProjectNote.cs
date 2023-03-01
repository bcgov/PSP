using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsProjectNote class, provides an entity for the datamodel to manage project notes.
    /// </summary>
    public partial class PimsProjectNote : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.ProjectNoteId; set => this.ProjectNoteId = value; }
    }
}
