using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionChecklistItem class, provides an entity for the datamodel to manage a list of acquisition checklist items.
    /// </summary>
    public partial class PimsAcquisitionChecklistItem : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.AcquisitionChecklistItemId; set => this.AcquisitionChecklistItemId = value; }
    }
}
