using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsProvinceState
    {
        public PimsProvinceState()
        {
            PimsAddresses = new HashSet<PimsAddress>();
        }

        public short ProvinceStateId { get; set; }
        public short CountryId { get; set; }
        public string ProvinceStateCode { get; set; }
        public string Description { get; set; }
        public bool? IsDisabled { get; set; }
        public int? DisplayOrder { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }

        public virtual PimsCountry Country { get; set; }
        public virtual ICollection<PimsAddress> PimsAddresses { get; set; }
    }
}
