using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Authentication.OAuth;
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
        private static readonly JsonSerializerOptions LtsaSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };

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
            Options = options.Value;
            _httpClientFactory = httpClientFactory;
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
                        client.DefaultRequestHeaders?.Add("Authorization", $"Bearer {token?.AccessToken}");
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
               _logger.LogError(ex, "Failed to process LTSA json: {Msg}", ex.Message);
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
                var response = await _authPolicy.ExecuteAsync(async (context) =>
                {
                    var token = (TokenModel)context["access_token"];

                    using var request = new HttpRequestMessage(HttpMethod.Post, url);
                    //request.Headers.Add("Accept", "application/json");
                    if (token != null)
                    {
                        request.Headers.Add("Authorization", $"Bearer {token?.AccessToken}");
                    }

                    var stringContent = JsonSerializer.Serialize(data, _jsonSerializerOptions);
                    var content = new StringContent(stringContent, Encoding.UTF8, "application/json");
                    request.Content = content;

                    var response = await client.SendAsync(request);
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
               _logger.LogError(ex, "Failed to process LTSA json: {Msg}", ex.Message);
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
        private async Task<TokenModel> RefreshAccessTokenAsync(TokenModel token, CancellationToken cancellationToken = default)
        {
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            // If the refresh token exists, try and refresh the token.
            if (token?.RefreshToken is not null)
            {
                try
                {
                    var parameters = new Dictionary<string, string>
                        {
                        ["grant_type"] = "refresh_token",
                        ["client_id"] = Options.ClientId,
                        ["client_secret"] = Options.ClientSecret,
                        ["refresh_token"] = token.RefreshToken,
                    };

                    using var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
                    request.Headers.Add("Accept", "application/json");
                    request.Content = new FormUrlEncodedContent(parameters);
                    var tokenResponse = await client.SendAsync(request, cancellationToken);
                    tokenResponse.EnsureSuccessStatusCode();

                    var tokenStr = await tokenResponse.Content.ReadAsStringAsync(cancellationToken);
                    AuthResponseTokens newToken = JsonSerializer.Deserialize<AuthResponseTokens>(tokenStr, LtsaSerializerOptions);

                    token.RenewToken(newToken.AccessToken, newToken.RefreshToken);
                }
                catch (HttpClientRequestException ex)
                {
                    _logger.LogError(ex, "Failed to send/receive auth refresh request: {AuthUrl}", Options.AuthUrl);
                    throw new LtsaException(ex.Message, ex, ex.StatusCode.Value);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Failed to parse refresh token JSON response");
                    throw new LtsaException(ex.Message, HttpStatusCode.InternalServerError);
                }
                catch (InvalidOperationException)
                {
                    token = await GetTokenAsync(cancellationToken: cancellationToken);
                }
            }
            else
            {
                token = await GetTokenAsync(cancellationToken: cancellationToken);
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
        public async Task<TokenModel> GetTokenAsync(string integratorUsername = null, string integratorPassword = null, string myLtsaUsername = null, string myLtsaUserPassword = null, CancellationToken cancellationToken = default)
        {
            var creds = new IntegratorCredentials()
            {
                IntegratorUsername = integratorUsername ?? Options.IntegratorUsername,
                IntegratorPassword = integratorPassword ?? Options.IntegratorPassword,
                MyLtsaUserName = myLtsaUsername ?? Options.MyLtsaUsername,
                MyLtsaUserPassword = myLtsaUserPassword ?? Options.MyLtsaUserPassword
            };

            string url = Options.AuthUrl.AppendToURL(Options.LoginIntegratorEndpoint);
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");

            try
            {
                var parameters = new Dictionary<string, string>
                {
                   { "client_id", Options.ClientId },
                   { "client_secret", Options.ClientSecret },
                   { "username", creds.MyLtsaUserName },
                   { "password", creds.MyLtsaUserPassword },
                   { "grant_type", Options.GrantType },
                   { "scope", Options.Scope }
                };

                using var request = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint);
                request.Headers.Add("Accept", "application/json");
                request.Content = new FormUrlEncodedContent(parameters);
                var tokenResponse = await client.SendAsync(request, cancellationToken);
                tokenResponse.EnsureSuccessStatusCode();

                var tokenStr = await tokenResponse.Content.ReadAsStringAsync(cancellationToken);
                AuthResponseTokens token = JsonSerializer.Deserialize<AuthResponseTokens>(tokenStr, LtsaSerializerOptions );

                return new TokenModel(token.AccessToken, token.RefreshToken);
            }
            catch (HttpClientRequestException ex)
            {
                Error error = await GetLtsaError(ex, url);
                throw new LtsaException(ex, client, error);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to process LTSA json: {Msg}", ex.Message);
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
            var url = Options.HostUri.AppendToURL(Options.TitleSummariesEndpoint, $"?filter=parcelIdentifier:{pid}");
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
            var url = Options.HostUri.AppendToURL(Options.OrdersEndpoint);
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
            var url = Options.HostUri.AppendToURL(Options.OrdersEndpoint);
            return await PostOrderAsync<ParcelInfo, OrderWrapper<ParcelInfoOrder>>(url, new OrderWrapper<ParcelInfoOrder>(order));
        }

        /// <summary>
        /// Post a Strata Plan Common Property order using the passed strataPlanNumber
        /// </summary>
        /// <param name="strataPlanNumber"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<OrderParent<StrataPlanCommonProperty>>> PostSpcpOrder(string strataPlanNumber)
        {
            var url = Options.HostUri.AppendToURL(Options.OrdersEndpoint);
            SpcpOrder order = new(new StrataPlanCommonPropertyOrderParameters(strataPlanNumber), productType: OrderParent<StrataPlanCommonProperty>.ProductTypeEnum.commonProperty);

            return await PostOrderAsync<StrataPlanCommonProperty, OrderWrapper<SpcpOrder>>(url, new OrderWrapper<SpcpOrder>(order));
        }

        /// <summary>
        /// Get an order by its id.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<OrderParent<T>>> GetOrderById<T>(string orderId) where T : IFieldedData
        {
            var url = Options.HostUri.AppendToURL(Options.OrdersEndpoint, orderId);
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
