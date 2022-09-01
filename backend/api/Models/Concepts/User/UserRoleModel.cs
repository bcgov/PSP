namespace Pims.Api.Models.Concepts
{
    public class UserRoleModel : BaseAppModel
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
