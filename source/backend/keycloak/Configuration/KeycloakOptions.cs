using Pims.Core.Exceptions;
using Pims.Core.Http.Configuration;

namespace Pims.Keycloak.Configuration
{
    /// <summary>
    /// KeycloakOptions class, provides a way to configure keycloak.
    /// </summary>
    public class KeycloakOptions : AuthClientOptions
    {
        #region Properties

        /// <summary>
        /// get/set - The keycloak service account configuration.
        /// This is for authenticating the api interaction with keycloak.
        /// </summary>
        public KeycloakServiceAccountOptions ServiceAccount { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the configuration for keycloak.
        /// </summary>
        /// <exception type="ConfigurationException">If the configuration property is invald.</exception>
        public override void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Authority))
            {
                throw new ConfigurationException("The configuration for Keycloak:Authority is invalid or missing.");
            }

            if (string.IsNullOrWhiteSpace(this.Audience))
            {
                throw new ConfigurationException("The configuration for Keycloak:Audience is invalid or missing.");
            }

            if (string.IsNullOrWhiteSpace(this.Client))
            {
                throw new ConfigurationException("The configuration for Keycloak:Client is invalid or missing.");
            }

            if (this.ServiceAccount == null)
            {
                throw new ConfigurationException("The configuration for Keycloak:ServiceAccount is invalid or missing.");
            }
        }
        #endregion
    }
}
