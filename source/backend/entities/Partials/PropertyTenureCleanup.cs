using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsPropTenureCleanup class, provides an entity for the datamodel to manage property tenure cleanup type.
    /// </summary>
    public partial class PimsPropTenureCleanup : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.PropTenureCleanupId; set => this.PropTenureCleanupId = value; }
    }
}
