using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

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
            repositories.AddScoped<IPropertyRepository, PropertyRepository>();
            repositories.AddScoped<IProvinceRepository, ProvinceRepository>();
            repositories.AddScoped<ILookupRepository, LookupRepository>();
            repositories.AddScoped<ISystemConstantRepository, SystemConstantRepository>();
            repositories.AddScoped<IPersonRepository, PersonRepository>();
            repositories.AddScoped<IUserRepository, UserRepository>();
            repositories.AddScoped<IRoleRepository, RoleRepository>();
            repositories.AddScoped<IClaimRepository, ClaimRepository>();
            repositories.AddScoped<IAccessRequestRepository, AccessRequestRepository>();
            repositories.AddScoped<ITenantRepository, TenantRepository>();
            repositories.AddScoped<ILeaseRepository, LeaseRepository>();
            repositories.AddScoped<IContactRepository, ContactRepository>();
            repositories.AddScoped<IAutocompleteRepository, AutocompleteRepository>();
            repositories.AddScoped<IOrganizationRepository, OrganizationRepository>();
            repositories.AddScoped<ILeasePeriodRepository, LeasePeriodRepository>();
            repositories.AddScoped<ISecurityDepositRepository, SecurityDepositRepository>();
            repositories.AddScoped<ILeasePaymentRepository, LeasePaymentRepository>();
            repositories.AddScoped<ISecurityDepositReturnRepository, SecurityDepositReturnRepository>();
            repositories.AddScoped<IResearchFileRepository, ResearchFileRepository>();
            repositories.AddScoped<IResearchFilePropertyRepository, ResearchFilePropertyRepository>();
            repositories.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
            repositories.AddScoped<INoteRepository, NoteRepository>();
            repositories.AddScoped<IDocumentRepository, DocumentRepository>();
            repositories.AddScoped<IAcquisitionFileRepository, AcquisitionFileRepository>();
            repositories.AddScoped<IAcquisitionFilePropertyRepository, AcquisitionFilePropertyRepository>();
            repositories.AddScoped<ISequenceRepository, SequenceRepository>();
            repositories.AddScoped<IPropertyLeaseRepository, PropertyLeaseRepository>();
            repositories.AddScoped<IProjectRepository, ProjectRepository>();
            repositories.AddScoped<IBusinessFunctionCodeRepository, BusinessFunctionCodeRepository>();
            repositories.AddScoped<IChartOfAccountsCodeRepository, ChartOfAccountsCodeRepository>();
            repositories.AddScoped<IYearlyFinancialCodeRepository, YearlyFinancialCodeRepository>();
            repositories.AddScoped<ICostTypeCodeRepository, CostTypeCodeRepository>();
            repositories.AddScoped<IFinancialActivityCodeRepository, FinancialActivityCodeRepository>();
            repositories.AddScoped<IWorkActivityCodeRepository, WorkActivityCodeRepository>();
            repositories.AddScoped<IResponsibilityCodeRepository, ResponsibilityCodeRepository>();
            repositories.AddScoped<IProductRepository, ProductRepository>();
            repositories.AddScoped<IDocumentRelationshipRepository<PimsResearchFileDocument>, ResearchFileDocumentRepository>();
            repositories.AddScoped<IDocumentRelationshipRepository<PimsAcquisitionFileDocument>, AcquisitionFileDocumentRepository>();
            repositories.AddScoped<IDocumentRelationshipRepository<PimsLeaseDocument>, LeaseDocumentRepository>();
            repositories.AddScoped<IDocumentRelationshipRepository<PimsProjectDocument>, ProjectDocumentRepository>();
            repositories.AddScoped<IDocumentRelationshipRepository<PimsDispositionFileDocument>, DispositionFileDocumentRepository>();
            repositories.AddScoped<IDocumentRelationshipRepository<PimsMgmtActivityDocument>, ManagementActivityDocumentRepository>();
            repositories.AddScoped<IDocumentRelationshipRepository<PimsManagementFileDocument>, ManagementFileDocumentRepository>();
            repositories.AddScoped<IDocumentRelationshipRepository<PimsPropertyDocument>, PropertyDocumentRepository>();
            repositories.AddScoped<ITakeRepository, TakeRepository>();
            repositories.AddScoped<IAcquisitionFileFormRepository, AcquisitionFileFormRepository>();
            repositories.AddScoped<IAcquisitionFileChecklistRepository, AcquisitionFileChecklistRepository>();
            repositories.AddScoped<IFormTypeRepository, FormTypeRepository>();
            repositories.AddScoped<IAgreementRepository, AgreementRepository>();
            repositories.AddScoped<ICompensationRequisitionRepository, CompensationRequisitionRepository>();
            repositories.AddScoped<IH120CategoryRepository, H120CategoryRepository>();
            repositories.AddScoped<ICompReqFinancialRepository, CompReqFinancialRepository>();
            repositories.AddScoped<IInterestHolderRepository, InterestHolderRepository>();
            repositories.AddScoped<IPropertyImprovementRepository, PropertyImprovementRepository>();
            repositories.AddScoped<ILeaseStakeholderRepository, LeaseStakeholderRepository>();
            repositories.AddScoped<IExpropriationPaymentRepository, ExpropriationPaymentRepository>();
            repositories.AddScoped<IPropertyContactRepository, PropertyContactRepository>();
            repositories.AddScoped<IDispositionFilePropertyRepository, DispositionFilePropertyRepository>();
            repositories.AddScoped<IDispositionFileRepository, DispositionFileRepository>();
            repositories.AddScoped<IDispositionFileChecklistRepository, DispositionFileChecklistRepository>();
            repositories.AddScoped<IPropertyOperationRepository, PropertyOperationRepository>();
            repositories.AddScoped<IHistoricalNumberRepository, HistoricalNumberRepository>();
            repositories.AddScoped<IPropertyTenureCleanupRepository, PropertyTenureCleanupRepository>();
            repositories.AddScoped<ILeaseRenewalRepository, LeaseRenewalRepostory>();
            repositories.AddScoped<IConsultationRepository, ConsultationRepository>();
            repositories.AddScoped<IDocumentQueueRepository, DocumentQueueRepository>();
            repositories.AddScoped<IPmbcBctfaPidRepository, PmbcBctfaPidRepository>();
            repositories.AddScoped<IExpropriationEventRepository, ExpropriationEventRepository>();
            repositories.AddScoped<IManagementFilePropertyRepository, ManagementFilePropertyRepository>();
            repositories.AddScoped<IManagementFileRepository, ManagementFileRepository>();
            repositories.AddScoped<IManagementActivityRepository, ManagementActivityRepository>();
            repositories.AddScoped<INoteRelationshipRepository<PimsAcquisitionFileNote>, AcquisitionFileNoteRepository>();
            repositories.AddScoped<INoteRelationshipRepository<PimsDispositionFileNote>, DispositionFileNoteRepository>();
            repositories.AddScoped<INoteRelationshipRepository<PimsLeaseNote>, LeaseNoteRepository>();
            repositories.AddScoped<INoteRelationshipRepository<PimsManagementFileNote>, ManagementFileNoteRepository>();
            repositories.AddScoped<INoteRelationshipRepository<PimsProjectNote>, ProjectNoteRepository>();
            repositories.AddScoped<INoteRelationshipRepository<PimsPropertyNote>, PropertyNoteRepository>();
            repositories.AddScoped<INoteRelationshipRepository<PimsResearchFileNote>, ResearchFileNoteRepository>();
            repositories.AddScoped<INotificationUserOutputRepository, NotificationUserOutputRepository>();
            repositories.AddScoped<INotificationRepository, NotificationRepository>();
            repositories.AddScoped<INotificationInboxRepository, NotificationInboxRepository>();
            repositories.AddScoped<IDocumentQueueRepository, DocumentQueueRepository>();
            repositories.AddScoped<INotificationRepository, NotificationRepository>();

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
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds);
                    sqlOptions.UseNetTopologySuite();
                });

                // Enable sensitive data logging in local environment only.
                // This should never be enabled in non-local environments as it may log personally identifiable information (PII) or other sensitive data.
                if (env.IsEnvironment("Local"))
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            return repositories;
        }
    }
}
