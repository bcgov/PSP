using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsUser
    {
        public PimsUser()
        {
            PimsAccessRequests = new HashSet<PimsAccessRequest>();
            PimsTasks = new HashSet<PimsTask>();
            PimsUserOrganizations = new HashSet<PimsUserOrganization>();
            PimsUserRoles = new HashSet<PimsUserRole>();
        }

        public long UserId { get; set; }
        public long PersonId { get; set; }
        public string BusinessIdentifierValue { get; set; }
        public Guid? GuidIdentifierValue { get; set; }
        public string Position { get; set; }
        public string Note { get; set; }
        public DateTime? LastLogin { get; set; }
        public string ApprovedById { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
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

        public virtual PimsPerson Person { get; set; }
        public virtual ICollection<PimsAccessRequest> PimsAccessRequests { get; set; }
        public virtual ICollection<PimsTask> PimsTasks { get; set; }
        public virtual ICollection<PimsUserOrganization> PimsUserOrganizations { get; set; }
        public virtual ICollection<PimsUserRole> PimsUserRoles { get; set; }
    }
}
