using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using nClam;
using Pims.Av.Configuration;

namespace Pims.Av
{
    /// <summary>
    /// ServiceCollectionExtensions static class, provides extension methods for ServiceCollection objects.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the AddClamAvService to the dependency injection service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IServiceCollection AddClamAvService(this IServiceCollection services, IConfigurationSection section)
        {

            return services
                .Configure<Configuration.ClamAvOptions>(section)
                .AddSingleton<IAvService, ClamAvService>()
                .AddSingleton<IClamClient, ClamClient>(x => new ClamClient(ClamAvOptions.DEFAULTURI) { MaxStreamSize = int.Parse(section["MaxFileSize"], CultureInfo.InvariantCulture), MaxChunkSize = int.Parse(section["MaxFileSize"], CultureInfo.InvariantCulture) });
        }
    }
}
