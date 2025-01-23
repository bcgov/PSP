using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqFileAcqFlTakeTyp class, provides an entity for the datamodel to manage Lease Purposes types.
    /// </summary>
    public partial class PimsAcqFileAcqFlTakeTyp : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => AcqFileAcqFlTakeTypeId; set => AcqFileAcqFlTakeTypeId = value; }
    }
}
