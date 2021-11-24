using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsAccessRequest
    {
        public PimsAccessRequest()
        {
            PimsAccessRequestOrganizations = new HashSet<PimsAccessRequestOrganization>();
        }

        public long AccessRequestId { get; set; }
        public long UserId { get; set; }
        public long? RoleId { get; set; }
        public string AccessRequestStatusTypeCode { get; set; }
        public string Note { get; set; }
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

        public virtual PimsAccessRequestStatusType AccessRequestStatusTypeCodeNavigation { get; set; }
        public virtual PimsRole Role { get; set; }
        public virtual PimsUser User { get; set; }
        public virtual ICollection<PimsAccessRequestOrganization> PimsAccessRequestOrganizations { get; set; }
    }
}
