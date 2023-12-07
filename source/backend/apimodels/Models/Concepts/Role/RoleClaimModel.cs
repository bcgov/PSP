using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Claim;

namespace Pims.Api.Models.Concepts.Role
{
    public class RoleClaimModel : BaseAuditModel
    {
        public long Id { get; set; }

        public long RoleId { get; set; }

        public RoleModel Role { get; set; }

        public long ClaimId { get; set; }

        public ClaimModel Claim { get; set; }
    }
}
