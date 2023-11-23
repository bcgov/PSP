using Pims.Api.Concepts.Models.Base;

namespace Pims.Api.Areas.Keycloak.Models.AccessRequest
{
    /// <summary>
    /// OrganizationModel class, provides a model that represents a role attached to an access request.
    /// </summary>
    public class OrganizationModel : BaseAuditModel
    {
        #region Properties

        /// <summary>
        /// get/set - Primary key to identify organization.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The name of the code.
        /// </summary>
        public string Name { get; set; }
        #endregion
    }
}
