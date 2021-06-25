namespace Pims.Api.Models.User
{
    /// <summary>
    /// AccessRequestRoleModel class, provides a model that represents a role attached to an access request.
    /// </summary>
    public class AccessRequestRoleModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify role.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The item's name.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
