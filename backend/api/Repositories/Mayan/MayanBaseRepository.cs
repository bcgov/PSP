using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Models;
using Pims.Api.Models.Config;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// MayanBaseRepository provides common methods to interact with the Mayan EDMS api.
    /// </summary>
    public abstract class MayanBaseRepository
    {
        private const string MayanConfigSectionKey = "Mayan";
        protected readonly MayanConfig _config;
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MayanBaseRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        protected MayanBaseRepository(
            ILogger logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _config = new MayanConfig();
            configuration.Bind(MayanConfigSectionKey, this._config);
        }

        public async Task<ExternalResult<T>> Get<T>(Uri endpoint, string authenticationToken)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            if (!string.IsNullOrEmpty(authenticationToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            ExternalResult<T> result = new ExternalResult<T>()
            {
                Status = ExternalResultStatus.Error,
            };

            try
            {
                HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(true);
                result = await ProcessResponse<T>(response);
            }
            catch (Exception e)
            {
                result.Status = ExternalResultStatus.Error;
                result.Message = "Exception during Get";
                this._logger.LogError("Unexpected exception during Get {e}", e);
            }
            return result;
        }

        public async Task<ExternalResult<T>> Post<T>(Uri endpoint, HttpContent content, string authenticationToken = null)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            if (!string.IsNullOrEmpty(authenticationToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            ExternalResult<T> result = new ExternalResult<T>()
            {
                Status = ExternalResultStatus.Error,
            };

            try
            {
                HttpResponseMessage response = await client.PostAsync(endpoint, content).ConfigureAwait(true);
                result = await ProcessResponse<T>(response);
            }
            catch (Exception e)
            {
                result.Status = ExternalResultStatus.Error;
                result.Message = "Exception during Post";
                this._logger.LogError("Unexpected exception during post {e}", e);
            }
            return result;
        }

        private async Task<ExternalResult<T>> ProcessResponse<T>(HttpResponseMessage response)
        {
            ExternalResult<T> result = new ExternalResult<T>()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogTrace("Response: {response}", response);
            string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    _logger.LogTrace("Response payload: {payload}", payload);
                    T requestTokenResult = JsonSerializer.Deserialize<T>(payload);
                    result.Status = ExternalResultStatus.Success;
                    result.Payload = requestTokenResult;
                    break;
                case HttpStatusCode.NoContent:
                    result.Status = ExternalResultStatus.Error;
                    result.Message = "No content was returned from the call";
                    break;
                case HttpStatusCode.Forbidden:
                    result.Status = ExternalResultStatus.Error;
                    result.Message = "Request was forbidden";
                    break;
                case HttpStatusCode.BadRequest:
                    result.Status = ExternalResultStatus.Error;
                    result.Message = payload;
                    break;
                default:
                    result.Status = ExternalResultStatus.Error;
                    result.Message = $"Unable to contact endpoint {response.RequestMessage.RequestUri}. Http status {response.StatusCode}";
                    break;
            }
            return result;
        }
    }
}
