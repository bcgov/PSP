using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsLeasePaymentMethodType
    {
        public PimsLeasePaymentMethodType()
        {
            PimsLeasePayments = new HashSet<PimsLeasePayment>();
        }

        public string LeasePaymentMethodTypeCode { get; set; }
        public string Description { get; set; }
        public bool? IsDisabled { get; set; }
        public int? DisplayOrder { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }

        public virtual ICollection<PimsLeasePayment> PimsLeasePayments { get; set; }
    }
}
