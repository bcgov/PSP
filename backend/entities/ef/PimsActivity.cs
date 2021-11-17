using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsActivity
    {
        public PimsActivity()
        {
            PimsPropertyActivities = new HashSet<PimsPropertyActivity>();
            PimsTasks = new HashSet<PimsTask>();
        }

        public long ActivityId { get; set; }
        public long? ProjectId { get; set; }
        public long? WorkflowId { get; set; }
        public long ActivityModelId { get; set; }
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

        public virtual PimsActivityModel ActivityModel { get; set; }
        public virtual PimsProject Project { get; set; }
        public virtual PimsProjectWorkflowModel Workflow { get; set; }
        public virtual ICollection<PimsPropertyActivity> PimsPropertyActivities { get; set; }
        public virtual ICollection<PimsTask> PimsTasks { get; set; }
    }
}
