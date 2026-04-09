using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pims.Api.Repositories.Ches;

namespace Pims.Api.Helpers.Healthchecks
{
    /// <summary>
    /// Health check for CHES service connectivity.
    /// </summary>
    public class ChesHealthCheck : IHealthCheck
    {
        private readonly IEmailRepository _repository;

        public ChesHealthCheck(IEmailRepository repository)
        {
            _repository = repository;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                const int maxJitterMilliseconds = 10000;
                var jitter = Random.Shared.Next(0, maxJitterMilliseconds + 1);
                if (jitter > 0)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(jitter), cancellationToken);
                }

                var response = await _repository.TryGetHealthAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new HealthCheckResult(HealthStatus.Degraded, $"CHES health check returned status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, $"CHES health check failed with exception: {ex.Message}");
            }
            return HealthCheckResult.Healthy();
        }
    }
}
