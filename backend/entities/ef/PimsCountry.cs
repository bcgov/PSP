using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsCountry
    {
        public PimsCountry()
        {
            PimsAddresses = new HashSet<PimsAddress>();
            PimsProvinceStates = new HashSet<PimsProvinceState>();
        }

        public short CountryId { get; set; }
        public string CountryCode { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }

        public virtual ICollection<PimsAddress> PimsAddresses { get; set; }
        public virtual ICollection<PimsProvinceState> PimsProvinceStates { get; set; }
    }
}
