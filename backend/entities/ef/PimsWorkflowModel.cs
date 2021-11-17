using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsWorkflowModel
    {
        public PimsWorkflowModel()
        {
            PimsProjectWorkflowModels = new HashSet<PimsProjectWorkflowModel>();
        }

        public long WorkflowModelId { get; set; }
        public string WorkflowModelTypeCode { get; set; }
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

        public virtual PimsWorkflowModelType WorkflowModelTypeCodeNavigation { get; set; }
        public virtual ICollection<PimsProjectWorkflowModel> PimsProjectWorkflowModels { get; set; }
    }
}
