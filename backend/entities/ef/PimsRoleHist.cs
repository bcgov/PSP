using System;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsRoleHist
    {
        public long RoleHistId { get; set; }
        public DateTime EffectiveDateHist { get; set; }
        public DateTime? EndDateHist { get; set; }
        public long RoleId { get; set; }
        public Guid RoleUid { get; set; }
        public Guid? KeycloakGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDisabled { get; set; }
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
    }
}
