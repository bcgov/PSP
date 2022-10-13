namespace Pims.Api.Models.Concepts
{
    public class RoleClaimModel : BaseAppModel
    {
        public long Id { get; set; }

        public long RoleId { get; set; }

        public RoleModel Role { get; set; }

        public long ClaimId { get; set; }

        public ClaimModel Claim { get; set; }
    }
}
