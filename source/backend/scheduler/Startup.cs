using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.Redis.StackExchange;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Handlers;
using Pims.Core.Api.Helpers;
using Pims.Core.Api.Middleware;
using Pims.Core.Converters;
using Pims.Core.Http;
using Pims.Core.Json;
using Pims.Core.Security;
using Pims.Keycloak.Configuration;
using Pims.Scheduler.Http.Configuration;
using Pims.Scheduler.Policies;
using Pims.Scheduler.Repositories.Pims;
using Pims.Scheduler.Rescheduler;
using Pims.Scheduler.Services;
using Prometheus;
using StackExchange.Redis;

namespace Pims.Scheduler
{
    /// <summary>
    /// Startup class, provides a way to startup the .netcore RESTful API and configure it.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        #region Properties

        /// <summary>
        /// get - The application configuration settings.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// get/set - The environment settings for the application.
        /// </summary>
        public IWebHostEnvironment Environment { get; }
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instances of a Startup class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.Configuration = configuration;
            this.Environment = env;
        }
        #endregion

        #region Methods

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDocumentQueueService, DocumentQueueService>();
            services.AddScoped<IOpenIdConnectRequestClient, OpenIdConnectRequestClient>();
            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddSingleton<IPimsDocumentQueueRepository, PimsDocumentQueueRepository>();
            services.AddSingleton<IJobRescheduler, JobRescheduler>();

            services.AddSerilogging(this.Configuration);
            var jsonSerializerOptions = this.Configuration.GenerateJsonSerializerOptions();
            services.Configure<JsonSerializerOptions>(options =>
            {
                options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.DefaultIgnoreCondition = jsonSerializerOptions.DefaultIgnoreCondition;
                options.PropertyNameCaseInsensitive = jsonSerializerOptions.PropertyNameCaseInsensitive;
                options.PropertyNamingPolicy = jsonSerializerOptions.PropertyNamingPolicy;
                options.WriteIndented = jsonSerializerOptions.WriteIndented;
                options.Converters.Add(new JsonStringEnumMemberConverter());
                options.Converters.Add(new Int32ToStringJsonConverter());
            });
            services.Configure<Core.Http.Configuration.AuthClientOptions>(this.Configuration.GetSection("Keycloak"));
            services.Configure<Core.Http.Configuration.OpenIdConnectOptions>(this.Configuration.GetSection("OpenIdConnect"));
            services.Configure<Keycloak.Configuration.KeycloakOptions>(this.Configuration.GetSection("Keycloak"));
            services.Configure<PimsOptions>(this.Configuration.GetSection("Pims:Environment"));
            services.AddOptions();
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = jsonSerializerOptions.DefaultIgnoreCondition;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = jsonSerializerOptions.PropertyNameCaseInsensitive;
                    options.JsonSerializerOptions.PropertyNamingPolicy = jsonSerializerOptions.PropertyNamingPolicy;
                    options.JsonSerializerOptions.WriteIndented = jsonSerializerOptions.WriteIndented;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.Converters.Add(new Int32ToStringJsonConverter());
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });

            services.AddMvcCore()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = jsonSerializerOptions.DefaultIgnoreCondition;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = jsonSerializerOptions.PropertyNameCaseInsensitive;
                    options.JsonSerializerOptions.PropertyNamingPolicy = jsonSerializerOptions.PropertyNamingPolicy;
                    options.JsonSerializerOptions.WriteIndented = jsonSerializerOptions.WriteIndented;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.Converters.Add(new Int32ToStringJsonConverter());
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var key = Encoding.ASCII.GetBytes(Configuration["Keycloak:Secret"]);
                    options.RequireHttpsMetadata = false;
                    options.Authority = Configuration["OpenIdConnect:Authority"];
                    options.Audience = Configuration["Keycloak:Audience"];
                    options.SaveToken = true;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidAlgorithms = new List<string>() { "RS256" },
                    };
                    if (key.Length > 0)
                    {
                        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);
                    }

                    options.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            context.NoResult();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            throw new AuthenticationException("Failed to authenticate", context.Exception);
                        },
                        OnForbidden = context =>
                        {
                            return Task.CompletedTask;
                        },
                    };
                });

            services.AddHttpClient();
            services.AddTransient<LoggingHandler>();
            services.AddHttpClient("Pims.Api.Logging").AddHttpMessageHandler<LoggingHandler>();
            services.AddHttpContextAccessor();

            services.AddTransient<ClaimsPrincipal>(s => s.GetService<IHttpContextAccessor>()?.HttpContext?.User);
            services.AddResponseCaching();
            services.AddMemoryCache();

            // Export metrics from all HTTP clients registered in services
            services.UseHttpClientMetrics();

            services.AddHealthChecks().ForwardToPrometheus();

            services.Configure<OpenApiInfo>(Configuration.GetSection(nameof(OpenApiInfo)));
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations(false, true);
                options.CustomSchemaIds(o => o.FullName);
                options.OperationFilter<SwaggerDefaultValues>();
                options.DocumentFilter<Api.Helpers.Swagger.SwaggerDocumentFilter>();
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Type = SecuritySchemeType.ApiKey,
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            ConnectionMultiplexer redisConnection = ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis"));
            services.AddHangfire(options =>
            {
                options.UseSimpleAssemblyNameTypeSerializer().UseRecommendedSerializerSettings().UseRedisStorage(redisConnection).UseSerilogLogProvider().UseConsole();
            });
            services.AddHangfireServer();
            services.AddHangfireConsoleExtensions();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                options.AllowedHosts = this.Configuration.GetValue<string>("AllowedHosts")?.Split(';').ToList<string>();
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMetricServer();
            app.UseHttpMetrics();

            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }

            var baseUrl = this.Configuration.GetValue<string>("BaseUrl");
            app.UsePathBase(baseUrl);
            app.UseForwardedHeaders();

            app.UseMiddleware<ResponseTimeMiddleware>();
            app.UseMiddleware<LogRequestMiddleware>();
            app.UseMiddleware<LogResponseMiddleware>();

            app.UseRouting();
            app.UseCors();

            // Exception handler middleware that changes HTTP response codes must be registered after UseHttpMetrics()
            // in order to ensure that prometheus-net reports the correct HTTP response status code.
            app.UseHttpMetrics();

            // Set responses secure headers.
            ConfigureSecureHeaders(app);

            app.UseResponseCaching();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] { new HangfireDashboardAuthorizationFilter(app.ApplicationServices.GetRequiredService<IOptionsMonitor<KeycloakOptions>>(), Permissions.SystemAdmin) },
            });

            var healthPort = this.Configuration.GetValue<int>("HealthChecks:Port");
            app.UseHealthChecks(this.Configuration.GetValue<string>("HealthChecks:LivePath"), healthPort, new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("SqlServer"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });
            app.UseHealthChecks(this.Configuration.GetValue<string>("HealthChecks:ReadyPath"), healthPort, new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("services") && !r.Tags.Contains("external"),
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });

            app.UseEndpoints(config =>
            {
                config.MapControllers();
                config.MapHangfireDashboard();

                // Enable the /metrics page to export Prometheus metrics
                config.MapMetrics();
            });

            ScheduleHangfireJobs(app.ApplicationServices);
        }

        private void ScheduleHangfireJobs(IServiceProvider services)
        {
            // provide default definition of all jobs.
            RecurringJob.AddOrUpdate<IDocumentQueueService>(nameof(DocumentQueueService.UploadQueuedDocuments), x => x.UploadQueuedDocuments(), Cron.Hourly);

            // override scheduled jobs with configuration.
            JobScheduleOptions jobOptions = this.Configuration.GetSection("JobOptions").Get<JobScheduleOptions>();
            services.GetService<IJobRescheduler>().LoadSchedules(jobOptions);
        }

        /// <summary>
        /// Configures the app to to use content security policies.
        /// </summary>
        /// <param name="app">The application builder provider.</param>
        private static void ConfigureSecureHeaders(IApplicationBuilder app)
        {
            app.Use(
            async (context, next) =>
            {
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=86400; includeSubDomains");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-XSS-Protection", "1");
                context.Response.Headers.Add("X-Frame-Options", " DENY");
                await next().ConfigureAwait(true);
            });
        }
        #endregion
    }
}
