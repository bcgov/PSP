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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pims.Ltsa
{
    /// <summary>
    /// LtsaService class, provides a service for integration with Ltsa API services.
    /// </summary>
    public class LtsaService : ILtsaService
    {
        #region Variables
        private TokenModel _token = null;
        private readonly JsonSerializerOptions _jsonSerializerOptions = null;
        private readonly ILogger<ILtsaService> _logger;
        private readonly AsyncRetryPolicy _authPolicy;
        #endregion
        #region Properties
        protected IHttpRequestClient Client { get; }
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
        public LtsaService(IOptions<LtsaOptions> options, IHttpRequestClient client, ILogger<ILtsaService> logger, IOptions<JsonSerializerOptions> serializerOptions)
        {

            this.Options = options.Value;
            this.Client = client;
            _logger = logger;
            _jsonSerializerOptions = serializerOptions.Value;
            _authPolicy = Policy
                .Handle<HttpClientRequestException>(ex => ex.StatusCode == HttpStatusCode.Forbidden || ex.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(async (exception, retryCount) =>
                {
                    _token = await RefreshAccessTokenAsync();
                    this.Client.Client?.DefaultRequestHeaders?.Clear();
                    this.Client.Client?.DefaultRequestHeaders?.Add("X-Authorization", $"Bearer {_token.AccessToken}");
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
        private async Task<TR> SendAsync<TR>(string url, HttpMethod method)
        {
            try
            {
                return await _authPolicy.ExecuteAsync(async () => await this.Client.SendAsync<TR>(url, method));
            }
            catch (HttpClientRequestException ex)
            {
                Error error = await GetLtsaError(ex, url);
                throw new LtsaException(ex, this.Client, error);
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
        private async Task<OrderWrapper<OrderParent<TR>>> SendOrderAsync<TR, TD>(string url, HttpMethod method, TD data) where TR : IFieldedData
            where TD : class
        {
            var orderProcessingPolicy = Policy
                .HandleResult<OrderWrapper<OrderParent<TR>>>(result => IsResponseMissingJsonAndProcessing(result))
                 .WaitAndRetryAsync(Options.MaxRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            try
            {
                var response = await _authPolicy.ExecuteAsync(async () => await this.Client.SendJsonAsync<OrderWrapper<OrderParent<TR>>, TD>(url, method, data));
                if (IsResponseMissingJsonAndProcessing(response))
                {
                    response = await _authPolicy.WrapAsync(orderProcessingPolicy).ExecuteAsync(async () => await GetOrderById<TR>(response?.Order?.OrderId));
                    if (IsResponseMissingJsonAndProcessing(response))
                    {
                        throw new LtsaException("Request timed out waiting for ltsa response", HttpStatusCode.RequestTimeout);
                    }
                }
                return response;
            }
            catch (HttpClientRequestException ex)
            {
                Error error = await GetLtsaError(ex, url);
                throw new LtsaException(ex, this.Client, error);
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
                    error = new Error(new List<String>() { ex.Message });
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
        private async Task<TokenModel> RefreshAccessTokenAsync()
        {
            //If the refresh token exists, try an refresh the token.
            if (_token?.RefreshToken != null)
            {
                try
                {
                    var refreshToken = _token.RefreshToken;
                    _token = null; // remove any existing token details so that the authpolicy will fetch a new token if this auth request fails.
                    var response = await _authPolicy.ExecuteAsync(async () => await this.Client.PostJsonAsync(this.Options.AuthUrl.AppendToURL(this.Options.RefreshEndpoint), new { refreshToken }));
                    var tokens = JsonSerializer.Deserialize<AuthResponseTokens>(await response.Content.ReadAsStringAsync(), _jsonSerializerOptions);
                    _token = new TokenModel(tokens.AccessToken, tokens.RefreshToken);
                }
                catch (HttpClientRequestException ex)
                {
                    _logger.LogError(ex, $"Failed to send/receive auth refresh request: ${this.Options.AuthUrl}");
                    throw new LtsaException(ex.Message, ex, ex.StatusCode.Value);
                }
            }
            else
            {
                _token = await GetTokenAsync();
            }
            return _token;
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
            try
            {
                var response = await this.Client.PostJsonAsync(url, creds);
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
                throw new LtsaException(ex, this.Client, error);
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
            return await SendAsync<TitleSummariesResponse>(url, HttpMethod.Get);
        }

        /// <summary>
        /// Post a Title Order using the passed titleNumber and landTitleDistrictCode
        /// </summary>
        /// <param name="titleNumber"></param>
        /// <param name="landTitleDistrictCode"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<OrderParent<Title>>> PostTitleOrder(string titleNumber, string landTitleDistrictCode)
        {
            TitleOrder order = new (new TitleOrderParameters(titleNumber, Enum.Parse<LandTitleDistrictCode>(landTitleDistrictCode)));
            var url = this.Options.HostUri.AppendToURL(this.Options.OrdersEndpoint);
            return await SendOrderAsync<Title, OrderWrapper<TitleOrder>>(url, HttpMethod.Post, new OrderWrapper<TitleOrder>(order));
        }

        /// <summary>
        /// Post a Parcel Info Order using the passed pid
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<OrderParent<ParcelInfo>>> PostParcelInfoOrder(string pid)
        {
            ParcelInfoOrder order = new (new ParcelInfoOrderParameters(pid));
            var url = this.Options.HostUri.AppendToURL(this.Options.OrdersEndpoint);
            return await SendOrderAsync<ParcelInfo, OrderWrapper<ParcelInfoOrder>>(url, HttpMethod.Post, new OrderWrapper<ParcelInfoOrder>(order));
        }

        /// <summary>
        /// Post a Strata Plan Common Property order using the passed strataPlanNumber
        /// </summary>
        /// <param name="strataPlanNumber"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<OrderParent<StrataPlanCommonProperty>>> PostSpcpOrder(string strataPlanNumber)
        {
            SpcpOrder order = new (new StrataPlanCommonPropertyOrderParameters(strataPlanNumber));
            var url = this.Options.HostUri.AppendToURL(this.Options.OrdersEndpoint);
            return await SendOrderAsync<StrataPlanCommonProperty, OrderWrapper<SpcpOrder>>(url, HttpMethod.Post, new OrderWrapper<SpcpOrder>(order));
        }

        /// <summary>
        /// Get an order by its id.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<OrderParent<T>>> GetOrderById<T>(string orderId) where T : IFieldedData
        {
            var url = this.Options.HostUri.AppendToURL(this.Options.OrdersEndpoint, orderId);
            return await SendAsync<OrderWrapper<OrderParent<T>>>(url, HttpMethod.Get);
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
