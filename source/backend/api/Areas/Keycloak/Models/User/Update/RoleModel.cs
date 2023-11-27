using Pims.Api.Models.Base;

namespace Pims.Api.Areas.Keycloak.Models.User.Update
{
    /// <summary>
    /// RoleModel class, provides a model that represents a role.
    /// </summary>
    public class RoleModel : BaseAuditModel
    {
        #region Properties

        /// <summary>
        /// get/set - A unique name to identify the role.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - The role description.
        /// </summary>
        public string Description { get; set; }
        #endregion
    }
}
