using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Exceptions;
using Pims.Core.Http.Configuration;
using Pims.Tools.Keycloak.Sync.Configuration;
using Polly.Registry;

namespace Pims.Tools.Keycloak.Sync
{
    /// <summary>
    /// RequestClient class, provides a way to make HTTP requests, handle errors and handle refresh tokens.
    /// </summary>
    public class RequestClient : Pims.Core.Http.OpenIdConnectRequestClient, IRequestClient
    {
        #region Variables
        private readonly RequestOptions _requestOptions;
        private readonly ILogger _logger;
        private readonly JsonSerializerOptions _serializerOptions;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an RequestClient class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="clientFactory"></param>
        /// <param name="tokenHandler"></param>
        /// <param name="authClientOptions"></param>
        /// <param name="openIdConnectOptions"></param>
        /// <param name="requestOptions"></param>
        /// <param name="serializerOptions"></param>
        /// <param name="logger"></param>
        /// <param name="pollyPipelineProvider">The polly retry policy.</param>
        public RequestClient(
            IHttpClientFactory clientFactory,
            JwtSecurityTokenHandler tokenHandler,
            IOptionsMonitor<AuthClientOptions> authClientOptions,
            IOptionsMonitor<OpenIdConnectOptions> openIdConnectOptions,
            IOptionsMonitor<RequestOptions> requestOptions,
            IOptionsMonitor<JsonSerializerOptions> serializerOptions,
            ILogger<RequestClient> logger,
            ResiliencePipelineProvider<string> pollyPipelineProvider)
            : base(clientFactory, tokenHandler, authClientOptions, openIdConnectOptions, serializerOptions, logger, pollyPipelineProvider)
        {
            _requestOptions = requestOptions.CurrentValue;
            _logger = logger;
            _serializerOptions = serializerOptions?.CurrentValue ?? new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }
        #endregion

        #region Methods

        /// <summary>
        /// Send an HTTP GET request.
        /// Deserialize the result into the specified 'TR' type.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public async Task<TR> HandleGetAsync<TR>(string url, Func<HttpResponseMessage, bool> onError = null)
            where TR : class
        {
            var response = await SendAsync(url, HttpMethod.Get);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                try
                {
                    return await JsonSerializer.DeserializeAsync<TR>(stream, _serializerOptions);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }

            // If the error handle is not provided, or if it returns false throw an error.
            if ((onError?.Invoke(response) ?? false) == false)
            {
                throw new HttpClientRequestException(response);
            }

            return null;
        }

        /// <summary>
        /// Send an HTTP request.
        /// Deserialize the result into the specified 'TR' type.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public virtual async Task<TR> HandleRequestAsync<TR>(HttpMethod method, string url, Func<HttpResponseMessage, bool> onError = null)
            where TR : class
        {
            var response = await SendAsync(url, method);

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<TR>(stream, _serializerOptions);
            }

            // If the error handle is not provided, or if it returns false throw an error.
            if ((onError?.Invoke(response) ?? false) == false)
            {
                throw new HttpClientRequestException(response);
            }

            return null;
        }
        #endregion
    }
}
