using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Exceptions;
using Pims.Core.Http;
using Pims.Core.Http.Models;
using Pims.Ltsa.Configuration;
using Pims.Ltsa.Extensions;
using Pims.Ltsa.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
        private JsonSerializerOptions _jsonSerializerOptions = null;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly ILogger<ILtsaService> _logger;
        private readonly int MAX_RETRIES = 3;
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
        public LtsaService(IOptions<LtsaOptions> options, IHttpRequestClient client, JwtSecurityTokenHandler tokenHandler, ILogger<ILtsaService> logger, IOptions<JsonSerializerOptions> serializerOptions)
        {

            this.Options = options.Value;
            this.Client = client;
            _tokenHandler = tokenHandler;
            _logger = logger;
            _jsonSerializerOptions = serializerOptions.Value;
        }
        #endregion

        #region methods

        /// <summary>
        /// Send a request to the specified endpoint.
        /// </summary>
        /// <typeparam name="TR"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        private async Task<TR> SendAsync<TR>(string url, HttpMethod method)
        {
            if (_token == null)
            {
                await RefreshAccessTokenAsync();
            }

            var headers = new HttpRequestMessage().Headers;
            headers.Add("X-Authorization", $"Bearer {_token.AccessToken}");

            for (int i = 0; i < MAX_RETRIES; i++)
            {
                try
                {
                    return await this.Client.SendAsync<TR>(url, method, headers);
                }
                catch (HttpClientRequestException ex)
                {
                    await handleHttpClientRequestException(ex, url);
                }
            }
            throw new LtsaException("max retries exceeded");
        }

        /// <summary>
        /// Send a request to the specified endpoint.
        /// Make a request to get an access token if required.
        /// </summary>
        /// <typeparam name="TR"></typeparam>
        /// <typeparam name="TD"></typeparam>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<TR> SendAsync<TR, TD>(string url, HttpMethod method, TD data)
            where TD : class
        {
            if (_token == null)
            {
                await RefreshAccessTokenAsync();
            }
            var headers = new HttpRequestMessage().Headers;
            headers.Add("X-Authorization", $"Bearer {_token.AccessToken}");

            for (int i = 0; i < MAX_RETRIES; i++)
            {
                try
                {
                    return await this.Client.SendJsonAsync<TR, TD>(url, method, headers, data);
                }
                catch (HttpClientRequestException ex)
                {
                    await handleHttpClientRequestException(ex, url);
                }
            }
            throw new LtsaException("max retries exceeded");
        }

        private async Task handleHttpClientRequestException(HttpClientRequestException ex, string url)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshAccessTokenAsync();
                return;
            }
            var error = JsonSerializer.Deserialize<Error>(await ex.Response.Content.ReadAsStringAsync(), _jsonSerializerOptions);
            _logger.LogError(ex, $"Failed to send/receive request: ${url}");
            throw new LtsaException(ex, this.Client, error);
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
                    var headers = new HttpRequestMessage().Headers;
                    headers.Add("ContentType", "application/json");
                    var response = await this.Client.PostJsonAsync(this.Options.AuthUrl.AppendToURL(this.Options.RefreshEndpoint), new { refreshToken = _token.RefreshToken });
                    var tokens = JsonSerializer.Deserialize<AuthResponseTokens>(await response.Content.ReadAsStringAsync(), _jsonSerializerOptions);
                    _token = new TokenModel(tokens.AccessToken, tokens.RefreshToken);
                }
                catch (HttpClientRequestException ex)
                {
                    //In this case, the refresh token has also expired, so get the token using the credentials.
                    if (ex.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        _token = await GetTokenAsync();
                    }
                    else
                    {
                        _logger.LogError(ex, $"Failed to send/receive auth refresh request: ${this.Options.AuthUrl}");
                        throw new LtsaException(ex.Message, ex, ex.StatusCode);
                    }
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

            try
            {
                var response = await this.Client.PostJsonAsync(this.Options.AuthUrl.AppendToURL(this.Options.LoginIntegratorEndpoint), creds);
                var tokens = JsonSerializer.Deserialize<AuthResponseTokens>(await response.Content.ReadAsStringAsync(), _jsonSerializerOptions);
                return new TokenModel(tokens.AccessToken, tokens.RefreshToken);
            }
            catch (HttpClientRequestException ex)
            {
                var error = JsonSerializer.Deserialize<Error>(await ex.Response.Content.ReadAsStringAsync(), _jsonSerializerOptions);
                _logger.LogError(ex, $"Failed to send/receive request: ${this.Options.AuthUrl.AppendToURL(this.Options.LoginIntegratorEndpoint)}");
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
        /// <param name="pid"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<TitleOrder>> PostTitleOrder(string titleNumber, string landTitleDistrictCode)
        {
            TitleOrder order = new TitleOrder(new TitleOrderParameters(titleNumber, Enum.Parse<LandTitleDistrictCode>(landTitleDistrictCode)));
            var url = this.Options.HostUri.AppendToURL(this.Options.OrdersEndpoint);
            return await SendAsync<OrderWrapper<TitleOrder>, OrderWrapper<TitleOrder>>(url, HttpMethod.Post, new OrderWrapper<TitleOrder>(order));
        }

        /// <summary>
        /// Post a Parcel Info Order using the passed pid
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<ParcelInfoOrder>> PostParcelInfoOrder(string pid)
        {
            ParcelInfoOrder order = new ParcelInfoOrder(new ParcelInfoOrderParameters(pid));
            var url = this.Options.HostUri.AppendToURL(this.Options.OrdersEndpoint);
            return await SendAsync<OrderWrapper<ParcelInfoOrder>, OrderWrapper<ParcelInfoOrder>>(url, HttpMethod.Post, new OrderWrapper<ParcelInfoOrder>(order));
        }

        /// <summary>
        /// Post a Strata Plan Common Property order using the passed strataPlanNumber
        /// </summary>
        /// <param name="strataPlanNumber"></param>
        /// <returns></returns>
        public async Task<OrderWrapper<SpcpOrder>> PostSpcpOrder(string strataPlanNumber)
        {
            SpcpOrder order = new SpcpOrder(new StrataPlanCommonPropertyOrderParameters(strataPlanNumber));
            var url = this.Options.HostUri.AppendToURL(this.Options.OrdersEndpoint);
            return await SendAsync<OrderWrapper<SpcpOrder>, OrderWrapper<SpcpOrder>>(url, HttpMethod.Post, new OrderWrapper<SpcpOrder>(order));
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
            var PostParcelInfoOrderTask = PostParcelInfoOrder(pid);
            var titleSummaries = await titleSummariesTask;

            ICollection<Task<OrderWrapper<TitleOrder>>> titleOrderTasks = new List<Task<OrderWrapper<TitleOrder>>>();
            foreach (TitleSummary titleSummary in titleSummaries.TitleSummaries)
            {
                titleOrderTasks.Add(PostTitleOrder(titleSummary.TitleNumber, titleSummary.LandTitleDistrictCode.ToString()));
            }
            var titleOrders = await Task.WhenAll(titleOrderTasks);
            var parcelInfo = await PostParcelInfoOrderTask;
            return new LtsaOrders()
            {
                TitleOrders = titleOrders.Select(titleOrder => titleOrder?.Order),
                ParcelInfo = parcelInfo.Order
            };
        }

        private static int ConvertPID(string pid)
        {
            int.TryParse(pid?.Replace("-", "") ?? "", out int value);
            return value;
        }

        #endregion
    }
}
