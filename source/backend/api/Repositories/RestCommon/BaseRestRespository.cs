using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pims.Api.Models;

namespace Pims.Api.Repositories.Rest
{
    /// <summary>
    /// BaseRestRepository provides common methods to interact with Rest-ful external interfaces.
    /// </summary>
    public abstract class BaseRestRepository : IRestRespository
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRestRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        protected BaseRestRepository(
            ILogger logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public abstract void AddAuthentication(HttpClient client, string authenticationToken = null);

        public async Task<ExternalResult<T>> GetAsync<T>(Uri endpoint, string authenticationToken)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            AddAuthentication(client, authenticationToken);
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

        public async Task<ExternalResult<T>> PostAsync<T>(Uri endpoint, HttpContent content, string authenticationToken = null)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            AddAuthentication(client, authenticationToken);
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

        public async Task<ExternalResult<T>> PutAsync<T>(Uri endpoint, HttpContent content, string authenticationToken = null)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            AddAuthentication(client, authenticationToken);
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

        public async Task<ExternalResult<string>> DeleteAsync(Uri endpoint, string authenticationToken = null)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            AddAuthentication(client, authenticationToken);
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

                    // If the expected return type is a string don't attempt to deserialize.
                    switch (Type.GetTypeCode(typeof(T)))
                    {
                        case TypeCode.String:
                            result.Payload = (T)Convert.ChangeType(payload, typeof(T));
                            break;
                        default:
                            T requestTokenResult = JsonSerializer.Deserialize<T>(payload);
                            result.Payload = requestTokenResult;
                            break;
                    }
                    result.Status = ExternalResultStatus.Success;
                    break;
                case HttpStatusCode.NoContent:
                    result.Status = ExternalResultStatus.Error;
                    result.Message = "No content was returned from the call";
                    break;
                case HttpStatusCode.NotFound:
                    result.Status = ExternalResultStatus.Error;
                    result.Message = "The requested resource does not exist on the server";
                    break;
                case HttpStatusCode.Forbidden:
                    result.Status = ExternalResultStatus.Error;
                    result.Message = "Request was forbidden";
                    break;
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.MethodNotAllowed:
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
