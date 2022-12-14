using Pims.Core.Http.Configuration;

namespace Pims.Tools.Core.Keycloak.Configuration
{
    /// <summary>
    /// KeycloakManagementOptions class, provides a way to configure the connection to Keycloak.
    /// </summary>
    public class KeycloakManagementOptions : AuthClientOptions
    {
        #region Properties

        /// <summary>
        /// get/set - The keycloak api route.
        /// </summary>
        public string Api { get; set; }

        /// <summary>
        /// get/set - The api environment.
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// get/set - The integration id.
        /// </summary>
        public int Integration { get; set; }
        #endregion
    }
}
