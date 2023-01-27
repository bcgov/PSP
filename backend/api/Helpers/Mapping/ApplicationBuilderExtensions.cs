using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Mapster;
using Microsoft.AspNetCore.Builder;

namespace Pims.Api.Helpers.Mapping
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMapster(this IApplicationBuilder app)
        {
            var config = new TypeAdapterConfig();
            config.Scan(Assembly.GetAssembly(typeof(Startup)));

            return app;
        }
    }
}
