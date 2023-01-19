using Pims.Core.Exceptions;

namespace Pims.Core.Http.Configuration
{
    /// <summary>
    /// OpenIdConnectOptions class, provides a way to configure keycloak open id connect endpoints.
    /// </summary>
    public class OpenIdConnectOptions
    {
        #region Properties

        /// <summary>
        /// get/set - The login authority uri.
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// get/set - The login endpoint path.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// get/set - The logout endpoint path.
        /// </summary>
        public string Logout { get; set; }

        /// <summary>
        /// get/set - The register endpoint path.
        /// </summary>
        public string Register { get; set; }

        /// <summary>
        /// get/set - The token endpoint path.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// get/set - The token introspect path.
        /// </summary>
        public string TokenIntrospect { get; set; }

        /// <summary>
        /// get/set - the user info path.
        /// </summary>
        public string UserInfo { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Validate the configuration.
        /// </summary>
        /// <exception type="ConfigurationException">The configuration is missing or invalid.</exception>
        public virtual void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Token))
            {
                throw new ConfigurationException("The configuration for Keycloak:OpenIdConnect:Token is invalid or missing.");
            }

            if (string.IsNullOrWhiteSpace(this.UserInfo))
            {
                throw new ConfigurationException("The configuration for Keycloak:OpenIdConnect:UserInfo is invalid or missing.");
            }
        }
        #endregion
    }
}
