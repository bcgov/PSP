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
            repositories.AddScoped<Repositories.ILeasePeriodRepository, Repositories.LeasePeriodRepository>();
            repositories.AddScoped<Repositories.ISecurityDepositRepository, Repositories.SecurityDepositRepository>();
            repositories.AddScoped<Repositories.ILeasePaymentRepository, Repositories.LeasePaymentRepository>();
            repositories.AddScoped<Repositories.ISecurityDepositReturnRepository, Repositories.SecurityDepositReturnRepository>();
            repositories.AddScoped<Repositories.IResearchFileRepository, Repositories.ResearchFileRepository>();
            repositories.AddScoped<Repositories.IResearchFilePropertyRepository, Repositories.ResearchFilePropertyRepository>();
            repositories.AddScoped<Repositories.IDocumentTypeRepository, Repositories.DocumentTypeRepository>();
            repositories.AddScoped<Repositories.INoteRepository, Repositories.NoteRepository>();
            repositories.AddScoped<Repositories.IEntityNoteRepository, Repositories.EntityNoteRepository>();
            repositories.AddScoped<Repositories.IDocumentRepository, Repositories.DocumentRepository>();
            repositories.AddScoped<Repositories.IAcquisitionFileRepository, Repositories.AcquisitionFileRepository>();
            repositories.AddScoped<Repositories.IAcquisitionFilePropertyRepository, Repositories.AcquisitionFilePropertyRepository>();
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
            repositories.AddScoped<Repositories.IFormTypeRepository, Repositories.FormTypeRepository>();
            repositories.AddScoped<Repositories.IAgreementRepository, Repositories.AgreementRepository>();
            repositories.AddScoped<Repositories.ICompensationRequisitionRepository, Repositories.CompensationRequisitionRepository>();
            repositories.AddScoped<Repositories.IH120CategoryRepository, Repositories.H120CategoryRepository>();
            repositories.AddScoped<Repositories.ICompReqFinancialRepository, Repositories.CompReqFinancialRepository>();
            repositories.AddScoped<Repositories.IInterestHolderRepository, Repositories.InterestHolderRepository>();
            repositories.AddScoped<Repositories.IPropertyImprovementRepository, Repositories.PropertyImprovementRepository>();
            repositories.AddScoped<Repositories.ILeaseStakeholderRepository, Repositories.LeaseStakeholderRepository>();
            repositories.AddScoped<Repositories.IExpropriationPaymentRepository, Repositories.ExpropriationPaymentRepository>();
            repositories.AddScoped<Repositories.IPropertyContactRepository, Repositories.PropertyContactRepository>();
            repositories.AddScoped<Repositories.IPropertyActivityRepository, Repositories.PropertyActivityRepository>();
            repositories.AddScoped<Repositories.IPropertyActivityDocumentRepository, Repositories.PropertyActivityDocumentRepository>();
            repositories.AddScoped<Repositories.IDispositionFilePropertyRepository, Repositories.DispositionFilePropertyRepository>();
            repositories.AddScoped<Repositories.IDispositionFileRepository, Repositories.DispositionFileRepository>();
            repositories.AddScoped<Repositories.IDispositionFileDocumentRepository, Repositories.DispositionFileDocumentRepository>();
            repositories.AddScoped<Repositories.IDispositionFileChecklistRepository, Repositories.DispositionFileChecklistRepository>();
            repositories.AddScoped<Repositories.IPropertyOperationRepository, Repositories.PropertyOperationRepository>();
            repositories.AddScoped<Repositories.IHistoricalNumberRepository, Repositories.HistoricalNumberRepository>();
            repositories.AddScoped<Repositories.ILeaseRenewalRepository, Repositories.LeaseRenewalRepostory>();
            repositories.AddScoped<Repositories.IConsultationRepository, Repositories.ConsultationRepository>();
            return repositories;
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
