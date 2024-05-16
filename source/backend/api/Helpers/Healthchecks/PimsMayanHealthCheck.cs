using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pims.Api.Services;

namespace Pims.Api.Helpers.Healthchecks
{
    public class PimsMayanHealthcheck : IHealthCheck
    {
        private readonly IDocumentService _documentService;
        private readonly ClaimsPrincipal _user;
        private readonly IConfiguration _configuration;

        public PimsMayanHealthcheck(IConfiguration configuration, IDocumentService documentService, ClaimsPrincipal user)
        {
            _documentService = documentService;
            _user = user;
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!this._user.Claims.Any())
                {
                    // for the purposes of this health check, grant the user in context the claim necessary to be treated as a service account.
                    this._user.AddIdentity(new ClaimsIdentity(new List<Claim>() { new Claim("clientId", this._configuration.GetValue<string>("Keycloak:Client")) }));
                }
                var documentTypes = await _documentService.GetStorageDocumentTypes(page: 1);
                if (documentTypes.HttpStatusCode != System.Net.HttpStatusCode.OK || documentTypes.Payload == null || documentTypes.Payload.Count == 0)
                {
                    return new HealthCheckResult(HealthStatus.Unhealthy, $"received invalid mayan response for document types");
                }
            }
            catch (Exception e)
            {
                return new HealthCheckResult(HealthStatus.Degraded, $"Mayan error response: {e.Message}");
            }
            return HealthCheckResult.Healthy();
        }
    }
}
