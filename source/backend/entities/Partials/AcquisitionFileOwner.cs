using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionOwner partial class.
    /// </summary>
    public partial class PimsAcquisitionOwner : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.AcquisitionOwnerId; set => this.AcquisitionOwnerId = value; }
    }
}
