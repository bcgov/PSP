using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsActivityModel
    {
        public PimsActivityModel()
        {
            PimsActivities = new HashSet<PimsActivity>();
            PimsTaskTemplateActivityModels = new HashSet<PimsTaskTemplateActivityModel>();
        }

        public long ActivityModelId { get; set; }
        public string Description { get; set; }
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

        public virtual ICollection<PimsActivity> PimsActivities { get; set; }
        public virtual ICollection<PimsTaskTemplateActivityModel> PimsTaskTemplateActivityModels { get; set; }
    }
}
