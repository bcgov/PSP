using System;
using System.Collections.Generic;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsRole
    {
        public PimsRole()
        {
            PimsAccessRequests = new HashSet<PimsAccessRequest>();
            PimsRoleClaims = new HashSet<PimsRoleClaim>();
            PimsUserOrganizations = new HashSet<PimsUserOrganization>();
            PimsUserRoles = new HashSet<PimsUserRole>();
        }

        public long RoleId { get; set; }
        public Guid RoleUid { get; set; }
        public Guid? KeycloakGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsDisabled { get; set; }
        public int SortOrder { get; set; }
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

        public virtual ICollection<PimsAccessRequest> PimsAccessRequests { get; set; }
        public virtual ICollection<PimsRoleClaim> PimsRoleClaims { get; set; }
        public virtual ICollection<PimsUserOrganization> PimsUserOrganizations { get; set; }
        public virtual ICollection<PimsUserRole> PimsUserRoles { get; set; }
    }
}
