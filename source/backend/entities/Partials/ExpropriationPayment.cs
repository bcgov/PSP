using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsExpropriationPayment class, provides an entity for the datamodel to manage Form 8 Expropriation.
    /// </summary>
    public partial class PimsExpropriationPayment : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.ExpropriationPaymentId; set => this.ExpropriationPaymentId = value; }
    }
}
