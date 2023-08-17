using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsExpropriationPayment class, provides an entity for the datamodel to manage Form 8 Expropriation Payment Item.
    /// </summary>
    public partial class PimsExpropPmtPmtItem : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.ExpropPmtPmtItemId; set => this.ExpropPmtPmtItemId = value; }
    }
}
