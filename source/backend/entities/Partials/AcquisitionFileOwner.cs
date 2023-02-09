using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcquisitionOwner partial class.
    /// </summary>
    public partial class PimsAcquisitionOwner : IdentityBaseAppEntity<long>
    {
        [NotMapped]
        public override long Id { get => this.AcquisitionOwnerId; set => this.AcquisitionOwnerId = value; }
    }
}
