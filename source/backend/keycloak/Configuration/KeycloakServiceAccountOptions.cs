using Pims.Core.Exceptions;
using Pims.Core.Http.Configuration;

namespace Pims.Keycloak.Configuration
{
    /// <summary>
    /// KeycloakServiceAccountOptions class, provides a way to configure keycloak service account.
    /// </summary>
    public class KeycloakServiceAccountOptions : AuthClientOptions
    {
        #region

        /// <summary>
        /// get/set - The uri of the sso gold keycloak rest api.
        /// </summary>
        public string Api { get; set; }

        /// <summary>
        /// get/set - The name of the sso gold integration that should be used for all service account management.
        /// </summary>
        public string Integration { get; set; }

        /// <summary>
        /// get/set - The name of the sso gold environment that should be used for all service account management.
        /// </summary>
        public string Environment { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Validate the configuration.
        /// </summary>
        /// <exception type="ConfigurationException">The configuration is missing or invalid.</exception>
        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Client))
            {
                throw new ConfigurationException("The configuration for Keycloak:ServiceAccount:Client is invalid or missing.");
            }

            if (string.IsNullOrWhiteSpace(this.Secret))
            {
                throw new ConfigurationException("The configuration for Keycloak:ServiceAccount:Secret is invalid or missing.");
            }

            if (string.IsNullOrWhiteSpace(this.Api))
            {
                throw new ConfigurationException("The configuration for Keycloak:ServiceAccount:Api is invalid or missing.");
            }

            if (string.IsNullOrWhiteSpace(this.Integration))
            {
                throw new ConfigurationException("The configuration for Keycloak:ServiceAccount:Integration is invalid or missing.");
            }

            if (string.IsNullOrWhiteSpace(this.Environment))
            {
                throw new ConfigurationException("The configuration for Keycloak:ServiceAccount:Environment is invalid or missing.");
            }
        }
        #endregion
    }
}
