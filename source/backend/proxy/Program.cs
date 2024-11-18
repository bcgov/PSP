using System;
using System.Diagnostics.CodeAnalysis;
using CommandLine;
using CommandLine.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Pims.Api.Configuration;
using Serilog;

namespace Pims.Proxy
{
    /// <summary>
    /// Program class, provides the main program starting point for the Geo-spatial application.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        /// <summary>
        /// The primary entry point for the application.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var results = Parser.Default.ParseArguments<ProgramOptions>(args);

            results.WithParsed((options) =>
                {
                    var builder = CreateWebHostBuilder(options);
                    builder.Build().Run();
                })
                .WithNotParsed((errors) =>
                {
                    var helpText = HelpText.AutoBuild(
                    results,
                    h => { return HelpText.DefaultParsingErrorsHandler(results, h); },
                    e => e);
                    Console.WriteLine(helpText);
                });
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            DotNetEnv.Env.Load();
            return Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddEnvironmentVariables();
                config.AddCommandLine(args);
            });
        }

        /// <summary>
        /// Create a default configuration and setup for a web application.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static IHostBuilder CreateWebHostBuilder(ProgramOptions options)
        {
            var args = options.ToArgs();
            DotNetEnv.Env.Load();
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webHostBuilder =>
                webHostBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .UseUrls(config.GetValue<string>("ASPNETCORE_URLS"))
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = 524288000; // 500MB
                })).UseSerilog();
        }
    }
}
