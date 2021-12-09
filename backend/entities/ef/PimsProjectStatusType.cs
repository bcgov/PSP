using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsProjectStatusType
    {
        public PimsProjectStatusType()
        {
            PimsProjects = new HashSet<PimsProject>();
        }

        public string ProjectStatusTypeCode { get; set; }
        public string CodeGroup { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public bool? IsMilestone { get; set; }
        public bool? IsTerminal { get; set; }
        public bool? IsDisabled { get; set; }
        public int? DisplayOrder { get; set; }
        public long ConcurrencyControlNumber { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string DbCreateUserid { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserid { get; set; }

        public virtual ICollection<PimsProject> PimsProjects { get; set; }
    }
}
