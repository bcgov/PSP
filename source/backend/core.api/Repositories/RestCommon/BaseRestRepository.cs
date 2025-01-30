using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Requests.Http;

namespace Pims.Core.Api.Repositories.Rest
{
    /// <summary>
    /// BaseRestRepository provides common methods to interact with Rest-ful external interfaces.
    /// </summary>
    public abstract class BaseRestRepository : IRestRespository
    {
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly ILogger _logger;
        protected readonly IOptions<JsonSerializerOptions> _jsonOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRestRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        protected BaseRestRepository(
            ILogger logger,
            IHttpClientFactory httpClientFactory,
            IOptions<JsonSerializerOptions> jsonOptions)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _jsonOptions = jsonOptions;
        }

        public abstract void AddAuthentication(HttpClient client, string authenticationToken = null);

        public async Task<ExternalResponse<T>> GetAsync<T>(Uri endpoint, string authenticationToken)
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
                return new ExternalResponse<T>()
                {
                    Status = ExternalResponseStatus.Error,
                    Message = "Exception during Get",
                };
            }
        }

        public async Task<HttpResponseMessage> GetRawAsync(Uri endpoint, string authenticationToken)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            AddAuthentication(client, authenticationToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            try
            {
                HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(true);
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError("Unexpected exception during Get {e}", e);
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                };
            }
        }

        public async Task<ExternalResponse<T>> PostAsync<T>(Uri endpoint, HttpContent content, string authenticationToken = null)
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
                return new ExternalResponse<T>()
                {
                    Status = ExternalResponseStatus.Error,
                    Message = "Exception during Post",
                };
            }
        }

        public async Task<ExternalResponse<T>> PutAsync<T>(Uri endpoint, HttpContent content, string authenticationToken = null)
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
                return new ExternalResponse<T>()
                {
                    Status = ExternalResponseStatus.Error,
                    Message = "Exception during Put",
                };
            }
        }

        public async Task<ExternalResponse<string>> DeleteAsync(Uri endpoint, string authenticationToken = null)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            AddAuthentication(client, authenticationToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            ExternalResponse<string> result = new ExternalResponse<string>()
            {
                Status = ExternalResponseStatus.Error,
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
                    case HttpStatusCode.Accepted:
                        _logger.LogTrace("Response payload: {payload}", payload);
                        result.Status = ExternalResponseStatus.Success;
                        break;
                    case HttpStatusCode.NoContent:
                        result.Status = ExternalResponseStatus.Success;
                        result.Message = "No content was returned from the call";
                        break;
                    case HttpStatusCode.Forbidden:
                        result.Status = ExternalResponseStatus.Error;
                        result.Message = "Request was forbidden";
                        break;
                    case HttpStatusCode.BadRequest:
                        result.Status = ExternalResponseStatus.Error;
                        result.Message = payload;
                        break;
                    default:
                        result.Status = ExternalResponseStatus.Error;
                        result.Message = $"Unable to contact endpoint {response.RequestMessage?.RequestUri}. Http status {response.StatusCode}";
                        break;
                }
                return result;
            }
            catch (Exception e)
            {
                result.Status = ExternalResponseStatus.Error;
                result.Message = "Exception during Delete";
                _logger.LogError("Unexpected exception during delete {e}", e);
            }
            return result;
        }

        protected async Task<ExternalResponse<FileDownloadResponse>> ProcessDownloadResponse(HttpResponseMessage response)
        {
            ExternalResponse<FileDownloadResponse> result = new ExternalResponse<FileDownloadResponse>()
            {
                Status = ExternalResponseStatus.Error,
            };

            byte[] responsePayload = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(true);
            _logger.LogTrace("Response: {response}", response);
            response.Content.Headers.TryGetValues("Content-Length", out IEnumerable<string> contentLengthHeaders);
            long contentLength = responsePayload.Length;
            if (contentLengthHeaders?.FirstOrDefault() != null)
            {
                contentLength = int.Parse(contentLengthHeaders.FirstOrDefault(), CultureInfo.InvariantCulture);
            }
            result.HttpStatusCode = response.StatusCode;
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    string contentDisposition = response.Content.Headers.GetValues("Content-Disposition").FirstOrDefault();
                    string fileName = GetFileNameFromContentDisposition(contentDisposition);

                    result.Status = ExternalResponseStatus.Success;
                    result.Payload = new FileDownloadResponse()
                    {
                        FilePayload = Convert.ToBase64String(responsePayload),
                        Size = contentLength,
                        Mimetype = response.Content.Headers.GetValues("Content-Type").FirstOrDefault(),
                        FileName = fileName,
                        FileNameExtension = Path.GetExtension(fileName).Replace(".", string.Empty),
                        FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName),
                    };

                    break;
                case HttpStatusCode.NoContent:
                    result.Status = ExternalResponseStatus.Success;
                    result.Message = "No content found";
                    break;
                case HttpStatusCode.Forbidden:
                    result.Status = ExternalResponseStatus.Error;
                    result.Message = "Forbidden";
                    break;
                default:
                    result.Status = ExternalResponseStatus.Error;
                    result.Message = $"Unable to contact endpoint {response.RequestMessage?.RequestUri}. Http status {response.StatusCode}";
                    break;
            }

            return result;
        }

        protected async Task<ExternalResponse<FileStreamResponse>> ProcessStreamResponse(HttpResponseMessage response)
        {
            ExternalResponse<FileStreamResponse> result = new ExternalResponse<FileStreamResponse>()
            {
                Status = ExternalResponseStatus.Error,
            };

            Stream responsePayload = await response.Content.ReadAsStreamAsync().ConfigureAwait(true);
            _logger.LogTrace("Response: {response}", response);
            response.Content.Headers.TryGetValues("Content-Length", out IEnumerable<string> contentLengthHeaders);
            long contentLength = responsePayload.Length;
            if (contentLengthHeaders?.FirstOrDefault() != null)
            {
                contentLength = int.Parse(contentLengthHeaders.FirstOrDefault(), CultureInfo.InvariantCulture);
            }
            result.HttpStatusCode = response.StatusCode;
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    string contentDisposition = response.Content.Headers.GetValues("Content-Disposition").FirstOrDefault();
                    string fileName = GetFileNameFromContentDisposition(contentDisposition);

                    result.Status = ExternalResponseStatus.Success;
                    result.Payload = new FileStreamResponse()
                    {
                        FilePayload = responsePayload,
                        Size = contentLength,
                        Mimetype = response.Content.Headers.GetValues("Content-Type").FirstOrDefault(),
                        FileName = fileName,
                        FileNameExtension = Path.GetExtension(fileName).Replace(".", string.Empty),
                        FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName),
                    };

                    break;
                case HttpStatusCode.NoContent:
                    result.Status = ExternalResponseStatus.Success;
                    result.Message = "No content found";
                    break;
                case HttpStatusCode.Forbidden:
                    result.Status = ExternalResponseStatus.Error;
                    result.Message = "Forbidden";
                    break;
                default:
                    result.Status = ExternalResponseStatus.Error;
                    result.Message = $"Unable to contact endpoint {response.RequestMessage.RequestUri}. Http status {response.StatusCode}";
                    break;
            }

            return result;
        }

        private static string GetFileNameFromContentDisposition(string contentDisposition)
        {
            const string fileNameFlag = "filename";
            string[] parts = contentDisposition.Split("; ");
            string fileNamePart = parts.FirstOrDefault(x => x.Contains(fileNameFlag));
            return fileNamePart[(fileNameFlag.Length + 1) ..].Replace("\"", string.Empty);
        }

        private async Task<ExternalResponse<T>> ProcessResponse<T>(HttpResponseMessage response)
        {
            ExternalResponse<T> result = new ExternalResponse<T>()
            {
                Status = ExternalResponseStatus.Error,
            };

            _logger.LogTrace("Response: {response}", response);
            var payload = await response.Content.ReadAsStreamAsync().ConfigureAwait(true);
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
                            if(payload.Length > 0)
                            {
                                result.Payload = (T)Convert.ChangeType(payload, typeof(T), CultureInfo.InvariantCulture);
                            }
                            break;
                        default:
                            T requestTokenResult = JsonSerializer.Deserialize<T>(payload, _jsonOptions.Value);
                            result.Payload = requestTokenResult;
                            break;
                    }
                    result.Status = ExternalResponseStatus.Success;
                    break;
                case HttpStatusCode.NoContent:
                    result.Status = ExternalResponseStatus.Success;
                    result.Message = "No content was returned from the call";
                    break;
                case HttpStatusCode.NotFound:
                    result.Status = ExternalResponseStatus.Error;
                    result.Message = "The requested resource does not exist on the server";
                    break;
                case HttpStatusCode.Forbidden:
                    result.Status = ExternalResponseStatus.Error;
                    result.Message = "Request was forbidden";
                    break;
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.MethodNotAllowed:
                    result.Status = ExternalResponseStatus.Error;
                    result.Message = await response.Content.ReadAsStringAsync();
                    break;
                default:
                    result.Status = ExternalResponseStatus.Error;
                    result.Message = $"Unable to contact endpoint {response.RequestMessage?.RequestUri}. Http status {response.StatusCode}";
                    break;
            }

            return result;
        }
    }
}
