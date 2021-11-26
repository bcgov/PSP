using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsSecDepHolderType
    {
        public PimsSecDepHolderType()
        {
            PimsSecurityDeposits = new HashSet<PimsSecurityDeposit>();
        }

        public string SecDepHolderTypeCode { get; set; }
        public string Description { get; set; }
        public bool? IsDisabled { get; set; }
        public int? DisplayOrder { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }

        public virtual ICollection<PimsSecurityDeposit> PimsSecurityDeposits { get; set; }
    }
}
