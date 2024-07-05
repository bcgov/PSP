using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pims.Api.Repositories.Cdogs;

namespace Pims.Api.Helpers.Healthchecks
{
    public class PimsCdogsHealthcheck : IHealthCheck
    {
        private readonly IDocumentGenerationRepository _generationRepository;

        public PimsCdogsHealthcheck(IDocumentGenerationRepository generationRepository)
        {
            _generationRepository = generationRepository;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {

                var health = await _generationRepository.TryGetHealthAsync();
                if (health.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return new HealthCheckResult(HealthStatus.Degraded, $"received invalid health response from CDOGS");
                }
            }
            catch (Exception e)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, $"Cdogs error response: {e.Message} {e.StackTrace}");
            }
            return HealthCheckResult.Healthy();
        }
    }
}
