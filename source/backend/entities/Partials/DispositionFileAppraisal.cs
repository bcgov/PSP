using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionAppraisal class, provides an entity for the datamodel.
    /// </summary>
    public partial class PimsDispositionAppraisal : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.DispositionAppraisalId; set => this.DispositionAppraisalId = value; }
    }
}
