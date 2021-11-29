using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsSecurityDeposit
    {
        public long SecurityDepositId { get; set; }
        public long LeaseId { get; set; }
        public string SecDepHolderTypeCode { get; set; }
        public string SecurityDepositTypeCode { get; set; }
        public string OtherDepHolderTypeDesc { get; set; }
        public string Description { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime DepositDate { get; set; }
        public decimal? AnnualInterestRate { get; set; }
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
        public virtual PimsSecDepHolderType SecDepHolderTypeCodeNavigation { get; set; }
        public virtual PimsSecurityDepositType SecurityDepositTypeCodeNavigation { get; set; }
    }
}
