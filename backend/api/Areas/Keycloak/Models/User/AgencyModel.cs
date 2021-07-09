using Pims.Api.Models;

namespace Pims.Api.Areas.Keycloak.Models.User
{
    /// <summary>
    /// AgencyModel class, provides a model to represent the agency.
    /// </summary>
    public class AgencyModel : BaseAppModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify agency.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The agency name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// get/set - Foreign key to the parent agency.
        /// </summary>
        public long? ParentId { get; set; }
        #endregion
    }
}
