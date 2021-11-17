using Mapster;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;
using System.Linq;

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
                .Map(dest => dest.RenewalCount, src => src.RenewalCount)
                .Map(dest => dest.Properties, src => src.GetProperties())
                .Map(dest => dest.Insurances, src => src.PimsInsurances)
                .Map(dest => dest.LFileNo, src => src.LFileNo)
                .Map(dest => dest.TfaFileNo, src => src.TfaFileNo)
                .Map(dest => dest.PsFileNo, src => src.PsFileNo)
                .Map(dest => dest.MotiName, src => src.GetMotiName())
                .Map(dest => dest.ExpiryDate, src => src.GetExpiryDate())
                .Map(dest => dest.StartDate, src => src.OrigStartDate)
                .Map(dest => dest.ProgramName, src => src.GetProgramName())
                .Map(dest => dest.PaymentReceivableTypeId, src => src.LeasePayRvblTypeCode)
                .Map(dest => dest.PaymentFrequencyTypeId, src => src.LeasePmtFreqTypeCode)
                .Map(dest => dest.PaymentFrequencyType, src => src.LeasePmtFreqTypeCodeNavigation.Description)
                .Map(dest => dest.Note, src => src.LeaseNotes)
                .Map(dest => dest.Description, src => src.LeaseDescription)
                .Map(dest => dest.IsResidential, src => src.IsSubjectToRta)
                .Map(dest => dest.IsCommercialBuilding, src => src.IsCommBldg)
                .Map(dest => dest.IsOtherImprovement, src => src.IsOtherImprovement)
                .Map(dest => dest.Persons, src => src.GetPersons())
                .Map(dest => dest.Organizations, src => src.GetOrganizations())
                .Map(dest => dest.TenantNotes, src => src.PimsLeaseTenants != null ? src.PimsLeaseTenants.Select(t => t.Note) : null)
                .Map(dest => dest.Improvements, src => src.GetImprovements());
                .Map(dest => dest.SecurityDeposits, src => src.SecurityDeposits)
                .Map(dest => dest.SecurityDepositReturns, src => src.SecurityDepositReturns);
        }
    }
}
