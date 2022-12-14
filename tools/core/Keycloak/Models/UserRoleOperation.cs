namespace Pims.Tools.Core.Keycloak.Models
{
    /// <summary>
    /// UserRoleOperation class, provides a model to represent a keycloak user.
    /// </summary>
    public class UserRoleOperation
    {
        #region Properties

        /// <summary>
        /// get/set - A unique role name to identify the role in keycloak.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// get/set - A unique username to identify the user in keycloak.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// get/set - An operation name, either add or remove.
        /// </summary>
        public string Operation { get; set; }
        #endregion
    }
}
