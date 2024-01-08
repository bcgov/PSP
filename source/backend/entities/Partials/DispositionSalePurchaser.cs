using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionPurchaser  class, provides an entity for the datamodel to manage the relationship between the Disposition Sale and the Purchaser(s).
    /// </summary>
    public partial class PimsDispositionPurchaser : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.DispositionPurchaserId; set => this.DispositionPurchaserId = value; }
    }
}
