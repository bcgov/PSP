using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionChecklistItem class, provides an entity for the datamodel to manage a list of disposition checklist items.
    /// </summary>
    public partial class PimsDispositionChecklistItem : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.DispositionChecklistItemId; set => this.DispositionChecklistItemId = value; }
    }
}
