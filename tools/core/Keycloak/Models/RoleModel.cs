namespace Pims.Tools.Core.Keycloak.Models
{
    /// <summary>
    /// RoleModel class, provides a model to represent a keycloak role.
    /// </summary>
    public class RoleModel
    {
        #region Properties

        /// <summary>
        /// get/set - A unique name to identify the role.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - true if this role has sub-roles.
        /// </summary>
        public bool? Composite { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a RoleModel class.
        /// </summary>
        public RoleModel()
        {
        }
        #endregion
    }
}
