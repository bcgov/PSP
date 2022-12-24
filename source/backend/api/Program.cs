using System;
using System.Diagnostics.CodeAnalysis;
using CommandLine;
using CommandLine.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Pims.Api.Configuration;
using Serilog;

namespace Pims.Api
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
        private static IWebHostBuilder CreateWebHostBuilder(ProgramOptions options)
        {
            var args = options.ToArgs();
            DotNetEnv.Env.Load();
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("connectionstrings.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"connectionstrings.{env}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("geocoder.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"geocoder.{env}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("ltsa.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"ltsa.{env}.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("av.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"av.{env}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                })
                .UseSerilog()
                .UseUrls(config.GetValue<string>("ASPNETCORE_URLS"))
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = 524288000; // 500MB
                });
        }
    }
}
