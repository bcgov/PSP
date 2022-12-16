using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Http.Configuration;
using Pims.Tools.Core;
using Pims.Tools.Core.Configuration;
using Pims.Tools.Core.Keycloak.Configuration;
using Pims.Tools.Keycloak.Sync.Configuration;

namespace Pims.Tools.Keycloak.Sync
{

    /// <summary>
    /// KeycloakRequestClient class, provides a way to make HTTP requests, handle errors and handle refresh tokens.
    /// </summary>
    public class PimsRequestClient : RequestClient, IPimsRequestClient
    {
        #region Variables
        private readonly ToolOptions _options;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an KeycloakRequestClient class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="clientFactory"></param>
        /// <param name="tokenHandler"></param>
        /// <param name="keycloakOptions"></param>
        /// <param name="openIdConnectOptions"></param>
        /// <param name="options"></param>
        /// <param name="requestOptions"></param>
        /// <param name="serializerOptions"></param>
        /// <param name="logger"></param>
        public PimsRequestClient(
            IHttpClientFactory clientFactory,
            JwtSecurityTokenHandler tokenHandler,
            IOptionsMonitor<KeycloakOptions> keycloakOptions,
            IOptionsMonitor<OpenIdConnectOptions> openIdConnectOptions,
            IOptionsMonitor<ToolOptions> options,
            IOptionsMonitor<RequestOptions> requestOptions,
            IOptionsMonitor<JsonSerializerOptions> serializerOptions,
            ILogger<RequestClient> logger)
            : base(clientFactory, tokenHandler, keycloakOptions, openIdConnectOptions, requestOptions, serializerOptions, logger)
        {
            this.OpenIdConnectOptions.Token = $"{keycloakOptions.CurrentValue.Authority}{this.OpenIdConnectOptions.Token}";
            _options = options.CurrentValue;
        }

        // Override the url by providing the base url from the pims api setting.
        public override Task<HttpResponseMessage> SendAsync(string url, HttpMethod method = null, HttpContent content = null)
        {
            return base.SendAsync($"{_options.Api.Uri}/{url}", method, content);
        }

        public override Task<HttpResponseMessage> SendJsonAsync<T>(string url, HttpMethod method = null, T data = null)
        {
            return base.SendJsonAsync($"{_options.Api.Uri}/{url}", method, data);
        }
        #endregion
    }
}
