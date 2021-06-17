namespace Pims.Api.Areas.Keycloak.Models.User
{
    /// <summary>
    /// AgencyModel class, provides a model to represent the agency.
    /// </summary>
    public class AgencyModel : Api.Models.CodeModel
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify agency.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The agency description.
        /// </summary>
        public string Description { get; set; }
        #endregion
    }
}
