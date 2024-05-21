using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pims.Core.Exceptions;

namespace Pims.Api.Helpers.Healthchecks
{
    public class PimsGeoserverHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _proxyOptions;
        private readonly string _url;
        private readonly int _statusCode;

        public PimsGeoserverHealthCheck(IConfiguration section)
        {
            _proxyOptions = section.GetSection("Geoserver");
            _url = section.GetValue<string>("HealthChecks:Geoserver:Url");
            _statusCode = section.GetValue<int>("HealthChecks:Geoserver:StatusCode");
        }

        public PimsGeoserverHealthCheck(IConfiguration section, string url, int statusCode)
        {
            _proxyOptions = section;
            _url = url;
            _statusCode = statusCode;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var basicAuth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{_proxyOptions.GetValue<string>("ServiceUser")}:{_proxyOptions.GetValue<string>("ServicePassword")}"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
                    var result = await client.GetAsync($"{_proxyOptions.GetValue<string>("ProxyUrl")}{_url}", cancellationToken);

                    if (result.StatusCode != HttpStatusCode.OK)
                    {
                        string errorContent = await result.Content.ReadAsStringAsync(cancellationToken);
                        return new HealthCheckResult(HealthStatus.Degraded, $"http response: {result.StatusCode} does not match: {_statusCode}. message: {errorContent}");
                    }
                }
                catch (HttpClientRequestException ex)
                {
                    return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
                }
            }

            return HealthCheckResult.Healthy();
        }
    }
}
