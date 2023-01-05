using System.Collections.Generic;

namespace Pims.Keycloak.Models
{
    /// <summary>
    /// UserRoleModel class, provides a way to manage users within keycloak.
    /// </summary>
    public class UserRoleModel
    {
        #region Properties

        /// <summary>
        /// get/set - The list of users for this user role mapping.
        /// </summary>
        public IEnumerable<UserModel> Users { get; set; }

        /// <summary>
        /// get/set - The list of roles for this user role mapping.
        /// </summary>
        public IEnumerable<RoleModel> Roles { get; set; }
        #endregion
    }
}
