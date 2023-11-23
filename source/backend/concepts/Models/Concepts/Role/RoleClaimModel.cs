using Pims.Api.Concepts.Models.Base;
using Pims.Api.Concepts.Models.Concepts.Claim;

namespace Pims.Api.Concepts.Models.Concepts.Role
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
