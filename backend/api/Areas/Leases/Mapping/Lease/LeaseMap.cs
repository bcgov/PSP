using System.Linq;
using Mapster;
using Pims.Api.Helpers.Extensions;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;
namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class LeaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLease, Model.LeaseModel>()
                .Map(dest => dest.Id, src => src.LeaseId)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.Amount, src => src.LeaseAmount)
                .Map(dest => dest.RenewalCount, src => src.PimsLeaseTerms.Count)
                .Map(dest => dest.Properties, src => src.GetProperties())
                .Map(dest => dest.Insurances, src => src.PimsInsurances)
                .Map(dest => dest.LFileNo, src => src.LFileNo)
                .Map(dest => dest.TfaFileNo, src => src.TfaFileNo)
                .Map(dest => dest.PsFileNo, src => src.PsFileNo)
                .Map(dest => dest.MotiName, src => src.GetMotiName())
                .Map(dest => dest.ExpiryDate, src => src.GetExpiryDate())
                .Map(dest => dest.StartDate, src => src.OrigStartDate)
                .Map(dest => dest.ProgramName, src => src.GetProgramName())
                .Map(dest => dest.PaymentReceivableType, src => src.LeasePayRvblTypeCodeNavigation)
                .Map(dest => dest.PaymentFrequencyType, src => src.LeasePmtFreqTypeCodeNavigation)
                .Map(dest => dest.CategoryType, src => src.LeaseCategoryTypeCodeNavigation)
                .Map(dest => dest.Type, src => src.LeaseLicenseTypeCodeNavigation)
                .Map(dest => dest.InitiatorType, src => src.LeaseInitiatorTypeCodeNavigation)
                .Map(dest => dest.PurposeType, src => src.LeasePurposeTypeCodeNavigation)
                .Map(dest => dest.StatusType, src => src.LeaseStatusTypeCodeNavigation)
                .Map(dest => dest.Terms, src => src.PimsLeaseTerms)
                .Map(dest => dest.ResponsibilityType, src => src.LeaseResponsibilityTypeCodeNavigation)
                .Map(dest => dest.ResponsibilityEffectiveDate, src => src.ResponsibilityEffectiveDate)
                .Map(dest => dest.Note, src => src.LeaseNotes)
                .Map(dest => dest.Description, src => src.LeaseDescription)
                .Map(dest => dest.IsResidential, src => src.IsSubjectToRta)
                .Map(dest => dest.IsCommercialBuilding, src => src.IsCommBldg)
                .Map(dest => dest.IsOtherImprovement, src => src.IsOtherImprovement)
                .Map(dest => dest.Persons, src => src.GetPersons())
                .Map(dest => dest.Organizations, src => src.GetOrganizations())
                .Map(dest => dest.TenantNotes, src => src.PimsLeaseTenants != null ? src.PimsLeaseTenants.Select(t => t.Note) : null)
                .Map(dest => dest.Improvements, src => src.GetImprovements())
                .Map(dest => dest.SecurityDeposits, src => src.PimsSecurityDeposits)
                .Map(dest => dest.SecurityDepositReturns, src => src.PimsSecurityDepositReturns)
                .Map(dest => dest.Tenants, src => src.PimsLeaseTenants)
                .Map(dest => dest.ReturnNotes, src => src.ReturnNotes);

            config.NewConfig<Model.LeaseModel, Entity.PimsLease>()
                .Map(dest => dest.LeaseId, src => src.Id)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Map(dest => dest.LeaseAmount, src => src.Amount)
                .Map(dest => dest.PimsPropertyLeases, src => src.Properties)
                .Map(dest => dest.LFileNo, src => src.LFileNo)
                .Map(dest => dest.TfaFileNo, src => src.TfaFileNo)
                .Map(dest => dest.PsFileNo, src => src.PsFileNo)
                .Map(dest => dest.MotiContact, src => src.MotiName)
                .Map(dest => dest.MotiRegion, src => src.MotiRegion)
                .Map(dest => dest.LeaseCategoryOtherDesc, src => src.OtherCategoryType)
                .Map(dest => dest.OtherLeaseProgramType, src => src.OtherProgramType)
                .Map(dest => dest.OtherLeasePurposeType, src => src.OtherPurposeType)
                .Map(dest => dest.LeasePurposeOtherDesc, src => src.OtherPurposeType)
                .Map(dest => dest.OtherLeaseLicenseType, src => src.OtherType)
                .Map(dest => dest.OrigExpiryDate, src => src.ExpiryDate)
                .Map(dest => dest.OrigStartDate, src => src.StartDate)
                .Map(dest => dest.LeaseProgramTypeCode, src => src.ProgramType.GetTypeId())
                .Map(dest => dest.LeasePayRvblTypeCode, src => src.PaymentReceivableType.GetTypeId())
                .Map(dest => dest.LeasePmtFreqTypeCode, src => src.PaymentFrequencyType.GetTypeId())
                .Map(dest => dest.LeaseCategoryTypeCode, src => src.CategoryType.GetTypeId())
                .Map(dest => dest.LeaseLicenseTypeCode, src => src.Type.GetTypeId())
                .Map(dest => dest.LeaseInitiatorTypeCode, src => src.InitiatorType.GetTypeId())
                .Map(dest => dest.LeasePurposeTypeCode, src => src.PurposeType.GetTypeId())
                .Map(dest => dest.LeaseResponsibilityTypeCode, src => src.ResponsibilityType.GetTypeId())
                .Map(dest => dest.LeaseStatusTypeCode, src => src.StatusType.GetTypeId())
                .Map(dest => dest.ResponsibilityEffectiveDate, src => src.ResponsibilityEffectiveDate)
                .Map(dest => dest.DocumentationReference, src => src.DocumentationReference)
                .Map(dest => dest.LeaseNotes, src => src.Note)
                .Map(dest => dest.LeaseDescription, src => src.Description)
                .Map(dest => dest.PimsLeaseTenants, src => src.Tenants)
                .IgnoreNullValues(true);
        }
    }
}
