using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pims.Core.Exceptions;
using Pims.Ltsa;

namespace Pims.Api.Helpers.Healthchecks
{
    public class PimsLtsaHealthcheck : IHealthCheck
    {
        private readonly int _pid;
        private readonly ILtsaService _ltsaService;

        public PimsLtsaHealthcheck(IConfiguration configuration, ILtsaService ltsaService)
        {
            _pid = int.Parse(configuration.GetSection("HealthChecks:Ltsa:Pid").Value);
            _ltsaService = ltsaService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var titleSummary = await _ltsaService.GetTitleSummariesAsync(_pid);
                if (titleSummary.TitleSummaries.Count == 0)
                {
                    return new HealthCheckResult(HealthStatus.Degraded, $"received invalid title summary response for pid: {_pid}");
                }
            }
            catch(LtsaException e)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, $"LTSA error response: {e.Message}");
            }
            return HealthCheckResult.Healthy();
        }
    }
}
