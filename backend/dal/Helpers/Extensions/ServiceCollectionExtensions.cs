using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pims.Dal
{
    /// <summary>
    /// ServiceCollectionExtensions static class, provides extension methods for IServiceCollection.
    /// </summary>
    ///
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add PimsService objects to the dependency injection service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPimsService(this IServiceCollection services)
        {
            services.AddScoped<IPimsService, PimsService>();
            services.AddScoped<Services.IPropertyService, Services.PropertyService>();
            services.AddScoped<Services.IProvinceService, Services.ProvinceService>();
            services.AddScoped<Services.ILookupService, Services.LookupService>();
            services.AddScoped<Services.ISystemConstantService, Services.SystemConstantService>();
            services.AddScoped<Services.IOrganizationService, Services.OrganizationService>();
            services.AddScoped<Services.IPersonService, Services.PersonService>();
            services.AddScoped<Services.IUserService, Services.UserService>();
            services.AddScoped<Services.IRoleService, Services.RoleService>();
            services.AddScoped<Services.IClaimService, Services.ClaimService>();
            services.AddScoped<Services.IAccessRequestService, Services.AccessRequestService>();
            services.AddScoped<Services.ITenantService, Services.TenantService>();
            services.AddScoped<Services.ILeaseService, Services.LeaseService>();
            services.AddScoped<Services.IContactService, Services.ContactService>();
            services.AddScoped<Services.IInsuranceService, Services.InsuranceService>();
            services.AddScoped<Services.IAutocompleteService, Services.AutocompleteService>();
            return services; // TODO: Use reflection to find all services.
        }

        /// <summary>
        /// Add the PIMS DB Context to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddPimsContext(this IServiceCollection services, IHostEnvironment env, string connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(connectionString));

            services.AddDbContext<PimsContext>(options =>
            {
                var sql = options.UseSqlServer(connectionString, options =>
                {
                    options.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds);
                    options.UseNetTopologySuite();
                });
                if (!env.IsProduction())
                {
                    var debugLoggerFactory = LoggerFactory.Create(builder => { builder.AddDebug(); }); // NOSONAR
                    sql.UseLoggerFactory(debugLoggerFactory);
                    options.EnableSensitiveDataLogging();
                    options.LogTo(Console.WriteLine);
                }
            });

            return services;
        }
    }
}
