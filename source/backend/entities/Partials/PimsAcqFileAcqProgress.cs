using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqFileAcqProgress class, provides an entity for the datamodel to manage Lease Purposes types.
    /// </summary>
    public partial class PimsAcqFileAcqProgress : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => AcqFileAcqProgressId; set => AcqFileAcqProgressId = value; }
    }
}
