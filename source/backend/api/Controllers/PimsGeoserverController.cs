using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Proxy;
using AspNetCore.Proxy.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Pims.Api.Policies;
using Pims.Core.Http.Configuration;
using Pims.Dal.Security;

namespace Pims.Api.Controllers
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
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/geoserver")]
    [Route("/geoserver")]
    public class PimsGeoserverController : ControllerBase
    {
        #region Variables

        private readonly IOptionsMonitor<GeoserverProxyOptions> _proxyOptions;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a PimsGeoserverController class.
        /// </summary>
        public PimsGeoserverController(IOptionsMonitor<GeoserverProxyOptions> options)
        {
            _proxyOptions = options;
        }
        #endregion

        #region Endpoints

        [Route("{**rest}")]
        [HasPermission(Permissions.PropertyView)]
        public Task ProxyCatchAll(string rest)
        {
            var queryString = this.Request.QueryString.Value;
            HttpProxyOptions httpOptions = AspNetCore.Proxy.Options.HttpProxyOptionsBuilder.Instance.WithShouldAddForwardedHeaders(false).WithBeforeSend((c, hrm) =>
            {
                // Set something that is needed for the downstream endpoint.
                var basicAuth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{_proxyOptions.CurrentValue.ServiceUser}:{_proxyOptions.CurrentValue.ServicePassword}"));
                hrm.Headers.Clear(); // remove all headers as pre-existing SITEMINDER headers may conflict with BASIC AUTH.
                hrm.Headers.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

                return Task.CompletedTask;
            }).Build();

            return this.HttpProxyAsync($"{_proxyOptions.CurrentValue.ProxyUrl}/{rest}{queryString}", httpOptions);
        }

        #endregion
    }
}
