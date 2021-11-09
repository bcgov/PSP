using System.Linq;
using Mapster;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class LeaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Lease, Model.LeaseModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.RowVersion, src => src.RowVersion)
                .Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.RenewalCount, src => src.RenewalCount)
                .Map(dest => dest.Properties, src => src.Properties)
                .Map(dest => dest.Insurances, src => src.Insurances)
                .Map(dest => dest.LFileNo, src => src.LFileNo)
                .Map(dest => dest.TfaFileNo, src => src.TfaFileNo)
                .Map(dest => dest.PsFileNo, src => src.PsFileNo)
                .Map(dest => dest.MotiName, src => src.GetMotiName())
                .Map(dest => dest.ExpiryDate, src => src.GetExpiryDate())
                .Map(dest => dest.StartDate, src => src.OrigStartDate)
                .Map(dest => dest.RenewalDate, src => src.RenewalDate)
                .Map(dest => dest.ProgramName, src => src.GetProgramName())
                .Map(dest => dest.PaymentReceivableTypeId, src => src.PaymentReceivableTypeId)
                .Map(dest => dest.PaymentFrequencyTypeId, src => src.PaymentFrequencyTypeId)
                .Map(dest => dest.PaymentFrequencyType, src => src.PaymentFrequencyType.Description)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsSubjectToRta, src => src.IsSubjectToRta)
                .Map(dest => dest.IsCommBldg, src => src.IsCommBldg)
                .Map(dest => dest.IsOtherImprovement, src => src.IsOtherImprovement)
                .Map(dest => dest.Persons, src => src.Persons)
                .Map(dest => dest.Organizations, src => src.Organizations)
                .Map(dest => dest.TenantNotes, src => src.TenantsManyToMany.Select(t => t.Note))
                .Map(dest => dest.Improvements, src => src.Improvements);
        }
    }
}
