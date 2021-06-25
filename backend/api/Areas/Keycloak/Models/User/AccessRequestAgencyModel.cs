namespace Pims.Api.Areas.Keycloak.Models.User
{
    /// <summary>
    /// AccessRequestAgencyModel class, provides a model that represents a role attached to an access request.
    /// </summary>
    public class AccessRequestAgencyModel : Api.Models.BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify agency.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The name of the code.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
