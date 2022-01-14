using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pims.Dal.Services;

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
        /// Add PimsRepository objects to the dependency injection service collection.
        /// </summary>
        /// <param name="Repositories"></param>
        /// <returns></returns>
        public static IServiceCollection AddPimsRepositories(this IServiceCollection repositories)
        {
            repositories.AddScoped<IPimsRepository, PimsRepository>();
            repositories.AddScoped<Repositories.IPropertyService, Repositories.PropertyService>();
            repositories.AddScoped<Repositories.IProvinceService, Repositories.ProvinceService>();
            repositories.AddScoped<Repositories.ILookupService, Repositories.LookupService>();
            repositories.AddScoped<Repositories.ISystemConstantService, Services.SystemConstantService>();
            repositories.AddScoped<Repositories.IOrganizationService, Repositories.OrganizationService>();
            repositories.AddScoped<Repositories.IPersonService, Repositories.PersonService>();
            repositories.AddScoped<Repositories.IUserService, Repositories.UserService>();
            repositories.AddScoped<Repositories.IRoleService, Repositories.RoleService>();
            repositories.AddScoped<Repositories.IClaimService, Repositories.ClaimService>();
            repositories.AddScoped<Repositories.IAccessRequestService, Repositories.AccessRequestService>();
            repositories.AddScoped<Repositories.ITenantService, Repositories.TenantService>();
            repositories.AddScoped<Repositories.ILeaseService, Repositories.LeaseService>();
            repositories.AddScoped<Repositories.IContactService, Repositories.ContactService>();
            repositories.AddScoped<Repositories.IInsuranceService, Repositories.InsuranceService>();
            repositories.AddScoped<Repositories.IAutocompleteService, Repositories.AutocompleteService>();
            return repositories; // TODO: Use reflection to find all Repositories.
        }

        /// <summary>
        /// Add PimsService objects to the dependency injection service collection.
        /// </summary>
        /// <param name="Repositories"></param>
        /// <returns></returns>
        public static IServiceCollection AddPimsServices(this IServiceCollection services)
        {
            services.AddScoped<IPimsService, PimsService>();
            return services; // TODO: Use reflection to find all Repositories.
            services.AddScoped<Services.IUserOrganizationService, Services.UserOrganizationService>();
            services.AddScoped<Services.IOrganizationService, Services.OrganizationService>();
        }

        /// <summary>
        /// Add the PIMS DB Context to the repository collection.
        /// </summary>
        /// <param name="repositories"></param>
        /// <param name="env"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddPimsContext(this IServiceCollection repositories, IHostEnvironment env, string connectionString)
        {
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(connectionString));

            repositories.AddDbContext<PimsContext>(options =>
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

            return repositories;
        }
    }
}
