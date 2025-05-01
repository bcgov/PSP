using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Entity containing the details regarding actions involving a property owner associated with an expropriation.
    /// </summary>
    public partial class PimsExpropOwnerHistory : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.ExpropOwnerHistoryId; set => this.ExpropOwnerHistoryId = value; }
    }
}
