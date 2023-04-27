using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsCompensationRequisition class, provides an entity for the datamodel to compensation requisition information.
    /// </summary>
    public partial class PimsCompensationRequisition : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.CompensationRequisitionId; set => this.CompensationRequisitionId = value; }
    }
}
