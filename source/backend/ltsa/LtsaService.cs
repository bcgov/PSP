using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Exceptions;
using Pims.Core.Http;
using Pims.Core.Http.Models;
using Pims.Ltsa.Configuration;
using Pims.Ltsa.Extensions;
using Pims.Ltsa.Models;
using Polly;
using Polly.Retry;

namespace Pims.Ltsa
{
    /// <summary>
    /// LtsaService class, provides a service for integration with Ltsa API services.
    /// </summary>
    public class LtsaService : ILtsaService
    {
        #region Variables
        private readonly JsonSerializerOptions _jsonSerializerOptions = null;
        private readonly ILogger<ILtsaService> _logger;
        private readonly AsyncRetryPolicy _authPolicy;
        private readonly IHttpClientFactory _httpClientFactory;
        #endregion
        #region Properties
        public LtsaOptions Options { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a LtsaService, initializes with specified arguments.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="client"></param>
        /// <param name="tokenHandler"></param>
        /// <param name="logger"></param>
        /// <param name="serializerOptions"></param>
        public LtsaService(IOptions<LtsaOptions> options, ILogger<ILtsaService> logger, IOptions<JsonSerializerOptions> serializerOptions, IHttpClientFactory httpClientFactory)
        {
            this.Options = options.Value;
            this._httpClientFactory = httpClientFactory;
            _logger = logger;
            _jsonSerializerOptions = serializerOptions.Value;
            _authPolicy = Policy
                .Handle<HttpClientRequestException>(ex => ex.StatusCode == HttpStatusCode.Forbidden || ex.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(async (exception, retryCount, context) =>
                {
                    var token = await RefreshAccessTokenAsync((TokenModel)context["access_token"]);
                    context["access_token"] = token;
                });
        }
        #endregion

        #region methods

        /// <summary>
        /// Send a request to the specified endpoint.
        /// </summary>
        /// <typeparam name="TR"></typeparam>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private async Task<TR> GetAsync<TR>(string url)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            try
            {
                return await _authPolicy.ExecuteAsync(async (context) =>
                {
                    var token = (TokenModel)context["access_token"];
                    if (token != null)
                    {
                        client?.DefaultRequestHeaders?.Add("X-Authorization", $"Bearer {token?.AccessToken}");
                    }

                    var response = await client.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpClientRequestException(response);
                    }
                    string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                    var finalResult = JsonSerializer.Deserialize<TR>(payload, _jsonSerializerOptions);
                    return finalResult;

                }, new Dictionary<string, object>
                {
                    { "access_token", null }
                });
            }
            catch (HttpClientRequestException ex)
            {
                Error error = await GetLtsaError(ex, url);
                throw new LtsaException(ex, client, error);
            }
            catch (JsonException ex)
            {
                this._logger.LogError("Failed to process LTSA json: ", ex);
                throw new LtsaException(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Send a request to the specified endpoint.
        /// Make a request to get an access token if required.
        /// </summary>
        /// <typeparam name="TR"></typeparam>
        /// <typeparam name="TD"></typeparam>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<OrderWrapper<OrderParent<TR>>> PostOrderAsync<TR, TD>(string url, TD data) where TR : IFieldedData
            where TD : class
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            var orderProcessingPolicy = Policy
                .HandleResult<OrderWrapper<OrderParent<TR>>>(result => IsResponseMissingJsonAndProcessing(result))
                 .Or<JsonException>()
                 .WaitAndRetryAsync(Options.MaxRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            try
            {
                var stringContent = JsonSerializer.Serialize(data, _jsonSerializerOptions);
                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");

                var response = await _authPolicy.ExecuteAsync(async (context) =>
                {
                    var token = (TokenModel)context["access_token"];
                    if (token != null)
                    {
                        client?.DefaultRequestHeaders?.Add("X-Authorization", $"Bearer {token?.AccessToken}");
                    }

                    var response = await client.PostAsync(url, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new HttpClientRequestException(response);
                    }
                    return response;
                }, new Dictionary<string, object>
                {
                    { "access_token", null }
                });

                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                var processedResponse = JsonSerializer.Deserialize<OrderWrapper<OrderParent<TR>>>(payload, _jsonSerializerOptions);
                if (IsResponseMissingJsonAndProcessing(processedResponse))
                {
                    processedResponse = await _authPolicy.WrapAsync(orderProcessingPolicy).ExecuteAsync(async () => await GetOrderById<TR>(processedResponse?.Order?.OrderId));
                    if (IsResponseMissingJsonAndProcessing(processedResponse))
                    {
                        throw new LtsaException("Request timed out waiting for ltsa response", HttpStatusCode.RequestTimeout);
                    }
                }
                return processedResponse;
            }
            catch (HttpClientRequestException ex)
            {
                Error error = await GetLtsaError(ex, url);
                throw new LtsaException(ex, client, error);
            }
            catch (JsonException ex)
            {
                this._logger.LogError("Failed to process LTSA json: ", ex);
                throw new LtsaException(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        private static bool IsResponseMissingJsonAndProcessing<T>(OrderWrapper<OrderParent<T>> response) where T : IFieldedData
        {
            return response?.Order?.Status == OrderParent<T>.StatusEnum.Processing && response?.Order?.OrderedProduct == null;
        }

        private async Task<Error> GetLtsaError(HttpClientRequestException ex, string url)
        {
            Error error = null;
            if (ex?.Response?.Content != null)
            {
                var errorContent = await ex.Response.Content.ReadAsStringAsync();
                try
                {
                    error = JsonSerializer.Deserialize<Error>(errorContent, _jsonSerializerOptions);
                }
                catch (JsonException)
                {
                    _logger.LogError(ex, $"Failed to deserialize error from remote host: ${errorContent}");
                    error = new Error(new List<String>() { ex.Message, "LTSA returned an invalid response. Please refresh your page to try again." });
                }
                _logger.LogError(ex, $"Failed to send/receive request: ${url}");
            }
            return error;
        }

        /// <summary>
        /// Ensure we have an active access token.
        /// Make an HTTP request if one is needed.
        /// </summary>
        /// <returns></returns>
        private async Task<TokenModel> RefreshAccessTokenAsync(TokenModel token)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            // If the refresh token exists, try and refresh the token.
            if (token?.RefreshToken != null)
            {
                try
                {
                    var refreshToken = token?.RefreshToken;
                    token = null; // remove any existing token details so that the authpolicy will fetch a new token if this auth request fails.
                    var stringContent = JsonSerializer.Serialize(refreshToken, _jsonSerializerOptions);
                    var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                    var response = await _authPolicy.ExecuteAsync(async () => await client.PostAsync(this.Options.AuthUrl.AppendToURL(this.Options.RefreshEndpoint), content));
                    var tokens = JsonSerializer.Deserialize<AuthResponseTokens>(await response.Content.ReadAsStringAsync(), _jsonSerializerOptions);
                    token = new TokenModel(tokens.AccessToken, tokens.RefreshToken);
                }
                catch (HttpClientRequestException ex)
                {
                    _logger.LogError(ex, $"Failed to send/receive auth refresh request: ${this.Options.AuthUrl}");
                    throw new LtsaException(ex.Message, ex, ex.StatusCode.Value);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, $"Failed to parse refresh token JSON response");
                    throw new LtsaException(ex.Message, HttpStatusCode.InternalServerError);
                }
                token = await GetTokenAsync();
            }
            else
            {
                token = await GetTokenAsync();
            }
            return token;
        }

        /// <summary>
        /// Make an HTTP request to Ltsa to get an access token for the specified 'integratorUsername' and 'integratorPassword'.
        /// </summary>
        /// <param name="integratorUsername"></param>
        /// <param name="integratorPassword"></param>
        /// <param name="myLtsaUsername"></param>
        /// <param name="myLtsaUserPassword"></param>
        /// <returns></returns>
        public async Task<TokenModel> GetTokenAsync(string integratorUsername = null, string integratorPassword = null, string myLtsaUsername = null, string myLtsaUserPassword = null)
        {
            var creds = new IntegratorCredentials()
            {
                IntegratorUsername = integratorUsername ?? this.Options.IntegratorUsername,
                IntegratorPassword = integratorPassword ?? this.Options.IntegratorPassword,
                MyLtsaUserName = myLtsaUsername ?? this.Options.MyLtsaUsername,
                MyLtsaUserPassword = myLtsaUserPassword ?? this.Options.MyLtsaUserPassword
            };

            string url = this.Options.AuthUrl.AppendToURL(this.Options.LoginIntegratorEndpoint);
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            try
            {
                var stringContent = JsonSerializer.Serialize(creds, _jsonSerializerOptions);
                var content = new StringContent(stringContent.ToString(), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new LtsaException("Received error response from LTSA when retrieving authorization token", response.StatusCode);
                }
                var tokens = JsonSerializer.Deserialize<AuthResponseTokens>(await response.Content.ReadAsStringAsync(), _jsonSerializerOptions);
                return new TokenModel(tokens.AccessToken, tokens.RefreshToken);
            }
            catch (HttpClientRequestException ex)
            {
                Error error = await GetLtsaError(ex, url);
                throw new LtsaException(ex, client, error);
            }
            catch (JsonException ex)
            {
                this._logger.LogError($"Failed to process LTSA json:", ex);
                throw new LtsaException(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get all of the title summaries for the given pid.
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public async Task<TitleSummariesResponse> GetTitleSummariesAsync(int pid)
        {
            var url = this.Options.HostUri.AppendToURL(new string[] { this.Options.TitleSummariesEndpoint, $"?filter=parcelIdentifier:{pid}" });
            return await GetAsync<TitleSummariesResponse>(url);
        }

        /// <summary>
        /// Post a Title Order using the passed titleNumber and landTitleDistrictCode
        /// </summary>
        /// <param name="titleNumber"></param>
        /// <param name="landTitleDistrictCode"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<OrderParent<Title>>> PostTitleOrder(string titleNumber, string landTitleDistrictCode)
        {
            TitleOrder order = new(new TitleOrderParameters(titleNumber, Enum.Parse<LandTitleDistrictCode>(landTitleDistrictCode)));
            var url = this.Options.HostUri.AppendToURL(this.Options.OrdersEndpoint);
            return await PostOrderAsync<Title, OrderWrapper<TitleOrder>>(url, new OrderWrapper<TitleOrder>(order));
        }

        /// <summary>
        /// Post a Parcel Info Order using the passed pid
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<OrderParent<ParcelInfo>>> PostParcelInfoOrder(string pid)
        {
            ParcelInfoOrder order = new(new ParcelInfoOrderParameters(pid));
            var url = this.Options.HostUri.AppendToURL(this.Options.OrdersEndpoint);
            return await PostOrderAsync<ParcelInfo, OrderWrapper<ParcelInfoOrder>>(url, new OrderWrapper<ParcelInfoOrder>(order));
        }

        /// <summary>
        /// Post a Strata Plan Common Property order using the passed strataPlanNumber
        /// </summary>
        /// <param name="strataPlanNumber"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<OrderParent<StrataPlanCommonProperty>>> PostSpcpOrder(string strataPlanNumber)
        {
            SpcpOrder order = new(new StrataPlanCommonPropertyOrderParameters(strataPlanNumber));
            var url = this.Options.HostUri.AppendToURL(this.Options.OrdersEndpoint);
            return await PostOrderAsync<StrataPlanCommonProperty, OrderWrapper<SpcpOrder>>(url, new OrderWrapper<SpcpOrder>(order));
        }

        /// <summary>
        /// Get an order by its id.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<OrderParent<T>>> GetOrderById<T>(string orderId) where T : IFieldedData
        {
            var url = this.Options.HostUri.AppendToURL(this.Options.OrdersEndpoint, orderId);
            return await GetAsync<OrderWrapper<OrderParent<T>>>(url);
        }

        /// <summary>
        /// Retrieve all of the title summaries for the given pid, and create a title order for each one. Return all of the title orders and the parcel info order for the given pid
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public async Task<LtsaOrders> PostLtsaFields(string pid)
        {
            int parcelIdentifier = ConvertPID(pid);
            var titleSummariesTask = GetTitleSummariesAsync(parcelIdentifier);
            var postParcelInfoOrderTask = PostParcelInfoOrder(pid);
            var titleSummaries = await titleSummariesTask;

            ICollection<Task<OrderWrapper<OrderParent<Title>>>> titleOrderTasks = new List<Task<OrderWrapper<OrderParent<Title>>>>();
            foreach (TitleSummary titleSummary in titleSummaries.TitleSummaries)
            {
                titleOrderTasks.Add(PostTitleOrder(titleSummary.TitleNumber, titleSummary.LandTitleDistrictCode.ToString()));
            }
            var titleOrders = await Task.WhenAll(titleOrderTasks);
            var parcelInfo = await postParcelInfoOrderTask;
            return new LtsaOrders()
            {
                TitleOrders = titleOrders.Select(titleOrder => titleOrder?.Order),
                ParcelInfo = parcelInfo.Order
            };
        }

        private static int ConvertPID(string pid)
        {
            _ = int.TryParse(pid?.Replace("-", "") ?? "", out int value);
            return value;
        }

        #endregion
    }
}
