using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsDispositionSale class, provides an entity for the datamodel to manage Disposition File's Sale.
    /// </summary>
    public partial class PimsDispositionSale : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.DispositionSaleId; set => this.DispositionSaleId = value; }
    }
}
