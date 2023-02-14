using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsLeasePayment class, provides an entity for the datamodel to manage lease payments.
    /// </summary>
    public partial class PimsLeasePayment : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.LeasePaymentId; set => this.LeasePaymentId = value; }
        #endregion
    }
}
