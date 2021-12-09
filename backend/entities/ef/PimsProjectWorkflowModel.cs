using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsProjectWorkflowModel
    {
        public PimsProjectWorkflowModel()
        {
            PimsActivities = new HashSet<PimsActivity>();
        }

        public long ProjectWorkflowModelId { get; set; }
        public long ProjectId { get; set; }
        public long WorkflowModelId { get; set; }
        public bool? IsDisabled { get; set; }
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

        public virtual PimsProject Project { get; set; }
        public virtual PimsWorkflowModel WorkflowModel { get; set; }
        public virtual ICollection<PimsActivity> PimsActivities { get; set; }
    }
}
