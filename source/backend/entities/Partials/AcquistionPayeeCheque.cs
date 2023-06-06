using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsAcqPayeeCheque class, provides an entity for the datamodel to manage Compensation requisition's Payees Cheque.
    /// </summary>
    public partial class PimsAcqPayeeCheque : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => AcqPayeeChequeId ; set => AcqPayeeChequeId = value; }
    }
}
