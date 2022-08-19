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

        /// <summary>
        /// Parses the environment configuration and returns 'JsonSerializerOptions'.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static JsonSerializerOptions GenerateJsonSerializerOptions(this IConfiguration configuration)
        {
            return new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = (!string.IsNullOrWhiteSpace(configuration["Serialization:Json:IgnoreNullValues"]) && bool.Parse(configuration["Serialization:Json:IgnoreNullValues"])) ? JsonIgnoreCondition.WhenWritingNull : JsonIgnoreCondition.Never,
                PropertyNameCaseInsensitive = !string.IsNullOrWhiteSpace(configuration["Serialization:Json:PropertyNameCaseInsensitive"]) && bool.Parse(configuration["Serialization:Json:PropertyNameCaseInsensitive"]),
                PropertyNamingPolicy = configuration["Serialization:Json:PropertyNamingPolicy"] == "CamelCase" ? JsonNamingPolicy.CamelCase : null,
                WriteIndented = !string.IsNullOrWhiteSpace(configuration["Serialization:Json:WriteIndented"]) && bool.Parse(configuration["Serialization:Json:WriteIndented"]),
                Converters = { new JsonStringEnumMemberConverter(JsonNamingPolicy.CamelCase) },
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
            };
        }
    }
}
