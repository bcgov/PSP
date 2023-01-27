namespace Pims.Api.Areas.Keycloak.Models.AccessRequest
{
    /// <summary>
    /// RoleModel class, provides a model that represents a role attached to an access request.
    /// </summary>
    public class RoleModel : Api.Models.BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify record.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The item's name.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
