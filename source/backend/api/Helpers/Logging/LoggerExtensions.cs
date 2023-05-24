using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Pims.Api.Helpers.Logging
{
    [ExcludeFromCodeCoverage]
    public static class LoggerExtensions
    {
        public static IServiceCollection AddSerilogging(
            this IServiceCollection services, IConfiguration configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment != null)
            {

                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.Debug(formatProvider: CultureInfo.CurrentCulture)
                    .WriteTo.Console(formatProvider: CultureInfo.CurrentCulture)
                    .Enrich.WithProperty("Environment", environment)
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
            }
            return services;
        }
    }
}
