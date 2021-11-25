using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsDistrict
    {
        public PimsDistrict()
        {
            PimsAddresses = new HashSet<PimsAddress>();
            PimsOrganizations = new HashSet<PimsOrganization>();
            PimsProperties = new HashSet<PimsProperty>();
        }

        public short DistrictCode { get; set; }
        public short RegionCode { get; set; }
        public string DistrictName { get; set; }
        public bool? IsDisabled { get; set; }
        public int? DisplayOrder { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }

        public virtual PimsRegion RegionCodeNavigation { get; set; }
        public virtual ICollection<PimsAddress> PimsAddresses { get; set; }
        public virtual ICollection<PimsOrganization> PimsOrganizations { get; set; }
        public virtual ICollection<PimsProperty> PimsProperties { get; set; }
    }
}
