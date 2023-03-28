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
        /// Add PimsRepository objects to the dependency injection service collection.
        /// </summary>
        /// <param name="repositories"></param>
        /// <returns></returns>
        public static IServiceCollection AddPimsDalRepositories(this IServiceCollection repositories)
        {
            repositories.AddScoped<Repositories.IPropertyRepository, Repositories.PropertyRepository>();
            repositories.AddScoped<Repositories.IProvinceRepository, Repositories.ProvinceRepository>();
            repositories.AddScoped<Repositories.ILookupRepository, Repositories.LookupRepository>();
            repositories.AddScoped<Repositories.ISystemConstantRepository, Repositories.SystemConstantRepository>();
            repositories.AddScoped<Repositories.IPersonRepository, Repositories.PersonRepository>();
            repositories.AddScoped<Repositories.IUserRepository, Repositories.UserRepository>();
            repositories.AddScoped<Repositories.IRoleRepository, Repositories.RoleRepository>();
            repositories.AddScoped<Repositories.IClaimRepository, Repositories.ClaimRepository>();
            repositories.AddScoped<Repositories.IAccessRequestRepository, Repositories.AccessRequestRepository>();
            repositories.AddScoped<Repositories.ITenantRepository, Repositories.TenantRepository>();
            repositories.AddScoped<Repositories.ILeaseRepository, Repositories.LeaseRepository>();
            repositories.AddScoped<Repositories.IContactRepository, Repositories.ContactRepository>();
            repositories.AddScoped<Repositories.IInsuranceRepository, Repositories.InsuranceRepository>();
            repositories.AddScoped<Repositories.IAutocompleteRepository, Repositories.AutocompleteRepository>();
            repositories.AddScoped<Repositories.IOrganizationRepository, Repositories.OrganizationRepository>();
            repositories.AddScoped<Repositories.ILeaseTermRepository, Repositories.LeaseTermRepository>();
            repositories.AddScoped<Repositories.ISecurityDepositRepository, Repositories.SecurityDepositRepository>();
            repositories.AddScoped<Repositories.ILeasePaymentRepository, Repositories.LeasePaymentRepository>();
            repositories.AddScoped<Repositories.ISecurityDepositReturnRepository, Repositories.SecurityDepositReturnRepository>();
            repositories.AddScoped<Repositories.IResearchFileRepository, Repositories.ResearchFileRepository>();
            repositories.AddScoped<Repositories.IResearchFilePropertyRepository, Repositories.ResearchFilePropertyRepository>();
            repositories.AddScoped<Repositories.IDocumentTypeRepository, Repositories.DocumentTypeRepository>();
            repositories.AddScoped<Repositories.INoteRepository, Repositories.NoteRepository>();
            repositories.AddScoped<Repositories.IEntityNoteRepository, Repositories.EntityNoteRepository>();
            repositories.AddScoped<Repositories.IDocumentActivityRepository, Repositories.DocumentActivityRepository>();
            repositories.AddScoped<Repositories.IDocumentActivityTemplateRepository, Repositories.DocumentActivityTemplateRepository>();
            repositories.AddScoped<Repositories.IDocumentRepository, Repositories.DocumentRepository>();
            repositories.AddScoped<Repositories.IAcquisitionFileRepository, Repositories.AcquisitionFileRepository>();
            repositories.AddScoped<Repositories.IAcquisitionFilePropertyRepository, Repositories.AcquisitionFilePropertyRepository>();
            repositories.AddScoped<Repositories.IActivityRepository, Repositories.ActivityRepository>();
            repositories.AddScoped<Repositories.IActivityTemplateRepository, Repositories.ActivityTemplateRepository>();
            repositories.AddScoped<Repositories.ISequenceRepository, Repositories.SequenceRepository>();
            repositories.AddScoped<Repositories.IPropertyLeaseRepository, Repositories.PropertyLeaseRepository>();
            repositories.AddScoped<Repositories.IProjectRepository, Repositories.ProjectRepository>();
            repositories.AddScoped<Repositories.IBusinessFunctionCodeRepository, Repositories.BusinessFunctionCodeRepository>();
            repositories.AddScoped<Repositories.IChartOfAccountsCodeRepository, Repositories.ChartOfAccountsCodeRepository>();
            repositories.AddScoped<Repositories.IYearlyFinancialCodeRepository, Repositories.YearlyFinancialCodeRepository>();
            repositories.AddScoped<Repositories.ICostTypeCodeRepository, Repositories.CostTypeCodeRepository>();
            repositories.AddScoped<Repositories.IFinancialActivityCodeRepository, Repositories.FinancialActivityCodeRepository>();
            repositories.AddScoped<Repositories.IWorkActivityCodeRepository, Repositories.WorkActivityCodeRepository>();
            repositories.AddScoped<Repositories.IResponsibilityCodeRepository, Repositories.ResponsibilityCodeRepository>();
            repositories.AddScoped<Repositories.IProductRepository, Repositories.ProductRepository>();
            repositories.AddScoped<Repositories.IAcquisitionFileDocumentRepository, Repositories.AcquisitionFileDocumentRepository>();
            repositories.AddScoped<Repositories.IResearchFileDocumentRepository, Repositories.ResearchFileDocumentRepository>();
            repositories.AddScoped<Repositories.ITakeRepository, Repositories.TakeRepository>();
            repositories.AddScoped<Repositories.IAcquisitionFileFormRepository, Repositories.AcquisitionFileFormRepository>();
            repositories.AddScoped<Repositories.IAcquisitionFileChecklistRepository, Repositories.AcquisitionFileChecklistRepository>();
            return repositories; // TODO: PSP-4424 Use reflection to find all Repositories.
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
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Argument is required and cannot be null, empty or whitespace.", nameof(connectionString));
            }

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
