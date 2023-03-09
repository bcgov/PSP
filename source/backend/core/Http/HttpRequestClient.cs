using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Exceptions;

namespace Pims.Core.Http
{
    /// <summary>
    /// HttpRequestClient class, provides a generic way to make HTTP requests to other services.
    /// </summary>
    public class HttpRequestClient : IHttpRequestClient
    {
        #region Variables
        private readonly JsonSerializerOptions _serializeOptions;
        private readonly ILogger<HttpRequestClient> _logger;
        #endregion

        #region Properties

        /// <summary>
        /// get - The HttpClient use to make requests.
        /// </summary>
        public HttpClient Client { get; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a HttpRequestClient class, initializes it with the specified arguments.
        /// </summary>
        /// <param name="clientFactory"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public HttpRequestClient(IHttpClientFactory clientFactory, IOptionsMonitor<JsonSerializerOptions> options, ILogger<HttpRequestClient> logger)
        {
            this.Client = clientFactory.CreateClient();
            _serializeOptions = options?.CurrentValue ?? new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
            _logger = logger;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Dispose managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Deserialize the specified 'response' into the specified type of 'TModel'.
        /// </summary>
        /// <typeparam name="TModel">The type of the response to return.</typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task<TModel> DeserializeAsync<TModel>(HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsByteArrayAsync();
            var contentType = response.Content.Headers.ContentType;
            try
            {
                if (contentType.MediaType.Contains("json", StringComparison.InvariantCultureIgnoreCase))
                {
                    return JsonSerializer.Deserialize<TModel>(data, _serializeOptions);
                }
            }
            catch (Exception ex)
            {
                var body = Encoding.Default.GetString(data);
                _logger.LogError(ex, "Failed to deserialize response: {body}", body);
                throw;
            }

            throw new HttpClientRequestException(response, $"Response must contain JSON but was '{contentType.MediaType}'.");
        }

        #region HttpResponseMessage Methods

        /// <summary>
        /// Send an HTTP request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <typeparam name="T">The type of the outgoing data object.</typeparam>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> SendJsonAsync<T>(string url, HttpMethod method = null, T data = null)
            where T : class
        {
            return await SendJsonAsync(url, method, null, data);
        }

        /// <summary>
        /// Send an HTTP request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> SendAsync(string url, HttpMethod method = null, HttpContent content = null)
        {
            return await SendAsync(url, method, null, content);
        }

        /// <summary>
        /// Send an HTTP request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="data"></param>
        /// <typeparam name="T">The type of the outgoing data object.</typeparam>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> SendJsonAsync<T>(string url, HttpMethod method, HttpRequestHeaders headers, T data = null)
            where T : class
        {
            if (data != null)
            {
                var json = JsonSerializer.Serialize(data, _serializeOptions);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                return await SendAsync(url, method, headers, content);
            }
            else
            {
                return await SendAsync(url, method, headers, null);
            }
        }

        /// <summary>
        /// Send an HTTP request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual Task<HttpResponseMessage> SendAsync(string url, HttpMethod method, HttpRequestHeaders headers, HttpContent content = null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException($"Argument '{nameof(url)}' must be a valid URL.");
            }

            return this.SendInternalAsync(url, method, headers, content);
        }

