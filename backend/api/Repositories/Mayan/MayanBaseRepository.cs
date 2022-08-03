using System;
using System.Collections.Generic;
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
        protected readonly MayanConfig _config;
        private const string MayanConfigSectionKey = "Mayan";
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
            configuration.Bind(MayanConfigSectionKey, _config);
        }

        protected async Task<ExternalResult<T>> GetAsync<T>(Uri endpoint, string authenticationToken)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            if (!string.IsNullOrEmpty(authenticationToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            try
            {
                HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(true);
                var result = await ProcessResponse<T>(response);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("Unexpected exception during Get {e}", e);
                return new ExternalResult<T>()
                {
                    Status = ExternalResultStatus.Error,
                    Message = "Exception during Get",
                };
            }
        }

        protected async Task<ExternalResult<T>> PostAsync<T>(Uri endpoint, HttpContent content, string authenticationToken = null)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            if (!string.IsNullOrEmpty(authenticationToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            try
            {
                HttpResponseMessage response = await client.PostAsync(endpoint, content).ConfigureAwait(true);
                var result = await ProcessResponse<T>(response);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("Unexpected exception during post {e}", e);
                return new ExternalResult<T>()
                {
                    Status = ExternalResultStatus.Error,
                    Message = "Exception during Post",
                };
            }
        }

        protected async Task<ExternalResult<T>> PutAsync<T>(Uri endpoint, HttpContent content, string authenticationToken = null)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            if (!string.IsNullOrEmpty(authenticationToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            try
            {
                HttpResponseMessage response = await client.PutAsync(endpoint, content).ConfigureAwait(true);
                var result = await ProcessResponse<T>(response);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("Unexpected exception during put {e}", e);
                return new ExternalResult<T>()
                {
                    Status = ExternalResultStatus.Error,
                    Message = "Exception during Put",
                };
            }
        }

        protected async Task<ExternalResult<string>> DeleteAsync(Uri endpoint, string authenticationToken = null)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            if (!string.IsNullOrEmpty(authenticationToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            ExternalResult<string> result = new ExternalResult<string>()
            {
                Status = ExternalResultStatus.Error,
                Payload = endpoint.AbsolutePath,
            };

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(endpoint).ConfigureAwait(true);

                _logger.LogTrace("Response: {response}", response);

                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                result.HttpStatusCode = response.StatusCode;
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        _logger.LogTrace("Response payload: {payload}", payload);
                        result.Status = ExternalResultStatus.Success;
                        break;
                    case HttpStatusCode.NoContent:
                        result.Status = ExternalResultStatus.Success;
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
            catch (Exception e)
            {
                result.Status = ExternalResultStatus.Error;
                result.Message = "Exception during Delete";
                _logger.LogError("Unexpected exception during delete {e}", e);
            }
            return result;
        }

        protected Dictionary<string, string> GenerateQueryParams(string ordering = "", int? page = null, int? pageSize = null)
        {
            Dictionary<string, string> queryParams = new ();

            if (!string.IsNullOrEmpty(ordering))
            {
                queryParams["ordering"] = ordering;
            }
            if (page.HasValue)
            {
                queryParams["page"] = page.ToString();
            }
            if (pageSize.HasValue)
            {
                queryParams["page_size"] = pageSize.ToString();
            }

            return queryParams;
        }

        private async Task<ExternalResult<T>> ProcessResponse<T>(HttpResponseMessage response)
        {
            ExternalResult<T> result = new ExternalResult<T>()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogTrace("Response: {response}", response);
            string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            result.HttpStatusCode = response.StatusCode;
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
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
