using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDspPurchAgent  class, provides an entity for the datamodel to manage the relationship between the Disposition Sale and the Purchaser Agent.
    /// </summary>
    public partial class PimsDspPurchSolicitor : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.DspPurchAgentId; set => this.DspPurchAgentId = value; }
    }
}
