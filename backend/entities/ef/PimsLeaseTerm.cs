using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsLeaseTerm
    {
        public PimsLeaseTerm()
        {
            PimsLeasePaymentForecasts = new HashSet<PimsLeasePaymentForecast>();
            PimsLeasePayments = new HashSet<PimsLeasePayment>();
        }

        public string LeaseTermId { get; set; }
        public long LeaseId { get; set; }
        public string LeaseTermStatusTypeCode { get; set; }
        public DateTime TermStartDate { get; set; }
        public DateTime? TermExpiryDate { get; set; }
        public DateTime? TermRenewalDate { get; set; }
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

        public virtual PimsLease Lease { get; set; }
        public virtual PimsLeaseTermStatusType LeaseTermStatusTypeCodeNavigation { get; set; }
        public virtual ICollection<PimsLeasePaymentForecast> PimsLeasePaymentForecasts { get; set; }
        public virtual ICollection<PimsLeasePayment> PimsLeasePayments { get; set; }
    }
}
