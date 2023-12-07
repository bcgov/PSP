using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.Role;

namespace Pims.Api.Models.Concepts.User
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
