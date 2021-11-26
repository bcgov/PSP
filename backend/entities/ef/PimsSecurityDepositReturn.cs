using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsSecurityDepositReturn
    {
        public long SecurityDepositReturnId { get; set; }
        public long LeaseId { get; set; }
        public string SecurityDepositTypeCode { get; set; }
        public DateTime TerminationDate { get; set; }
        public decimal DepositTotal { get; set; }
        public decimal? ClaimsAgainst { get; set; }
        public decimal ReturnAmount { get; set; }
        public DateTime ReturnDate { get; set; }
        public string ChequeNumber { get; set; }
        public string PayeeName { get; set; }
        public string PayeeAddress { get; set; }
        public string TerminationNote { get; set; }
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
        public virtual PimsSecurityDepositType SecurityDepositTypeCodeNavigation { get; set; }
    }
}
