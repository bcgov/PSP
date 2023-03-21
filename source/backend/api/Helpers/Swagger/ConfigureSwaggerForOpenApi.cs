using System;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Pims.Api.Helpers.Swagger
{
    public static class ConfigureSwaggerForOpenApi
    {
        public static IServiceCollection AddMultiVersionToSwagger(this IServiceCollection services, Action<SwaggerGenOptions> configSwagger = null)
        {
            services.AddSwaggerGen(options =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var provider = serviceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
                var info = serviceProvider.GetService<IOptions<OpenApiInfo>>().Value;
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var apiInfo = CopyOpenApiInfo(info, description.ApiVersion?.ToString());
                    options.SwaggerDoc(description.GroupName, apiInfo);
                }
                configSwagger?.Invoke(options);
            });
            return services;
        }

        public static IApplicationBuilder ConfigureSwaggerUI(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, Action<SwaggerUIOptions> uiOptions = null)
        {
            void AddSwaggerUI(SwaggerUIOptions options)
            {
                foreach (var item in provider.ApiVersionDescriptions)
                {
                    var url = string.Format(CultureInfo.CurrentCulture, "/swagger/{0}/swagger.json", item.GroupName);
                    var description = string.Format(CultureInfo.CurrentCulture, "{0} Beschreibung", item.GroupName);
                    options.SwaggerEndpoint(url, description);
                    uiOptions?.Invoke(options);
                }
            }
            return app.UseSwaggerUI(AddSwaggerUI);
        }

        private static OpenApiInfo CopyOpenApiInfo(OpenApiInfo current, string apiVersion)
        {
            current = current ?? new OpenApiInfo();
            return new OpenApiInfo
            {
                Contact = current.Contact,
                Description = current.Description,
                Extensions = current.Extensions,
                License = current.License,
                TermsOfService = current.TermsOfService,
                Title = string.Format(CultureInfo.CurrentCulture, current.Title ?? "API Version {0}", apiVersion),
                Version = apiVersion,
            };
        }
    }
}
