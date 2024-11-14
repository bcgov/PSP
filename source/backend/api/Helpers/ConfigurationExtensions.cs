using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Pims.Dal;

namespace Pims.Api.Helpers
{
    /// <summary>
    /// ConfigurationExtensions static class, provides extension methods for IConfiguration.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Parses the environment configuration and returns a 'PimsOptions'.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static PimsOptions GeneratePimsOptions(this IConfiguration configuration)
        {
            return new PimsOptions()
            {
                Tenant = configuration["Pims:Tenant"],
                HelpDeskEmail = configuration["Pims:HelpDeskEmail"],
            };
        }
    }
}
