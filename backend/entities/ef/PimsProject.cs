using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsProject
    {
        public PimsProject()
        {
            PimsActivities = new HashSet<PimsActivity>();
            PimsProjectNotes = new HashSet<PimsProjectNote>();
            PimsProjectProperties = new HashSet<PimsProjectProperty>();
            PimsProjectWorkflowModels = new HashSet<PimsProjectWorkflowModel>();
        }

        public long ProjectId { get; set; }
        public string ProjectTypeCode { get; set; }
        public string ProjectStatusTypeCode { get; set; }
        public string ProjectRiskTypeCode { get; set; }
        public string ProjectTierTypeCode { get; set; }
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

        public virtual PimsProjectRiskType ProjectRiskTypeCodeNavigation { get; set; }
        public virtual PimsProjectStatusType ProjectStatusTypeCodeNavigation { get; set; }
        public virtual PimsProjectTierType ProjectTierTypeCodeNavigation { get; set; }
        public virtual PimsProjectType ProjectTypeCodeNavigation { get; set; }
        public virtual ICollection<PimsActivity> PimsActivities { get; set; }
        public virtual ICollection<PimsProjectNote> PimsProjectNotes { get; set; }
        public virtual ICollection<PimsProjectProperty> PimsProjectProperties { get; set; }
        public virtual ICollection<PimsProjectWorkflowModel> PimsProjectWorkflowModels { get; set; }
    }
}