        /// <summary>
        /// GET request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await SendAsync(url, HttpMethod.Get);
        }

        /// <summary>
        /// POST request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <typeparam name="T">The type of the outgoing data object.</typeparam>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostJsonAsync<T>(string url, T data = null)
            where T : class
        {
            return await SendJsonAsync(url, HttpMethod.Post, data);
        }

        /// <summary>
        /// POST request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content = null)
        {
            return await SendAsync(url, HttpMethod.Post, content);
        }

        /// <summary>
        /// DELETE request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> DeleteAsync(string url, HttpContent content = null)
        {
            return await SendAsync(url, HttpMethod.Delete, content);
        }

        /// <summary>
        /// Send an HTTP request to the specified 'uri'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <typeparam name="T">The type of the outgoing data object.</typeparam>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> SendJsonAsync<T>(Uri url, HttpMethod method = null, T data = null)
            where T : class
        {
            return await SendJsonAsync(url, method, null, data);
        }

        /// <summary>
        /// Send an HTTP request to the specified 'uri'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> SendAsync(Uri url, HttpMethod method = null, HttpContent content = null)
        {
            return await SendAsync(url, method, null, content);
        }

        /// <summary>
        /// Send an HTTP request to the specified 'uri'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="data"></param>
        /// <typeparam name="T">The type of the outgoing data object.</typeparam>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> SendJsonAsync<T>(Uri url, HttpMethod method, HttpRequestHeaders headers, T data = null)
            where T : class
        {
            return await SendJsonAsync(url.OriginalString, method, headers, data);
        }

        /// <summary>
        /// Send an HTTP request to the specified 'uri'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual async Task<HttpResponseMessage> SendAsync(Uri url, HttpMethod method, HttpRequestHeaders headers, HttpContent content = null)
        {
            return await SendAsync(url.OriginalString, method, headers, content);
        }

        /// <summary>
        /// POST request to the specified 'uri'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(Uri url, HttpContent content = null)
        {
            return await SendAsync(url, HttpMethod.Post, content);
        }
        #endregion

        #region Serialization Methods

        /// <summary>
        /// Send an HTTP request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="onError"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <typeparam name="T">The type of the outgoing data object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public virtual async Task<TModel> SendJsonAsync<TModel, T>(string url, HttpMethod method = null, T data = null, Func<HttpResponseMessage, bool> onError = null)
            where T : class
        {
            return await SendJsonAsync<TModel, T>(url, method, null, data, onError);
        }

        /// <summary>
        /// Send an HTTP request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <param name="onError"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public virtual async Task<TModel> SendAsync<TModel>(string url, HttpMethod method = null, HttpContent content = null, Func<HttpResponseMessage, bool> onError = null)
        {
            return await SendAsync<TModel>(url, method, null, content, onError);
        }

        /// <summary>
        /// Send an HTTP request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="data"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <typeparam name="T">The type of the outgoing data object.</typeparam>
        /// <param name="onError"></param>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public virtual async Task<TModel> SendJsonAsync<TModel, T>(string url, HttpMethod method, HttpRequestHeaders headers, T data = null, Func<HttpResponseMessage, bool> onError = null)
            where T : class
        {
            if (data != null)
            {
                var json = JsonSerializer.Serialize(data, _serializeOptions);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                return await SendAsync<TModel>(url, method, headers, content, onError);
            }
            else
            {
                return await SendAsync<TModel>(url, method, headers, null, onError);
            }
        }

        /// <summary>
        /// Send an HTTP request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        /// <param name="onError"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public virtual async Task<TModel> SendAsync<TModel>(string url, HttpMethod method, HttpRequestHeaders headers, HttpContent content = null, Func<HttpResponseMessage, bool> onError = null)
        {
            var response = await SendAsync(url, method, headers, content);

            if (response.IsSuccessStatusCode)
            {
                return await DeserializeAsync<TModel>(response);
            }

            // If the error handle is not provided, or if it returns false throw an error.
            if (!(onError?.Invoke(response) ?? false))
            {
                var error = new HttpClientRequestException(response);
                _logger.LogError(error, "HttpClient request error exception", error.Message);
                throw error;
            }

            return default;
        }

        /// <summary>
        /// GET request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public async Task<TModel> GetAsync<TModel>(string url)
        {
            return await SendAsync<TModel>(url, HttpMethod.Get);
        }

        /// <summary>
        /// POST request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <typeparam name="T">The type of the outgoing data object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public async Task<TModel> PostJsonAsync<TModel, T>(string url, T data = null)
            where T : class
        {
            return await SendJsonAsync<TModel, T>(url, HttpMethod.Post, data);
        }

        /// <summary>
        /// POST request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public async Task<TModel> PostAsync<TModel>(string url, HttpContent content = null)
        {
            return await SendAsync<TModel>(url, HttpMethod.Post, content);
        }

        /// <summary>
        /// PUT request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <typeparam name="T">The type of the outgoing data object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public async Task<TModel> PutJsonAsync<TModel, T>(string url, T data = null)
            where T : class
        {
            return await SendJsonAsync<TModel, T>(url, HttpMethod.Put, data);
        }

        /// <summary>
        /// PUT request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public async Task<TModel> PutAsync<TModel>(string url, HttpContent content = null)
        {
            return await SendAsync<TModel>(url, HttpMethod.Put, content);
        }

        /// <summary>
        /// DELETE request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <typeparam name="T">The type of the outgoing data object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public async Task<TModel> DeleteJsonAsync<TModel, T>(string url, T data = null)
            where T : class
        {
            return await SendJsonAsync<TModel, T>(url, HttpMethod.Delete, data);
        }

        /// <summary>
        /// DELETE request to the specified 'url'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public async Task<TModel> DeleteAsync<TModel>(string url, HttpContent content = null)
        {
            return await SendAsync<TModel>(url, HttpMethod.Delete, content);
        }

        /// <summary>
        /// Send an HTTP request to the specified 'uri'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <param name="onError"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <typeparam name="T">The type of the outgoing data object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public virtual async Task<TModel> SendJsonAsync<TModel, T>(Uri url, HttpMethod method = null, T data = null, Func<HttpResponseMessage, bool> onError = null)
            where T : class
        {
            return await SendJsonAsync<TModel, T>(url, method, null, data, onError);
        }

        /// <summary>
        /// Send an HTTP request to the specified 'uri'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="content"></param>
        /// <param name="onError"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public virtual async Task<TModel> SendAsync<TModel>(Uri url, HttpMethod method = null, HttpContent content = null, Func<HttpResponseMessage, bool> onError = null)
        {
            return await SendAsync<TModel>(url, method, null, content, onError);
        }

        /// <summary>
        /// Send an HTTP request to the specified 'uri'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="data"></param>
        /// <param name="onError"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <typeparam name="T">The type of the outgoing data object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public virtual async Task<TModel> SendJsonAsync<TModel, T>(Uri url, HttpMethod method, HttpRequestHeaders headers, T data = null, Func<HttpResponseMessage, bool> onError = null)
            where T : class
        {
            return await SendJsonAsync<TModel, T>(url.OriginalString, method, headers, data, onError);
        }

        /// <summary>
        /// Send an HTTP request to the specified 'uri'.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        /// <param name="onError"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public virtual async Task<TModel> SendAsync<TModel>(Uri url, HttpMethod method, HttpRequestHeaders headers, HttpContent content = null, Func<HttpResponseMessage, bool> onError = null)
        {
            return await SendAsync<TModel>(url.OriginalString, method, headers, content, onError);
        }

        /// <summary>
        /// GET request to the specified 'uri'.
        /// </summary>
        /// <param name="url"></param>
        /// <typeparam name="TModel">The type of the response object.</typeparam>
        /// <exception cref="HttpClientRequestException">Response did not return a success status code.</exception>
        /// <returns></returns>
        public async Task<TModel> GetAsync<TModel>(Uri url)
        {
            return await SendAsync<TModel>(url, HttpMethod.Get);
        }

        /// <summary>
        /// Dispose the HttpClient.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                this.Client.Dispose();
            }
        }

        /// <summary>
        /// Send an HTTP request to the specified 'url'.
        /// Note: Internal implementation to avoid throw on different threads.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> SendInternalAsync(string url, HttpMethod method, HttpRequestHeaders headers, HttpContent content = null)
        {
            if (method == null)
            {
                method = HttpMethod.Get;
            }

            using var message = new HttpRequestMessage(method, url);
            message.Headers.Add("User-Agent", "Pims.Api");
            message.Content = content;

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }
            }

            _logger.LogInformation("HTTP request made '{message.RequestUri}'", message.RequestUri);
            return await this.Client.SendAsync(message);
        }
        #endregion
        #endregion
    }
}
