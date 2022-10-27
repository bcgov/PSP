using Pims.Api.Models;

namespace Pims.Api.Areas.Keycloak.Models.User
{
    /// <summary>
    /// OrganizationModel class, provides a model to represent the organization.
    /// </summary>
    public class OrganizationModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify organization.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The organization name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - Foreign key to the parent organization.
        /// </summary>
        public long? ParentId { get; set; }
        #endregion
    }
}
