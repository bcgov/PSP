using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pims.Core.Exceptions;
using Pims.Dal;
using Pims.Ltsa;

namespace Pims.Api.Helpers.Healthchecks
{
    public class PimsLtsaHealthcheck : IHealthCheck
    {
        private readonly int? _pid;
        private readonly ILtsaService _ltsaService;

        public PimsLtsaHealthcheck(LtsaHealthCheckOptions options, ILtsaService ltsaService)
        {
            string pidValue = options.Pid;
            _pid = pidValue?.Length > 0 ? int.Parse(pidValue) : null;
            _ltsaService = ltsaService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!_pid.HasValue || _pid.Value == 0)
                {
                    var token = await _ltsaService.GetTokenAsync();
                    if (token.AccessToken != null)
                    {
                        return new HealthCheckResult(HealthStatus.Degraded, $"received invalid token response from LTSA");
                    }
                }
                else
                {
                    var titleSummary = await _ltsaService.GetTitleSummariesAsync(_pid.Value);
                    if (titleSummary.TitleSummaries.Count == 0)
                    {
                        return new HealthCheckResult(HealthStatus.Degraded, $"received invalid title summary response for pid: {_pid.Value}");
                    }
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
