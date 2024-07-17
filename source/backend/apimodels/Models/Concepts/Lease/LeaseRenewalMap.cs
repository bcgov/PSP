using Mapster;
using Pims.Api.Models.Base;
using Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Lease
{
    public class LeaseRenewalMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PimsLeaseRenewal, LeaseRenewalModel>()
                .Map(dest => dest.Id, src => src.LeaseRenewalId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.CommencementDt, src => src.CommencementDt)
                .Map(dest => dest.ExpiryDt, src => src.ExpiryDt)
                .Map(dest => dest.IsExercised, src => src.IsExercised)
                .Map(dest => dest.RenewalNote, src => src.RenewalNote)
                .Map(dest => dest.Lease, src => src.Lease)
                .Inherits<IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<LeaseRenewalModel, PimsLeaseRenewal>()
                .Map(dest => dest.LeaseRenewalId, src => src.Id)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.CommencementDt, src => src.CommencementDt)
                .Map(dest => dest.ExpiryDt, src => src.ExpiryDt)
                .Map(dest => dest.IsExercised, src => src.IsExercised)
                .Map(dest => dest.RenewalNote, src => src.RenewalNote)
                .Map(dest => dest.Lease, src => src.Lease)
                .Inherits<BaseAuditModel, IBaseAppEntity>();
        }
    }
}
