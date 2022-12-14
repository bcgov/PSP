using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Exceptions;
using Pims.Core.Http.Configuration;
using Pims.Tools.Core.Configuration;

namespace Pims.Tools.Core
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
        public RequestClient(
            IHttpClientFactory clientFactory,
            JwtSecurityTokenHandler tokenHandler,
            IOptionsMonitor<AuthClientOptions> authClientOptions,
            IOptionsMonitor<OpenIdConnectOptions> openIdConnectOptions,
            IOptionsMonitor<RequestOptions> requestOptions,
            IOptionsMonitor<JsonSerializerOptions> serializerOptions,
            ILogger<RequestClient> logger)
            : base(clientFactory, tokenHandler, authClientOptions, openIdConnectOptions, serializerOptions, logger)
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
        /// Recursively retry after a failure based on configuration.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="attempt"></param>
        /// <returns></returns>
        public async Task<TR> RetryAsync<TR>(HttpMethod method, string url, int attempt = 1)
            where TR : class
        {
            try
            {
                return await HandleRequestAsync<TR>(method, url);
            }
            catch (HttpClientRequestException)
            {
                // Make another attempt;
                if (_requestOptions.RetryAfterFailure && attempt <= _requestOptions.RetryAttempts)
                {
                    _logger.LogInformation($"Retry attempt: {attempt} of {_requestOptions.RetryAttempts}");
                    return await RetryAsync<TR>(method, url, ++attempt);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Recursively retry after a failure based on configuration.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="attempt"></param>
        /// <returns></returns>
        public async Task<TR> RetryAsync<TR, T>(HttpMethod method, string url, T data = default, int attempt = 1)
            where TR : class
            where T : class
        {
            try
            {
                return await HandleRequestAsync<TR, T>(method, url, data);
            }
            catch (HttpClientRequestException ex)
            {
                _logger.LogError($"Request failed: status: {ex.StatusCode} Details: {ex.Message}");

                // Make another attempt;
                if (_requestOptions.RetryAfterFailure && attempt <= _requestOptions.RetryAttempts)
                {
                    _logger.LogInformation($"Retry attempt: {attempt} of {_requestOptions.RetryAttempts}");
                    return await RetryAsync<TR, T>(method, url, data, ++attempt);
                }
                else
                {
                    throw;
                }
            }
        }

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

        /// <summary>
        /// Send the items in an HTTP request.
        /// Deserialize the result into the specified 'TR' type.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public async Task<TR> HandleRequestAsync<TR, T>(HttpMethod method, string url, T data, Func<HttpResponseMessage, bool> onError = null)
            where TR : class
            where T : class
        {
            StringContent body = null;
            if (data != null)
            {
                var json = JsonSerializer.Serialize(data, _serializerOptions);
                body = new StringContent(json, Encoding.UTF8, "application/json");
            }

            try
            {
                var response = await SendAsync(url, method, body);

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
            }
            finally
            {
                if (body != null)
                {
                    body.Dispose();
                }
            }

            return null;
        }
        #endregion
    }
}
