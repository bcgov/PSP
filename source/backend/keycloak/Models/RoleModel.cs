namespace Pims.Keycloak.Models
{
    /// <summary>
    /// RoleModel class, provides a way to manage roles within keycloak.
    /// </summary>
    public class RoleModel
    {
        #region Properties

        /// <summary>
        /// get/set - The unique name for this role.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - whether or not this role is a composite role.
        /// </summary>
        public bool Composite { get; set; }
        #endregion
    }
}
