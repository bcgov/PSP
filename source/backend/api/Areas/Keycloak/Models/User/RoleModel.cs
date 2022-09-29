namespace Pims.Api.Areas.Keycloak.Models.User
{
    /// <summary>
    /// RoleModel class, provides a model that represents a role.
    /// </summary>
    public class RoleModel : Api.Models.BaseModel
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify the role.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The role description.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
