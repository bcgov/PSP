using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsRegion
    {
        public PimsRegion()
        {
            PimsAddresses = new HashSet<PimsAddress>();
            PimsDistricts = new HashSet<PimsDistrict>();
            PimsOrganizations = new HashSet<PimsOrganization>();
            PimsProperties = new HashSet<PimsProperty>();
        }

        public short RegionCode { get; set; }
        public string RegionName { get; set; }
        public bool? IsDisabled { get; set; }
        public int? DisplayOrder { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }

        public virtual ICollection<PimsAddress> PimsAddresses { get; set; }
        public virtual ICollection<PimsDistrict> PimsDistricts { get; set; }
        public virtual ICollection<PimsOrganization> PimsOrganizations { get; set; }
        public virtual ICollection<PimsProperty> PimsProperties { get; set; }
    }
}
