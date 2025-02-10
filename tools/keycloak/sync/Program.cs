using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pims.Core.Exceptions;
using Pims.Core.Configuration;
using Pims.Core.Http;
using Pims.Core.Http.Configuration;

using Pims.Keycloak;
using Pims.Keycloak.Configuration;
using Pims.Tools.Keycloak.Sync.Configuration;
using Pims.Tools.Keycloak.Sync.Configuration.Realm;
using Polly;

namespace Pims.Tools.Keycloak.Sync
{
    /// <summary>
    /// Program class, provides a console application that will sync keycloak and PIMS roles, groups and users.
    /// </summary>
    /// <reference url="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1"/>
    internal static class Program
    {
        #region Methods

        /// <summary>
        /// Entry point to program.
        /// </summary>
        /// <param name="args"></param>
        public static async Task<int> Main(string[] args)
        {
            var config = Configure(args)
                .Build();

            var services = new ServiceCollection()
                .Configure<ToolOptions>(config)
                .Configure<RequestOptions>(config)
                .Configure<ApiOptions>(config.GetSection("Api"))
                .Configure<RealmOptions>(config.GetSection("Realm"))
                .Configure<KeycloakOptions>(config.GetSection("Auth:Keycloak"))
                .Configure<KeycloakServiceAccountOptions>(config.GetSection("Auth:Keycloak:ServiceAccount"))
                .Configure<OpenIdConnectOptions>(config.GetSection("Auth:OpenIdConnect"))
                .Configure<JsonSerializerOptions>(options =>
                {
                    if (bool.TryParse(config["Serialization:Json:IgnoreNullValues"], out bool ignoreNullValues))
                    {
                        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    }

                    if (bool.TryParse(config["Serialization:Json:PropertyNameCaseInsensitive"], out bool nameCaseInsensitive))
                    {
                        options.PropertyNameCaseInsensitive = nameCaseInsensitive;
                    }

                    if (config["Serialization:Json:PropertyNamingPolicy"] == "CamelCase")
                    {
                        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    }

                    if (bool.TryParse(config["Serialization:Json:WriteIndented"], out bool writeIndented))
                    {
                        options.WriteIndented = writeIndented;
                    }

                    options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                })
                .AddSingleton<IConfiguration>(config)
                .AddLogging(options =>
                {
                    options.AddConfiguration(config.GetSection("Logging"));
                    options.AddConsole();
                })
                .Configure<LoggerFilterOptions>(options =>
                {
                    options.MinLevel = LogLevel.Information;
                })
                .AddTransient<Startup>()
                .AddTransient<JwtSecurityTokenHandler>()
                .AddScoped<IKeycloakRepository, KeycloakRepository>()
                .AddScoped<IPimsRequestClient, PimsRequestClient>()
                .AddScoped<IOpenIdConnectRequestClient, OpenIdConnectRequestClient>()
                .AddTransient<ISyncFactory, SyncFactory>();

            services.AddHttpClient("Pims.Tools.Keycloak.Sync", client => { });

            PollyOptions pollyOptions = new();
            config.GetSection("Polly").Bind(pollyOptions);

            services.AddResiliencePipeline<string, HttpResponseMessage>("retry-network-policy", (builder) =>
            {
                builder.AddRetry(new()
                {
                    BackoffType = DelayBackoffType.Exponential,
                    Delay = TimeSpan.FromSeconds(pollyOptions.DelayInSeconds),
                    MaxRetryAttempts = pollyOptions.MaxRetries,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>().Handle<HttpRequestException>().HandleResult(response => (int)response.StatusCode >= 500 && (int)response.StatusCode <= 599),
                });
            });

            var provider = services.BuildServiceProvider();
            var logger = provider.GetService<ILogger<Startup>>();

            var result = 0;
            try
            {
                result = await provider.GetService<Startup>().Run(args);
            }
            catch (HttpClientRequestException ex)
            {
                logger.LogCritical(ex, $"An HTTP request failed - {ex.StatusCode}: {ex.Message}");
                result = 1;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "An unhandled error has occurred.");
                result = 1;
            }

            provider.Dispose();
            return result;
        }

        /// <summary>
        /// Configure host.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static IConfigurationBuilder Configure(string[] args)
        {
            DotNetEnv.Env.TraversePath().Load();
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args ?? new string[0]);
        }
        #endregion
    }
}
