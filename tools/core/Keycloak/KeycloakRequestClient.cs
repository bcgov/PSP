using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Http.Configuration;
using Pims.Tools.Core.Configuration;
using Pims.Tools.Core.Keycloak.Configuration;

namespace Pims.Tools.Core.Keycloak
{

    /// <summary>
    /// KeycloakRequestClient class, provides a way to make HTTP requests to the keycloak management api provided by sso gold.
    /// </summary>
    public class KeycloakRequestClient : RequestClient, IKeycloakRequestClient
    {
        #region Variables
        private readonly KeycloakManagementOptions _keycloakManagementOptions;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an KeycloakRequestClient class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="clientFactory"></param>
        /// <param name="tokenHandler"></param>
        /// <param name="keycloakManagementOptions"></param>
        /// <param name="openIdConnectOptions"></param>
        /// <param name="requestOptions"></param>
        /// <param name="serializerOptions"></param>
        /// <param name="logger"></param>
        public KeycloakRequestClient(
            IHttpClientFactory clientFactory,
            JwtSecurityTokenHandler tokenHandler,
            IOptionsMonitor<KeycloakManagementOptions> keycloakManagementOptions,
            IOptionsMonitor<OpenIdConnectOptions> openIdConnectOptions,
            IOptionsMonitor<RequestOptions> requestOptions,
            IOptionsMonitor<JsonSerializerOptions> serializerOptions,
            ILogger<RequestClient> logger)
            : base(clientFactory, tokenHandler, keycloakManagementOptions, openIdConnectOptions, requestOptions, serializerOptions, logger)
        {
            this.OpenIdConnectOptions.Token = $"{keycloakManagementOptions.CurrentValue.Authority}{this.OpenIdConnectOptions.Token}";
            _keycloakManagementOptions = keycloakManagementOptions.CurrentValue;
        }
        #endregion

        #region Methods

        // Override the provided url with the base url provided with for the keycloak management api.
        public override Task<HttpResponseMessage> SendAsync(string url, HttpMethod method = null, HttpContent content = null)
        {
            return base.SendAsync($"{_keycloakManagementOptions.Api}/{url}", method, content);
        }

        public override Task<HttpResponseMessage> SendJsonAsync<T>(string url, HttpMethod method = null, T data = null)
        {
            return base.SendJsonAsync($"{_keycloakManagementOptions.Api}/{url}", method, data);
        }

        // Get the integrations endpoint for a specific environment provided by the appsettings file.
        public string GetIntegrationEnvUri()
        {
            return $"integrations/{_keycloakManagementOptions.Integration}/{_keycloakManagementOptions.Environment}";
        }

        // Get just the integration endpoint provided by the appsettings file.
        public string GetIntegrationUri()
        {
            return $"integrations/{_keycloakManagementOptions.Integration}";
        }

        // Get just the environment from the appsettings file.
        public string GetEnvUri()
        {
            return $"{_keycloakManagementOptions.Environment}";
        }
        #endregion
    }
}
