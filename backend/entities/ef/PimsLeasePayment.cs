using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsLeasePayment
    {
        public long LeasePaymentId { get; set; }
        public string LeaseTermId { get; set; }
        public long LeasePaymentPeriodId { get; set; }
        public string LeasePaymentMethodTypeCode { get; set; }
        public DateTime PaymentReceivedDate { get; set; }
        public decimal PaymentAmountPreTax { get; set; }
        public decimal PaymentAmountPst { get; set; }
        public decimal PaymentAmountGst { get; set; }
        public decimal PaymentAmountTotal { get; set; }
        public string Note { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserid { get; set; }
        public Guid? AppCreateUserGuid { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public Guid? AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }

        public virtual PimsLeasePaymentMethodType LeasePaymentMethodTypeCodeNavigation { get; set; }
        public virtual PimsLeasePaymentPeriod LeasePaymentPeriod { get; set; }
        public virtual PimsLeaseTerm LeaseTerm { get; set; }
    }
}
