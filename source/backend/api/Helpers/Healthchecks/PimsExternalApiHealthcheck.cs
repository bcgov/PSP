using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pims.Core.Exceptions;

namespace Pims.Api.Helpers.Healthchecks
{
    public class PimsExternalApiHealthcheck : IHealthCheck
    {
        private readonly string url;
        private readonly int statusCode;

        public PimsExternalApiHealthcheck(string url)
            : this(url, statusCode: (int)HttpStatusCode.OK)
        {
        }

        public PimsExternalApiHealthcheck(IConfigurationSection section)
        {
            this.url = section["Url"];
            this.statusCode = int.Parse(section["StatusCode"]);
        }

        public PimsExternalApiHealthcheck(string url, int statusCode)
        {
            this.url = url;
            this.statusCode = statusCode;
        }

        public string ConnectionString { get; }

        public string ProbeExpectedResult { get; }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return CheckExternalApi(context, cancellationToken);
        }

        protected async Task<HealthCheckResult> CheckExternalApi(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync(url, cancellationToken);

                    if ((int)result.StatusCode != statusCode)
                    {
                        string errorContent = await result.Content.ReadAsStringAsync(cancellationToken);
                        return new HealthCheckResult(HealthStatus.Degraded, $"http response: {result.StatusCode} does not match: {statusCode}. message: {errorContent}");
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
