namespace Pims.Api.Areas.Keycloak.Models.User.Update
{
    /// <summary>
    /// UserOrganizationModel class, provides a model to represent a user organization.
    /// </summary>
    public class UserOrganizationModel
    {
        #region Properties

        /// <summary>
        /// get/set - The unique identify for the organization.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The unique name to identify the organization.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
