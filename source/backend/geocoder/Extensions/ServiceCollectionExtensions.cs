using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pims.Core.Http;

namespace Pims.Geocoder
{
    /// <summary>
    /// ServiceCollectionExtensions static class, provides extension methods for ServiceCollection objects.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the AddGeocoderService to the dependency injection service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IServiceCollection AddGeocoderService(this IServiceCollection services, IConfigurationSection section)
        {
            // In this instance the auth service is bundled with the geocoder service itself, and the auth service is intended to be a singleton.
            return services
                .Configure<Configuration.GeocoderOptions>(section)
                .AddSingleton<IGeocoderService, GeocoderService>()
                .AddSingleton<IHttpRequestClient, HttpRequestClient>();
        }
    }
}
