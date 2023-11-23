using Pims.Api.Concepts.Models.Base;
using Pims.Api.Concepts.Models.Concepts.Role;

namespace Pims.Api.Concepts.Models.Concepts.User
{
    public class UserRoleModel : BaseAuditModel
    {
        #region Properties
        public long Id { get; set; }

        public long UserId { get; set; }

        public long RoleId { get; set; }

        public UserModel User { get; set; }

        public RoleModel Role { get; set; }
        #endregion
    }
}
