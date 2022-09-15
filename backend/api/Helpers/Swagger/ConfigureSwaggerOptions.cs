using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Pims.Api.Helpers.Swagger
{

    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "PIMS API", // TODO: PSP-4420 From configuration
                Version = description.ApiVersion.ToString(),
                Description = "Property Inventory Management System", // TODO: PSP-4420 From configuration
                Contact = new OpenApiContact() { Name = "Support", Email = "support@pims.gov.bc.ca" }, // TODO: PSP-4420 From configuration
                License = new OpenApiLicense() { Name = "APACHE", Url = new Uri("https://github.com/bcgov/PSP/blob/dev/LICENSE") }, // TODO: PSP-4420 From configuration
            };

            if (description.IsDeprecated)
            {
                info.Description += " (This API version has been deprecated).";
            }

            return info;
        }
    }
}
