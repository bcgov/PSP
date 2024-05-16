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
    public class PimsCdogsHealthcheck : IHealthCheck
    {
        private readonly IDocumentGenerationService _generationService;
        private readonly IConfiguration _configuration;
        private readonly ClaimsPrincipal _user;

        public PimsCdogsHealthcheck(IDocumentGenerationService generationService, IConfiguration configuration, ClaimsPrincipal user)
        {
            _generationService = generationService;
            _configuration = configuration;
            _user = user;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // for the purposes of this health check, grant the user in context the claim necessary to be treated as a service account.
                if (!this._user.Claims.Any())
                {
                    this._user.AddIdentity(new ClaimsIdentity(new List<Claim>() { new Claim("clientId", this._configuration.GetValue<string>("Keycloak:Client")) }));
                }
                var fileTypes = await _generationService.GetSupportedFileTypes();
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
