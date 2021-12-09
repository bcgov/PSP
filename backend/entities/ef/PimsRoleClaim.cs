using System;

#nullable disable

namespace Pims.Dal.Entities
{
    public partial class PimsRoleClaim
    {
        public long RoleClaimId { get; set; }
        public long RoleId { get; set; }
        public long ClaimId { get; set; }
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

        public virtual PimsClaim Claim { get; set; }
        public virtual PimsRole Role { get; set; }
    }
}
