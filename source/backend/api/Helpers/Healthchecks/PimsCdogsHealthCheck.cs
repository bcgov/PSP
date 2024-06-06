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

                var fileTypes = await _generationRepository.TryGetFileTypesAsync();
                if (fileTypes.HttpStatusCode != System.Net.HttpStatusCode.OK || fileTypes.Payload == null || fileTypes.Payload.Dictionary.Count == 0)
                {
                    return new HealthCheckResult(HealthStatus.Unhealthy, $"received invalid file types response from CDOGS");
                }
            }
            catch (Exception e)
            {
                return new HealthCheckResult(HealthStatus.Degraded, $"Cdogs error response: {e.Message} {e.StackTrace}");
            }
            return HealthCheckResult.Healthy();
        }
    }
}
