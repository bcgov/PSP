using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PimsCompensationRequisition class, provides an entity for the datamodel to manage Compensation requisitions.
    /// </summary>
    public partial class PimsCompensationRequisition : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        [NotMapped]
        public override long Internal_Id { get => this.CompensationRequisitionId; set => this.CompensationRequisitionId = value; }

        [NotMapped]
        public decimal PayeeChequesPreTaxTotalAmount => (decimal)PimsCompReqH120s.Sum(x => x.PretaxAmt);

        [NotMapped]
        public decimal PayeeChequesTaxTotalAmount => (decimal)PimsCompReqH120s.Sum(x => x.TaxAmt);

        [NotMapped]
        public decimal PayeeChequesTotalAmount => (decimal)PimsCompReqH120s.Sum(x => x.TotalAmt);
    }
}
