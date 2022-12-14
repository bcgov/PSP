using System.Collections.Generic;

namespace Pims.Tools.Core.Keycloak.Models
{
    /// <summary>
    /// UserModel class, provides a model to represent a keycloak user.
    /// </summary>
    public class UserRoleModel
    {
        #region Properties
        public IEnumerable<UserModel> Users { get; set; }

        public IEnumerable<RoleModel> Roles { get; set; }
        #endregion
    }
}
