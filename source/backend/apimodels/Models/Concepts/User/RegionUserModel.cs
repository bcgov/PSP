using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.User
{
    public class RegionUserModel : BaseAuditModel
    {
        public long Id { get; set; }

        public UserModel User { get; set; }

        public long UserId { get; set; }

        public CodeTypeModel<short> Region { get; set; }

        public short RegionCode { get; set; }
    }
}
