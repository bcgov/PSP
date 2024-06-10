using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pims.Api.Repositories.Mayan;

namespace Pims.Api.Helpers.Healthchecks
{
    public class PimsMayanHealthcheck : IHealthCheck
    {
        private readonly IEdmsDocumentRepository _documentRepository;

        public PimsMayanHealthcheck(IEdmsDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var documentTypes = await _documentRepository.TryGetDocumentTypesAsync(string.Empty, 1, 1);
                if (documentTypes.HttpStatusCode != System.Net.HttpStatusCode.OK || documentTypes.Payload == null || documentTypes.Payload.Count == 0)
                {
                    return new HealthCheckResult(HealthStatus.Unhealthy, $"received invalid mayan response for document types");
                }
            }
            catch (Exception e)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, $"Mayan error response: {e.Message}");
            }
            return HealthCheckResult.Healthy();
        }
    }
}
