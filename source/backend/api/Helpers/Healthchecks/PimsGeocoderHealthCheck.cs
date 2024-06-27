using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pims.Geocoder;

namespace Pims.Api.Helpers.Healthchecks
{
    public class PimsGeocoderHealthcheck : IHealthCheck
    {
        private readonly IGeocoderService _geocoderService;
        private readonly string _address;

        public PimsGeocoderHealthcheck(IConfiguration configuration, IGeocoderService geocoderService)
        {
            _geocoderService = geocoderService;
            _address = configuration.GetSection("HealthChecks:Geocoder:Address").Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var sites = await _geocoderService.GetSiteAddressesAsync(_address);
                if (sites == null || !sites.Features.Any())
                {
                    return new HealthCheckResult(HealthStatus.Degraded, $"received invalid file types response from Geocoder");
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
