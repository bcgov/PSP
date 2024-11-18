using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Api.Policies;
using Pims.Core.Extensions;
using Pims.Core.Http.Configuration;
using Pims.Core.Security;

namespace Pims.Proxy.Controllers
{
    /// <summary>
    /// PimsGeoserverController class, provides an authenticated proxy to the pims inventory layer.
    /// Not possible to authenticate geoserver with keycloak gold standard.
    /// Authentication with SITEMINDER/WebAde flawed as keycloak does not refresh SITEMINDER token and they expire independently of each other.
    /// Basic auth (via internal gov network and service account) provides a workable method for authentication to Geoserver.
    /// Authorization of the user is provided at this level and not at the Geoserver level.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("v{version:apiVersion}/geoserver")]
    [Route("/geoserver")]
    public class PimsGeoserverController : ControllerBase
    {
        #region Variables

        private readonly IOptionsMonitor<GeoserverProxyOptions> _proxyOptions;
        private readonly ILogger _logger;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PimsGeoserverController class.
        /// </summary>
        public PimsGeoserverController(IOptionsMonitor<GeoserverProxyOptions> options, ILogger<PimsGeoserverController> logger)
        {
            _proxyOptions = options;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        [Route("{**rest}")]
        [HasPermission(Permissions.PropertyView)]
        public Task ProxyCatchAll(string rest)
        {
            var queryString = this.Request.QueryString.Value;
            _logger.LogInformation(
                "Request received by Controller: {Controller}, Action: {ControllerAction}, User: {User}, DateTime: {DateTime}",
                nameof(PimsGeoserverController),
                nameof(ProxyCatchAll),
                User.GetUsername(),
                DateTime.Now);

            HttpProxyOptions httpOptions = AspNetCore.Proxy.Options.HttpProxyOptionsBuilder.Instance.WithShouldAddForwardedHeaders(false).WithBeforeSend((c, hrm) =>
            {
                // Set something that is needed for the downstream endpoint.
                var basicAuth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{_proxyOptions.CurrentValue.ServiceUser}:{_proxyOptions.CurrentValue.ServicePassword}"));
                hrm.Headers.Clear(); // remove all headers as pre-existing SITEMINDER headers may conflict with BASIC AUTH.
                hrm.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

                return Task.CompletedTask;
            }).WithAfterReceive((c, hrm) =>
            {
                _logger.LogInformation("Response from geoserver proxy: {StatusCode}, {ReasonPhrase}, {Headers} ", hrm.StatusCode, hrm.ReasonPhrase, hrm.Headers);

                return Task.CompletedTask;
            }).WithHandleFailure((c, e) =>
            {
                _logger.LogInformation("Response from geoserver proxy: {Exception}, {StatusCode}, {Body}, {Headers} ", e.Message, c.Response?.StatusCode, c.Response?.Body, c.Response?.Headers);

                return Task.CompletedTask;
            }).Build();
            var proxyUrl = $"{_proxyOptions.CurrentValue.ProxyUrl}/{rest}{queryString}";
            _logger.LogInformation("Proxying request to: {proxyUrl}", proxyUrl);
            return this.HttpProxyAsync(proxyUrl, httpOptions);
        }

        #endregion
    }
}
