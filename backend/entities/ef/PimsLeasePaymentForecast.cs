using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsLeasePaymentForecast
    {
        public long LeasePaymentForecastId { get; set; }
        public long LeaseTermId { get; set; }
        public long LeasePaymentPeriodId { get; set; }
        public string LeasePaymentStatusTypeCode { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public decimal ForecastPaymentPreTax { get; set; }
        public decimal ForecastPaymentPst { get; set; }
        public decimal ForecastPaymentGst { get; set; }
        public decimal ForecastPaymentTotal { get; set; }
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

        public virtual PimsLeasePaymentPeriod LeasePaymentPeriod { get; set; }
        public virtual PimsLeasePaymentStatusType LeasePaymentStatusTypeCodeNavigation { get; set; }
        public virtual PimsLeaseTerm LeaseTerm { get; set; }
    }
}
